using UnityEngine;
using PJ.EasyUtils;

namespace PJ.EasyRestApi
{
    [CreateAssetMenu(fileName = "RestApiConfig", menuName = "ScriptableObjects/RestApiConfig")]
    public class RestApiConfig : SingletonScriptableObject<RestApiConfig>
    {
        [SerializeField] private string baseUrl;

        public string BaseUrl => baseUrl;

        protected override string GetAssetPath()
        {
            return "RestApiConfig";
        }
    }
}
