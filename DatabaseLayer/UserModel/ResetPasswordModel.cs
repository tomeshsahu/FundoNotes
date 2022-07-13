using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DatabaseLayer
{
    public class ResetPasswordModel
    {
        [Required]
        [RegularExpression("(?=.*[A-Z])(?=.*[0-9])(?=.*[@#$_])[a-zA-Z0-9@#$_]{8,}", ErrorMessage = "Please Enter Password At least 1 numeric,1 Capital char,1 Special char")] //Password Validation
        public string Password { get; set; }

        [Required]
        [RegularExpression("(?=.*[A-Z])(?=.*[0-9])(?=.*[@#$_])[a-zA-Z0-9@#$_]{8,}", ErrorMessage = "Confirm Password Must Be Same!")] //Password Validation
        public string CPassword { get; set; }
    }
}
