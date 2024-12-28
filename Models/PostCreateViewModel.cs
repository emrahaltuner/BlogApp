using System.ComponentModel.DataAnnotations;

namespace BlogApp.Models
{
    public class PostCreateViewModel
    {



        [Required]
        [Display(Name = "Başlık")]
        public string? Title { get; set; }
        [Required]
        [Display(Name = "İçerik")]
        public string? Content { get; set; }
        [Required]
        [Display(Name = "Description")]
        public string? Desc { get; set; }

        [Display(Name = "Blog Resim")]
        public string? Image { get; set; }
        [Display(Name = "Url")]
        public string? Url { get; set; }




    }
}