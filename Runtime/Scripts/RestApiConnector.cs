using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using PJ.Easy.Utils;

namespace PJ.Easy.RestApi
{
    public class RestApiConnector : SelfPersistentSingletonMonoBehaviour<RestApiConnector>
    {
        protected WWWForm GetFormFields(Dictionary<string, object> parameters)
        {
            WWWForm formFields = new WWWForm();
            foreach (var parameter in parameters)
            {
                formFields.AddField(parameter.Key, parameter.Value.ToString());
            }
            return formFields;
        }

        #region Get Mothod
        public void GetRequest(string methodName, Dictionary<string, object> parameters, Action<string> onSuccess, Action<string> onFail)
        {
            StartCoroutine(GetRequestCoroutine(methodName, parameters, onSuccess, onFail));
        }

        private IEnumerator GetRequestCoroutine(string methodName, Dictionary<string, object> parameters, Action<string> onSuccess, Action<string> onFail)
        {
            if (RestApiConfig.Instance == null)
            {
                Log(GET, CONFIG_CHECK, $"No RestApiConfig found in Resources folder");
                onFail?.Invoke("RestApiConfig not found");
                yield break;
            }
            string url = RestApiConfig.Instance.BaseUrl + methodName;
            Log(GET, CALL, $"{url}\nParameters : {parameters.ToJson()}".ToYellow());
            var formFields = GetFormFields(parameters);
            using (UnityWebRequest request = UnityWebRequest.Get(url))
            {
                foreach (var formField in formFields.headers)
                {
                    request.SetRequestHeader(formField.Key, formField.Value);
                }

                yield return request.SendWebRequest();

                if (request.HasError())
                {
                    Log(GET, RESPONSE, $"{request.error}\n{url}".ToRed());
                    onFail?.Invoke(request.error);
                }
                else
                {
                    Log(GET, RESPONSE, $"{request.downloadHandler.text}\n{url}".ToGreen());
                    onSuccess?.Invoke(request.downloadHandler.text);
                }
            }
        }
        #endregion

        #region Post Mothod
        public void PostRequest(string methodName, Dictionary<string, object> parameters, Action<string> onSuccess, Action<string> onFail)
        {
            StartCoroutine(PostRequestCoroutine(methodName, parameters, onSuccess, onFail));
        }

        private IEnumerator PostRequestCoroutine(string methodName, Dictionary<string, object> parameters, Action<string> onSuccess, Action<string> onFail)
        {
            if (RestApiConfig.Instance == null)
            {
                Log(POST, CONFIG_CHECK, $"RestApiConfig not found in Resources folder");
                onFail?.Invoke("RestApiConfig not found");
                yield break;
            }
            string url = RestApiConfig.Instance.BaseUrl + methodName;
            Log(POST, CALL, $"{url}\nParameters : {parameters.ToJson()}".ToYellow());
            var formFields = GetFormFields(parameters);
            using (UnityWebRequest request = UnityWebRequest.Post(url, formFields))
            {
                yield return request.SendWebRequest();

                if (request.HasError())
                {
                    Log(POST, RESPONSE, $"{request.error}\n{url}".ToRed());
                    onFail?.Invoke(request.error);
                }
                else
                {
                    Log(POST, RESPONSE, $"{request.downloadHandler.text}\n{url}".ToGreen());
                    onSuccess?.Invoke(request.downloadHandler.text);
                }
            }
        }
        #endregion

        #region GetTexture Method
        public void GetTexture(string url, Action<Texture> onSuccess, Action<string> onFail)
        {
            StartCoroutine(GetTextureCoroutine(url, onSuccess, onFail));
        }

        private IEnumerator GetTextureCoroutine(string url, Action<Texture> onSuccess, Action<string> onFail)
        {
            Log(GET_TEXTURE, CALL, $"{url}".ToYellow());
            using (UnityWebRequest request = UnityWebRequestTexture.GetTexture(url))
            {
                yield return request.SendWebRequest();

                if (request.HasError())
                {
                    Log(GET_TEXTURE, RESPONSE, $"{request.error}\n{url}".ToRed());
                    onFail?.Invoke(request.error);
                }
                else
                {
                    Log(GET_TEXTURE, RESPONSE, $"Texture Download Success\n{url}".ToGreen());
                    onSuccess?.Invoke(((DownloadHandlerTexture)request.downloadHandler).texture);
                }
            }
        }
        #endregion

        #region Logs
        private static bool loggingEnabled = true;
        private const string TAG = "[RestApiConnector]";
        private const string CONFIG_CHECK = "ConfigCheck";
        private const string GET = "GET";
        private const string POST = "POST";
        private const string GET_TEXTURE = "GetTexture";
        private const string CALL = "Call";
        private const string RESPONSE = "Response";

        internal static void Log(string method, string action, object message)
        {
            if (!loggingEnabled) return;
            Debug.Log($"{TAG} [{method}] [{action}] | {message}");
        }

        public static void SetLogging(bool enabled)
        {
            loggingEnabled = enabled;
        }
        #endregion
    }
}
