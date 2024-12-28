using System.ComponentModel.DataAnnotations;

namespace BlogApp.Models
{
    public class RegisterViewModel
    {
        [Required]
        [Display(Name = "Kullanıcı Adı")]
        public string? UserName { get; set; }
        [Required]
        [Display(Name = "Ad Soyad")]
        public string? Name { get; set; }
        [Required]
        [EmailAddress]
        [Display(Name = "Email Adresi")]
        public string? Email { get; set; }
        [Required]
        [StringLength(10, ErrorMessage = "Max 10 karakter belirtiniz."), MinLength(6, ErrorMessage = "Min 6 karakter belirtin")]
        [Display(Name = "Parola")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        [Required]
        [Display(Name = "Parola Tekrar")]
        [Compare(nameof(Password), ErrorMessage = "Parolanız Eşleşmiyor")]
        [DataType(DataType.Password)]
        public string? ConfirmPassword { get; set; }
        [Display(Name = "Profil Resmi")]
        public string? Image { get; set; }


    }
}