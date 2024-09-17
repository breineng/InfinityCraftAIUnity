using System;
using UnityEngine;

namespace InfinityCraft.Managers
{
    public static class SettingsManager
    {
        private const string GENIE_TOKEN_KEY = "genie_token";
        private const string OPENAI_KEY = "openai_key";
        private const string OPENAI_VALID_KEY = "openai_valid_key";
        private const string SHOW_STARTUP_KEY = "showstartup_key";
        
        public static event Action OnSettingsChanged;
        
        public static string GenieToken
        {
            get => GetString(GENIE_TOKEN_KEY);
            set => SetString(GENIE_TOKEN_KEY, value);
        }
        
        public static string OpenAIKey
        {
            get => GetString(OPENAI_KEY);
            set => SetString(OPENAI_KEY, value);
        }
        
        public static bool ShowStartup
        {
            get => GetBool(SHOW_STARTUP_KEY, true);
            set => SetBool(SHOW_STARTUP_KEY, value);
        }

        public static bool IsOpenAIKeyValid
        {
            get => GetBool(OPENAI_VALID_KEY, false);
            set => SetBool(OPENAI_VALID_KEY, value);
        }

        public static void Flush()
        {
            PlayerPrefs.Save();
        }

        private static string GetString(string key)
        {
            return PlayerPrefs.GetString(key);
        }

        private static void SetString(string key, string value)
        {
            PlayerPrefs.SetString(key, value);
            OnSettingsChanged?.Invoke();
        }

        private static bool GetBool(string key, bool defaultValue)
        {
            return PlayerPrefs.GetInt(key, defaultValue ? 1 : 0) == 1;
        }

        private static void SetBool(string key, bool value)
        {
            PlayerPrefs.SetInt(key, value ? 1 : 0);
            OnSettingsChanged?.Invoke();
        }
    }
}