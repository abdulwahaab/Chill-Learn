using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChillLearn.CustomModels
{
    class GenericModels
    {
    }
    public class RequestsModel
    {
        public int Id { get; set; }
        public string ClassId { get; set; }
        public string ClassTitle { get; set; }
        public string StudentName { get; set; }
        public string StudentId { get; set; }
        public byte[] ProfilePicture { get; set; }
        public int? RequestStatus { get; set; }
        public DateTime? RequestDate { get; set; }


    }
}
