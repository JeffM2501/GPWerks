using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;

using Services.Models;

namespace Services
{
    public static class UserDB
    {
        public static string HashCredentials(string creds)
        {
            //ConfigurationManager.ConnectionStrings["gpwerk_user_db"].ConnectionString;
        }

        public static int RegisterUser(string email, string credentialHash)
        {

        }
    }
}