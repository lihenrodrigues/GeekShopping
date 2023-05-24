using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GeekShopping.Web.Models;

namespace GeekShopping.Web.Services.IServices;
public interface IProductService
{
    Task<IEnumerable<ProductModel>> FindAllProducts(string token);
    Task<ProductModel> FindProductById(string token, long id);
    Task<ProductModel> CreateProduct(string token, ProductModel model);
    Task<ProductModel> UpdateProduct(string token, ProductModel model);
    Task<bool> DeleteProductById(string token, long id);
}
