using AspNetCore.Reporting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using Point_of_Sale.Interface;
using Point_of_Sale.Models;
using Point_of_Sale.Models.DBContext;
using System.Data;
using System.Data.SqlClient;

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

            var sp_list = db.sp_receipt.FromSqlRaw("EXEC sp_receipt {0}", InvoiceId).ToList();

            LocalReport localReport = new LocalReport(path);

            decimal TotalAmount = db.tbl_invoice.Where(x => x.Id == InvoiceId).FirstOrDefault().AmountTotal;

            parameters.Add("TotalAmount", TotalAmount.ToString());

            localReport.AddDataSource("ds_receipt", sp_list);

            var result = localReport.Execute(RenderType.Pdf, extension, parameters, mimtype);

            return File(result.MainStream, "application/pdf");
        }

        public IActionResult GenerateList(int typeId, string dateFrom, string dateTo)
        {
            string mimtype = "";
            int extension = 1;

            var dt = new DataTable();

            var path = $"{webHostEnvirnoment.WebRootPath}\\reports\\GeneratedList.rdlc";

            Dictionary<string, string> parameters = new Dictionary<string, string>();

            if (typeId == 1)
            {
                //List<sp_generated_list> sp_list = new List<sp_generated_list>();

                var sp_list = db.sp_generated_list.FromSqlRaw("EXEC sp_generated_list {0},{1},{2}", typeId, dateFrom, dateTo).ToList();

                LocalReport localReport = new LocalReport(path);

                parameters.Add("listname", "List of all Products");

                localReport.AddDataSource("ds_generated_list", sp_list);

                var result = localReport.Execute(RenderType.Pdf, extension, parameters, mimtype);

                return File(result.MainStream, "application/pdf");
            }
            else if (typeId == 2)
            {
                var sp_list = db.sp_generated_list.FromSqlRaw("EXEC sp_generated_list {0},{1},{2}", typeId, dateFrom, dateTo).ToList();

                LocalReport localReport = new LocalReport(path);

                parameters.Add("listname", "List of all Products Sold");

                localReport.AddDataSource("ds_generated_list", sp_list);

                var result = localReport.Execute(RenderType.Pdf, extension, parameters, mimtype);

                return File(result.MainStream, "application/pdf");
            }
            else
            {
                var sp_list = db.tbl_item.ToList();

                LocalReport localReport = new LocalReport(path);

                parameters.Add("listname", "List of all Products Sold");

                localReport.AddDataSource("ds_generated_list", sp_list);

                var result = localReport.Execute(RenderType.Pdf, extension, parameters, mimtype);

                return File(result.MainStream, "application/pdf");
            }
        }

    }
}
