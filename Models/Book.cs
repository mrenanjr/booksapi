using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace booksapi.Models
{
    public class Book
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "O título é obrigatório.")]
        [MinLength(3, ErrorMessage = "Título deve conter no mínimo 3 caracteres.")]
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public string Summary { get; set; }

        [Required(ErrorMessage = "A quantidade de páginas é obrigatóra.")]
        public int PageNumber { get; set; }
        public int AuthorId { get; set; }
        public Author Author { get; set; }
    }
}
