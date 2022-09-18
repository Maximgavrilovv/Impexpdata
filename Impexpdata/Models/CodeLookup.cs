using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Impexpdata.Models
{
    public class CodeLookup
    {
        [Key]
        public int Id { get; set; }
        public int CodeId { get; set; }
        public string CodeName { get; set; }
    }
}
