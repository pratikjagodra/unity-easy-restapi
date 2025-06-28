using UnityEngine.Networking;

namespace PJ.EasyRestApi
{
    public static class ApiExtensions
    {
        public static bool HasError(this UnityWebRequest request)
        {
            return request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError || request.result == UnityWebRequest.Result.DataProcessingError;
        }
    }
}
