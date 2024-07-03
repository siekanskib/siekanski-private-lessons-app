using Microsoft.AspNetCore.Identity;


namespace siekanski_private_lessons_backend.Models
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Role { get; set; }
    }
}

