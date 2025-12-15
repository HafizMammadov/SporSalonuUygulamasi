using Microsoft.AspNetCore.Identity;
using System;

namespace SporSalonuUygulamasi.Models
{
    public class AppUser : IdentityUser
    {
        public string FirstName { get; set; } // ? Eklendi
        public string LastName { get; set; } // ? Eklendi

        public DateTime? BirthDate { get; set; }
        public string Gender { get; set; }
    }
}