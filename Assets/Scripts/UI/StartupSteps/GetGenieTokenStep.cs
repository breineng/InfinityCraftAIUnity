using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using InfinityCraft.Managers;
using UnityEngine;
using VoltstroStudios.UnityWebBrowser.Core;

namespace InfinityCraft.UI.StartupSteps
{
    public class GetGenieTokenStep : SequenceStepBase
    {
        [SerializeField]
        private BaseUwbClientManager _uwb;

        private bool _isTokenReceived;

        private void Awake()
        {
            _uwb.browserClient.RegisterJsMethod<string>("SendToken", OnTokenLoaded);
        }

        public override async Task Execute()
        {
            gameObject.SetActive(true);
            
            _isTokenReceived = false;

            await UniTask.WaitUntil(() => _uwb.browserClient.IsConnected);
            
            _uwb.browserClient.OnLoadFinish += BrowserClientOnOnLoadFinish;
            _uwb.browserClient.LoadUrl("https://lumalabs.ai/genie?showAuth=true");

            await UniTask.WaitUntil(() => _isTokenReceived);
            
            _uwb.browserClient.OnLoadFinish -= BrowserClientOnOnLoadFinish;
            
            gameObject.SetActive(false);
        }

        private void OnTokenLoaded(string token)
        {
            // Debug.Log("Token:" + token);
            SettingsManager.GenieToken = token;
            SettingsManager.Flush();
                 
            _isTokenReceived = true;
        }

        private void BrowserClientOnOnLoadFinish(string url)
        {
            _uwb.browserClient.ExecuteJs(@"
function getCookie(name) {
    const value = `; ${document.cookie}`;
    const parts = value.split(`; ${name}=`);
    if (parts.length === 2) return parts.pop().split(';').shift();
    return null;
}
var token = getCookie('refreshToken');
if(token != null)
    uwb.ExecuteJsMethod('SendToken', token);");
        }
    }
}