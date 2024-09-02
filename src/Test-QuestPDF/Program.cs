using QuestPDF;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using QuestPDF.Previewer;
using Test_QuestPDF.Templates;

namespace Test_QuestPDF;

class Program
{
    static void Main(string[] args)
    {
        Settings.License = LicenseType.Community;
        
        //var model = InvoiceDocumentDataSource.GetInvoiceDetails();
        //var document = new InvoiceDocument(model);
        
        var document = new BillDocument();
        
        document.GeneratePdfAndShow(); 
        //document.ShowInPreviewerAsync(12345);
        
        Console.WriteLine("Hello, World!");

        var s = Console.ReadLine();
    }
}