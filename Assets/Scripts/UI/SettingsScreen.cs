using Cysharp.Threading.Tasks;
using InfinityCraft.Managers;
using InfinityCraft.OpenAI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace InfinityCraft.UI
{
    public class SettingsScreen : MonoBehaviour, IScreen
    {
        [SerializeField]
        private TMP_InputField _genieTokenField;
        [SerializeField]
        private TMP_InputField _openaiKeyField;
        [SerializeField]
        private Button[] _buttons;

        private ScreenManager _screenManager;
        
        public bool? ShowCursor => true;
        public bool CanDeactivate => true;

        public void Init(ScreenManager screenManager)
        {
            _screenManager = screenManager;
        }
        
        private void OnEnable()
        {
            _genieTokenField.text = SettingsManager.GenieToken;
            _openaiKeyField.text = SettingsManager.OpenAIKey;
            RefreshVerifiedColors();
            _openaiKeyField.onEndEdit.AddListener(OnOpenAIEndEdit);
        }

        private void OnDisable()
        {
            SettingsManager.GenieToken = _genieTokenField.text;
            SettingsManager.OpenAIKey = _openaiKeyField.text;
            SettingsManager.Flush();
            
            _openaiKeyField.onEndEdit.RemoveListener(OnOpenAIEndEdit);
        }

        public void Activate()
        {
            gameObject.SetActive(true);
        }

        public void Deactivate()
        {
            gameObject.SetActive(false);
        }
        
        private void OnOpenAIEndEdit(string key)
        {
            CheckKeyVerifyAsync(key);
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
            _openaiKeyField.image.color = SettingsManager.IsOpenAIKeyValid
                ? new Color(0.76f, 1f, 0.81f)
                : new Color(1f, 0.76f, 0.76f);
        }

        private void EnableInteractions(bool enable)
        {
            foreach (Button button in _buttons)
                button.interactable = enable;
            _openaiKeyField.interactable = enable;
        }

        public void OnMasterButtonClick()
        {
            _screenManager.OpenScreen<StartupSequence>();
        }

        public void OnCloseButtonClick()
        {
            Close();
        }

        private void Close()
        {
            _screenManager.CloseCurrentScreen();
        }
    }
}