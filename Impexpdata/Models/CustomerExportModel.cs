using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Impexpdata.Models
{
    public class CustomerExportModel
    {
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string? Notes { get; set; }
        public string? CodeName { get; set; }
        public DateTime? ImportDate { get; set; }
        public DateTime? ExportDate { get; set; }

        public string GetCsvString()
        {
            return $"{this.CustomerId};{this.CustomerName};{this.Notes};{this.CodeName};{this.ImportDate};{this.ExportDate}\n";
        }
    }
}
