
using System.ComponentModel;

namespace ChillLearn.Enums
{
    public enum UserRoles
    {
        [Description("Student")]
        Student = 1,

        [Description("Teacher")]
        Teacher = 2,

        [Description("Parent")]
        Parent = 3
        //Admin = 4,
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
}