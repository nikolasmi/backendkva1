using Microsoft.AspNetCore.Identity;

namespace projekatKVA.DTOs
{
    public class UserDTO
    {
        public int UserId { get; set; }

        public string Name { get; set; } = null!;

        public string Surname { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string Phone { get; set; } = null!;

        public string Address { get; set; } = null!;

        public string Username { get; set; } = null!;

        public string Password { get; set; } = null!;

        public int FavouriteItems { get; set; } = 0;
    }
}
