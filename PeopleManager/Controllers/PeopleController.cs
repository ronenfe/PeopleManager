using iText.IO.Font;
using iText.Kernel.Colors;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using PeopleManager.Helpers;
using PeopleManager.Models;
using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static iText.Kernel.Font.PdfFontFactory;

namespace PeopleManager.Controllers
{
    public class PeopleController : Controller
    {
        private readonly PeopleDbContext db = new PeopleDbContext();

        public ActionResult Index(string q)
        {
            var people = db.People.AsQueryable();
            if (!string.IsNullOrEmpty(q))
            {
                people = people.Where(p => p.FullName.Contains(q));
            }
            return View(people.OrderBy(p => p.FullName).ToList());
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Person model, HttpPostedFileBase photo)
        {
            if (ModelState.IsValid)
            {
                if (photo != null && photo.ContentLength > 0)
                {
                    var uploads = Server.MapPath("~/Uploads");
                    if (!Directory.Exists(uploads)) Directory.CreateDirectory(uploads);
                    var fileName = Guid.NewGuid() + Path.GetExtension(photo.FileName);
                    var path = Path.Combine(uploads, fileName);
                    photo.SaveAs(path);
                    model.PhotoFileName = fileName;

                    // also store bytes in DB for easier PDF embedding
                    using (var ms = new MemoryStream())
                    {
                        photo.InputStream.CopyTo(ms);
                        model.PhotoData = ms.ToArray();
                    }
                    model.PhotoContentType = photo.ContentType;
                }
                db.People.Add(model);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }

        public ActionResult ExportPdf()
        {
            var people = db.People.OrderBy(p => p.FullName).ToList();

            using (var ms = new MemoryStream())
            {
                PdfWriter writer = new PdfWriter(ms);
                PdfDocument pdf = new PdfDocument(writer);
                Document document = new Document(pdf);

                // Load a TTF font that supports Hebrew
                string fontPath = Server.MapPath("~/App_Data/fonts/NotoSansHebrew-VariableFont_wdth,wght.ttf");
                if (!System.IO.File.Exists(fontPath))
                    throw new Exception("קובץ גופן לא נמצא: " + fontPath);

                PdfFont font = PdfFontFactory.CreateFont(
                    fontPath,
                    PdfEncodings.IDENTITY_H,
                    EmbeddingStrategy.PREFER_EMBEDDED
                );

                // Add a title to the PDF
                var title = new Paragraph(BidiHelper.ReverseHebrewRunsAndOrder("רשימת אנשים"))
                    .SetFont(font)
                    .SetFontSize(18)
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetMarginBottom(12);
                document.Add(title);

                // Create table with columns in reverse order: email, phone, name, image
                Table table = new Table(UnitValue.CreatePercentArray(new float[] { 2, 2, 3, 1 }))
                    .UseAllAvailableWidth();

                var cellBorder = new iText.Layout.Borders.SolidBorder(ColorConstants.BLACK, 1);

                // Add headers (email, phone, name, image) - reversed order to match UI request
                table.AddHeaderCell(new Cell().Add(new Paragraph(BidiHelper.ReverseHebrewRunsAndOrder("אימייל")).SetFont(font).SetFontColor(ColorConstants.BLACK).SetFontSize(14)).SetTextAlignment(TextAlignment.RIGHT).SetBorder(cellBorder));
                table.AddHeaderCell(new Cell().Add(new Paragraph(BidiHelper.ReverseHebrewRunsAndOrder("טלפון")).SetFont(font).SetFontColor(ColorConstants.BLACK).SetFontSize(14)).SetTextAlignment(TextAlignment.RIGHT).SetBorder(cellBorder));
                table.AddHeaderCell(new Cell().Add(new Paragraph(BidiHelper.ReverseHebrewRunsAndOrder("שם מלא")).SetFont(font).SetFontColor(ColorConstants.BLACK).SetFontSize(14)).SetTextAlignment(TextAlignment.RIGHT).SetBorder(cellBorder));
                table.AddHeaderCell(new Cell().Add(new Paragraph(BidiHelper.ReverseHebrewRunsAndOrder("תמונה")).SetFont(font).SetFontColor(ColorConstants.BLACK).SetFontSize(14)).SetTextAlignment(TextAlignment.CENTER).SetBorder(cellBorder));

                foreach (var p in people)
                {
                    // Email
                    table.AddCell(new Cell().Add(new Paragraph(p.Email).SetFont(font).SetFontSize(12).SetFontColor(ColorConstants.BLACK)).SetTextAlignment(TextAlignment.RIGHT).SetBorder(cellBorder));

                    // Phone
                    table.AddCell(new Cell().Add(new Paragraph(p.Phone).SetFont(font).SetFontColor(ColorConstants.BLACK).SetFontSize(12)).SetTextAlignment(TextAlignment.RIGHT).SetBorder(cellBorder));

                    // Name
                    table.AddCell(new Cell().Add(new Paragraph(BidiHelper.ReverseHebrewRunsAndOrder(p.FullName)).SetFont(font).SetFontColor(ColorConstants.BLACK).SetFontSize(12)).SetTextAlignment(TextAlignment.RIGHT).SetBorder(cellBorder));

                    // Image cell (last column)
                    if (p.PhotoData != null && p.PhotoData.Length > 0)
                    {
                        try
                        {
                            var imgData = iText.IO.Image.ImageDataFactory.Create(p.PhotoData);
                            var img = new iText.Layout.Element.Image(imgData);
                            img.ScaleToFit(48, 48);
                            // right-align the image inside the cell
                            img.SetHorizontalAlignment(HorizontalAlignment.RIGHT);
                            var imgCell = new Cell().Add(img)
                                .SetBorder(cellBorder)
                                .SetPadding(4)
                                .SetVerticalAlignment(VerticalAlignment.MIDDLE)
                                .SetTextAlignment(TextAlignment.RIGHT);
                            table.AddCell(imgCell);
                        }
                        catch
                        {
                            table.AddCell(new Cell().Add(new Paragraph(" ")).SetBorder(cellBorder));
                        }
                    }
                    else
                    {
                        table.AddCell(new Cell().Add(new Paragraph(" ")).SetBorder(cellBorder));
                    }
                }

                document.Add(table);
                document.Close();

                return File(ms.ToArray(), "application/pdf", "רשימת_אנשים.pdf");
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) db.Dispose();
            base.Dispose(disposing);
        }
    }
}
