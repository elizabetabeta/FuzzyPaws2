using AutoMapper;
using FuzzyPaws2.Data;
using FuzzyPaws2.Interfaces;
using FuzzyPaws2.ViewModels.Appointments;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text;

namespace FuzzyPaws2.Controllers
{
    [Authorize]
    public class AppointmentController : Controller
    { 
        private readonly IAppointmentService _appointmentService;
        private readonly ISelectListService _selectListService;
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _context;

        public AppointmentController(IAppointmentService appointmentService,
                                     IMapper mapper,
                                     ApplicationDbContext context,
                                     ISelectListService selectListService)
        {
            _appointmentService = appointmentService;
            _mapper = mapper;
            _context = context;
            _selectListService = selectListService;
        }

        public async Task<IActionResult> Index()
        {
            var model = await _appointmentService.GetAppointmentsAsync();
            return View(model);
        }

        [Authorize(Roles = "Vet")]
        public async Task<IActionResult> WaitingAppointments()
        {
            var model = await _appointmentService.GetAppointmentsAsync();
            return View(model);
        }

        [Authorize(Roles = "Vet")]
        public async Task<IActionResult> AppointmentsToday()
        {
            var model = await _appointmentService.GetAppointmentsAsync();
            return View(model);
        }

        public async Task<IActionResult> Notification()
        {
            var model = await _appointmentService.GetAppointmentsAsync();
            return View(model);
        }

        [Authorize(Roles = "Vet")]
        public async Task<IActionResult> Profit()
        {
            var model = await _appointmentService.GetAppointmentsAsync();
            return View(model);
        }

        public async Task<IActionResult> Details(int id)
        {
            var model = await _appointmentService.GetAppointmentsById(id);
            if (model == null)
                return NotFound();

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var model = await _appointmentService.MakeAnAppointmentAsync();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CreateAppointmentViewModel model)
        {
            var day = model.Time;
            if (day < DateTime.Now)
            {
                TempData["error"] = "You can't choose past date. Please select a valid time.";
                return RedirectToAction("Create");
            }
            else if (_context.Appointments.Any(x => x.Time == model.Time))
            {
                TempData["error"] = "Someone already booked at this time. Please check which time is available.";
            }
            else if (day.DayOfWeek == DayOfWeek.Sunday || day.DayOfWeek == DayOfWeek.Saturday)
            {
                TempData["error"] = "Invalid date. Cannot select Saturday or Sunday.";
            } else if (day.Hour < 8 || day.Hour > 15)
            {
                TempData["error"] = "Invalid time. Working hour starts at 8AM and ends at 4PM.";
            }
            else
            {
                _appointmentService.CreateAsync(model);

                TempData["success"] = "Appointment booked successfully. Please wait for vet's confirmation.";
            }

            return RedirectToAction("Index");

        }

        [Authorize(Roles = "Vet")]
        public async Task<IActionResult> Status(int id)
        {
            var appointment = _appointmentService.GetById(id);

            var mappedApp = _mapper.Map<CreateAppointmentViewModel>(appointment);
            mappedApp.MyPets = await _selectListService.GetMyPets(false);

            return View(mappedApp);

        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Status(int id, CreateAppointmentViewModel model)
        {
            var app = _appointmentService.GetById(id);

            app.status = model.status;
            app.ExpectedPrice = model.ExpectedPrice;
            app.FinalPrice = model.FinalPrice;

            _context.Update(app);
            _context.SaveChanges();

            TempData["success"] = "Appointment edited successfully";

            return RedirectToAction("Details", new
            {
                id = model.Id

            });
        }

        public FileResult CreatePdf()
        {
            MemoryStream workStream = new MemoryStream();
            StringBuilder status = new StringBuilder("");
            DateTime dTime = DateTime.Now;
            string strPDFFileName = string.Format("AppointmentDetailsPdf" + dTime.ToString("yyyyMMdd") + "-" + ".pdf");
            Document doc = new Document();
            doc.SetMargins(0, 0, 0, 0);
            PdfPTable tableLayout = new PdfPTable(4);
            PdfPTable tableLayout2 = new PdfPTable(4);
            doc.SetMargins(10, 10, 10, 0);
            PdfWriter.GetInstance(doc, workStream).CloseStream = false;
            doc.Open();
            BaseFont bf = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            Font fontInvoice = new Font(bf, 20, Font.NORMAL);
            Paragraph paragraph = new Paragraph("Appointments", fontInvoice);
            paragraph.Alignment = Element.ALIGN_CENTER;
            doc.Add(paragraph);

            var allProfit = _context.Appointments.Where(x => x.FinalPrice > 0).Sum(b => b.FinalPrice);
            var monthlyProfit = _context.Appointments.Where(x => x.FinalPrice > 0 && x.Time.Month == DateTime.Now.Month).Sum(b => b.FinalPrice);

            Paragraph mp = new Paragraph("\nProfit for this month: $" + monthlyProfit);
            mp.Alignment = Element.ALIGN_RIGHT;
            doc.Add(mp);

            Paragraph p3 = new Paragraph();
            p3.SpacingAfter = 6;
            doc.Add(p3);

            Paragraph month = new Paragraph("Finished appointments in this month: \n\n");
            doc.Add(month);
            doc.Add(Add_Content_To_PDF_This_Month(tableLayout));

            Paragraph ap = new Paragraph("\nAll time profit: $" + allProfit);
            ap.Alignment = Element.ALIGN_RIGHT;
            doc.Add(ap);

            Paragraph all = new Paragraph("All finished appointments: \n\n");
            doc.Add(all);
            doc.Add(Add_Content_To_PDF(tableLayout2));

            var year = DateTime.Now.Year;
            var now = DateTime.Now.Date;
            var nowYear = now.Year;
            var nowMonth = now.Month;
            var nowDay = now.Day;

            var userId = this.User.FindFirstValue(ClaimTypes.Name);

            Paragraph stamp = new Paragraph("\n\n\n\n\n\n©" + year + " - FuzzyPaws\nDate: " +
                nowDay + "." + nowMonth + "." + nowYear + "." +
                "\nVet: " + userId);
            stamp.Alignment = Element.ALIGN_CENTER;          
            doc.Add(stamp);

            doc.Close();
            byte[] byteInfo = workStream.ToArray();
            workStream.Write(byteInfo, 0, byteInfo.Length);
            workStream.Position = 0;
            return File(workStream, "application/pdf", strPDFFileName);
        }

        public FileResult CreatePdfForToday()
        {
            MemoryStream workStream = new MemoryStream();
            StringBuilder status = new StringBuilder("");
            DateTime dTime = DateTime.Now;
            string strPDFFileName = string.Format("AppointmentsTodayDetailsPdf" + dTime.ToString("yyyyMMdd") + "-" + ".pdf");
            Document doc = new Document();
            doc.SetMargins(0, 0, 0, 0);
            PdfPTable tableLayoutForToday = new PdfPTable(4);
            doc.SetMargins(10, 10, 10, 0);
            PdfWriter.GetInstance(doc, workStream).CloseStream = false;
            doc.Open();
            BaseFont bf = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            Font fontInvoice = new Font(bf, 20, Font.NORMAL);
            Paragraph paragraph = new Paragraph("Today's Appointments", fontInvoice);
            paragraph.Alignment = Element.ALIGN_CENTER;
            doc.Add(paragraph);

            var expectedProfit = _context.Appointments.Where(x => x.ExpectedPrice > 0 && x.Time.Day == DateTime.Now.Day && x.status == Models.Status.Finished).Sum(b => b.ExpectedPrice);
            var actualProfit = _context.Appointments.Where(x => x.FinalPrice > 0 && x.Time.Day == DateTime.Now.Day).Sum(b => b.FinalPrice);

            Paragraph mp = new Paragraph("\nExpected profit: $" + expectedProfit);
            mp.Alignment = Element.ALIGN_RIGHT;
            doc.Add(mp);

            Paragraph p3 = new Paragraph();
            p3.SpacingAfter = 6;
            doc.Add(p3);

            Paragraph month = new Paragraph("Today's appointments: \n\n");
            doc.Add(month);
            doc.Add(Add_Content_To_PDF_For_Today(tableLayoutForToday));

            Paragraph ap = new Paragraph("\nActual profit: $" + actualProfit);
            ap.Alignment = Element.ALIGN_RIGHT;
            doc.Add(ap);


            var year = DateTime.Now.Year;
            var now = DateTime.Now.Date;
            var nowYear = now.Year;
            var nowMonth = now.Month;
            var nowDay = now.Day;

            var userId = this.User.FindFirstValue(ClaimTypes.Name);

            Paragraph stamp = new Paragraph("\n\n\n\n\n\n©" + year + " - FuzzyPaws\nDate: " +
                nowDay + "." + nowMonth + "." + nowYear + "." +
                "\nVet: " + userId);
            stamp.Alignment = Element.ALIGN_CENTER;
            doc.Add(stamp);

            doc.Close();
            byte[] byteInfo = workStream.ToArray();
            workStream.Write(byteInfo, 0, byteInfo.Length);
            workStream.Position = 0;
            return File(workStream, "application/pdf", strPDFFileName);
        }

        protected PdfPTable Add_Content_To_PDF(PdfPTable tableLayout)
        {
            float[] headers = { 50, 24, 45, 35 }; //Header Widths  
            tableLayout.SetWidths(headers); //Set the pdf headers  
            tableLayout.WidthPercentage = 100; //Set the PDF File witdh percentage  
            tableLayout.HeaderRows = 1;
            var count = 1;
            //Add header  
            AddCellToHeader(tableLayout, "Date/Time");
            AddCellToHeader(tableLayout, "Status");
            AddCellToHeader(tableLayout, "Expected price");
            AddCellToHeader(tableLayout, "Final Price");

            foreach (var app in _context.Appointments.Where(x => x.status == Models.Status.Finished))
            {
                if (count >= 1)
                {
                    //Add body  
                    AddCellToBody(tableLayout, app.Time.ToString(), count);
                    AddCellToBody(tableLayout, app.status.ToString(), count);
                    AddCellToBody(tableLayout, "$" + app.ExpectedPrice.ToString(), count);
                    AddCellToBody(tableLayout, "$" + app.FinalPrice.ToString(), count);
                    count++;
                }
            }
            return tableLayout;
        }

        protected PdfPTable Add_Content_To_PDF_For_Today(PdfPTable tableLayoutToday)
        {
            float[] headers = { 50, 24, 45, 35 }; //Header Widths  
            tableLayoutToday.SetWidths(headers); //Set the pdf headers  
            tableLayoutToday.WidthPercentage = 100; //Set the PDF File witdh percentage  
            tableLayoutToday.HeaderRows = 1;
            var count = 1;
            //Add header  
            AddCellToHeader(tableLayoutToday, "Date/Time");
            AddCellToHeader(tableLayoutToday, "Status");
            AddCellToHeader(tableLayoutToday, "Expected price");
            AddCellToHeader(tableLayoutToday, "Final Price");

            foreach (var app in _context.Appointments.Where(x => x.status == Models.Status.Finished
                                                              && x.Time.Day == DateTime.Now.Day))
            {
                if (count >= 1)
                {
                    //Add body  
                    AddCellToBody(tableLayoutToday, app.Time.ToString(), count);
                    AddCellToBody(tableLayoutToday, app.status.ToString(), count);
                    AddCellToBody(tableLayoutToday, "$" + app.ExpectedPrice.ToString(), count);
                    AddCellToBody(tableLayoutToday, "$" + app.FinalPrice.ToString(), count);
                    count++;
                }
            }
            return tableLayoutToday;
        }

        protected PdfPTable Add_Content_To_PDF_This_Month(PdfPTable tableLayout)
        {
            float[] headers = { 50, 24, 45, 35 }; //Header Widths  
            tableLayout.SetWidths(headers); //Set the pdf headers  
            tableLayout.WidthPercentage = 100; //Set the PDF File witdh percentage  
            tableLayout.HeaderRows = 1;
            var count = 1;
            //Add header  
            AddCellToHeader(tableLayout, "Date/Time");
            AddCellToHeader(tableLayout, "Status");
            AddCellToHeader(tableLayout, "Expected price");
            AddCellToHeader(tableLayout, "Final Price");

            foreach (var app in _context.Appointments
                .Where(x => x.status == Models.Status.Finished && x.Time.Month == DateTime.Now.Month))
            {
                if (count >= 1)
                {
                    //Add body  
                    AddCellToBody(tableLayout, app.Time.ToString(), count);
                    AddCellToBody(tableLayout, app.status.ToString(), count);
                    AddCellToBody(tableLayout, "$" + app.ExpectedPrice.ToString(), count);
                    AddCellToBody(tableLayout, "$" + app.FinalPrice.ToString(), count);
                    count++;
                }
            }
            return tableLayout;
        }

        private static void AddCellToHeader(PdfPTable tableLayout, string cellText)
        {
            tableLayout.AddCell(new PdfPCell(new Phrase(cellText, new Font(Font.FontFamily.HELVETICA, 8, 1, BaseColor.BLACK)))
            {
                HorizontalAlignment = Element.ALIGN_LEFT,
                Padding = 8,
                BackgroundColor = new BaseColor(255, 255, 255)
            });
        }

        private static void AddCellToBody(PdfPTable tableLayout, string cellText, int count)
        {
            if (count % 2 == 0)
            {
                tableLayout.AddCell(new PdfPCell(new Phrase(cellText, new Font(Font.FontFamily.HELVETICA, 8, 1, iTextSharp.text.BaseColor.BLACK)))
                {
                    HorizontalAlignment = Element.ALIGN_LEFT,
                    Padding = 8,
                    BackgroundColor = new iTextSharp.text.BaseColor(255, 255, 255)
                });
            }
            else
            {
                tableLayout.AddCell(new PdfPCell(new Phrase(cellText, new Font(Font.FontFamily.HELVETICA, 8, 1, iTextSharp.text.BaseColor.BLACK)))
                {
                    HorizontalAlignment = Element.ALIGN_LEFT,
                    Padding = 8,
                    BackgroundColor = new BaseColor(211, 211, 211)
                });
            }
        }
     
    }
}
