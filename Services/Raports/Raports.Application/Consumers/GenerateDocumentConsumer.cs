using Azure.Storage.Blobs.Models;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Raports.Application.Consumers;

internal class GenerateDocumentConsumer(ILogger<GenerateDocumentConsumer> logger,
                                       IPublishEndpoint publish,
                                       BlobContainerClient container,
                                       RaportsDBContext database) : IConsumer<GenerateDocument>
{
    public async Task Consume(ConsumeContext<GenerateDocument> context)
    {
        logger.LogInformation("GenerateDocumentConsumer: generating document for RaportID={RaportId}", context.Message.Raport.ID);

        var ct = context.CancellationToken;

        QuestPDF.Settings.License = LicenseType.Community;

        async Task PublishRaportFailedAsync(DefaultRaportDTO raportDto, int raportId, string description, CancellationToken cancellationToken)
        {
            try
            {
                logger.LogWarning("GenerateDocumentConsumer: publishing RaportFailed for Raport {RaportId}: {Description}", raportId, description);
                var message = new RaportFailed()
                {
                    FailedDate = DateTime.UtcNow,
                    Description = description,
                    Raport = raportDto
                };

                await publish.Publish(message, cancellationToken);
                logger.LogInformation("GenerateDocumentConsumer: RaportFailed published for Raport {RaportId}", raportId);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "GenerateDocumentConsumer: error while publishing RaportFailed for Raport {RaportId}", raportId);
            }
        }

        try
        {
            var dbRaport = await database.Raports
                .Include(x => x.Period)
                .Include(x => x.Status)
                .Include(x => x.MeasurementGroups).ThenInclude(mg => mg.Measurement)
                .Include(x => x.MeasurementGroups).ThenInclude(mg => mg.LocationGroups).ThenInclude(lg => lg.Location)
                .Include(x => x.MeasurementGroups).ThenInclude(mg => mg.LocationGroups).ThenInclude(lg => lg.SampleGroups)
                .FirstOrDefaultAsync(x => x.ID == context.Message.Raport.ID, ct);

            if (dbRaport is null)
            {
                logger.LogWarning("GenerateDocumentConsumer: Raport {RaportId} not found", context.Message.Raport.ID);
                return;
            }

            // Generate PDF document
            var document = QuestPDF.Fluent.Document.Create(doc =>
            {
                doc.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(15, QuestPDF.Infrastructure.Unit.Point);

                    page.Header().Element(c => ComposeHeader(c, dbRaport));

                    page.Footer()
                        .AlignCenter()
                        .Text(text =>
                        {
                            text.CurrentPageNumber();
                            text.Span(" / ");
                            text.TotalPages();
                        });

                    page.Content().Element(c => ComposeContent(c, dbRaport));
                });
            });

            // Generate PDF to memory stream
            using var pdfStream = new MemoryStream();
            document.GeneratePdf(pdfStream);
            pdfStream.Position = 0;

            // Upload to blob storage with raport ID in the name
            var blobName = $"Raport-{dbRaport.ID}-{dbRaport.StartDate:yyyy-MM-dd}-{DateTime.UtcNow:yyyy-MM-dd_HH-mm-ss}.pdf";
            var blobClient = container.GetBlobClient(blobName);

            // Set blob metadata tags for easy identification and filtering
            var blobHttpHeaders = new BlobHttpHeaders
            {
                ContentType = "application/pdf"
            };

            var blobMetadata = new Dictionary<string, string>
            {
                { "RaportID", dbRaport.ID.ToString() },
                { "PeriodID", dbRaport.PeriodID.ToString() },
                { "PeriodName", dbRaport.Period?.Name ?? "Unknown" },
                { "StatusID", dbRaport.StatusID.ToString() },
                { "StatusName", dbRaport.Status?.Name ?? "Unknown" },
                { "StartDate", dbRaport.StartDate.ToString("yyyy-MM-dd") },
                { "EndDate", dbRaport.EndDate.ToString("yyyy-MM-dd") },
                { "RaportCreationDate", dbRaport.RaportCreationDate.ToString("yyyy-MM-dd_HH-mm-ss") },
                { "RaportCompletedDate", dbRaport.RaportCompletedDate.ToString("yyyy-MM-dd_HH-mm-ss") },
                { "DocumentGeneratedDate", DateTime.UtcNow.ToString("yyyy-MM-dd_HH-mm-ss") },
                { "MeasurementGroupsCount", dbRaport.MeasurementGroups.Count.ToString() },
                { "Version", "1.0" }
            };

            await blobClient.UploadAsync(
                pdfStream,
                new BlobUploadOptions
                {
                    HttpHeaders = blobHttpHeaders,
                    Metadata = blobMetadata
                },
                cancellationToken: ct);

            logger.LogInformation("GenerateDocumentConsumer: uploaded PDF '{BlobName}' for Raport {RaportId} with metadata", blobName, dbRaport.ID);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "GenerateDocumentConsumer: error while generating document for Raport {RaportId}", context.Message.Raport.ID);
            await PublishRaportFailedAsync(context.Message.Raport, context.Message.Raport.ID, "Error while generating document: " + ex.Message, ct);
        }
    }

    private void ComposeHeader(IContainer container, Raport raport)
    {
        container.Row(row =>
        {
            row.RelativeItem().Column(column =>
            {
                column.Item().Text("Homee System").AlignLeft().Bold().FontSize(16);
            });

            row.RelativeItem().Column(column =>
            {
                column.Item().Text($"{raport.Period?.Name ?? "Unknown"} Report").AlignCenter().Bold().FontSize(20);
            });

            row.RelativeItem().Column(column =>
            {
                column.Item().Text($"Date: {raport.StartDate:dd.MM.yyyy} - {raport.EndDate:dd.MM.yyyy}").AlignRight();
            });
        });
    }

    private void ComposeContent(IContainer container, Raport raport)
    {
        container.Column(column =>
        {
            column.Spacing(15);

            // Add separator line after header with more space above, less below
            column.Item().PaddingTop(8);
            column.Item().LineHorizontal(1).LineColor("#000000");
            column.Item().PaddingBottom(3);

            // Title section with Raport ID prominently displayed
            column.Item().Row(row =>
            {
                row.RelativeItem().Text($"Report ID: {raport.ID}").FontSize(14).Bold();
                row.RelativeItem().Text($"Generated: {raport.RaportCreationDate:dd.MM.yyyy HH:mm}").FontSize(12).AlignRight();
            });

            column.Item().PaddingVertical(10);

            // FIRST PAGE: Summary of included locations and measurements
            column.Item().Text("Report Summary").FontSize(18).Bold();
            column.Item().PaddingVertical(5);

            // List of measurements included
            column.Item().Text("Measurements Included:").FontSize(14).Bold();
            column.Item().PaddingVertical(3);
            
            foreach (var measurementGroup in raport.MeasurementGroups)
            {
                var measurementName = measurementGroup.Measurement?.Name ?? "Unknown Measurement";
                var unit = measurementGroup.Measurement?.Unit ?? "";
                column.Item().Text($"• {measurementName} ({unit})").FontSize(11);
            }

            column.Item().PaddingVertical(10);

            // List of locations included
            column.Item().Text("Locations Included:").FontSize(14).Bold();
            column.Item().PaddingVertical(3);

            var allLocations = raport.MeasurementGroups
                .SelectMany(mg => mg.LocationGroups)
                .Select(lg => lg.Location?.Name ?? "Unknown")
                .Distinct()
                .OrderBy(name => name)
                .ToList();

            foreach (var locationName in allLocations)
            {
                column.Item().Text($"• {locationName}").FontSize(11);
            }

            column.Item().PaddingVertical(10);

            // Report period information
            column.Item().Text("Report Period:").FontSize(14).Bold();
            column.Item().PaddingVertical(3);
            column.Item().Text($"From: {raport.StartDate:dd.MM.yyyy HH:mm}").FontSize(11);
            column.Item().Text($"To: {raport.EndDate:dd.MM.yyyy HH:mm}").FontSize(11);

            // START FROM SECOND PAGE: Charts and descriptions
            // Iterate through measurement groups
            foreach (var measurementGroup in raport.MeasurementGroups)
            {
                column.Item().PageBreak();

                // Add separator line at top of each page with consistent spacing
                column.Item().PaddingTop(8);
                column.Item().LineHorizontal(1).LineColor("#000000");
                column.Item().PaddingBottom(3);

                var measurementName = measurementGroup.Measurement?.Name ?? "Unknown Measurement";

                column.Item().Text(measurementName).AlignCenter().Bold().FontSize(18);
                column.Item().PaddingVertical(5);

                // Measurement group summary
                if (!string.IsNullOrWhiteSpace(measurementGroup.Summary))
                {
                    column.Item().Text(measurementGroup.Summary).Justify().FontSize(10);
                    column.Item().PaddingVertical(5);
                }

                // Single chart showing all locations for this measurement
                if (measurementGroup.LocationGroups.Any(lg => lg.SampleGroups.Any()))
                {
                    var chartSvg = ComposeMultiLocationChartImage(measurementGroup, raport);
                    if (!string.IsNullOrEmpty(chartSvg))
                    {
                        // Chart with title, labels, and legend all rendered by ScottPlot as SVG
                        column.Item().Svg(chartSvg).FitWidth();
                    }
                }

                column.Item().PaddingVertical(10);

                // Location-specific summaries
                column.Item().Column(summaryColumn =>
                {
                    summaryColumn.Spacing(8);

                    foreach (var locationGroup in measurementGroup.LocationGroups)
                    {
                        var locationName = locationGroup.Location?.Name ?? "Unknown Location";

                        summaryColumn.Item().Row(summaryRow =>
                        {
                            summaryRow.RelativeItem().Column(locColumn =>
                            {
                                locColumn.Item().Text(locationName).Bold().FontSize(12);

                                if (!string.IsNullOrWhiteSpace(locationGroup.Summary))
                                {
                                    locColumn.Item().Text(locationGroup.Summary).FontSize(9);
                                }
                            });
                        });
                    }
                });
            }
        });
    }

    private string ComposeMultiLocationChartImage(MeasurementGroup measurementGroup, Raport raport)
    {
        try
        {
            var measurement = measurementGroup.Measurement;
            if (measurement == null)
            {
                logger.LogWarning("Measurement is null in chart generation");
                return string.Empty;
            }

            var locationGroups = measurementGroup.LocationGroups.Where(lg => lg.SampleGroups.Any()).ToList();
            if (!locationGroups.Any())
            {
                logger.LogWarning("No location groups with samples found");
                return string.Empty;
            }

            logger.LogInformation("Generating chart for {MeasurementName} with {LocationCount} locations",
                measurement.Name, locationGroups.Count);

            logger.LogInformation("Generating TEST chart with multi-language text");

            ScottPlot.Plot myPlot = new();

            myPlot.Font.Automatic(); // set font for each item based on its content

            // Define colors
            var colors = new List<ScottPlot.Color>
            {
                ScottPlot.Colors.Blue, ScottPlot.Colors.Red, ScottPlot.Colors.Green,
                ScottPlot.Colors.Orange, ScottPlot.Colors.Purple, ScottPlot.Colors.Cyan,
                ScottPlot.Colors.Magenta, ScottPlot.Colors.Brown, ScottPlot.Colors.Pink,
                ScottPlot.Colors.Lime, ScottPlot.Colors.Navy, ScottPlot.Colors.Teal,
                ScottPlot.Colors.Maroon
            };

            int colorIndex = 0;

            // Add data series
            foreach (var locationGroup in locationGroups)
            {
                var samples = locationGroup.SampleGroups.OrderBy(s => s.Date).ToList();
                if (!samples.Any()) continue;

                var dates = samples.Select(s => s.Date).ToArray();
                var values = samples.Select(s => s.Value).ToArray();

                var scatter = myPlot.Add.Scatter(dates, values);
                scatter.LegendText = locationGroup.Location?.Name ?? "Unknown";
                scatter.Color = colors[colorIndex % colors.Count];
                scatter.LineWidth = 3;
                scatter.MarkerSize = 8;
                scatter.MarkerShape = ScottPlot.MarkerShape.FilledCircle;

                colorIndex++;
            }

            // Configure axes
            myPlot.Axes.SetLimitsY(measurement.MinChartYValue, measurement.MaxChartYValue);
            myPlot.Axes.SetLimitsX(raport.StartDate.ToOADate(), raport.EndDate.ToOADate());
            myPlot.Axes.DateTimeTicksBottom();

            // Set chart title and labels
            myPlot.Title($"{measurement.Name} ({measurement.Unit})");
            myPlot.XLabel("Time");
            myPlot.YLabel(measurement.Unit);

            // Show legend
            myPlot.ShowLegend();

            // Configure font sizes
            myPlot.Axes.Title.Label.FontSize = 16;
            myPlot.Axes.Bottom.Label.FontSize = 12;
            myPlot.Axes.Left.Label.FontSize = 12;
            myPlot.Axes.Bottom.TickLabelStyle.FontSize = 10;
            myPlot.Axes.Left.TickLabelStyle.FontSize = 10;

            // Generate SVG (vector format - scales perfectly, no font issues)
            var svg = myPlot.GetSvgXml(600, 400);

            logger.LogInformation("Generated TEST chart SVG with {Size} characters", svg.Length);

            return svg;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error generating chart image");
            return string.Empty;
        }
    }
}
