using AspNetCore.Reporting;
using Microsoft.AspNetCore.Mvc;
using Point_of_Sale.DTO;
using Point_of_Sale.Interface;
using Point_of_Sale.Models.DBContext;
using System.Data;

namespace Point_of_Sale.Controllers
{
    public class ReportsController : Controller
    {
        private readonly PointOfSaleDbContext db;
        private readonly IGlobal global;
        private readonly IWebHostEnvironment webHostEnvirnoment;

        public ReportsController(PointOfSaleDbContext context, IWebHostEnvironment webHostEnvironment, IGlobal repository)
        {
            db = context;
            global = repository;
            webHostEnvirnoment = webHostEnvironment;
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult PreviewReceipt(int InvoiceId)
        {
            string mimtype = "";
            int extension = 1;

            var dt = new DataTable();

            var path = $"{webHostEnvirnoment.WebRootPath}\\reports\\Receipt.rdlc";
            Dictionary<string, string> parameters = new Dictionary<string, string>();

            var list = db.tbl_sales.Where(x => x.InvoiceId == InvoiceId).ToList();

            List<ReceiptDTO> objList = new List<ReceiptDTO>();

            foreach (var item in list)
            {
                var obj = new ReceiptDTO
                {
                    InvoiceId = InvoiceId,
                    Description = db.tbl_item.Where(x => x.Id == item.ProductId).FirstOrDefault().Description,
                    Quantity = item.Quantity,
                    SubTotal = item.SubTotal,
                };
                objList.Add(obj);
            }

            LocalReport localReport = new LocalReport(path);

            parameters.Add("rp1","Hello World! asdsadsad");

            localReport.AddDataSource("DataSet1", objList);
            var result = localReport.Execute(RenderType.Pdf, extension, parameters, mimtype);
            return File(result.MainStream, "application/pdf");
        }

    }
}
