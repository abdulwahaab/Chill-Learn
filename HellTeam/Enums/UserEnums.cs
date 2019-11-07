using System.ComponentModel;

namespace ChillLearn.Enums
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
        Pending = 1,
        Verified = 2,
        Approved = 3,
        Blocked = 4,
        Deleted = 5
    }

    public enum ClassStatus
    {
        Pending = 1,
        Approved = 2,
        Cancelled = 3,
        Deleted = 4
    }

    public enum ClassJoinStatus
    {
        Pending = 1,
        Approved = 2,
        Rejected = 3
    }
}