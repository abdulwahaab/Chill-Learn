using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChillLearn.CustomModels
{
    public class ProfileModel
    {
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public byte[] Picture { get; set; }
        public string ContactNumber { get; set; }
        public string Country { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public byte[] ProfileImage { get; set; }
        public DateTime BirthDate { get; set; }
    }
}
