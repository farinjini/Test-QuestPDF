using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using Test_QuestPDF.Models;

namespace Test_QuestPDF.Templates;

public class BillDocument : IDocument
{
    public Bills Bills { get; }
    
    private static Image LogoImage { get; } = Image.FromFile("Images/logo.png");
    
    readonly TextStyle _titleStyle = TextStyle.Default.FontSize(13).SemiBold();

    readonly TextStyle _titleStyle2 = TextStyle.Default.FontSize(10);

    readonly TextStyle _titleStyle3 = TextStyle.Default.FontSize(12).SemiBold();

    readonly TextStyle _titleStyle4 = TextStyle.Default.FontSize(11);

    readonly TextStyle _titleStyle5 = TextStyle.Default.FontSize(8);

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
            Title = "KEDCO Bill",
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
                foreach (var _ in Enumerable.Range(0, 1))
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
                                    /*foreach (var _ in Enumerable.Range(0, 20))
                                        col.Item().Text(Placeholders.Sentence());*/
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
                                //Logo Region, Barcode and Customer Info
                                col.Item()
                                    .Height(200.5f)
                                    .Column(col =>
                                    {
                                        //Logo, Region and Barcode
                                        col.Item().Row(row =>
                                        {
                                            //Logo
                                            row.ConstantItem(190)
                                                .Image(LogoImage)
                                                .FitWidth()
                                                .WithCompressionQuality(ImageCompressionQuality.Medium);
                                            
                                            //Region and Barcode
                                            row.RelativeItem()
                                                .Background(Colors.Amber.Lighten3)
                                                .Column(col =>
                                                {
                                                    //Region
                                                    col.Item().Height(80).Text("Region").FontColor(Colors.Black).Style(_titleStyle);
                                                    
                                                    //Barcode
                                                    col.Item().Text("Barcode").FontColor(Colors.Black).Style(_titleStyle);
                                                });

                                        });
                                        
                                        //Customer Info
                                        col.Item()
                                            .Background(Colors.Blue.Lighten3)
                                            .ExtendVertical()
                                            .Row(row =>
                                            {
                                                row.RelativeItem().Text("Customer Info");
                                            });
                                    });
                                
                                //Bill Details
                                col.Item()
                                    //.Background(Colors.Green.Lighten3)
                                    .DefaultTextStyle(dts => dts.FontSize(7.5f))
                                    .Column(col =>
                                    {
                                        //Meter Account Details
                                        col
                                            .Item()
                                            .Table(table =>
                                            {
                                                IContainer DefaultCellStyle(IContainer container, string backgroundColor)
                                                {
                                                    return container
                                                        .PaddingVertical(5)
                                                        .AlignLeft()
                                                        .AlignMiddle();
                                                }

                                                table.ColumnsDefinition(columns =>
                                                {
                                                    columns.RelativeColumn();
                                                    columns.RelativeColumn();
                                                    columns.ConstantColumn(70);
                                                    columns.ConstantColumn(70);
                                                    columns.RelativeColumn();
                                                    columns.RelativeColumn();
                                                });

                                                table.Header(header =>
                                                {
                                                    header.Cell().ColumnSpan(6).BorderBottom(1).BorderTop(1).Element(CellStyle)
                                                    .ExtendHorizontal().AlignLeft().PaddingLeft(15).Text("Meter and Account Details").FontSize(10).Bold();

                                                    IContainer CellStyle(IContainer container) => DefaultCellStyle(container, Colors.Grey.Lighten3);
                                                });


                                                //Row 1
                                                table.Cell().ColumnSpan(2).Element(CellStyle).PaddingLeft(15).Text(text =>
                                                {
                                                    text.Span("Meter Number:").Bold();
                                                    text.Span($"");
                                                });
                                                table.Cell().Element(CellStyle).PaddingLeft(5).Text(text =>
                                                {
                                                    text.Span("Dials:").Bold();
                                                    text.Span($"");
                                                });
                                                table.Cell().Element(CellStyle).PaddingLeft(5).Text(text =>
                                                {
                                                    text.Span("Multiplier:").Bold();
                                                    text.Span($"");
                                                });
                                                table.Cell().ColumnSpan(2).Element(CellStyle).Text(text =>
                                                {
                                                    text.Span("Meter Read Date:").Bold();
                                                    text.Span("");
                                                });

                                                //Row 2
                                                table.Cell().ColumnSpan(2).Element(CellStyle).PaddingLeft(15).Text(text =>
                                                {
                                                    text.Span("Last Actual Reading: ").Bold();
                                                    text.Span($"");
                                                });
                                                table.Cell().ColumnSpan(2).Element(CellStyle).PaddingLeft(5).Text(text =>
                                                {
                                                    text.Span("LAR Date: ").Bold();
                                                    text.Span("");
                                                });
                                                table.Cell().ColumnSpan(2).Element(CellStyle).Text(text =>
                                                {
                                                    text.Span("ADC: ").Bold();
                                                    text.Span("");
                                                });

                                                //Row 3
                                                table.Cell().ColumnSpan(2).Element(CellStyle).PaddingLeft(15).Text(text =>
                                                {
                                                    text.Span("Tariff: ").Bold();
                                                    text.Span($"");
                                                });
                                                table.Cell().ColumnSpan(2).Element(CellStyle).PaddingLeft(5).Text(text =>
                                                {
                                                    text.Span("Rate: ").Bold();
                                                    text.Span($"");
                                                });
                                                table.Cell().ColumnSpan(2).Element(CellStyle).Text(text =>
                                                {
                                                    text.Span("Old Acc: ").Bold();
                                                    text.Span("");
                                                });

                                                //Row 4
                                                table.Cell().ColumnSpan(2).Element(CellStyle).PaddingLeft(15).Text(text =>
                                                {
                                                    text.Span("Feeder: ").Bold();
                                                    text.Span($"");
                                                });

                                                table.Cell().ColumnSpan(2).Element(CellStyle).PaddingLeft(5).Text(text =>
                                                {
                                                    text.Span("DT: ").Bold();
                                                    text.Span($"");
                                                });

                                                table.Cell().ColumnSpan(2).Element(CellStyle).PaddingLeft(0).Text(text =>
                                                {
                                                    text.Span("Band: ").Bold();
                                                    text.Span($"");
                                                });


                                                IContainer CellStyle(IContainer container) => DefaultCellStyle(container, Colors.White).ShowOnce();

                                            });
                                        
                                        //Details of Charges
                                        col
                                            .Item()
                                            .Table(table =>
                                            {
                                                IContainer DefaultCellStyle(IContainer container, string backgroundColor)
                                                {
                                                    return container
                                                        .PaddingVertical(4)
                                                        .AlignLeft()
                                                        .AlignMiddle();
                                                }

                                                table.ColumnsDefinition(columns =>
                                                {
                                                    columns.RelativeColumn();
                                                    columns.RelativeColumn();
                                                    columns.RelativeColumn();
                                                    columns.RelativeColumn();
                                                    columns.RelativeColumn();
                                                });

                                                table.Header(header =>
                                                {
                                                    header.Cell().ColumnSpan(5).BorderBottom(1).BorderTop(1).Element(CellStyle)
                                                    .ExtendHorizontal().AlignLeft().PaddingLeft(15).Text($"Details of Charges : This bill is based on ...").FontSize(10).Bold();

                                                    IContainer CellStyle(IContainer container) => DefaultCellStyle(container, Colors.Grey.Lighten3);
                                                });

                                                //Row 1
                                                table.Cell().ColumnSpan(5).Element(CellStyle).PaddingLeft(15).Text("Energy Usage Calculation:").FontSize(8).Bold();

                                                //Row 2
                                                table.Cell().Element(CellStyle).PaddingLeft(15).Text("Service Period").FontSize(8);
                                                table.Cell().Element(CellStyle).Text("Number of Days").FontSize(8);
                                                table.Cell().Element(CellStyle).Text("Current Reading").FontSize(8);
                                                table.Cell().Element(CellStyle).Text("Previous Reading").FontSize(8);
                                                table.Cell().Element(CellStyle).Text("Total Usage").FontSize(8);

                                                //Row 3
                                                table.Cell().BorderBottom(1).BorderTop(1).BorderColor(Colors.Grey.Darken1).Element(CellStyle).PaddingLeft(15).Text("").FontSize(8);
                                                table.Cell().BorderBottom(1).BorderTop(1).BorderColor(Colors.Grey.Darken1).Element(CellStyle).Text("").FontSize(8);
                                                table.Cell().BorderBottom(1).BorderTop(1).BorderColor(Colors.Grey.Darken1).Element(CellStyle).Text($"").FontSize(8);
                                                table.Cell().BorderBottom(1).BorderTop(1).BorderColor(Colors.Grey.Darken1).Element(CellStyle).Text($"").FontSize(8);
                                                table.Cell().BorderBottom(1).BorderTop(1).BorderColor(Colors.Grey.Darken1).Element(CellStyle).Text($"").FontSize(8);


                                                IContainer CellStyle(IContainer container) => DefaultCellStyle(container, Colors.White).ShowOnce();
                                               

                                            });
                                        
                                        //Payments and charges calculation
                                        col
                                            .Item()
                                            .Table(table =>
                                            {
                                                IContainer DefaultCellStyle(IContainer container, string backgroundColor)
                                                {
                                                    return container
                                                        .PaddingVertical(4)
                                                        .PaddingBottom(30)
                                                        .AlignLeft()
                                                        .AlignMiddle();
                                                }

                                                table.ColumnsDefinition(columns =>
                                                {
                                                    columns.RelativeColumn();
                                                    columns.RelativeColumn();
                                                    columns.RelativeColumn();
                                                });

                                                //Row 1
                                                table.Cell().Element(CellStyle).PaddingLeft(15).Text("Payments and Charges Calculation:").FontSize(7).Bold();
                                                table.Cell().Element(CellStyle).Text($"Last Pay Date: ").FontSize(7);
                                                table.Cell().Element(CellStyle).Text($"Last Pay Amount : ₦").FontSize(7);


                                                IContainer CellStyle(IContainer container) => DefaultCellStyle(container, Colors.White).ShowOnce();


                                            });

                                        //Before this Bill
                                        col
                                            .Item()
                                            .Table(table =>
                                            {
                                                IContainer DefaultCellStyle(IContainer container, string backgroundColor)
                                                {
                                                    return container
                                                        .PaddingVertical(4)
                                                        .AlignLeft()
                                                        .AlignMiddle();
                                                }

                                                table.ColumnsDefinition(columns =>
                                                {
                                                    columns.RelativeColumn();
                                                    columns.RelativeColumn();
                                                    columns.RelativeColumn();
                                                });

                                                table.Header(header =>
                                                {
                                                    header.Cell().ColumnSpan(3).BorderBottom(1).BorderTop(1).Element(CellStyle)
                                                        .ExtendHorizontal().AlignLeft().PaddingLeft(15).Text("Last Bill Summary").FontSize(10).Bold();

                                                    IContainer CellStyle(IContainer container) => DefaultCellStyle(container, Colors.Grey.Lighten3);
                                                });

                                                /*
                                                //Row 1
                                                table.Cell().Element(CellStyle).PaddingLeft(15).Text("Last Bill Period:");
                                                table.Cell().ColumnSpan(2).Element(CellStyle).Text($"{_model.LastBillPeriod}");

                                                //Row 1
                                                table.Cell().Element(CellStyle).PaddingLeft(15).Text("Opening Balance:");
                                                //If there's a credit on previous balance
                                                if(_model.LastBillOpeningBalance < 0)
                                                {
                                                    table.Cell().ColumnSpan(2).Element(CellStyle).Text($"₦[{_model.LastBillOpeningBalance:n2}]");
                                                }
                                                else
                                                {
                                                    table.Cell().ColumnSpan(2).Element(CellStyle).Text($"₦{_model.LastBillOpeningBalance:n2}");
                                                }

                                                //Row 1
                                                table.Cell().Element(CellStyle).PaddingLeft(15).Text("Billed Amount:");
                                                table.Cell().ColumnSpan(2).Element(CellStyle).Text($"₦{_model.LastBillAmount:n2}");

                                                //Row 1
                                                table.Cell().Element(CellStyle).PaddingLeft(15).Text("Adjustment:");
                                                table.Cell().ColumnSpan(2).Element(CellStyle).Text($"₦{_model.LastBillAdjustments:n2}");
                                                */
                                                
                                                //Row 1
                                                table.Cell().Element(CellStyle).PaddingLeft(15).Text("Closing Balance:");
                                                table.Cell().ColumnSpan(2).Element(CellStyle).Text($"₦");


                                                //Row 1
                                                table.Cell().Element(CellStyle).PaddingLeft(15).Text("Payments:");
                                                table.Cell().ColumnSpan(2).Element(CellStyle).Text($"₦");

                                                //Row 1
                                                table.Cell().Element(CellStyle).PaddingLeft(15).Text("Adjustments:");
                                                table.Cell().ColumnSpan(2).Element(CellStyle).Text($"₦");


                                                IContainer CellStyle(IContainer container) => DefaultCellStyle(container, Colors.White).ShowOnce();


                                            });


                                        //This bill charges
                                        col
                                            .Item()
                                            .Table(table =>
                                            {
                                                IContainer DefaultCellStyle(IContainer container, string backgroundColor)
                                                {
                                                    return container
                                                        .PaddingVertical(4)
                                                        .AlignLeft()
                                                        .AlignMiddle();
                                                }

                                                table.ColumnsDefinition(columns =>
                                                {
                                                    columns.RelativeColumn();
                                                    columns.RelativeColumn();
                                                    columns.RelativeColumn();
                                                });

                                                table.Header(header =>
                                                {
                                                    header.Cell().ColumnSpan(3).BorderBottom(1).BorderTop(1).Element(CellStyle)
                                                        .ExtendHorizontal().AlignLeft().PaddingLeft(15).Text("This Bill's Charges").FontSize(10).Bold();

                                                    IContainer CellStyle(IContainer container) => DefaultCellStyle(container, Colors.Grey.Lighten3);
                                                });

                                                //Row 1
                                                table.Cell().Element(CellStyle).PaddingLeft(15).Text("Energy Charge:");
                                                table.Cell().ColumnSpan(2).Element(CellStyle).Text($"₦/kWH x kWH = ₦");

                                                //Row 2
                                                table.Cell().Element(CellStyle).PaddingLeft(15).Text("Fixed Charge:");
                                                table.Cell().ColumnSpan(2).Element(CellStyle).Text("₦0");

                                                //Row 3
                                                table.Cell().Element(CellStyle).PaddingLeft(15).Text("Value Added Tax (VAT @7.5%):");
                                                table.Cell().ColumnSpan(2).Element(CellStyle).Text($"0.075 x ₦ = ₦");

                                                //Row 4
                                                table.Cell().Element(CellStyle).PaddingLeft(15).Text("Charges this month:");
                                                table.Cell().ColumnSpan(2).Element(CellStyle).Text($"₦ + ₦ = ₦");

                                                //Row 5
                                                table.Cell().Element(CellStyle).PaddingLeft(15).Text("Total Due:");
                                                table.Cell().ColumnSpan(2).Element(CellStyle).Text($"₦  + ₦ = ₦");


                                                IContainer CellStyle(IContainer container) => DefaultCellStyle(container, Colors.White).ShowOnce();


                                            });
                                    });

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
                                            .PaddingHorizontal(3)
                                            .Text("Contact Us").FontColor(Colors.White).Style(_titleStyle4);
                                    });
                                
                                col.Item()
                                    .Width(190)
                                    .Column(col =>
                                    {
                                        //Billing Details
                                        col.Item().Background(Colors.Grey.Darken1)
                                            .Padding(5)
                                            .PaddingHorizontal(3)
                                            .Text("Bill Overview").FontColor(Colors.White).Style(_titleStyle4);
                                    });
                                    
                            });
                    });
                
                //Bottom Section
                col.Item()
                    .Height(190)
                    .Width(PageSizes.A4.Width)
                    .BorderTop(5)
                    .BorderColor(Colors.Red.Lighten1)
                    //.Background(Colors.Grey.Darken3)
                    .Column(col =>
                    {
                        col
                            .Item()
                            .PaddingHorizontal(15)
                            .PaddingVertical(10)
                            .Text($"Region").Bold();

                        col
                            .Item()
                            .DefaultTextStyle(dts =>
                                dts.FontSize(8f)
                            )
                            .PaddingHorizontal(15)
                            .Table(table =>
                            {
                                IContainer DefaultCellStyle(IContainer container, string backgroundColor)
                                {
                                    return container
                                        .PaddingVertical(2)
                                        .AlignLeft()
                                        .AlignMiddle();
                                }

                                table.ColumnsDefinition(columns =>
                                {
                                    columns.ConstantColumn(60);
                                    columns.RelativeColumn();
                                    columns.ConstantColumn(130);
                                    columns.RelativeColumn();
                                });

                                //Row 1
                                table.Cell().Element(CellStyle).Text("Bill Month:").Bold();
                                table.Cell().Element(CellStyle).Text($"");
                                table.Cell().Element(CellStyle).Text("Net Arrears:").Bold();
                                table.Cell().Element(CellStyle).Text($"₦");
                                

                                //Row 2
                                table.Cell().Element(CellStyle).Text("Acc:").Bold();
                                table.Cell().Element(CellStyle).Text($"").Bold();
                                table.Cell().Element(CellStyle).Text("Current Charges").Underline().Bold();
                                table.Cell().Element(CellStyle).Text("");

                                //Row 3
                                table.Cell().Element(CellStyle).Text("Meter Number:").Bold();
                                table.Cell().Element(CellStyle).Text($"");
                                table.Cell().Element(CellStyle).Text("Consumption:").Bold();
                                table.Cell().Element(CellStyle).Text($"kWH");

                                //Row 4
                                table.Cell().Element(CellStyle).Text("Tariff:").Bold();
                                table.Cell().Element(CellStyle).Text($"");
                                table.Cell().Element(CellStyle).Text("Energy Charge:").Bold();
                                table.Cell().Element(CellStyle).Text($"₦");

                                //Row 5
                                table.Cell().Element(CellStyle).Text("Customer:").Bold();
                                table.Cell().Element(CellStyle).Text($"").ClampLines(1);
                                table.Cell().Element(CellStyle).Text("Fixed Charge:").Bold();
                                table.Cell().Element(CellStyle).Text("₦0");

                                //Row 6
                                table.Cell().Element(CellStyle).Text("Address:").Bold();
                                table.Cell().Element(CellStyle).Text($"").ClampLines(1);
                                table.Cell().Element(CellStyle).Text("Value Added Tax (VAT @7.5%):").Bold();
                                table.Cell().Element(CellStyle).Text($"₦");

                                //Row 7
                                table.Cell().Element(CellStyle).Text("Due Date:").Bold();
                                table.Cell().Element(CellStyle).Text($"");
                                table.Cell().Element(CellStyle).Text("Charges This Month:").Bold();
                                table.Cell().Element(CellStyle).Text($"₦");

                                //Row 8
                                table.Cell().Element(CellStyle).Text("Band: ");
                                table.Cell().Element(CellStyle).Text($"");
                                table.Cell().Element(CellStyle).Text("Total Due:").Bold();
                                //For credit on total due
                                table.Cell().Element(CellStyle).Text($"₦").Bold();
                                
                                IContainer CellStyle(IContainer container) => DefaultCellStyle(container, Colors.White).ShowOnce();
                                
                            });
                    });
            });
    }
    
    private void ComposeFooter(IContainer container)
    {
        container
            //.Background(Colors.Blue.Lighten3)
            .DefaultTextStyle(x => x.FontSize(10))
            .PaddingHorizontal(10)
            .PaddingVertical(3)
            .AlignLeft()
            .Text(x =>
                {
                    x.Span("Page ");
                    x.CurrentPageNumber();
                    x.Span(" of ");
                    x.TotalPages();
                });
    }
}