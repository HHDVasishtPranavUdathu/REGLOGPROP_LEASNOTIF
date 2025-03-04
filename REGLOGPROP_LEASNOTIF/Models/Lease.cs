using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using REGLOGPROP_LEASNOTIF.Models;


namespace REGLOGPROP_LEASNOTIF.Models
{
    public class Lease
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LeaseId { get; set; }

        [Required]
        public string? ID { get; set; }

        [ForeignKey("ID")]
        public virtual Registration? Tenant { get; set; }

        [Required]
        public int? Property_Id { get; set; }

        [ForeignKey("Property_Id")]
        public virtual Prop? Prop { get; set; }

        [Required]
        [DataType(DataType.Date, ErrorMessage = "wrong format")]
        public DateTime? StartDate { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime? EndDate { get; set; }

        [Required]
        public bool? Tenant_Signature { get; set; }

        [Required]
        public bool? Owner_Signature { get; set; }

        [Required]
        public bool? Lease_status { get; set; }
    }
}
