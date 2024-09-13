using InfinityCraft.Managers;
using InfinityCraft.UI.StartupSteps;
using UnityEngine;

namespace InfinityCraft.UI
{
    public class StartupSequence : MonoBehaviour, IScreen
    {
        [SerializeField]
        private SequenceStepBase[] _sequenceSteps;

        public bool? ShowCursor => true;
        public bool CanDeactivate => _sequenceCompleted;

        private ScreenManager _screenManager;
        private bool _sequenceCompleted;

        public void Init(ScreenManager screenManager)
        {
            _screenManager = screenManager;
        }
        
        private void Awake()
        {
            foreach (SequenceStepBase step in _sequenceSteps)
                step.gameObject.SetActive(false);
        }

        private async void StartSequence()
        {
            _sequenceCompleted = false;
            foreach (SequenceStepBase step in _sequenceSteps)
                await step.Execute();
            
            SettingsManager.ShowStartup = false;
            _sequenceCompleted = true;
            _screenManager.CloseCurrentScreen();
        }

        public void Activate()
        {
            gameObject.SetActive(true);
            StartSequence();
        }

        public void Deactivate()
        {
            gameObject.SetActive(false);
        }
    }
}