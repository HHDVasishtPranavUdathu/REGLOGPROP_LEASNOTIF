using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using REGLOGPROP_LEASNOTIF.Models;


namespace REGLOGPROP_LEASNOTIF.Models
{
    public class Prop
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Property_Id { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public bool AvailableStatus { get; set; }

        [ForeignKey("Registration")]
        public string Owner_Id { get; set; }

        public Registration? Registration { get; set; }

        public string Owner_Name => Registration?.Name;

        public long Owner_PhoneNumber => Registration?.PhoneNumber ?? 0;
    }
}
