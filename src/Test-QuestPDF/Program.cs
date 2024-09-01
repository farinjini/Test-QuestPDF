using QuestPDF;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using Test_QuestPDF.Data;
using Test_QuestPDF.Templates;

namespace Test_QuestPDF;

class Program
{
    public Program()
    {
        //QuestPDF.Settings.License = LicenseType.Community;
    }
    
    static void Main(string[] args)
    {
        Settings.License = LicenseType.Community;
        
        var model = InvoiceDocumentDataSource.GetInvoiceDetails();
        var document = new InvoiceDocument(model);
        
        document.GeneratePdfAndShow();
        
        Console.WriteLine("Hello, World!");
    }
}