using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace iTaskAPI.Models
{
    public class Utilizador
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string? Nome { get; set; }
        [Required]
        public string? Email { get; set; }
        [Required]
        public string? Password { get; set; }
       
       // Relações com outras entidades podem ser adicionadas aqui
       public Gestor? Gestor { get; set; }
       public Programador? Programador { get; set; }
    }
}