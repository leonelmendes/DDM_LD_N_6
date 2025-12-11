using iTaskAPI.Connection;
using iTaskAPI.Models;
using iTaskAPI.Models.DTOs;
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

        public async Task<bool> UpdatePerfilGestorAsync(UpdateGestorProfileDTO dto)
        {
            // O SEGREDO: Usar .Include(x => x.Utilizador)
            var gestor = await _connection.Gestores
                .Include(g => g.Utilizador) // Traz os dados da tabela pai
                .FirstOrDefaultAsync(g => g.Id == dto.Id);

            if (gestor == null) return false;

            // 1. Atualiza dados da tabela Gestor
            gestor.Departamento = dto.Departamento;

            // 2. Atualiza dados da tabela Utilizador (Graças ao Include)
            gestor.Utilizador.Nome = dto.Nome;
            gestor.Utilizador.Username = dto.Username;

            // 3. Lógica de Password (Opcional)
            if (!string.IsNullOrWhiteSpace(dto.Password))
            {
                gestor.Utilizador.Password = dto.Password; // Idealmente, usar hash aqui
            }

            // 4. Salva tudo numa única transação
            await _connection.SaveChangesAsync();
            return true;
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