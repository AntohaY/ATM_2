using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ATM.Models
{
    public class LoginModel
    {
        [Required]
        public string CardId { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string PinCode { get; set; }
    }
}