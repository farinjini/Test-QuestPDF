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
            
            page.Background().Element(ComposeBackgroundLayer);
            //page.Header().Element(ComposeHeader);
            page.Content().Element(ComposeContent);
            page.Footer().Element(ComposeFooter);
        });
    }

    private void ComposeHeader(IContainer container)
    {
        container.Text("This is the header");
    }

    private void ComposeContent(IContainer container)
    {
        container
            .Padding(25)
            .Column(column =>
            {
                column.Spacing(5);

                foreach (var _ in Enumerable.Range(0, 40))
                    column.Item().Text(Placeholders.Sentence());
            });
    }
    
    private void ComposeBackgroundLayer(IContainer container)
    {
        container
            .Height(PageSizes.A4.Height)
            .Width(PageSizes.A4.Width)
            .Background(Colors.Grey.Lighten3)
            .Column(col =>
            {
                col.Item()
                    .Height(PageSizes.A4.Height - 200)
                    .Width(PageSizes.A4.Width)
                    .Row(row =>
                    {
                        row.RelativeItem()
                            .Text("One Side");
                        row.ConstantItem(200)
                            .Text("The other side");
                    });
                col.Item()
                    .Height(200)
                    .Width(PageSizes.A4.Width)
                    .Background(Colors.Grey.Darken3).Text("Bottom");
            });
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
    }
}