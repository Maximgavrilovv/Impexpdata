using System.ComponentModel.DataAnnotations;

namespace Impexpdata.Models
{
    public class Customer
    {
        [Key]
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string? Notes { get; set; }
        public int? CodeId { get; set; }
        public DateTime? ImportDate { get; set; }
        public DateTime? ExportDate { get; set; }

    }
}
