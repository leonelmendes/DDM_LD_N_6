using iTaskAPI.Connection;
using iTaskAPI.Models;
using iTaskAPI.Models.DTOs;
using Microsoft.EntityFrameworkCore;

namespace iTaskAPI.Repository.ProgramadorRepository
{
    public class ProgramadorRepository : IProgramadorRepository
    {
        private readonly ConnectionDB _connection;

        public ProgramadorRepository(ConnectionDB connection)
        {
            _connection = connection;
        }

        // GET /api/programador
        // Retorna todos os programadores
        public async Task<IEnumerable<Programador>> GetAllAsync()
        {
            IEnumerable<Programador> programadores = await _connection.Programadores
                .Include(p => p.Utilizador)
                .Include(p => p.Gestor)
                .ToListAsync();

            return programadores;
        }

        public async Task<bool> UpdatePerfilProgramadorAsync(UpdateProgramadorProfileDTO dto)
        {
            var dev = await _connection.Programadores
                .Include(p => p.Utilizador)
                .FirstOrDefaultAsync(p => p.Id == dto.Id);

            if (dev == null) return false;

            // Tabela Programador
            dev.NivelExperiencia = dto.NivelExperiencia;

            // Tabela Utilizador
            dev.Utilizador.Nome = dto.Nome;
            dev.Utilizador.Username = dto.Username;

            if (!string.IsNullOrWhiteSpace(dto.Password))
            {
                dev.Utilizador.Password = dto.Password;
            }

            await _connection.SaveChangesAsync();
            return true;
        }

        public async Task<List<ProgramadorCardEquipeDTO>> GetProgramadoresParaEquipeAsync()
        {
            return await _connection.Programadores
                .AsNoTracking() // Performance: não precisamos rastrear alterações aqui
                .Select(p => new ProgramadorCardEquipeDTO
                {
                    Id = p.Id,

                    // Busca o nome na tabela Utilizadores via relacionamento
                    Nome = p.Utilizador.Nome,

                    NivelExperiencia = p.NivelExperiencia,

                    // CONTAGEM DE ATIVAS:
                    // Consideramos "Ativas" tudo o que NÃO está "Done".
                    // Ou seja, soma de "To Do" e "Doing".
                    Ativas = p.Tarefas.Count(t => t.EstadoAtual != "Done"),

                    // CONTAGEM DE CONCLUÍDAS:
                    // Apenas as que estão exatamente como "Done".
                    Concluidas = p.Tarefas.Count(t => t.EstadoAtual == "Done")
                })
                .ToListAsync();
        }

        public async Task<List<ProgramadorCardEquipeDTO>> GetProgramadoresPorGestorAsync(int idGestor)
        {
            // A única alteração é o .Where(p => p.IdGestor == idGestor)
            return await _connection.Programadores
                .AsNoTracking()
                .Where(p => p.IdGestor == idGestor) // FILTRO APLICADO AQUI!
                .Select(p => new ProgramadorCardEquipeDTO
                {
                    Id = p.Id,
                    Nome = p.Utilizador.Nome,
                    NivelExperiencia = p.NivelExperiencia,
                    Ativas = p.Tarefas.Count(t => t.EstadoAtual != "Done"),
                    Concluidas = p.Tarefas.Count(t => t.EstadoAtual == "Done")
                })
                .ToListAsync();
        }

        public async Task<ProgramadorDetalhe> GetDetalheByIdAsync(int id)
        {
            return await _connection.Programadores
                .AsNoTracking()
                .Where(p => p.Id == id)
                .Select(p => new ProgramadorDetalhe
                {
                    Id = p.Id,
                    // Dados da tabela Utilizador (Programador)
                    Nome = p.Utilizador.Nome,
                    Email = p.Utilizador.Email,
                    Username = p.Utilizador.Username,

                    // Dados da tabela Programador
                    NivelExperiencia = p.NivelExperiencia,

                    // Dados da tabela Gestor -> Utilizador (Nome do Gestor)
                    // Navegamos: Programador -> Gestor -> Utilizador -> Nome
                    GestorResponsavel = p.Gestor.Utilizador.Nome
                })
                .FirstOrDefaultAsync();
        }

        // GET /api/programador/{id}
        // Retorna um programador específico
        public async Task<Programador?> GetByIdAsync(int id)
        {
            Programador? programador = await _connection.Programadores
                .Include(p => p.Utilizador)
                .Include(p => p.Gestor)
                .Include(p => p.Tarefas)
                .FirstOrDefaultAsync(p => p.Id == id);

            return programador;
        }

        // GET /api/programador/{id}/tarefas
        // Retorna todas as tarefas atribuídas a um programador
        public async Task<IEnumerable<Tarefa>> GetTarefasByProgramadorAsync(int programadorId)
        {
            IEnumerable<Tarefa> tarefas = await _connection.Tarefas
                .Include(t => t.Gestor)
                .Include(t => t.TipoTarefa)
                .Where(t => t.IdProgramador == programadorId)
                .ToListAsync();

            return tarefas;
        }

        // POST /api/programador
        // Cria um novo programador
        public async Task AddAsync(Programador programador)
        {
            await _connection.Programadores.AddAsync(programador);
            await _connection.SaveChangesAsync();
        }

        // PUT /api/programador/{id}
        // Atualiza os dados de um programador
        public async Task<bool> UpdateAsync(ProgramadorDetalhe model)
        {
            // 1. Busca o Programador e INCLUI os dados da tabela Utilizador
            // Isso é fundamental para podermos editar Nome, Email e Username
            var programadorBanco = await _connection.Programadores
                .Include(p => p.Utilizador)
                .FirstOrDefaultAsync(p => p.Id == model.Id);

            if (programadorBanco == null)
            {
                return false; // Programador não encontrado
            }

            // 2. Atualizar dados da tabela PROGRAMADOR
            programadorBanco.NivelExperiencia = model.NivelExperiencia;

            // 3. Atualizar dados da tabela UTILIZADOR (Via Navegação)
            // Como usamos o .Include acima, o objeto .Utilizador não é nulo
            if (programadorBanco.Utilizador != null)
            {
                programadorBanco.Utilizador.Nome = model.Nome;
                programadorBanco.Utilizador.Email = model.Email;
                programadorBanco.Utilizador.Username = model.Username;
            }

            // 4. Lógica Especial: Atualizar o GESTOR (String vs Int)
            // O modelo tem "Nome do Gestor" (string), mas o banco pede "IdGestor" (int).
            if (!string.IsNullOrEmpty(model.GestorResponsavel))
            {
                // Tentamos achar o ID do gestor baseando-se no nome fornecido
                // Nota: Isso assume que Gestor tem um Utilizador associado para vermos o nome
                var gestorEncontrado = await _connection.Gestores
                    .Include(g => g.Utilizador)
                    .FirstOrDefaultAsync(g => g.Utilizador.Nome == model.GestorResponsavel);

                if (gestorEncontrado != null)
                {
                    programadorBanco.IdGestor = gestorEncontrado.Id;
                }
            }

            // 5. Salva tudo (EF gerencia a transação para ambas as tabelas)
            try
            {
                await _connection.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateException)
            {
                // Tratar erros de banco (ex: Email ou Username duplicado)
                return false;
            }
        }

        // DELETE /api/programador/{id}
        // Remove um programador
        public async Task DeleteAsync(Programador programador)
        {
            _connection.Programadores.Remove(programador);
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