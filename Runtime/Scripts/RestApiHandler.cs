using System;
using System.Collections.Generic;
using PJ.Easy.Utils;

namespace PJ.Easy.RestApi
{
    public abstract class RestApiHandler
    {
        protected Dictionary<string, object> parameters = new Dictionary<string, object>();
        protected string methodName;

        protected void GetRequest<T>(Action<T> onSuccess, Action<string> onFail)
        {
            RestApiConnector.Instance.GetRequest(methodName, parameters,
                (response) =>
                {
                    try
                    {
                        T responseObject = response.ToObject<T>();
                        onSuccess?.Invoke(responseObject);
                    }
                    catch (Exception e)
                    {
                        RestApiConnector.Log("GET", "Deserialize", $"There was a problem while deserializing the response to {typeof(T)}");
                        onFail?.Invoke(e.Message);
                    }
                },
                (response) =>
                {
                    onFail?.Invoke(response);
                }
            );
        }

        protected void PostRequest<T>(Action<T> onSuccess, Action<string> onFail)
        {
            RestApiConnector.Instance.PostRequest(methodName, parameters,
                (response) =>
                {
                    try
                    {
                        T responseObject = response.ToObject<T>();
                        onSuccess?.Invoke(responseObject);
                    }
                    catch (Exception e)
                    {
                        RestApiConnector.Log("POST", "Deserialize", $"There was a problem while deserializing the response to {typeof(T)}");
                        onFail?.Invoke(e.Message);
                    }
                },
                (response) =>
                {
                    onFail?.Invoke(response);
                }
            );
        }
    }
}