using iTask_App_Mobile.DTOs;
using iTask_App_Mobile.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace iTask_App_Mobile.Services.TarefaService
{
    public class TarefaService : ITarefaService
    {

        private string BaseUrl =>
            DeviceInfo.Platform == DevicePlatform.Android
            ? "http://10.0.2.2:5055"
            : "http://localhost:5055";

        public async Task<bool> CriarTarefaAsync(TarefaModel model)
        {
            string url = $"{BaseUrl}/api/Tarefa/Create";

            using HttpClient client = new HttpClient();
            var response = await client.PostAsJsonAsync(url, model);
            return response.IsSuccessStatusCode;
        }

        public async Task<double> GetPrevisaoEntregaAsync()
        {
            string url = $"{BaseUrl}/api/Tarefa/PrevisaoTempoToDo";
            try
            {
                // Supondo que a API retorne um JSON { "totalHorasPrevistas": 12.5 }
                // Ou se retornar direto o double:
                using HttpClient client = new HttpClient();
                var response = await client.GetStringAsync(url);

                // Parse simples (ajuste conforme o JSON de retorno da sua API)
                using var doc = JsonDocument.Parse(response);
                return doc.RootElement.GetProperty("totalHorasPrevistas").GetDouble();
            }
            catch
            {
                return 0;
            }
        }

        public async Task<DashboardDTO> GetDashboardGlobalAsync()
        {
            string url = $"{BaseUrl}/api/Tarefa/DashboardGlobal";
            try
            {
                using HttpClient _httpClient = new HttpClient();
                return await _httpClient.GetFromJsonAsync<DashboardDTO>(url) ?? new DashboardDTO();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro Dashboard: {ex.Message}");
                return new DashboardDTO { PrevisaoTexto = "--" };
            }
        }

        public async Task<IEnumerable<TarefaDetalhesDTO>> GetTarefasDetalhesAsync()
        {
            string url = $"{BaseUrl}/api/Tarefa/GetAllTarefasDetalhes";

            try
            {
                // Use o Handler para ignorar SSL se necessário (Opção 2 da resposta anterior)
                HttpClientHandler handler = new HttpClientHandler();
                handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;

                using HttpClient client = new HttpClient(handler);
                //client.Timeout = TimeSpan.FromSeconds(20); // Aumente o tempo

                // Faz a chamada
                var response = await client.GetAsync(url);

                // --- MOMENTO DA VERDADE ---
                if (!response.IsSuccessStatusCode)
                {
                    // Se cair aqui, a API retornou erro (ex: 500 Internal Server Error)
                    string erroServidor = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"[ERRO API] Status: {response.StatusCode} | Detalhe: {erroServidor}");
                    return Enumerable.Empty<TarefaDetalhesDTO>();
                }

                // Se deu sucesso (200 OK), lê o JSON
                var resultado = await response.Content.ReadFromJsonAsync<IEnumerable<TarefaDetalhesDTO>>();
                return resultado ?? Enumerable.Empty<TarefaDetalhesDTO>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERRO MOBILE]: {ex.Message}");
                return Enumerable.Empty<TarefaDetalhesDTO>();
            }
        }

        public async Task<bool> AtualizarTarefaAsync(int id, TarefaModel model)
        {
            string url = $"{BaseUrl}/api/Tarefa/Update/{id}";

            using HttpClient client = new HttpClient();
            var response = await client.PutAsJsonAsync(url, model);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> AtualizarEstadoAsync(int id, string novoEstado)
        {
            string url = $"{BaseUrl}/api/Tarefa/{id}/{novoEstado}";

            using HttpClient client = new HttpClient();
            var response = await client.PatchAsync(url, null);
            return response.IsSuccessStatusCode;
        }

        public async Task<TarefaModel?> GetTarefaByIdAsync(int id)
        {
            string url = $"{BaseUrl}/api/Tarefa/GetById/{id}";

            using HttpClient client = new HttpClient();
            try
            {
                return await client.GetFromJsonAsync<TarefaModel>(url);
            }
            catch
            {
                return null;
            }
        }

        public async Task<IEnumerable<TarefaModel>> GetTarefasByGestorAsync(int gestorId)
        {
            string url = $"{BaseUrl}/api/Tarefa/GetByGestor/{gestorId}";

            using HttpClient client = new HttpClient();
            try
            {
                return await client.GetFromJsonAsync<IEnumerable<TarefaModel>>(url)
                       ?? Enumerable.Empty<TarefaModel>();
            }
            catch
            {
                return Enumerable.Empty<TarefaModel>();
            }
        }

        public async Task<IEnumerable<TarefaModel>> GetTarefasByProgramadorAsync(int programadorId)
        {
            string url = $"{BaseUrl}/api/Tarefa/GetByProgramador/{programadorId}";

            using HttpClient client = new HttpClient();
            try
            {
                return await client.GetFromJsonAsync<IEnumerable<TarefaModel>>(url)
                       ?? Enumerable.Empty<TarefaModel>();
            }
            catch
            {
                return Enumerable.Empty<TarefaModel>();
            }
        }

        public async Task<bool> DeleteTarefaAsync(int id)
        {
            string url = $"{BaseUrl}/api/Tarefa/Delete/{id}";

            using HttpClient client = new HttpClient();
            var response = await client.DeleteAsync(url);
            return response.IsSuccessStatusCode;
        }
    }
}

