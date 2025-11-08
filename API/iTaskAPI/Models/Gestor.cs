using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace iTaskAPI.Models
{
    public class Gestor
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string Departamento { get; set; } = string.Empty;

        public bool IsAdmin { get; set; } = false;

        // Chave estrangeira para o Utilizador base
        [ForeignKey("Utilizador")]
        public int IdUtilizador { get; set; }
        public Utilizador? Utilizador { get; set; }

        // Relações 1:N
        public ICollection<Programador>? Programadores { get; set; }
        public ICollection<Tarefa>? Tarefas { get; set; }

    }
}