using System.ComponentModel.DataAnnotations;
using BlogApp.Entity;

namespace BlogApp.Models
{
    public class PostCreateViewModel
    {


        public int PostId { get; set; }
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
        public bool IsActive { get; set; }
        public List<Tag> Tags { get; set; } = new();




    }
}