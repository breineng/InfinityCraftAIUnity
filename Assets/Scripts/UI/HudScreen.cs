using InfinityCraft.Interaction;
using TMPro;
using UnityEngine;

namespace InfinityCraft.UI
{
    public class HudScreen : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text _itemName;
        [SerializeField]
        private GameObject _crosshair;
        [SerializeField]
        private InteractionDetector _interactionDetector;
        [SerializeField]
        private ObjectDragger _rigidbodyController;

        private void Update()
        {
            if (_interactionDetector.DetectedItem != null)
                _itemName.text = _interactionDetector.DetectedItem.Name;
            else
                _itemName.text = string.Empty;
            
            _crosshair.SetActive(!_rigidbodyController.IsHoldingObject);
            _itemName.gameObject.SetActive(!_rigidbodyController.IsHoldingObject);
        }
    }
}