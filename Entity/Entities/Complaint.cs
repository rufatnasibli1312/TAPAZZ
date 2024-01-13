using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.Interface;

namespace Entity.Entities
{
    public class Complaint : BaseEntity, IAuditEntity
    {

        public string? UserID { get; set; }
        public User User { get; set; }
        [MaxLength(200)]
        public string ComplaintText { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
