using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Models.CustomerModels.Response
{
    public class CustomerInformationViewModel
    {
        public int CustomerId { get; set; }

        public string Username { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string? FullName { get; set; }

        public string? Gender { get; set; }

        public DateOnly? DateOfBirth { get; set; }

        public string? ProfilePicture { get; set; }

        public DateTime? DateJoined { get; set; }

        public string? Status { get; set; }

        public List<string>? Subscription { get; set; }

        public bool? IsDoneProfileSetup { get; set; }
    }
}
