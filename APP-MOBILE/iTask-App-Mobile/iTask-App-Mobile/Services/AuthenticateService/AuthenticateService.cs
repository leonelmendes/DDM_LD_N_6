using iTask_App_Mobile.DTOs;
using iTask_App_Mobile.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace iTask_App_Mobile.Services.AuthenticateService
{
    public class AuthenticateService : IAuthenticateService
    {
        private string BaseUrl =>
            DeviceInfo.Platform == DevicePlatform.Android
            ? "http://10.0.2.2:5055"
            : "http://localhost:5055";

        public async Task<LoginResponseModel> LoginAsync(LoginRequestModel model)
        {
            string url = $"{BaseUrl}/api/Authenticate/login";

            using HttpClient client = new HttpClient();
            var response = await client.PostAsJsonAsync(url, model);

            if (!response.IsSuccessStatusCode)
                return null;

            return await response.Content.ReadFromJsonAsync<LoginResponseModel>();
        }

        public async Task<bool> RegisterManagerAsync(RegisterManagerDTO model)
        {
            string url = DeviceInfo.Platform == DevicePlatform.Android
                    ? "http://10.0.2.2:5055/api/Authenticate/register-manager"
                    : "http://localhost:5055/api/Authenticate/register-manager";

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    var response = await client.PostAsJsonAsync(url, model);

                    if (!response.IsSuccessStatusCode)
                    {
                        Console.WriteLine("Status da API: " + response.StatusCode);
                        return false;
                    }

                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("[DOTNET] Erro ao chamar API: " + ex.Message);
                    return false;
                }
            }
        }
        

        public async Task<RegisterProgrammerDTO> RegisterProgrammerAsync(RegisterProgrammerDTO model)
        {
            string url = $"{BaseUrl}/api/Authenticate/register-programmer";

            using HttpClient client = new HttpClient();
            var response = await client.PostAsJsonAsync(url, model);

            if (!response.IsSuccessStatusCode)
                return null;

            return await response.Content.ReadFromJsonAsync<RegisterProgrammerDTO>();
        }
    }
}
