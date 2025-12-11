using iTask_App_Mobile.DTOs;
using iTask_App_Mobile.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace iTask_App_Mobile.Services.ProgramadorService
{
    public class ProgramadorService : IProgramadorService
    {
        private string BaseUrl =>
            DeviceInfo.Platform == DevicePlatform.Android
            ? "http://10.0.2.2:5055"
            : "http://localhost:5055";

        public async Task<List<ProgramadorCardEquipeDTO>> GetAllAsync()
        {
            var url = DeviceInfo.Platform == DevicePlatform.Android
                ? "http://10.0.2.2:5055/api/Programador/EquipeCards"
                : "http://localhost:5055/api/Programador/EquipeCards";

            try
            {
                using (var client = new HttpClient())
                {
                    var resp = await client.GetAsync(url);

                    if (resp.IsSuccessStatusCode)
                    {
                        return await resp.Content.ReadFromJsonAsync<List<ProgramadorCardEquipeDTO>>();
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erro ao buscar programadores: {ex.Message}");
            }

            return new List<ProgramadorCardEquipeDTO>(); // Retorna lista vazia em caso de erro
        }

        public async Task<DashboardDTO> GetDashboardProgramadorAsync(int idUtilizador)
        {
            string url = $"{BaseUrl}/api/Tarefa/DashboardProgramador/{idUtilizador}";
            try
            {
                using HttpClient _httpClient = new HttpClient();
                return await _httpClient.GetFromJsonAsync<DashboardDTO>(url) ?? new DashboardDTO();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro Dashboard Dev: {ex.Message}");
                return new DashboardDTO();
            }
        }

        public async Task<List<ProgramadorCardEquipeDTO>> GetByGestorAsync(int idGestor)
        {
            var url = DeviceInfo.Platform == DevicePlatform.Android
                ? $"http://10.0.2.2:5055/api/Programador/EquipeCards/{idGestor}"
                : $"http://localhost:5055/api/Programador/EquipeCards/{idGestor}";

            // ... lógica de HttpClient e desserialização (usando PropertyNameCaseInsensitive) ...
            // Deves reutilizar o mesmo bloco de HttpClient do GetAllAsync

            using (var client = new HttpClient())
            {
                var resp = await client.GetAsync(url);
                // ... (resto da lógica de desserialização, conforme implementado) ...
                if (resp.IsSuccessStatusCode)
                {
                    // Assumindo que a desserialização está no padrão case-insensitive:
                    return await resp.Content.ReadFromJsonAsync<List<ProgramadorCardEquipeDTO>>();
                }
            }
            return new List<ProgramadorCardEquipeDTO>();
        }

        public async Task<ProgramadorDetalhe> GetDetalheAsync(int id)
        {
            var url = DeviceInfo.Platform == DevicePlatform.Android
                ? $"http://10.0.2.2:5055/api/Programador/{id}/detalhe"
                : $"http://localhost:5055/api/Programador/{id}/detalhe";

            using (var client = new HttpClient())
            {
                var resp = await client.GetAsync(url);

                if (resp.IsSuccessStatusCode)
                {
                    var options = new System.Text.Json.JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };
                    return await resp.Content.ReadFromJsonAsync<ProgramadorDetalhe>(options);
                }
            }
            return null;
        }

        public async Task<IEnumerable<ProgramadorListDTO>> GetAllProgramadoresAsync()
        {
            string url = $"{BaseUrl}/api/Programador";

            using HttpClient client = new HttpClient();
            try
            {
                return await client.GetFromJsonAsync<IEnumerable<ProgramadorListDTO>>(url)
                       ?? Enumerable.Empty<ProgramadorListDTO>();
            }
            catch
            {
                return Enumerable.Empty<ProgramadorListDTO>();
            }
        }

        public async Task<IEnumerable<ProgramadorListDTO>> GetProgramadoresByGestorAsync(int gestorId)
        {
            string url = $"{BaseUrl}/api/Gestor/GetProgramadores/{gestorId}/programadores";

            using HttpClient client = new HttpClient();

            try
            {
                var data = await client.GetFromJsonAsync<IEnumerable<ProgramadorListDTO>>(url);

                if (data == null)
                    Console.WriteLine("[GET PROGRAMADORES] Retornou null");

                Console.WriteLine($"[GET PROGRAMADORES] Chamou: {url}");
                return data ?? Enumerable.Empty<ProgramadorListDTO>();
            }
            catch (Exception ex)
            {
                Console.WriteLine("[GET PROGRAMADORES] Erro: " + ex.Message);
                return Enumerable.Empty<ProgramadorListDTO>();
            }
        }

        public async Task<ProgramadorDetalhe?> GetProgramadorByIdAsync(int id)
        {
            string url = $"{BaseUrl}/api/Programador/{id}/detalhe";

            using HttpClient client = new HttpClient();
            try
            {
                return await client.GetFromJsonAsync<ProgramadorDetalhe>(url);
            }
            catch
            {
                return null;
            }
        }

        public async Task<bool> CreateProgramadorAsync(CreateProgramadorDTO model)
        {
            string url = $"{BaseUrl}/api/Authenticate/register-programmer";

            using HttpClient http = new HttpClient();
            var response = await http.PostAsJsonAsync(url, model);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateProgramadorAsync(ProgramadorDetalhe programador)
        {
            try
            {
                using HttpClient http = new HttpClient();
                // 1. Serializar o objeto para JSON
                string json = JsonSerializer.Serialize(programador);

                // 2. Configurar o conteúdo da requisição
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                // 3. Enviar o PUT (Note o ID na URL)
                string url = $"{BaseUrl}/api/Programador/Update/{programador.Id}";
                HttpResponseMessage response = await http.PutAsync(url, content);

                // 4. Verificar se deu sucesso (Status 204 ou 200)
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                // Logar o erro
                Console.WriteLine($"Erro ao atualizar: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> DeleteProgramadorAsync(int id)
        {
            string url = $"{BaseUrl}/api/Programador/Delete/{id}";

            using HttpClient client = new HttpClient();
            var response = await client.DeleteAsync(url);
            return response.IsSuccessStatusCode;
        }
    }
}

