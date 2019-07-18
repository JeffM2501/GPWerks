using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using ServiceCore.Models;

namespace ServiceCore
{
    public static class Emailer
    {
        public static void SendVerificationEmail(User user)
        {

        }

        public static string SendPasswordResetEmail(User user)
        {
            return string.Empty; // return the email address that the email will be from
        }
    }
}
