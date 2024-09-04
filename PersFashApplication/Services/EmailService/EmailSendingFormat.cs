using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.EmailService
{
    public class EmailSendingFormat
    {
        public required string Title { get; set; }
        public required string Information { get; set; }
    }
}
