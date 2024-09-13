using System;
using UnityEngine;
using UnityEngine.Events;

namespace InfinityCraft.Interaction
{
    public class PushableButton : MonoBehaviour, IInteractable
    {
        [SerializeField]
        private UnityEvent _onPushButtonDown;
        [SerializeField]
        private string _hint;

        public string Name
        {
            get => _hint;
            set => throw new Exception();
        }

        public void Interact()
        {
            _onPushButtonDown?.Invoke();
        }
    }
}