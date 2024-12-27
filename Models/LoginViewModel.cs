using System.ComponentModel.DataAnnotations;

namespace BlogApp.Models
{
    public class LoginViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email Adresi")]
        public string? Email { get; set; }
        [Required]
        [StringLength(10, ErrorMessage = "Max 10 karakter belirtiniz."), MinLength(6, ErrorMessage = "Min 6 karakter belirtin")]
        [Display(Name = "Parola")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

    }
}