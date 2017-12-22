using System.Collections.Generic;
using System.Threading.Tasks;

namespace Skyresponse.Wrappers.HttpWrappers
{
    public interface IHttpWrapper
    {
        /// <summary>
        /// Returns an accesstoken
        /// </summary>
        /// <param name="dict"></param>
        /// <returns></returns>
        Task<string> GetAccessToken(Dictionary<string, string> dict);

        /// <summary>
        /// Register for push messages
        /// </summary>
        /// <param name="accesstoken"></param>
        /// <returns></returns>
        Task RegisterForPush(string accesstoken);

        /// <summary>
        /// Get info on the alarm
        /// </summary>
        /// <param name="alarmId"></param>
        /// <param name="accesstoken"></param>
        /// <returns></returns>
        Task<int> GetAlarmInfo(string alarmId, string accesstoken);
    }
}