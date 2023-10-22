using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Commander.Models
{
    public class Command
    {   
        [Key]
        public int Id { get; set; }

        [Required] //   <- ex of data annotation
        [MaxLength(250)]          //   <- Constrain length of the string
        public string HowTo { get; set; }

        [Required]
        public string Line { get; set; }

        [Required]
        public string Platform { get; set; }
    }
}
