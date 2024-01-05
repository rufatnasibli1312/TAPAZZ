using BLL.Persistence.Service.Abstraction;
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
    [Authorize]
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
            var model = JsonSerializer.Serialize(complaintAddDto);
            Log.Information($"{nameof(ComplaintController)}.{nameof(CreateComplaint)}({model})");

            ComplaintAddValidator validator = new ComplaintAddValidator();
            var result = validator.Validate(complaintAddDto);
            var error = result.Errors.Select(m => m.ErrorMessage).ToList();
            if (result.IsValid)
            {
                try
                {
                    await _service.AddAsync(complaintAddDto);
                    return Ok();
                }
                catch (Exception ex)
                {
                    Log.Error($"{nameof(ComplaintController)}.{nameof(CreateComplaint)}({model})");
                    return BadRequest(ex.Message);
                }
            }
            Log.Error($"{nameof(ComplaintController)}.{nameof(CreateComplaint)}({model})");
            return BadRequest(error);

        }
        [HttpGet]

        public async Task<IActionResult> GetComplaint(int id)
        {
            Log.Information($"{nameof(ComplaintController)}.{nameof(GetComplaint)}({id})");
            try
            {
                var complaint = await _service.GetAsync(id);
                if (complaint == null)
                {
                    return NotFound();
                }
                return Ok(complaint);


            }
            catch (Exception ex)
            {
                Log.Error($"{nameof(ComplaintController)}.{nameof(GetComplaint)}({id})");
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("GetAllComplaints")]

        public async Task<IActionResult> GetAllComplaint()
        {
            Log.Information($"{nameof(ComplaintController)}.{nameof(GetAllComplaint)}()");
            try
            {
                var complaints = await _service.GetAllAsync();
                if (complaints == null)
                {
                    return NotFound();
                }
                return Ok(complaints);

            }
            catch (Exception ex)
            {
                Log.Error($"{nameof(ComplaintController)}.{nameof(GetAllComplaint)}()");
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete]

        public async Task<IActionResult> DeleteComplaint(DeleteComplaintDto entity)
        {
            var model = JsonSerializer.Serialize(entity);
            Log.Information($"{nameof(ComplaintController)}.{nameof(DeleteComplaint)}({model})");
            DeleteComplaintValidator validator = new DeleteComplaintValidator();
            var result = validator.Validate(entity);
            var error = result.Errors.Select(m => m.ErrorMessage).ToList();
            if (result.IsValid)
            {
                try
                {
                    var complaint = await _service.GetAsync(entity.Id);
                    if (complaint == null)
                    {
                        return NotFound();
                    }
                    await _service.Delete(entity);
                    return Ok();
                }
                catch (Exception ex)
                {
                    Log.Error($"{nameof(ComplaintController)}.{nameof(DeleteComplaint)}({model})");
                    return BadRequest(ex.Message);
                }
            }
            Log.Error($"{nameof(ComplaintController)}.{nameof(DeleteComplaint)}({model})");
            return BadRequest(error);

        }
        [HttpPut]
        public async Task<IActionResult> UpdateComplaint(UpdateComplaintDto entity)
        {

            var model = JsonSerializer.Serialize(entity);
            Log.Information($"{nameof(ComplaintController)}.{nameof(UpdateComplaint)}({model})");
            UpdateComplaintValidator validator = new UpdateComplaintValidator();
            var result = validator.Validate(entity);
            var error = result.Errors.Select(m => m.ErrorMessage).ToList();
            if (result.IsValid)
            {
                try
                {
                    if (entity == null)
                    {
                        return BadRequest();
                    }
                    await _service.UpdateAsync(entity);
                    return Ok(entity);
                }
                catch (Exception ex)
                {
                    Log.Error($"{nameof(ComplaintController)}.{nameof(UpdateComplaint)}({model})");
                    return BadRequest();

                }
            }
            Log.Error($"{nameof(ComplaintController)}.{nameof(UpdateComplaint)}({model})");
            return BadRequest(error);




        }

    }
}
