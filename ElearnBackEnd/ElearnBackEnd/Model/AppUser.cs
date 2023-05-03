using Microsoft.AspNetCore.Identity;

namespace ElearnBackEnd.Model
{
    public class AppUser:IdentityUser
    {
        public string FullName { get; set; }
    }
}
