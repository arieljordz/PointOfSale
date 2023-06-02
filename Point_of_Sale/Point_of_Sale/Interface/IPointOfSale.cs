using Microsoft.AspNetCore.Mvc;
using Point_of_Sale.Models;
using System.Threading.Tasks;

namespace Point_of_Sale.Interface
{
    public interface IPointOfSale
    {
        int SaveInvoice(int UserId);

        bool SaveSales(int UserId, int InvoiceId);

        bool DeductItem(int InvoiceId);

        decimal GetTotalAmount(int InvoiceId);

        FileStreamResult GenerateReport();
    }
}
