using System.ComponentModel.DataAnnotations;

namespace brightIdeasTest.Models
{
    public class RegisterViewModel : BaseEntity
    {
        [Required]
        [RegularExpression(@"^[A-Za-z\s]{2,}$", ErrorMessage="Letters and spaces only.")]
        public string Name { get; set; }

        [Required]
        [RegularExpression(@"^[A-Za-z\d]{2,}$", ErrorMessage="Alpha-numeric characters only.")]
        public string Alias { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MinLength(8, ErrorMessage="Password must be at least 8 characters")]
        [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)(?=.*[$@$!%*#?&])[A-Za-z\d$@$!%*#?&]{8,}$", ErrorMessage="Password must contain at least 1 number, 1 letter, and 1 special character.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage="Passwords do not match")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}