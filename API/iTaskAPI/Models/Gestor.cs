using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace iTaskAPI.Models
{
     [Table("Gestores")]
    public class Gestor
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Departamento { get; set; } = string.Empty;
        [Required]
        public bool IsAdmin { get; set; } = false;
        // Ligação com o Utilizador base
        [Required]
        [ForeignKey("Utilizador")]
        public int IdUtilizador { get; set; }

        [JsonIgnore]
        public Utilizador? Utilizador { get; set; }

        // Relações 1:N
        [JsonIgnore]
        public ICollection<Programador>? Programadores { get; set; }

        [JsonIgnore]
        public ICollection<Tarefa>? Tarefas { get; set; }
    }
}