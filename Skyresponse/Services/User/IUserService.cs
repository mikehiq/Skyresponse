using System.Collections.Generic;
using System.Threading.Tasks;

namespace Skyresponse.Services.User
{
    public interface IUserService
    {
        /// <summary>
        /// Return access token
        /// </summary>
        /// <returns></returns>
        Task GetAccessTokenResponse();

        void SaveUserInfo();

        Task<string> GetAccessToken();

        void ClearUserInfo();

        Dictionary<string, string> UserInfoDictionary { get; }
    }
}