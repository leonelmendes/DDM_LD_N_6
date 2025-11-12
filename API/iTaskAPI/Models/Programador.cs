using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace iTaskAPI.Models
{
    [Table("Programadores")]
    public class Programador
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string NivelExperiencia { get; set; } = string.Empty;

        // Ligação com Gestor
        [ForeignKey("Gestor")]
        [Required]
        public int IdGestor { get; set; }

        [JsonIgnore]
        public Gestor? Gestor { get; set; }

        // Ligação com Utilizador
        [ForeignKey("Utilizador")]
        [Required]
        public int IdUtilizador { get; set; }

        [JsonIgnore]
        public Utilizador? Utilizador { get; set; }

        // Relação 1:N com Tarefas
        [JsonIgnore]
        public ICollection<Tarefa>? Tarefas { get; set; }
    }
}