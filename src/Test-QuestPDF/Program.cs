using QuestPDF;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using Test_QuestPDF.Data;
using Test_QuestPDF.Models;
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

        var data = new List<InvoiceModel>();
        
        data.Add(InvoiceDocumentDataSource.GetInvoiceDetails());
        data.Add(InvoiceDocumentDataSource.GetInvoiceDetails());
        data.Add(InvoiceDocumentDataSource.GetInvoiceDetails());
        
        var model = InvoiceDocumentDataSource.GetInvoiceDetails();
        var document = new InvoiceDocument(data);
        
        document.GeneratePdfAndShow();
        
        Console.WriteLine("Hello, World!");
    }
}