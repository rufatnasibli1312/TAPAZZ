using AutoMapper;
using BLL.Persistence.Service.Abstraction;
using BLL.ServiceExtensions;
using DAL.Persistence.Repository.Abstraction;
using DAL.Persistence.Repository.Implementation;
using DTO;
using DTO.CategoryDto_s;
using DTO.ComplaintDto_s;
using DTO.ProductDto_s;
using Entity.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Persistence.Service.Implementation
{
    public class ComplaintService : IComplaintService
    {
        public IMapper _mapper { get; }
        public IComplaintRepository _repository { get; }
        public JwtTokenExtractor _jwtTokenExtractor { get; }

        public ComplaintService(IMapper mapper, IComplaintRepository repository, JwtTokenExtractor jwtTokenExtractor)
        {
            _mapper = mapper;
            _repository = repository;
            _jwtTokenExtractor = jwtTokenExtractor;
        }


        public async Task AddAsync(ComplaintAddDto complaintAddDto)
        {
            Complaint complaint = _mapper.Map<Complaint>(complaintAddDto);
            var fullname = _jwtTokenExtractor.GetFullnameFromJwtToken();
            complaint.CustomerName = fullname;
            await _repository.AddAsync(complaint);

        }

        public async Task<ComplaintFindIdDto> GetAsync(int id)
        {
            Complaint complaint = await _repository.GetAsync(id);
            var fullname = _jwtTokenExtractor.GetFullnameFromJwtToken();
            if (complaint.CustomerName == fullname)
            {
                var result = _mapper.Map<ComplaintFindIdDto>(complaint);
                return result;
            }
            throw new InvalidOperationException("Complaint not found or does not belong to the authenticated user.");

        }

        public async Task<List<ComplaintFindIdDto>> GetAllAsync()
        {
            List<Complaint> complaints = await _repository.GetAllAsync();
            var fullname = _jwtTokenExtractor.GetFullnameFromJwtToken();
            List<ComplaintFindIdDto> result = _mapper.Map<List<ComplaintFindIdDto>>(
      complaints.Where(comp => comp.CustomerName == fullname).ToList()); ;
            return result;

        }

        public async Task Delete(DeleteComplaintDto entity)
        {
            var complaint = await _repository.GetAsync(entity.Id);
            if(complaint == null)
            {
                throw new InvalidOperationException("Complaint not found.");
            }
           
                var fullName = _jwtTokenExtractor.GetFullnameFromJwtToken();
                if (complaint.CustomerName != fullName)
                {
                    throw new InvalidOperationException("Complaint does not belong to the authenticated user.");


                }

                await _repository.Delete(complaint);

            
          
        }

        public async Task UpdateAsync(UpdateComplaintDto complaintDto)
        {
            var existingComplaint = await _repository.GetAsync(complaintDto.Id);
            var complaint = _mapper.Map(complaintDto, existingComplaint);
            if (complaint != null)
            {
                var fullname = _jwtTokenExtractor.GetFullnameFromJwtToken();

                if (complaint.CustomerName == fullname)
                {
                    await _repository.UpdateAsync(complaint);
                }
            }
        }
    }
}
