using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace iTaskAPI.Models
{
    [Table("Tarefas")]
    public class Tarefa
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string? Titulo { get; set; }

        [ForeignKey("Gestor")]
        [Required]
        public int IdGestor { get; set; }

        [Required]
        [ForeignKey("Programador")]
        public int IdProgramador { get; set; }

        [ForeignKey("TipoTarefa")]
        [Required]
        public int IdTipoTarefa { get; set; }

        [Required]
        [StringLength(100)]
        public string? OrdemExecucao { get; set; }
        [StringLength(500)]
        [Required]
        public string? Descricao { get; set; }
        [Required]
        public DateTime? DataPrevistaInicio { get; set; }
        [Required]
        public DateTime? DataPrevistaFim { get; set; }
        [Required]
        public int? StoryPoints { get; set; }

        public DateTime? DataRealInicio { get; set; }
        public DateTime? DataRealFim { get; set; }
        public DateTime DataCriacao { get; set; } = DateTime.UtcNow;

        [Required]
        [StringLength(30)]
        public string EstadoAtual { get; set; } = "To Do";

        // Relações
        [JsonIgnore]
        public Gestor? Gestor { get; set; }

        [JsonIgnore]
        public Programador? Programador { get; set; }

        [JsonIgnore]
        public TipoTarefa? TipoTarefa { get; set; }
    }
}