using iTask_App_Mobile.DTOs;
using iTask_App_Mobile.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iTask_App_Mobile.Services.AuthenticateService
{
    public interface IAuthenticateService
    {
        Task<LoginResponseModel> LoginAsync(LoginRequestModel model);
        Task<bool> RegisterManagerAsync(RegisterManagerDTO model);
        Task<RegisterProgrammerDTO> RegisterProgrammerAsync(RegisterProgrammerDTO model);
    }
}
