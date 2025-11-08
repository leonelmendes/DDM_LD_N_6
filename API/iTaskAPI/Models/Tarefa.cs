using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace iTaskAPI.Models
{
    public class Tarefa
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey("Gestor")]
        public int IdGestor { get; set; }

        [ForeignKey("Programador")]
        public int IdProgramador { get; set; }

        [ForeignKey("TipoTarefa")]
        public int IdTipoTarefa { get; set; }

        public string? OrdemExecucao { get; set; }
        public string? Descricao { get; set; }
        public DateTime? DataPrevistaInicio { get; set; }
        public DateTime? DataPrevistaFim { get; set; }
        public int? StoryPoints { get; set; }
        public DateTime? DataRealInicio { get; set; }
        public DateTime? DataRealFim { get; set; }
        public DateTime DataCriacao { get; set; } = DateTime.UtcNow;

        [Required]
        public string EstadoAtual { get; set; } = "To Do";

        // Relações
        public Gestor? Gestor { get; set; }
        public Programador? Programador { get; set; }
        public TipoTarefa? TipoTarefa { get; set; }

    }
}