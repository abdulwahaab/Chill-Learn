using System;

namespace ChillLearn.ViewModels
{
    public partial class NotificationModel
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string FromUser { get; set; }
        public string ToUser { get; set; }
        public string URL { get; set; }
        public int Type { get; set; }
        public bool IsRead { get; set; }
        public DateTime? CreationDate { get; set; }
        public DateTime? ReadingDate { get; set; }
    }
}
