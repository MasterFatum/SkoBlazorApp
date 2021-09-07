using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace SkoBlazorApp
{
    public class Course
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        [Required(ErrorMessage = "Это поле должно быть заполнено!")]
        public string Title { get; set; }
        public int? Evaluation { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string Category { get; set; }
        public string Date { get; set; }
        public string Hyperlink { get; set; }
        public string Evaluating { get; set; }
        public string FileName { get; set; }
        public string DateEdit { get; set; }
        public int Year { get; set; }

        public  User User { get; set; }
    }
}
