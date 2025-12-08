using OpenAI.Chat;
using Raports.DataTransferObjects.RaportSummaryContainers;

namespace Raports.Application.Consumers;

internal class GenerateSummaryConsumer(ILogger<GenerateSummaryConsumer> logger,
                                       IPublishEndpoint publish,
                                       ChatClient openAiClient,
                                       RaportsDBContext database) : IConsumer<GenerateSummary>
{
    private static readonly TimeZoneInfo PolandTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time");

    public async Task Consume(ConsumeContext<GenerateSummary> context)
    {
        logger.LogInformation("GenerateSummaryConsumer: generating summaries for RaportID={RaportId}", context.Message.Raport.ID);

        var ct = context.CancellationToken;

        var dbRaport = await database.Raports
            .Include(x => x.RequestedMeasurements)
            .Include(x => x.RequestedLocations)
            .Include(x => x.MeasurementGroups)
                .ThenInclude(y => y.LocationGroups)
                    .ThenInclude(z => z.SampleGroups)
            .Include(x => x.MeasurementGroups).ThenInclude(mg => mg.Measurement)
            .Include(x => x.MeasurementGroups).ThenInclude(mg => mg.LocationGroups).ThenInclude(lg => lg.Location)
            .Include(x => x.Period)
            .FirstOrDefaultAsync(x => x.ID == context.Message.Raport.ID, ct);

        if (dbRaport is null)
        {
            logger.LogWarning("GenerateSummaryConsumer: Raport {RaportId} not found", context.Message.Raport.ID);
            return;
        }

        const string lorem = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.";

        var modified = false;


        var messages = new ChatMessage[]
        {
            "I have data of measurements (for example temperature) that was captured for variuls locations and for various measurement types. Your job is to create short summary of this data." +
            "Data that i will provide for you will have its period, for example 'Daily' or 'Weekly', you should generate simmary of its data accordingly to its type. " +
            "Dataset will have measurement groups, each measurement group represent single type of measrueent, for example 'Temperature'. One measurement group can have multiple 'Location Gropus'" +
            "Each location gropu represent a location where this measruement was captured. ",
        };


        foreach (var mg in dbRaport.MeasurementGroups)
        {
            var locationSummaries = new List<string>();

            foreach (var lg in mg.LocationGroups)
            {
                if (!string.IsNullOrWhiteSpace(lg.Summary))
                {
                    logger.LogInformation("Summary already exists for LocationGroup ID {LGId}, reusing", lg.ID);
                    locationSummaries.Add($"{lg.Location?.Name}: {lg.Summary}");
                    continue;
                }

                var samplesText = lg.SampleGroups?.Any() == true
                    ? string.Join(", ", lg.SampleGroups.Select(s => 
                    {
                        var localTime = TimeZoneInfo.ConvertTimeFromUtc(s.Date, PolandTimeZone);
                        return $"Time: {localTime:yyyy-MM-dd HH:mm}, Value: {s.Value}";
                    }))
                    : "No samples";

                var locGroupMessage = new LocationGroupDescription(
                    lg.Location.Name,
                    mg.Measurement.Name,
                    mg.Measurement.Unit,
                    mg.Raport.Period.Name,
                    samplesText
                );

                var locationAnalysisMessage = new ChatMessage[]
                {
                    new SystemChatMessage(
                        "You are a data analyst specializing in home automation measurements. " +
                        "Your job is to analyze measurement data and provide concise summaries. " +
                        "All timestamps are in Central European Time (Poland). " +
                        "Output ONLY the summary text, no introductory phrases, no extra formatting."
                    ),
                    new UserChatMessage(
                        "Analyze the following measurement data and provide a brief summary (2-3 sentences):\n\n" +
                        $"{locGroupMessage}"
                    )
                };

                try
                {
                    var response = await openAiClient.CompleteChatAsync(locationAnalysisMessage, cancellationToken: ct);
                    var summary = response.Value.Content[0].Text?.Trim();

                    if (!string.IsNullOrWhiteSpace(summary))
                    {
                        lg.Summary = summary;
                        locationSummaries.Add($"{lg.Location?.Name}: {summary}");
                        modified = true;
                        logger.LogInformation("Generated AI summary for LocationGroup ID {LGId} (Location: {Location}): {Summary}", 
                            lg.ID, lg.Location?.Name, summary.Substring(0, Math.Min(50, summary.Length)) + "...");
                    }
                    else
                    {
                        lg.Summary = lorem;
                        locationSummaries.Add($"{lg.Location?.Name}: {lorem}");
                        modified = true;
                        logger.LogWarning("Empty AI response for LocationGroup ID {LGId}, using placeholder", lg.ID);
                    }
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Error generating AI summary for LocationGroup ID {LGId}, using placeholder", lg.ID);
                    lg.Summary = lorem;
                    locationSummaries.Add($"{lg.Location?.Name}: {lorem}");
                    modified = true;
                }
            }

            // Now generate the overall measurement summary from all location summaries
            if (string.IsNullOrWhiteSpace(mg.Summary) && locationSummaries.Any())
            {
                var combinedLocationSummaries = string.Join("\n\n", locationSummaries);

                var measurementAnalysisMessage = new ChatMessage[]
                {
                    new SystemChatMessage(
                        "You are a data analyst specializing in home automation measurements. " +
                        "Your job is to analyze summaries from multiple locations and create a comprehensive overview. " +
                        "All timestamps are in Central European Time (Poland). " +
                        "Output ONLY the summary text, no introductory phrases, no extra formatting."
                    ),
                    new UserChatMessage(
                        $"I have {mg.Measurement?.Name} ({mg.Measurement?.Unit}) measurements from multiple locations during a {mg.Raport.Period?.Name} period.\n\n" +
                        $"Here are the individual location summaries:\n\n{combinedLocationSummaries}\n\n" +
                        $"Create a comprehensive summary (3-4 sentences) that analyzes the overall {mg.Measurement?.Name} patterns across all locations, " +
                        $"highlighting key trends, comparisons between locations, and any notable observations."
                    )
                };

                try
                {
                    var response = await openAiClient.CompleteChatAsync(measurementAnalysisMessage, cancellationToken: ct);
                    var summary = response.Value.Content[0].Text?.Trim();

                    if (!string.IsNullOrWhiteSpace(summary))
                    {
                        mg.Summary = summary;
                        modified = true;
                        logger.LogInformation("Generated AI summary for MeasurementGroup ID {MGId} (Measurement: {Measurement}): {Summary}",
                            mg.ID, mg.Measurement?.Name, summary.Substring(0, Math.Min(50, summary.Length)) + "...");
                    }
                    else
                    {
                        mg.Summary = lorem;
                        modified = true;
                        logger.LogWarning("Empty AI response for MeasurementGroup ID {MGId}, using placeholder", mg.ID);
                    }
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Error generating AI summary for MeasurementGroup ID {MGId}, using placeholder", mg.ID);
                    mg.Summary = lorem;
                    modified = true;
                }
            }
            else if (!string.IsNullOrWhiteSpace(mg.Summary))
            {
                logger.LogInformation("Summary already exists for MeasurementGroup ID {MGId}, skipping", mg.ID);
            }
            else
            {
                logger.LogWarning("No location summaries available for MeasurementGroup ID {MGId}, using placeholder", mg.ID);
                mg.Summary = lorem;
                modified = true;
            }
        }

        if (modified)
        {
            try
            {
                await database.SaveChangesAsync(ct);
                logger.LogInformation("GenerateSummaryConsumer: saved summaries for Raport {RaportId}", dbRaport.ID);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "GenerateSummaryConsumer: error while saving summaries for Raport {RaportId}", dbRaport.ID);
                throw;
            }
        }
        else
        {
            logger.LogInformation("GenerateSummaryConsumer: no summaries needed for Raport {RaportId}", dbRaport.ID);
        }

        var message = new GenerateDocument()
        {
            Raport = context.Message.Raport
        };

        await publish.Publish(message, ct);
    }
}
