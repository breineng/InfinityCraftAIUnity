using InfinityCraft.Managers;
using UnityEngine;

namespace InfinityCraft.UI
{
    public class MenuScreen : MonoBehaviour, IScreen
    {
        private ScreenManager _screenManager;

        public bool? ShowCursor => true;
        public bool CanDeactivate => true;

        public void Init(ScreenManager screenManager)
        {
            _screenManager = screenManager;
        }

        public void Activate()
        {
            gameObject.SetActive(true);
        }

        public void Deactivate()
        {
            gameObject.SetActive(false);
        }
        
        public void OnContinueButtonClick()
        {
            _screenManager.CloseCurrentScreen();
        }
        
        public void OnSettingsButtonClick()
        {
            _screenManager.OpenScreen<SettingsScreen>();
        }
        
        public void OnQuitButtonClick()
        {
            Application.Quit();
        }
    }
}