using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.Interface;

namespace Entity.Entities
{
    public class Complaint : BaseEntity, IAuditEntity
    {

        public string CustomerName { get; set; }
        public string ComplaintText { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
