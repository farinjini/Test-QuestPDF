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
        /*container
            .Padding(25)
            .Column(column =>
            {
                column.Spacing(5);

                foreach (var _ in Enumerable.Range(0, 40))
                    column.Item().Text(Placeholders.Sentence());
            });*/
        container
            .Width(PageSizes.A4.Width)
            .Column(col =>
            {
                foreach (var _ in Enumerable.Range(0, 5))
                {
                    //Top Section
                    col.Item()
                        .Height(PageSizes.A4.Height - 190)
                        .Width(PageSizes.A4.Width)
                        .Row(row =>
                        {
                            //Top Section - Left Side
                            row.RelativeItem()
                                .Padding(20)
                                .PaddingTop(80)
                                .Column(col =>
                                {
                                    //This is where the main content should go
                                    foreach (var _ in Enumerable.Range(0, 20))
                                        col.Item().Text(Placeholders.Sentence());
                                });

                            //Top Section - Right Side
                            row.ConstantItem(190)
                                .Column(col =>
                                {
                                    col.Item()
                                        .Width(190)
                                        .Height(200)
                                        .Column(col =>
                                        {
                                            //This is where the sidebar contact details should go
                                        });

                                    col.Item()
                                        .Width(190)
                                        .Column(col =>
                                        {
                                            //This is there the billing details should go
                                        });
                                });
                        });

                    //Bottom Section
                    /*col.Item()
                        .Height(190)
                        .Width(PageSizes.A4.Width)
                        .BorderTop(5)
                        .BorderColor(Colors.Red.Lighten1)
                        .Background(Colors.Grey.Darken3)
                        .Text("Bottom");*/
                }
                
            });
    }
    
    private void ComposeBackgroundLayer(IContainer container)
    {
        container
            .Height(PageSizes.A4.Height)
            .Width(PageSizes.A4.Width)
            .Column(col =>
            {
                //Top Section
                col.Item()
                    .Height(PageSizes.A4.Height - 190)
                    .Width(PageSizes.A4.Width)
                    .Row(row =>
                    {
                        //Top Section - Left Side
                        row.RelativeItem()
                            .Column(col =>
                            {
                                //KEDCO Logo
                                col.Item()
                                    .Height(76)
                                    .Image(LogoImage)
                                    .FitArea()
                                    .WithCompressionQuality(ImageCompressionQuality.Medium);
                            });
                        
                        //Top Section - Right Side
                        row.ConstantItem(190)
                            .Background(Colors.Grey.Lighten2)
                            .Column(col =>
                            {
                                col.Item()
                                    .Width(190)
                                    .Height(200)
                                    .Column(col =>
                                    {
                                        //Contact Us
                                        col.Item().Background(Colors.Grey.Darken1)
                                            .Padding(5)
                                            .Text("Contact Us").FontColor(Colors.White);
                                        
                                        //Contact Details
                                        col.Item().Text(text =>
                                        {
                                            text.Span("Contact Details").FontColor(Colors.White);
                                        });
                                    });
                                
                                col.Item()
                                    .Width(190)
                                    .Column(col =>
                                    {
                                        //Billing Details
                                        col.Item().Background(Colors.Grey.Darken1)
                                            .Padding(5)
                                            .Text("Bill Overview").FontColor(Colors.White);
                                    });
                                    
                            });
                    });
                
                //Bottom Section
                col.Item()
                    .Height(190)
                    .Width(PageSizes.A4.Width)
                    .BorderTop(5)
                    .BorderColor(Colors.Red.Lighten1)
                    .Background(Colors.Grey.Darken3).Text("Bottom");
            });
    }
    
    private void ComposeFooter(IContainer container)
    {
        container
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