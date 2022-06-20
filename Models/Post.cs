﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace csharp_blog_backend.Models
{

    public class Post
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Il campo è obbligatorio")]
        [StringLength(75, ErrorMessage = "Il titolo non può essere oltre i 75 caratteri")]

        public string Title { get; set; }

        [Required(ErrorMessage = "Il campo è obbligatorio")]
        [Column(TypeName = "text")]
        public string Description { get; set; }

        public string? Image { get; set; }

        [NotMapped()]
        public IFormFile? File { get; set; } = null;

        //crezione blob per salvare immagini
        public byte[]? ImageBytes { get; set; }

        public Post()
        {

        }

        public Post(string title, string description, string image)
        {
            this.Title = title;
            this.Description = description;
            this.Image = image;
        }
    }
}