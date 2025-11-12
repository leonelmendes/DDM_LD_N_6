using iTaskAPI.Connection;
using iTaskAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace iTaskAPI.Repository.TipoTarefaRepository
{
    public class TipoTarefaRepository : ITipoTarefaRepository
    {
        private readonly ConnectionDB _connection;

        public TipoTarefaRepository(ConnectionDB connection)
        {
            _connection = connection;
        }

        // GET /api/tipotarefa
        // Retorna todos os tipos de tarefa
        public async Task<IEnumerable<TipoTarefa>> GetAllAsync()
        {
            IEnumerable<TipoTarefa> tipos = await _connection.TiposTarefa.ToListAsync();
            return tipos;
        }

        // GET /api/tipotarefa/{id}
        // Retorna um tipo de tarefa específico
        public async Task<TipoTarefa?> GetByIdAsync(int id)
        {
            TipoTarefa? tipo = await _connection.TiposTarefa.FirstOrDefaultAsync(t => t.Id == id);
            return tipo;
        }

        // POST /api/tipotarefa
        // Cria um novo tipo de tarefa
        public async Task AddAsync(TipoTarefa tipoTarefa)
        {
            await _connection.TiposTarefa.AddAsync(tipoTarefa);
            await _connection.SaveChangesAsync();
        }

        // PUT /api/tipotarefa/{id}
        // Atualiza um tipo de tarefa existente
        public async Task UpdateAsync(TipoTarefa tipoTarefa)
        {
            _connection.TiposTarefa.Update(tipoTarefa);
            await _connection.SaveChangesAsync();
        }

        // DELETE /api/tipotarefa/{id}
        // Remove um tipo de tarefa
        public async Task DeleteAsync(TipoTarefa tipoTarefa)
        {
            _connection.TiposTarefa.Remove(tipoTarefa);
            await _connection.SaveChangesAsync();
        }

        // SAVE
        // Salva alterações no banco de dados
        public async Task SaveAsync()
        {
            await _connection.SaveChangesAsync();
        }
    }
}