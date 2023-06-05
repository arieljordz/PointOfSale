using Microsoft.AspNetCore.Mvc;
using Point_of_Sale.DTO;
using Point_of_Sale.Models;
using System.Threading.Tasks;

namespace Point_of_Sale.Interface
{
    public interface IProducts
    {
        List<ProductsDTO> GetItems();

        List<ProductsDTO> GetItemDetails();

        bool SaveItem(tbl_Item item);

        bool SaveItemDetails(tbl_ItemDetails dtls);

    }
}
