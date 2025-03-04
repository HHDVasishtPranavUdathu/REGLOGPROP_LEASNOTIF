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
    public class Notification
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public int Notification_Id { get; set; }

        [Required]
        public string? sendersId { get; set; }

        [Required]
        public string? receiversId { get; set; }

        [DataType(DataType.DateTime)]
        [Required]
        public DateTime CreatedDate { get; set; }

        [Required]
        public string? notification_Descpirtion { get; set; }
    }
}
