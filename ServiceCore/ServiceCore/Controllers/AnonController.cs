using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using ServiceCore;
using ServiceCore.Models;

namespace ServiceCore.Controllers
{
    [Produces("application/json")]
    [Route("api/Anon")]
    public class AnonController : Controller
    {
        protected string[] Prefixes = new string[]
        {
            "Flawless",
            "Bright",
            "Mute",
            "Innocent",
            "Black",
            "False",
            "Numb",
            "Imaginary",
            "Merry",
            "Guilty",
            "Aching",
            "Warm",
            "Crafty",
            "Infinite",
            "Crooked",
            "Sweet",
            "Frozen",
            "Complex",
            "Canine",
            "rctic",
            "General",
            "Precious",
            "Jolly",
            "Late",
            "Modest",
            "Empty",
            "Quiet",
            "Idle",
            "Light",
            "Twinkle",
            "Hollow",
            "Toxic",
            "Fresh",
            "Needy",
            "Mellow",
            "Dual",
            "Fat",
            "Father",
            "Faint",
            "Plain",
            "Fancy",
            "Jungle",
            "Fatal",
            "Critical",
            "Brown",
            "Grim",
            "Distant",
            "Lucky",
            "White",
            "Joint",
            "Golden",
            "Eager",
            "Heavy",
            "Virtual",
            "Good",
            "Wavy",
            "Pretty",
            "Hoarse",
            "Feisty",
            "Hot",
            "Electric",
            "Closed",
            "Evil",
            "Amazing",
            "Humble",
            "Perfect",
            "Fire",
            "Bronze",
            "Shy",
            "Worried",
            "Humming",
            "Hard",
            "Damaged",
            "Coarse",
            "Creepy",
            "Chief",
            "Prime",
            "Alert",
            "Steel",
            "Glass",
            "Prowling",
            "Tired",
            "Puzzled",
            "Bleak",
            "Loyal",
            "Ancient",
            "Fine",
            "Graceful"
        };

        public string[] Suffixes = new string[]
        {
            "Mustang",
            "Packer",
            "Fox",
            "Shadow",
            "Author",
            "Ninja",
            "Balboa",
            "Omega",
            "Flamingo",
            "Tinkerbell",
            "Pebble",
            "Sunburn",
            "Wasp",
            "Neptune",
            "Rosebud",
            "Liberty",
            "Bear",
            "Angel",
            "Dancer",
            "Hound",
            "Tailor",
            "Citadel",
            "Falcon",
            "Hammer",
            "Panther",
            "Warrior",
            "Lizard",
            "Master",
            "Volunteer",
            "Salesman",
            "Baron",
            "Witch",
            "Templer",
            "Widow",
            "Venus",
            "Prince",
            "Giant",
            "Traveler",
            "Guardian",
            "Gambit",
            "Girl",
            "Zeus",
            "Watchdog",
            "Rose",
            "Acrobat",
            "Harpie",
            "Behemoth",
            "Bird",
            "Angler",
            "Geyser",
            "Ghost",
            "Spectator",
            "Riddler",
            "Darling",
            "Volcano",
            "Baroness",
            "Demon",
            "Bandit",
            "Pawn",
            "Hawk",
            "Cayman",
            "Lancer",
            "Wolf",
            "Grandpa",
            "Diamond",
            "Rainbow",
            "Lightfoot",
            "Centurion",
            "Clown",
            "Witness",
            "Beehive",
            "Wizard",
            "Duster",
            "Robin",
            "Dasher",
            "Tiger",
            "Dynamo",
            "Cameo",
            "Starburst",
            "Watchman",
            "Stalker",
            "Grandma",
            "Knight",
            "Boy",
            "Lotus"
        };

        protected string GetRandomName(bool useNumber)
        {
            var rng = new Random();

            string name = string.Empty;
            name += Prefixes[rng.Next(0, Prefixes.Length - 1)];
            name += Suffixes[rng.Next(0, Suffixes.Length - 1)];
            if (useNumber)
                name += rng.Next(10, 999).ToString();

            return name;
        }

        // POST api/values
        [HttpPost]
        public IActionResult Post()
        {
            using (var context = new DataAccess.UserDBContext())
            {
                string name = GetRandomName(false);

                while (!context.ValidAnonName(name))
                    name = GetRandomName(true);

                User usr = new User();
                usr.verification_token = "ANON";
                usr.email = name.ToLowerInvariant();
                usr.enabled = 1;
                usr.hash = context.GenEmailToken();
                usr.verified = 0;

                context.users.Add(usr);
                context.SaveChanges();

                Callsign callsign = new Callsign();
                callsign.user = usr;
                callsign.name = name;
                callsign.enabled = 1;
                callsign.created = DateTime.Now;
                context.callsigns.Add(callsign);
                context.SaveChanges();

                return Ok(new string[] { name, usr.hash });
            }
        }
    }
}