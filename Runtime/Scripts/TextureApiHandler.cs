using System;
using UnityEngine;
using PJ.Easy.RestApi;

public class TextureApiHandler : RestApiHandler
{
    public void GetTexture(string url, Action<Texture> onSuccess, Action<string> onFail)
    {
        RestApiConnector.Instance.GetTexture(url, onSuccess, onFail);
    }
}
