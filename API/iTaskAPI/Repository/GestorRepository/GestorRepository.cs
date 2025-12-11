using iTaskAPI.Connection;
using iTaskAPI.Models;
using iTaskAPI.Models.DTOs;
using Microsoft.EntityFrameworkCore;
using System.Net;

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
            // 1. BUSCAR O GESTOR USANDO O ID DO UTILIZADOR (A chave mágica)
            // Usamos o .Include para trazer os dados da tabela pai (Utilizador) junto.
            var gestor = await _connection.Gestores
                .Include(g => g.Utilizador)
                .FirstOrDefaultAsync(g => g.IdUtilizador == dto.Id); // <--- MUDANÇA CRUCIAL: Busca pelo FK

            if (gestor == null) return false;

            // 2. VERIFICAR SE O USERNAME JÁ EXISTE (Para evitar o erro 500 do banco)
            // Verifica na tabela Utilizadores se existe algum username igual, mas ignora o próprio utilizador
            bool usernameEmUso = await _connection.Utilizadores
                .AnyAsync(u => u.Username == dto.Username && u.Id != gestor.IdUtilizador);

            if (usernameEmUso)
            {
                // Lança erro para o Controller apanhar e devolver BadRequest
                throw new InvalidOperationException("Este nome de utilizador já está em uso.");
            }

            // 3. ATUALIZAR DADOS NAS DUAS TABELAS

            // Tabela Específica (Gestores)
            gestor.Departamento = dto.Departamento;

            // Tabela Pai (Utilizadores)
            gestor.Utilizador.Nome = dto.Nome;
            gestor.Utilizador.Username = dto.Username;

            // Atualiza senha apenas se o utilizador digitou algo
            if (!string.IsNullOrWhiteSpace(dto.Password))
            {
                gestor.Utilizador.Password = dto.Password;
            }

            // 4. SALVAR TUDO (O EF Core gera o UPDATE para as duas tabelas sozinho)
            try
            {
                await _connection.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao salvar no banco: {ex.Message}");
                return false;
            }
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