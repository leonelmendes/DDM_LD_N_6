using iTask_App_Mobile.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace iTask_App_Mobile.Services.TipoTarefaService
{
    public class TipoTarefaService : ITipoTarefaService
    {
        private string BaseUrl =>
           DeviceInfo.Platform == DevicePlatform.Android ?
           "http://10.0.2.2:5055" :
           "http://localhost:5055";

        public async Task<List<TipoTarefaModel>> GetAllAsync()
        {
            string url = $"{BaseUrl}/api/tipotarefa/all";
            using HttpClient client = new();

            return await client.GetFromJsonAsync<List<TipoTarefaModel>>(url);
        }

        public async Task<bool> CreateTipoTarefaAsync(TipoTarefaModel dto)
        {
            string url = $"{BaseUrl}/api/tipotarefa/create";
            using HttpClient client = new();

            var response = await client.PostAsJsonAsync(url, dto);
            return response.IsSuccessStatusCode;
        }
    }
}
