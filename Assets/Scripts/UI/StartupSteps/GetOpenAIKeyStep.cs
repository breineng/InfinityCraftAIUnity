using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using InfinityCraft.Managers;
using InfinityCraft.OpenAI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace InfinityCraft.UI.StartupSteps
{
    public class GetOpenAIKeyStep : SequenceStepBase
    {
        [SerializeField]
        private TMP_InputField _inputField;
        [SerializeField]
        private Button _button;

        private bool _shouldContinue;

        private void Awake()
        {
            _button.onClick.AddListener(OnContinueButtonClick);
            _inputField.onEndEdit.AddListener(OnOpenAIEndEdit);
        }

        private void OnDestroy()
        {
            _button.onClick.RemoveListener(OnContinueButtonClick);
            _inputField.onEndEdit.RemoveListener(OnOpenAIEndEdit);
        }

        private void OnContinueButtonClick()
        {
            _shouldContinue = true;
        }
        
        private void OnOpenAIEndEdit(string key)
        {
            CheckKeyVerifyAsync(key);
        }

        public override async Task Execute()
        {
            if(SettingsManager.IsOpenAIKeyValid)
                return;
            
            gameObject.SetActive(true);
            _shouldContinue = false;
            
            RefreshVerifiedColors();

            _inputField.text = SettingsManager.OpenAIKey;

            await UniTask.WaitUntil(() => _shouldContinue);

            SettingsManager.OpenAIKey = _inputField.text;
            SettingsManager.Flush();
            
            gameObject.SetActive(false);
        }
        
        private async void CheckKeyVerifyAsync(string key)
        {
            await UniTask.WaitForEndOfFrame(this);
            EnableInteractions(false);
            bool verifyed = await GptClient.VerifyApiKey(key);
            SettingsManager.IsOpenAIKeyValid = verifyed;
            RefreshVerifiedColors();
            EnableInteractions(true);
        }
        
        private void RefreshVerifiedColors()
        {
            _inputField.image.color = SettingsManager.IsOpenAIKeyValid
                ? new Color(0.76f, 1f, 0.81f)
                : new Color(1f, 0.76f, 0.76f);
        }

        private void EnableInteractions(bool enable)
        {
            _button.interactable = enable;
            _inputField.interactable = enable;
        }
    }
}