using Point_of_Sale.Models;
using System.Threading.Tasks;

namespace Point_of_Sale.Interface
{
    public interface IGlobal
    {
        decimal ComputePrice(int quantity, int productId);
        decimal GetItemPrice(int productId);
    }
}
