using DAL.Data;
using DAL.Persistence.Repository.Abstract;
using DAL.Persistence.Repository.Implementation;
using Entity.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Persistence.Repository.Concrete
{
    public class CityRepository : GenericRepository<City>, ICityRepository
    {
        public MyDbContext _context { get; }
        public CityRepository(MyDbContext context) : base(context)
        {
            _context = context;
        }


    }
}
