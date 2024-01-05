using DAL.Data;
using DAL.Persistence.Repository.Abstraction;
using Entity.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Persistence.Repository.Implementation
{
    public class ComplaintRepository : GenericRepository<Complaint>, IComplaintRepository
    {
        public MyDbContext _context { get; }
        public ComplaintRepository(MyDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
