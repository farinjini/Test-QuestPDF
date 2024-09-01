using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using Test_QuestPDF.Models;

namespace Test_QuestPDF.Templates;

public class BillDocument : IDocument
{
    public Bills Bills { get; }

    public BillDocument(Bills bills)
    {
        Bills = bills;
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
            page.DefaultTextStyle(TextStyle.Default.FontSize(14));
            page.PageColor(Colors.White);
            
            //page.Header().Element(ComposeHeader);
            page.Content().Element(ComposeContent);
            page.Footer().Element(ComposeFooter);
        });
    }
    
    private void ComposeContent(IContainer container)
    {
        container.Layers(layer =>
        {
            layer.Layer().Element(ComposeBackgroundLayer);
            layer.PrimaryLayer().Element(ComposePrimaryLayer);
        });
    }
    
    private void ComposeBackgroundLayer(IContainer container)
    {
        throw new NotImplementedException();
    }

    private void ComposePrimaryLayer(IContainer container)
    {
        throw new NotImplementedException();
    }
    
    private void ComposeFooter(IContainer container)
    {
        container.AlignCenter().Text(x =>
        {
            x.Span($"Page {x.CurrentPageNumber()} of {x.TotalPages()} ");
        });
    }
}