using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace iTaskAPI.Models
{
    [Table("TiposTarefa")]
    [Index(nameof(Nome), IsUnique = true)]
    public class TipoTarefa
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Nome { get; set; } = string.Empty;

        [JsonIgnore]
        public ICollection<Tarefa>? Tarefas { get; set; }
    }
}