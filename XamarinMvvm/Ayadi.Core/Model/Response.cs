using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ayadi.Core.Model
{
    public class Response : BaseModel
    {
        public bool Ok { get; set; }
        public string Message { get; set; }
    }
}
