
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
}