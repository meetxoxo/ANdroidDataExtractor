using System.Collections.Generic;
using System.IO;
using AndroidDataExtractor.Models;
using iTextSharp.text;
using iTextSharp.text.pdf;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using static System.Net.Mime.MediaTypeNames;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Tab;
using System.Drawing;
using System.Xml.Linq;
using System;
using System.Windows.Forms.VisualStyles;

namespace AndroidDataExtractor.Services
{
    public partial class ReportGenerator
    {
        public VisualStyleElement.Tab.Body body { get; private set; }

        public void GeneratePdfReport(List<Contact> contacts, List<Message> messages, List<CallLog> calls, DeviceInfo deviceInfo, string outputPath)
        {
            DocumentFormat.OpenXml.Wordprocessing.Document document = new DocumentFormat.OpenXml.Wordprocessing.Document();
            PdfWriter pdfWriter = PdfWriter.GetInstance(document, new FileStream(outputPath, FileMode.Create));
            object value = document.OpenXmlPart();

            DocumentFormat.OpenXml.Spreadsheet.Font headerFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 14);
            DocumentFormat.OpenXml.Spreadsheet.Font textFont = FontFactory.GetFont(FontFactory.HELVETICA, 12);

            document.AddChild(new DocumentFormat.OpenXml.Drawing.Paragraph("""Android Device Report""", headerFont));
            document.AddChild(new DocumentFormat.OpenXml.Drawing.Paragraph("\nDevice Info:\n", headerFont));
            document.AddChild(new DocumentFormat.OpenXml.Drawing.Paragraph($"CPU Info: {deviceInfo.CPUInfo}", textFont));
            document.AddChild(new DocumentFormat.OpenXml.Drawing.Paragraph($"Memory Info: {deviceInfo.MemoryInfo}\n", textFont));

            document.AddChild(new DocumentFormat.OpenXml.Drawing.Paragraph("Contacts:\n", headerFont));
            foreach (var contact in contacts)
            {
                document.AddChild(new DocumentFormat.OpenXml.Drawing.Paragraph($"Name: {contact.Name}, Phone: {contact.PhoneNumber}, Email: {contact.Email}", textFont));
            }

            document.AddChild(new DocumentFormat.OpenXml.Drawing.Paragraph("\nMessages:\n", headerFont));
            foreach (var msg in messages)
            {
                document.AddChild(new DocumentFormat.OpenXml.Drawing.Paragraph($"From: {msg.Sender}, Content: {msg.MessageContent}, Time: {msg.Timestamp}", textFont));
            }

            document.AddChild(new DocumentFormat.OpenXml.Drawing.Paragraph("\nCall Logs:\n", headerFont));
            foreach (var call in calls)
            {
                document.AddChild(new DocumentFormat.OpenXml.Drawing.Paragraph($"Number: {call.PhoneNumber}, Type: {call.CallType}, Duration: {call.Duration}", textFont));
            }

            object value1 = document.Close();
        }

        public void GenerateWordReport(List<Contact> contacts, List<Message> messages, List<CallLog> calls, DeviceInfo deviceInfo, string outputPath, WordprocessingDocument wordDoc)
        {
            WordprocessingDocument wordprocessingDocument = WordprocessingDocument.Create(outputPath, DocumentFormat.OpenXml.WordprocessingDocumentType.Document);
            MainDocumentPart mainPart = wordDoc.AddMainDocumentPart();
            mainPart.Document = new DocumentFormat.OpenXml.Wordprocessing.Document();
            body = new Body();

            object value = body.Append(new Paragraph(new Run(new Text("Android Device Report"))) { ParagraphProperties = new ParagraphProperties(new Justification() { Val = JustificationValues.Center }) });

            body.Append(new Paragraph(new Run(new Text("\nDevice Info:"))));
            body.Append(new Paragraph(new Run(new Text($"CPU Info: {deviceInfo.CPUInfo}"))));
            body.Append(new Paragraph(new Run(new Text($"Memory Info: {deviceInfo.MemoryInfo}"))));

            body.Append(new Paragraph(new Run(new Text("\nContacts:"))));
            foreach (var contact in contacts)
            {
                body.Append(new Paragraph(new Run(new Text($"Name: {contact.Name}, Phone: {contact.PhoneNumber}, Email: {contact.Email}"))));
            }

            body.Append(new Paragraph(new Run(new Text("\nMessages:"))));
            foreach (var msg in messages)
            {
                body.Append(new Paragraph(new Run(new Text($"From: {msg.Sender}, Content: {msg.MessageContent}, Time: {msg.Timestamp}"))));
            }

            body.Append(new Paragraph(new Run(new Text("\nCall Logs:"))));
            foreach (var call in calls)
            {
                body.Append(new Paragraph(new Run(new Text($"Number: {call.PhoneNumber}, Type: {call.CallType}, Duration: {call.Duration}"))));
            }

            mainPart.Document.Append(body);
            mainPart.Document.Save();
        }

        internal void GeneratePdfReport(List<Contact> contacts, List<System.Windows.Forms.Message> messages, List<CallLog> callLogs, DeviceInfo deviceInfo, string pdfPath)
        {
            throw new NotImplementedException();
        }

        internal void GenerateWordReport(List<Contact> contacts, List<System.Windows.Forms.Message> messages, List<CallLog> callLogs, DeviceInfo deviceInfo, string wordPath)
        {
            throw new NotImplementedException();
        }
    }
}
