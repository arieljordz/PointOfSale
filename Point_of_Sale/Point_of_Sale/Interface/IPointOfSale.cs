using Microsoft.AspNetCore.Mvc;
using Point_of_Sale.DTO;
using Point_of_Sale.Models;
using System.Threading.Tasks;

namespace Point_of_Sale.Interface
{
    public interface IPointOfSale
    {
        int SaveInvoice(int UserId);

        bool SaveSales(int UserId, int InvoiceId);

        ResultDTO DeductQuantity(int InvoiceId);

        decimal GetTotalAmount(int InvoiceId);

    }
}
