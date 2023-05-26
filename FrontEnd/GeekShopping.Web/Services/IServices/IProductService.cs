using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GeekShopping.Web.Models;

namespace GeekShopping.Web.Services.IServices;
public interface IProductService
{
    Task<IEnumerable<ProductViewModel>> FindAllProducts(string token);
    Task<ProductViewModel> FindProductById(string token, long id);
    Task<ProductViewModel> CreateProduct(string token, ProductViewModel model);
    Task<ProductViewModel> UpdateProduct(string token, ProductViewModel model);
    Task<bool> DeleteProductById(string token, long id);
}
