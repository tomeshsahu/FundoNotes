using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DatabaseLayer
{
    public class UserModel
    {
        [Required]
        [RegularExpression("^[A-Z][A-Za-z]{3,20}$", ErrorMessage = "Please Enter For Firstname At least 3 Characters and First letter Capital")] //First Name Validation
        public string Firstname { get; set; }
        [Required]
        [RegularExpression("^[A-Z][A-Za-z]{3,20}$", ErrorMessage = "Please Enter For Lastname At least 3 Characters and First letter Capital")] //Last Name Validdation
        public string Lastname { get; set; }
        [Required]
        [RegularExpression("^([A-Za-z0-9]{3,20})([.][A-Za-z0-9]{1,10})*([@][A-Za-z]{2,5})+[.][A-Za-z]{2,3}([.][A-Za-z]{2,3})?$", ErrorMessage = "Please Enter Valid Email Eg.abc123@gamil.com")] //Email validation
        public string Email { get; set; }
        [Required]
        [RegularExpression("(?=.*[A-Z])(?=.*[0-9])(?=.*[@#$_])[a-zA-Z0-9@#$_]{8,}", ErrorMessage = "Please Enter For Password At least 1 numeric,1 Capital char,1 Special char")] //Password Validation
        public string Password { get; set; }
    }
}
