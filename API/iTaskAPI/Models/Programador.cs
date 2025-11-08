using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace iTaskAPI.Models
{
    public class Programador
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string NivelExperiencia { get; set; } = string.Empty;

        // Ligação com o gestor responsável
        [ForeignKey("Gestor")]
        public int IdGestor { get; set; }
        public Gestor? Gestor { get; set; }

        // Ligação com o utilizador base
        [ForeignKey("Utilizador")]
        public int IdUtilizador { get; set; }
        public Utilizador? Utilizador { get; set; }

        // Relação 1:N
        public ICollection<Tarefa>? Tarefas { get; set; }
    }
}