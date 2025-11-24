using iTask_App_Mobile.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace iTask_App_Mobile.Services.GestorService
{
    public class GestorService : IGestorService
    {
        private string BaseUrl =>
            DeviceInfo.Platform == DevicePlatform.Android ?
            "http://10.0.2.2:5055" :
            "http://localhost:5055";

        public async Task<GestorModel> GetByIdAsync(int id)
        {
            string url = $"{BaseUrl}/api/gestor/{id}";
            using HttpClient client = new();

            return await client.GetFromJsonAsync<GestorModel>(url);
        }

        public async Task<List<GestorModel>> GetAllAsync()
        {
            string url = $"{BaseUrl}/api/gestor/all";
            using HttpClient client = new();

            return await client.GetFromJsonAsync<List<GestorModel>>(url);
        }

        public async Task<bool> CreateGestorAsync(GestorModel dto)
        {
            string url = $"{BaseUrl}/api/gestor/create";
            using HttpClient client = new();

            var response = await client.PostAsJsonAsync(url, dto);
            return response.IsSuccessStatusCode;
        }
    }
}
