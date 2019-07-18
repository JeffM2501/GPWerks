using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using ServiceCore.Models;

namespace ServiceCore.DataAccess
{
    public class UserDBContext : DbContext
    {
        private Random RNG = new Random();

        public DbSet<User> users { get; set; }
        public DbSet<AccessToken> access_tokens { get; set; }
        public DbSet<Callsign> callsigns { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySQL("server=localhost;database=gpwerks_users;user=gpuser");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public User FindByEmail(string email)
        {
            var userswithEmail = users.Where(s => s.email == email.ToLowerInvariant() && s.enabled != 0).ToList();

            return userswithEmail.FirstOrDefault<User>();
        }

        public User FindUserByToken(string token)
        {
            var tok = GetAccessToken(token);
            if (tok == null || tok.user == null)
                return null;

            return tok.user;
        }

        public IEnumerable<AccessToken> AccessTokensForUser(User user)
        {
            return access_tokens.Where(s => s.user == user && s.expires > DateTime.Now).ToList();
        }

        public AccessToken GetAccessToken(string token)
        {
            return access_tokens.Where(s => s.token == token && s.expires > DateTime.Now).ToList().FirstOrDefault();
        }

        public bool RefreshToken(string token)
        {
            var tok = GetAccessToken(token);
            if (tok == null)
                return false;

            tok.expires = DateTime.Now + new TimeSpan(0, 5, 0);

            return SaveChanges() > 1;
        }

        public IEnumerable<Callsign> CallsignsForUser(User user)
        {
            return callsigns.Where(s => s.user == user && s.enabled != 0).ToList();
        }

        public bool CallsignAvailable(string name)
        {
            return callsigns.Where(s => s.name.ToLowerInvariant() == name.ToLowerInvariant()).FirstOrDefault() == null;
        }

        public string GenEmailToken()
        {
            return RNG.Next(1000, 9999).ToString() + RNG.Next(1000, 9999).ToString();
        }

        private string GenToken()
        {

            return RNG.Next(1000, 9999).ToString() + RNG.Next(1000, 9999).ToString() + RNG.Next(1000, 9999).ToString() + RNG.Next(1000, 9999).ToString();
        }

        protected bool AccessTokenExists(string tok)
        {
            var toks = access_tokens.Where(s => s.token == tok && s.expires > DateTime.Now).ToList();

            return toks.Count != 0;
        }

        public string GetAccessToken()
        {
            string tok = GenToken();
            while (AccessTokenExists(tok))
                tok = GenToken();

            return tok;
        }
    }
}
