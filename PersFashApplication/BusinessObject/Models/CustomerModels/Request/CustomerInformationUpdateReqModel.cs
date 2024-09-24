using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Models.CustomerModels.Request
{
    public class CustomerInformationUpdateReqModel
    {
        [Required (ErrorMessage ="CustomerID is required")]
        public int CustomerId { get; set; }

        public string? Email { get; set; } = null!;

        public string? FullName { get; set; }

        public string? Gender { get; set; }

        public DateOnly? DateOfBirth { get; set; }

        public string? ProfilePicture { get; set; }
    }
}
