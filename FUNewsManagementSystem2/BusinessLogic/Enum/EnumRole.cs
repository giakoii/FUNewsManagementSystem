using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Enum
{
    public enum EnumRole
    {
        Staff = 1,
        Lecturer = 2,
        Admin = 3
    }
    
    public static class ConstRole
    {
        public const string Staff = "Staff";
        public const string Lecturer = "Lecturer";
        public const string Admin = "Admin";
    }
}
