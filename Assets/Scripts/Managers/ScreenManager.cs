using System;
using System.Collections.Generic;
using InfinityCraft.UI;
using UnityEngine;

namespace InfinityCraft.Managers
{
    public class ScreenManager : MonoBehaviour
    {
        private readonly Dictionary<Type, IScreen> _screens = new();
        private IScreen _currentScreen;
        
        private void Awake()
        {
            IScreen[] screens = GetComponentsInChildren<IScreen>(true);
            foreach (IScreen screen in screens)
            {
                _screens.Add(screen.GetType(), screen);
                screen.Init(this);
            }
        }

        private void Start()
        {
            if(SettingsManager.ShowStartup)
                OpenScreen<StartupSequence>();
            else
                ShowCursor(false);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (_currentScreen is MenuScreen)
                    CloseCurrentScreen();
                else
                    OpenScreen<MenuScreen>();
            }
        }

        public void OpenScreen<T>() where T : IScreen
        {
            if (_screens.TryGetValue(typeof(T), out IScreen screen))
            {
                if(_currentScreen != null && !_currentScreen.CanDeactivate)
                    return;
                
                CloseCurrentScreen();

                _currentScreen = screen;
                screen.Activate();
                
                if (screen.ShowCursor.HasValue)
                    ShowCursor(screen.ShowCursor.Value);
            }
            else
            {
                CloseCurrentScreen();
            }
        }

        public void CloseCurrentScreen()
        {
            if (_currentScreen == null) 
                return;
            
            _currentScreen.Deactivate();
            _currentScreen = null;
                
            ShowCursor(false);
        }
        
        private void ShowCursor(bool show)
        {
            Cursor.visible = show;
            Cursor.lockState = show ? CursorLockMode.None : CursorLockMode.Locked;
        }
    }
}