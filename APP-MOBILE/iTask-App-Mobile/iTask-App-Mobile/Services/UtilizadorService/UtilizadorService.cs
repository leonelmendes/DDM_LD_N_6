using iTask_App_Mobile.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace iTask_App_Mobile.Services.UtilizadorService
{
    public class UtilizadorService : IUtilizadorService
    {
        private string BaseUrl =>
            DeviceInfo.Platform == DevicePlatform.Android
            ? "http://10.0.2.2:5055"
            : "http://localhost:5055";

        public async Task<UtilizadorModel> GetByIdAsync(int id)
        {
            string url = $"{BaseUrl}/api/utilizador/{id}";
            using HttpClient client = new HttpClient();

            return await client.GetFromJsonAsync<UtilizadorModel>(url);
        }

        public async Task<bool> UpdateUtilizadorAsync(int id, UtilizadorModel dto)
        {
            string url = $"{BaseUrl}/api/utilizador/update/{id}";
            using HttpClient client = new HttpClient();

            var result = await client.PutAsJsonAsync(url, dto);
            return result.IsSuccessStatusCode;
        }
    }
}
