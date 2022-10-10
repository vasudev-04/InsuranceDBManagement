using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace InsuranceMgmtDB
{
    public class ResetPwdModel
    {
       
            [Required]
            public string currentPassword { get; set; }
            //[Required]
            //public string token { get; set; }
            [Required]
            [DataType(DataType.Password)]
            public string NewPassword { get; set; }
            [Required]
            [DataType(DataType.Password)]
            [System.ComponentModel.DataAnnotations.Compare("NewPassword", ErrorMessage = "New Password and confirmNew Password Should be Same!!!!!")]
            public string confirmNewPassword { get; set; }
        
    }
}