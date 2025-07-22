using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using ScottPlot;

namespace Raports.Infrastructure.Generators;

public static class RaportGenerator
{
    public static byte[] Logo = Properties.Resources.HomeeSystemLogo;
    public static Document GenerateRaport(IEnumerable<MeasurementPacket> measurementsPackets, DateTime date)
    {
        QuestPDF.Settings.License = LicenseType.Community;

        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(15, Unit.Point);

                page.Footer().AlignCenter()
                    .Text(text =>
                    {
                        text.CurrentPageNumber();
                        text.Span(" / ");
                        text.TotalPages();
                    });

                page.Header().Element(x => ComposeHeader(x, date));
                page.Content().Element(x => ComposeContent(x, measurementsPackets));
            });
        });

        var currTitle = document.GetMetadata().Title;

        string fileName = $"Raport-{date:yyyy-MM-dd}-Timestamp-{DateTime.Now:yyyy-MM-dd_HH-mm-ss}";

        document.GetMetadata().Title = fileName;

        //document.ShowInCompanion();

        return document;
    }

    private static void ComposeHeader(IContainer container, DateTime date)
    {
        container.Row(row =>
        {
            row.RelativeItem().Column(column =>
            {
                column.Item().Width(64).Height(64).AlignLeft().Image(Logo);
            });

            row.RelativeItem().Column(column =>
            {
                column.Item().Text($"Daily raport").AlignCenter().Bold().FontSize(20);
            });

            row.RelativeItem().Column(column =>
            {
                column.Item().Text($"Date: {date.ToString("dd.MM.yyyy")}").AlignRight();
            });
        });
    }

    private static void ComposeChart(IContainer container, MeasurementPacket packet)
    {
        container.Svg(size =>
        {
            ScottPlot.Plot myPlot = new();

            foreach (var data in packet.Measurements)
            {
                var scatter = myPlot.Add.Scatter(packet.Time, data.Data);
                scatter.LegendText = data.Name;
            }

            myPlot.YLabel(packet.MeasurementName, 12);
            myPlot.XLabel("Time", 12);

            //myPlot.Axes.SetLimitsY(minY, maxY);
            //myPlot.Axes.SetLimitsY(packet.MinY, packet.MaxY);

            myPlot.Axes.Bottom.TickLabelStyle.Rotation = -45;
            myPlot.Axes.Bottom.TickLabelStyle.Alignment = Alignment.MiddleRight;

            // create a manual DateTime tick generator and add ticks
            ScottPlot.TickGenerators.DateTimeManual ticks = new();

            // add ticks for Mondays only
            for (int i = 0; i < packet.Time.Length; i++)
            {
                if (i == (packet.Time.Length - 1) || i % 4 == 0)
                {
                    DateTime date = packet.Time[i];
                    string label = $"{date:HH}:{date:ss}";
                    ticks.AddMajor(date, label);
                }
            }

            // tell the horizontal axis to use the custom tick generator
            myPlot.Axes.Bottom.TickGenerator = ticks;

            myPlot.ShowLegend();

            //myPlot.SavePng("demo.png", 100, 100);

            return myPlot.GetSvgXml((int)size.Width, (int)size.Height);
        });
    }

    private static void ComposeContent(IContainer container, IEnumerable<MeasurementPacket> packets)
    {
        container.MultiColumn(multiColumn =>
        {
            multiColumn.Columns(2);
            multiColumn.Spacing(15);

            multiColumn.Content()
                .Column(column =>
                {
                    column.Spacing(15);

                    foreach (var packet in packets)
                    {
                        column.Item().Text(packet.MeasurementName).AlignCenter().Bold().FontSize(16);

                        column.Item().Text(packet.Description).Justify();

                        column.Item().AspectRatio(1).Element(x => ComposeChart(x, packet));

                        column.Item().LineHorizontal(1);
                    }
                });
        });
    }
}
