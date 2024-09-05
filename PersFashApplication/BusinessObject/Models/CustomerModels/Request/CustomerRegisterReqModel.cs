using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Models.CustomerModels.Request
{
    public class CustomerRegisterReqModel
    {
        [Required(ErrorMessage = "Username is required")]
        [MaxLength(25, ErrorMessage = "Username contains 25 maximum 25 characters")]
        public string Username { get; set; } = null!;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address format")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Password is required")]
        [RegularExpression("^(?=.*[A-Z])(?=.*\\d)(?=.*[!@#$%^&*])[A-Za-z\\d!@#$%^&*]{6,12}$",
            ErrorMessage = "Password must be 8-12 characters with at least \" +\r\n            \"one uppercase letter, one number, and one special character (!@#$%^&*)")]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters long")]
        public string Password { get; set; } = null!;

        [Required(ErrorMessage = "Confirm Password is required")]
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        public string ConfirmPassword { get; set; } = null!;

        [Required(ErrorMessage ="Full name is required")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Gender is required")]
        public string Gender { get; set; }

        [Required(ErrorMessage = "Date of birth is required")]
        public DateOnly DateOfBirth { get; set; }
    }
}
