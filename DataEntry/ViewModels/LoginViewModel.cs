using System.ComponentModel.DataAnnotations;

namespace dataentry.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string Nonce { get; set; }
    }
}
