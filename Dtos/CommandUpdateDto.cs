using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Commander.Dtos
{
    public class CommandUpdateDto
    {
        // public int Id { get; set; } ... is created by our database no need to include it before hand
        [Required]
        [MaxLength(250)]
        public string HowTo { get; set; }

        [Required]  //  <--------------------------\
        public string Line { get; set; }            //  -Gives us a 400 bad request error instead of 500 and detailed error feedback JSON text in Postman to troubleshoot                               

        [Required]  //  <---------------------------/
        public string Platform { get; set; }
    }
}
