using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Interface
{
    public interface IAuditEntity
    {
        DateTime CreateDate { get; set; }


    }
}
