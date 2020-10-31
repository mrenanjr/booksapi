using System.ComponentModel.DataAnnotations;

namespace booksapi.Models
{
    public class Author
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "O nome do autor é obrigatório.")]
        [MinLength(10, ErrorMessage = "O nome deve conter no mínimo 10 caracteres.")]
        public string Name { get; set; }
    }
}
