using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REGLOGPROP_LEASNOTIF.Models
{
    public class Maintainance
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RequestId { get; set; }
        [Required(ErrorMessage = "Property ID is required.")]
        public int PropertyId { get; set; }
        [Required(ErrorMessage = "Property ID is required.")]
        public string TenantId { get; set; }
        [Required(ErrorMessage = "Description is required.")]
        [StringLength(500, ErrorMessage = "Description cannot be longer than 500 characters.")]
        public string? Description { get; set; }

        [Required]
        public string? Status { get; set; }

        public string? ImagePath { get; set; }
    }
}
