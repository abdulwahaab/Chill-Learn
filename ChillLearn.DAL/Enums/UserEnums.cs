using System.ComponentModel;

namespace ChillLearn
{
    public enum UserRoles
    {
        [Description("Student")]
        Student = 1,

        [Description("Teacher")]
        Teacher = 2,

        //[Description("Parent")]
        //Parent = 4,

        //[Description("Admin")]
        //Admin = 3,
        //SuperAdmin = 5
    }

    public enum UserType
    {
        [Description("Student")]
        Student = 1,

        [Description("Teacher")]
        Teacher = 2,

        [Description("Admin")]
        Admin = 3
    }

    public enum UserStatus
    {
        Pending = 1,
        Verified = 2,
        Approved = 3,
        Blocked = 4,
        Deleted = 5
    }

    public enum SignupSource
    {
        App = 1,
        Facebook = 2,
        Linkedin = 3
    }

    public enum SessionType
    {
        [Description("Live Session")]
        Live = 1,

        [Description("Written Session")]
        Written = 2,

    }

    public enum BidStatus
    {
        Created = 1,
        Viewed = 2,
        Offered = 3,
        Accepted = 4,
        Declined = 5,
        Deleted = 6
    }

    public enum ClassStatus
    {
        Created = 1,
        Approved = 2,
        BidAccepted = 3,
        OfferAccepted = 4,
        OfferDeclined = 5,
        Requested = 6,
        Cancelled = 7,
        Completed = 8,
        Archived = 9,
        Deleted = 10
    }

    public enum ClassJoinStatus
    {
        Pending = 1,
        Approved = 2,
        Rejected = 3,
        Processed = 4,
        Invited = 5,
        Accepted = 6
    }
    public enum PaymentSource
    {
        Paypal = 1
    }

    public enum Languages
    {
        [Description("English")]
        English = 1,

        [Description("Arabic")]
        Arabic = 2,
    }
    public enum LanguageLevel
    {
        [Description("Beginner")]
        Beginner = 1,

        [Description("Intermediate")]
        Intermediate = 2,

        [Description("Expert")]
        Expert = 3,

        [Description("Native")]
        Native = 4,
    }

}