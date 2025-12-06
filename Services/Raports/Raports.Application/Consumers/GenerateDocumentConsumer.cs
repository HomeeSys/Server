using Azure.Storage.Blobs.Models;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Raports.Application.Consumers;

internal class GenerateDocumentConsumer(ILogger<GenerateDocumentConsumer> logger,
                                       IPublishEndpoint publish,
                                       IHubContext<RaportsHub> hub,
                                       BlobContainerClient container,
                                       RaportsDBContext database) : IConsumer<GenerateDocument>
{
    private static readonly TimeZoneInfo PolandTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time");

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

            var documentHash = Guid.NewGuid();

            // Upload to blob storage with raport ID in the name
            var blobName = $"{documentHash}.pdf";
            var blobClient = container.GetBlobClient(blobName);

            // Set blob metadata tags for easy identification and filtering
            var blobHttpHeaders = new BlobHttpHeaders
            {
                ContentType = "application/pdf"
            };

            var blobMetadata = new Dictionary<string, string>
            {
                { "RaportID", dbRaport.ID.ToString() },
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

            //  Update raport status to processing
            var completedStatus = await database.Statuses.FirstOrDefaultAsync(x => x.Name == "Completed");
            if (completedStatus is null)
            {
                //  Handle
                throw new InvalidOperationException();
            }

            var raport = await database.Raports
                .Include(x => x.RequestedLocations)
                .Include(x => x.RequestedMeasurements)
                .Include(x => x.Period)
                .Include(x => x.Status)
                .FirstOrDefaultAsync(x => x.ID == context.Message.Raport.ID);
            if (raport is null)
            {
                //  Handle
                throw new InvalidOperationException();
            }

            raport.StatusID = completedStatus.ID;
            raport.DocumentHash = documentHash;

            var entry = database.Entry(raport);
            database.ChangeTracker.DetectChanges();

            var wasChanged = entry.Properties.Any(p => p.IsModified) || entry.ComplexProperties.Any(c => c.IsModified);

            if (wasChanged)
            {
                var dto = raport.Adapt<DefaultRaportDTO>();

                await database.SaveChangesAsync(ct);
                await hub.Clients.All.SendAsync("RaportStatusChanged", dto, ct);
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "GenerateDocumentConsumer: error while generating document for Raport {RaportId}", context.Message.Raport.ID);
            await PublishRaportFailedAsync(context.Message.Raport, context.Message.Raport.ID, "Error while generating document: " + ex.Message, ct);
        }
    }

    private void ComposeHeader(IContainer container, Raport raport)
    {
        var startDateLocal = TimeZoneInfo.ConvertTimeFromUtc(raport.StartDate, PolandTimeZone);
        var endDateLocal = TimeZoneInfo.ConvertTimeFromUtc(raport.EndDate, PolandTimeZone);

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
                column.Item().Text($"Date: {startDateLocal:dd.MM.yyyy} - {endDateLocal:dd.MM.yyyy}").AlignRight();
            });
        });
    }

    private void ComposeContent(IContainer container, Raport raport)
    {
        var creationDateLocal = TimeZoneInfo.ConvertTimeFromUtc(raport.RaportCreationDate, PolandTimeZone);
        var startDateLocal = TimeZoneInfo.ConvertTimeFromUtc(raport.StartDate, PolandTimeZone);
        var endDateLocal = TimeZoneInfo.ConvertTimeFromUtc(raport.EndDate, PolandTimeZone);

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
                row.RelativeItem().Text($"Generated: {creationDateLocal:dd.MM.yyyy HH:mm}").FontSize(12).AlignRight();
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
            column.Item().Text($"From: {startDateLocal:dd.MM.yyyy HH:mm}").FontSize(11);
            column.Item().Text($"To: {endDateLocal:dd.MM.yyyy HH:mm}").FontSize(11);

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

            ScottPlot.Plot myPlot = new();

            myPlot.Font.Automatic();

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

            // Add data series with local time conversion
            foreach (var locationGroup in locationGroups)
            {
                var samples = locationGroup.SampleGroups.OrderBy(s => s.Date).ToList();
                if (!samples.Any()) continue;

                // Convert UTC dates to Poland local time
                var localDates = samples.Select(s => TimeZoneInfo.ConvertTimeFromUtc(s.Date, PolandTimeZone)).ToArray();
                var values = samples.Select(s => s.Value).ToArray();

                var scatter = myPlot.Add.Scatter(localDates, values);
                scatter.LegendText = locationGroup.Location?.Name ?? "Unknown";
                scatter.Color = colors[colorIndex % colors.Count];
                scatter.LineWidth = 3;
                scatter.MarkerSize = 8;
                scatter.MarkerShape = ScottPlot.MarkerShape.FilledCircle;

                colorIndex++;
            }

            // Convert UTC to local time for chart limits
            var startDateLocal = TimeZoneInfo.ConvertTimeFromUtc(raport.StartDate, PolandTimeZone);
            var endDateLocal = TimeZoneInfo.ConvertTimeFromUtc(raport.EndDate, PolandTimeZone);

            // Configure axes
            myPlot.Axes.SetLimitsY(measurement.MinChartYValue, measurement.MaxChartYValue);
            myPlot.Axes.SetLimitsX(startDateLocal.ToOADate(), endDateLocal.ToOADate());
            myPlot.Axes.DateTimeTicksBottom();

            // Set chart title and labels
            myPlot.Title($"{measurement.Name} ({measurement.Unit})");
            myPlot.XLabel("Time (CET/CEST)");
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

            logger.LogInformation("Generated chart SVG with {Size} characters for {MeasurementName}", svg.Length, measurement.Name);

            return svg;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error generating chart image");
            return string.Empty;
        }
    }
}
