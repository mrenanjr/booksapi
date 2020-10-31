using System.ComponentModel.DataAnnotations;

namespace booksapi.Models
{
    public class Book
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "O título é obrigatório.")]
        [MinLength(3, ErrorMessage = "Título deve conter no mínimo 3 caracteres.")]
        public string Title { get; set; }
        [Required(ErrorMessage = "O ano é obrigatório.")]
        public int Year { get; set; }
        public int AuthorId { get; set; }
        public Author Author { get; set; }
    }
}
