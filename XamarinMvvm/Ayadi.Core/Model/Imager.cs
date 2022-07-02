using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ayadi.Core.Model
{
    public class Imager : BaseModel
    {
        public string Src { get; set; }
        public string Attachment { get; set; }
        public int Position { get; set; }

    }
}
