using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.UtilsTools
{
    public static class Utils
    {
        //just to demonstrate usage of utils class and its methods
        public static bool ValidateEmail(string email)
        {
            if (email == null ||
                email.Length == 0 ||
                !email.Contains('@') ||
                email.IndexOf('@') != email.LastIndexOf('@')) 
                return false;

            return true;
        }
    }
}
