using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using Test_QuestPDF.Models;

namespace Test_QuestPDF.Templates;

public class BillDocument : IDocument
{
    public Bills Bills { get; }
    
    private static Image LogoImage { get; } = Image.FromFile("Images/logo.png");

    public BillDocument(Bills bills)
    {
        Bills = bills;
    }

    public BillDocument()
    {
        
    }
    
    public DocumentMetadata GetMetadata()
    {
        return new DocumentMetadata
        {
            Title = "KEDCO Invoice",
            Author = "Deckstream",
            Keywords = "Bill, Customer, KEDCO Bill",
            Producer = "Venn Technology Limited",
            Creator = "Deckstream",
            Subject = "Deckstream Bill",
            CreationDate = DateTimeOffset.Now
        };
    }
    
    public DocumentSettings GetSettings()
    {
        return new DocumentSettings
        {
            CompressDocument = true,
            ContentDirection = ContentDirection.LeftToRight,
            PdfA = true,
            ImageCompressionQuality = ImageCompressionQuality.High,
            ImageRasterDpi = 300
        };
    }
    
    public void Compose(IDocumentContainer container)
    {
        container.Page(page =>
        {
            //Common
            page.Margin(0);
            page.Size(PageSizes.A4);
            page.DefaultTextStyle(x => x.FontSize(14).FontFamily("Microsoft Sans Serif"));
            page.PageColor(Colors.White);
            
            //page.Header().Element(ComposeHeader);
            page.Content().Element(ComposeContent);
            page.Footer().Element(ComposeFooter);
        });
    }
    
    private void ComposeContent(IContainer container)
    {
        /*container.Layers(layer =>
        {
            layer.Layer().Element(ComposeBackgroundLayer);
            layer.PrimaryLayer().Element(ComposePrimaryLayer);
        });*/
        container
            .Layers(layers =>
            {
                // layer below main content
                layers
                    .Layer()
                    .Element(ComposeBackgroundLayer);

                layers
                    .PrimaryLayer()
                    .Padding(25)
                    .Column(column =>
                    {
                        column.Spacing(5);

                        foreach (var _ in Enumerable.Range(0, 7))
                            column.Item().Text(Placeholders.Sentence());
                    });

                // layer above the main content    
                layers
                    .Layer()
                    .AlignCenter()
                    .AlignMiddle()
                    .Text("Watermark")
                    .FontSize(48).Bold().FontColor(Colors.Green.Lighten3);

                /*layers
                    .Layer()
                    .AlignBottom()
                    .PageNumber("Page {number}")
                    .FontSize(16).FontColor(Colors.Green.Medium)*/;
            });
    }

    private void Testlayer(IContainer container)
    {
        container.Height(100)
            .Width(100)
            .Background(Colors.Grey.Lighten3);
    }
    
    private void ComposeBackgroundLayer(IContainer container)
    {
        container
            .Height(PageSizes.A4.Height - 50)
            .Width(PageSizes.A4.Width)
            .Background(Colors.Grey.Lighten3);
            /*.Column(col =>
            {
                col.Item()
                    .Height(PageSizes.A4.Height - 200)
                    .Width(PageSizes.A4.Width).Text("Top");
                col.Item()
                    .Height(200)
                    .Width(PageSizes.A4.Width)
                    .Background(Colors.Grey.Darken3).Text("Bottom");
            });*/
    }

    private void ComposePrimaryLayer(IContainer container)
    {
        container.Text("Hi There");
    }
    
    private void ComposeFooter(IContainer container)
    {
        container
            //.Height(10)
            .Background(Colors.Blue.Lighten3)
            .DefaultTextStyle(x => x.FontSize(10))
            .AlignCenter()
            .Text(x =>
                {
                    x.Span("Page ");
                    x.CurrentPageNumber();
                    x.Span(" of ");
                    x.TotalPages();
                });
        
        /*container.AlignCenter().Text(x =>
        {
            x.CurrentPageNumber();
            x.Span(" / ");
            x.TotalPages();
        });*/
    }
}