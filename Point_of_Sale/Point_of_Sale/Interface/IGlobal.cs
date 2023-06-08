using Point_of_Sale.DTO;
using Point_of_Sale.Models;
using System.Threading.Tasks;

namespace Point_of_Sale.Interface
{
    public interface IGlobal
    {
        List<ProductsDTO> GetItems();
        List<ProductsDTO> GetItemDetails();
        decimal ComputePrice(int quantity, int productId);
        decimal GetItemPrice(int productId);
        string FormatDateMMDDYYYY(string date);
    }
}
