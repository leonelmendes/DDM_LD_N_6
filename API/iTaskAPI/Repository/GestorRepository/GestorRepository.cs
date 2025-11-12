using iTaskAPI.Connection;
using iTaskAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace iTaskAPI.Repository.GestorRepository
{
    public class GestorRepository : IGestorRepository
    {
        private readonly ConnectionDB _connection;

        public GestorRepository(ConnectionDB connection)
        {
            _connection = connection;
        }

        // Listar todos os gestores
        public async Task<IEnumerable<Gestor>> GetAllAsync()
        {
            // Busca todos os gestores com o Utilizador associado
            IEnumerable<Gestor> gestores = await _connection.Gestores
                .Include(g => g.Utilizador)
                .Include(g => g.Programadores)
                .ToListAsync();

            return gestores;
        }

        // Buscar um gestor pelo ID
        public async Task<Gestor?> GetByIdAsync(int id)
        {
            // Busca o gestor pelo ID e inclui o Utilizador associado
            Gestor? gestor = await _connection.Gestores
                .Include(g => g.Utilizador)
                .Include(g => g.Programadores)
                .Include(g => g.Tarefas)
                .FirstOrDefaultAsync(g => g.Id == id);

            return gestor;
        }

        // Buscar programadores de um gestor específico
        public async Task<IEnumerable<Programador>> GetProgramadoresByGestorAsync(int gestorId)
        {
            // Busca todos os programadores cujo IdGestor é o mesmo do parâmetro
            IEnumerable<Programador> programadores = await _connection.Programadores
                .Include(p => p.Utilizador)
                .Where(p => p.IdGestor == gestorId)
                .ToListAsync();

            return programadores;
        }

        // Criar um novo gestor
        public async Task AddAsync(Gestor gestor)
        {
            await _connection.Gestores.AddAsync(gestor);
            await _connection.SaveChangesAsync();
        }

        // Atualizar um gestor existente
        public async Task UpdateAsync(Gestor gestor)
        {
            _connection.Gestores.Update(gestor);
            await _connection.SaveChangesAsync();
        }

        // Excluir um gestor
        public async Task DeleteAsync(Gestor gestor)
        {
            _connection.Gestores.Remove(gestor);
            await _connection.SaveChangesAsync();
        }

        // Salvar alterações manuais
        public async Task SaveAsync()
        {
            await _connection.SaveChangesAsync();
        }
    }
}