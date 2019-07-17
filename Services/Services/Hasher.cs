using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Cryptography;
using System.Configuration;

namespace Services
{
    public static class Hasher
    {
        internal static string Pepper = "asdkjlf902[3yu5rq0wdfhna;lojsdhnfasdfasdfha-t9y]24-=r9uwodgajzskfd";

        public static string GenHash(string password)
        {
            string p = ConfigurationManager.AppSettings.Get("GPWerks.User.Pepper");
            if (string.IsNullOrEmpty(p))
                p = Pepper;

            byte[] salt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            var pdb = new System.Security.Cryptography.Rfc2898DeriveBytes(password + p, salt);

            return "1" + Convert.ToBase64String(pdb.GetBytes(32)) + ":" + Convert.ToBase64String(salt);
        }

        public static bool ValidateHash(string password, string hash)
        {
            try
            {
                string[] nubs = hash.Split(":".ToCharArray());
                if (nubs.Length != 2)
                    return false;

                byte[] salt = Convert.FromBase64String(nubs[1]);
                string p = ConfigurationManager.AppSettings.Get("GPWerks.User.Pepper");
                if (string.IsNullOrEmpty(p))
                    p = Pepper;

                var pdb = new System.Security.Cryptography.Rfc2898DeriveBytes(password + p, salt);
                return nubs[0] == "1" + Convert.ToBase64String(pdb.GetBytes(32));
            }
            catch (Exception)
            {
                return false;
            }
          
        }
    }
}