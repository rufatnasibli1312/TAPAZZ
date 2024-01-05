using DTO;
using DTO.CategoryDto_s;
using DTO.ComplaintDto_s;
using DTO.LocationDto_s;
using DTO.ProductDto_s;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Persistence.Service.Abstraction
{
    public interface IComplaintService
    {
        Task AddAsync(ComplaintAddDto complaintAddDto);
        Task<ComplaintFindIdDto> GetAsync(int id);
        Task<List<ComplaintFindIdDto>> GetAllAsync();
        Task Delete(DeleteComplaintDto entity);

        Task UpdateAsync(UpdateComplaintDto complaintDto);

    }
}
