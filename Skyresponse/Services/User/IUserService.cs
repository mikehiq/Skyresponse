using System.Collections.Generic;
using System.Threading.Tasks;

namespace Skyresponse.Services.User
{
    public interface IUserService
    {
        Dictionary<string, string> GetUserInfoDictionary();

        /// <summary>
        /// Return access token
        /// </summary>
        /// <returns></returns>
        Task GetAccessTokenResponse();

        void SaveUserInfo();

        Task<string> GetAccessToken();
    }
}