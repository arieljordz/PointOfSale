using Microsoft.AspNetCore.Mvc;
using Point_of_Sale.Models;
using System.Threading.Tasks;

namespace Point_of_Sale.Interface
{
    public interface IProducts
    {
        tbl_Item? SaveItem(tbl_Item item);

        bool SaveItemDetails(tbl_Item item);

    }
}
