

using DataLayer.Entities;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;

namespace BusinessLogicLayer.Services
{
    public class PDFService
    {
        public byte[] GeneratePDF(List<Order> orders, Employee employee, decimal totalAmount)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                // Initialize PDF writer
                PdfWriter writer = new PdfWriter(stream);
                // Initialize PDF document
                PdfDocument pdf = new PdfDocument(writer);
                // Initialize document
                Document document = new Document(pdf);

                // Add content to the document
                // Header
                document.Add(new Paragraph("Receipt")
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetFontSize(20));
                // Invoice data
                document.Add(new Paragraph($"Employee: {employee.Name}"));

                // Table for invoice items
                Table table = new Table(new float[] { 3, 1, 1, 1, 1 });
                table.SetWidth(UnitValue.CreatePercentValue(90));
                table.AddHeaderCell("DateTime");
                table.AddHeaderCell("Drink");
                table.AddHeaderCell("Quantity");
                table.AddHeaderCell("Price");
                table.AddHeaderCell("Total Amount");

                // Populate table with order data
                foreach (var item in orders)
                {
                    table.AddCell(new Cell().Add(new Paragraph(item.DateTimeOfOrder.Date.ToString())));
                    table.AddCell(new Cell().Add(new Paragraph(item.Drink.Name)));
                    table.AddCell(new Cell().Add(new Paragraph(item.Quantity.ToString()))); // Assuming Quantity is a decimal
                    table.AddCell(new Cell().Add(new Paragraph(item.Drink.Price.ToString("C")))); // Assuming Price is a decimal
                    table.AddCell(new Cell().Add(new Paragraph((item.Quantity * item.Drink.Price).ToString("C"))));
                }

                // Add the Table to the PDF Document
                document.Add(table);

                // Total Amount
                document.Add(new Paragraph($"Total Amount: {totalAmount.ToString("C")}")
                    .SetTextAlignment(TextAlignment.RIGHT));

                // Close the Document
                document.Close();

                return stream.ToArray();
            }
        }

    }
}



