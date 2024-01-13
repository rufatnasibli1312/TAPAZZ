using BLL.Persistence.Service.Abstraction;
using DAL.Filter.ActionFilter;
using DTO;
using DTO.AccountDto_s;
using DTO.ComplaintDto_s;
using DTO.LocationDto_s;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Serilog;
using System.Text.Json;
using Validation.ComplaintValidator;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "User")]
    [ServiceFilter(typeof(StandardizeResponseFilter))]
    public class ComplaintController : ControllerBase
    {
        public IComplaintService _service { get; }
        public ComplaintController(IComplaintService service)
        {
            _service = service;
        }


        [HttpPost]
        public async Task CreateComplaint(ComplaintAddDto complaintAddDto)
        {


             await _service.AddAsync(complaintAddDto);
        }
        [HttpGet]
        public async Task<ComplaintFindIdDto> GetComplaint(int id)
        {

            return await _service.GetAsync(id);
        }
        [HttpGet("GetAllComplaints")]
        public async Task<List<ComplaintFindIdDto>> GetAllComplaint()
        {


            return await _service.GetAllAsync();


        }

        [HttpDelete]
        public async Task<ComplaintFindIdDto> DeleteComplaint(DeleteComplaintDto entity)
        {

            return await _service.GetAsync(entity.Id);


        }
        [HttpPut]
        public async Task UpdateComplaint(UpdateComplaintDto entity)
        {
            await _service.UpdateAsync(entity);

        }

    }
}
