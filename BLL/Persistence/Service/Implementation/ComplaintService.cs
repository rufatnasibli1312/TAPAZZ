using AutoMapper;
using BLL.Persistence.Service.Abstraction;
using BLL.Persistence.Service.Concrete;
using BLL.ServiceExtensions;
using DAL.Persistence.Repository.Abstraction;
using DAL.Persistence.Repository.Implementation;
using DTO;
using DTO.CategoryDto_s;
using DTO.ComplaintDto_s;
using DTO.ProductDto_s;
using Entity.Entities;
using Microsoft.AspNetCore.Http;
using Serilog;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Validation.ComplaintValidator;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BLL.Persistence.Service.Implementation
{
    public class ComplaintService : IComplaintService
    {
        public IMapper _mapper { get; }
        public IComplaintRepository _repository { get; }  
        public JwtTokenExtractor _jwtTokenExtractor { get; }  //oke

        public ComplaintService(IMapper mapper, IComplaintRepository repository, JwtTokenExtractor jwtTokenExtractor)
        {
            _mapper = mapper;
            _repository = repository;
            _jwtTokenExtractor = jwtTokenExtractor;
        }
        public async Task AddAsync(ComplaintAddDto complaintAddDto)
        {
            List<string> errors = new List<string>();
            
                var model = JsonSerializer.Serialize(complaintAddDto);


                ComplaintAddValidator validator = new ComplaintAddValidator();
                var result = validator.Validate(complaintAddDto);
                errors = result.Errors.Select(m => m.ErrorMessage).ToList();
                if (result.IsValid)
                {

                    Complaint complaint = _mapper.Map<Complaint>(complaintAddDto);
                    var userId = _jwtTokenExtractor.GetUserIdFromJwtToken();
                    complaint.UserID = userId;
                    Log.Information($"{nameof(ComplaintService)}.{nameof(AddAsync)} - Complaint Added Succesfully. Data: {model}");
                    await _repository.AddAsync(complaint);


                }
                else
                {
                    throw new ValidationException($"Validation failed-{string.Join(", ", errors)}.");
                }


        }

        public async Task<ComplaintFindIdDto> GetAsync(int id)
        {
            List<string> errors = new List<string>();
           
                Complaint complaint = await _repository.GetAsync(id);
                if (complaint != null)
                {
                    var userId = _jwtTokenExtractor.GetUserIdFromJwtToken();
                    if (complaint.UserID == userId)
                    {
                        var result = _mapper.Map<ComplaintFindIdDto>(complaint);
                        Log.Information($"{nameof(ComplaintService)}.{nameof(GetAsync)} - Complaint Getted Succesfully. Id: {id}");
                        return result;
                    }
                    else
                    {
                        errors.Add("Complaint not found or does not belong to the authenticated user.");
                        throw new InvalidOperationException($"{string.Join(", ", errors)}");
                    }
                }
                else
                {
                    errors.Add("Complaint not found");
                    throw new InvalidOperationException($"{string.Join(", ", errors)}");
                }

           
           



        }

        public async Task<List<ComplaintFindIdDto>> GetAllAsync()
        {
            List<string> errors = new List<string>();
           
                List<Complaint> complaints = await _repository.GetAllAsync();
                var UserId = _jwtTokenExtractor.GetUserIdFromJwtToken();
                List<ComplaintFindIdDto> result = _mapper.Map<List<ComplaintFindIdDto>>(
          complaints.Where(comp => comp.UserID == UserId).ToList());
                if (result.Count > 0)
                {
                    Log.Information($"{nameof(ComplaintService)}.{nameof(GetAllAsync)} - Complaint Added Succesfully.");
                    return result;
                }
                else
                {
                    errors.Add("Complaints not found");
                    throw new InvalidOperationException($"{string.Join(", ", errors)}");
                }

            

        }

        public async Task Delete(DeleteComplaintDto entity)
        {
            List<string> errors = new List<string>();

            
                var model = JsonSerializer.Serialize(entity);

                DeleteComplaintValidator validator = new DeleteComplaintValidator();
                var result = validator.Validate(entity);
                errors = result.Errors.Select(m => m.ErrorMessage).ToList();
                if (result.IsValid)
                {
                    var complaint = await _repository.GetAsync(entity.Id);
                    if (complaint == null)
                    {
                        errors.Add("Complaint not found.");
                        throw new InvalidOperationException($"{string.Join(", ", errors)}");
                    }

                    var UserId = _jwtTokenExtractor.GetUserIdFromJwtToken();
                    if (complaint.UserID != UserId)
                    {
                        errors.Add("Complaint does not belong to the authenticated user.");
                        throw new InvalidOperationException($"{string.Join(", ", errors)}");

                    }
                    Log.Information($"{nameof(ComplaintService)}.{nameof(Delete)} - Complaint Deleted Succesfully. Id = {entity.Id}");
                    await _repository.Delete(complaint);
                }
                else
                {
                    throw new ValidationException($"Validation failed-{string.Join(", ", errors)}.");
                }

            





        }

        public async Task UpdateAsync(UpdateComplaintDto complaintDto)
        {
            List<string> errors = new List<string>();
           

                var model = JsonSerializer.Serialize(complaintDto);

                UpdateComplaintValidator validator = new UpdateComplaintValidator();
                var result = validator.Validate(complaintDto);
                errors = result.Errors.Select(m => m.ErrorMessage).ToList();
                if (result.IsValid)
                {
                    var existingComplaint = await _repository.GetAsync(complaintDto.Id);

                    var complaint = _mapper.Map(complaintDto, existingComplaint);
                    if (existingComplaint != null)
                    {
                        var userId = _jwtTokenExtractor.GetUserIdFromJwtToken();

                        if (complaint.UserID == userId)
                        {
                            Log.Information($"{nameof(ComplaintService)}.{nameof(UpdateAsync)} - Complaint Updated Succesfully.");
                            await _repository.UpdateAsync(complaint);
                        }
                        else
                        {
                            errors.Add("Complaint not found or does not belong to the authenticated user.");
                            throw new InvalidOperationException($"{string.Join(", ", errors)}");
                        }
                    }
                    else
                    {
                        errors.Add("Complaints not found");
                        throw new InvalidOperationException($"{string.Join(", ", errors)}");
                    }
                }
                else
                {
                    throw new ValidationException($"Validation failed-{string.Join(", ", errors)}.");
                }




          

        }
    }
}
