using DTO.LocationDto_s;
using DTO.ProductDto_s;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Persistence.Service.Abstraction
{
    public interface IProductService
    {
        Task AddAsync(ProductAddDto productDto, string webRootPath);
        Task<ProductFindIdDto> GetAsync(int id);
        Task<List<ProductToListDto>> GetAllAsync();

        Task<List<ProductToListDto>> GetMyProductsAsync();

        Task<bool> Delete(DeleteProductDto entity);

        Task UpdateAsync(UpdateProductDto updateProductDto, string webRootPath);
        
    }
}
