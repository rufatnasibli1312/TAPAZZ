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
        public async Task<IActionResult> CreateComplaint(ComplaintAddDto complaintAddDto)
        {


            try
            {
                await _service.AddAsync(complaintAddDto);
                return Ok();
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetComplaint(int id)
        {

            try
            {
                var complaint = await _service.GetAsync(id);

                return Ok(complaint);


            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }
        [HttpGet("GetAllComplaints")]
        public async Task<IActionResult> GetAllComplaint()
        {

            try
            {
                var complaints = await _service.GetAllAsync();

                return Ok(complaints);

            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteComplaint(DeleteComplaintDto entity)
        {


            try
            {
                var complaint = await _service.GetAsync(entity.Id);

                await _service.Delete(entity);
                return Ok();
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }



        }
        [HttpPut]
        public async Task<IActionResult> UpdateComplaint(UpdateComplaintDto entity)
        {
            try
            {

                await _service.UpdateAsync(entity);
                return Ok(entity);
            }
            catch (Exception ex)
            {

                return BadRequest(ex);

            }

        }

    }
}
