using iTask_App_Mobile.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace iTask_App_Mobile.Services.TarefaService
{
    public class TarefaService : ITarefaService
    {

        private string BaseUrl =>
            DeviceInfo.Platform == DevicePlatform.Android
            ? "http://10.0.2.2:5055"
            : "http://localhost:5055";

        public async Task<List<TarefaModel>> GetTarefasPorProgramadorAsync(int idProgramador)
        {
            string url = $"{BaseUrl}/api/tarefa/programador/{idProgramador}";
            using HttpClient client = new HttpClient();

            return await client.GetFromJsonAsync<List<TarefaModel>>(url);
        }

        public async Task<bool> CriarTarefaAsync(TarefaModel dto)
        {
            string url = $"{BaseUrl}/api/tarefa/create";
            using HttpClient client = new HttpClient();

            var result = await client.PostAsJsonAsync(url, dto);
            return result.IsSuccessStatusCode;
        }
    }
}
