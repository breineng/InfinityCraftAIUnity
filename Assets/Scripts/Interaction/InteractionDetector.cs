using UnityEngine;

namespace InfinityCraft.Interaction
{
    public class InteractionDetector : MonoBehaviour
    {
        [SerializeField]
        private float _raycastDistance = 2f;
        
        private Camera _camera;
        private IInteractable _detectedItem;
        
        public IInteractable DetectedItem => _detectedItem;
        
        private void Start()
        {
            _camera = Camera.main;
        }
        
        private void Update()
        {
            if (Cursor.lockState != CursorLockMode.Locked)
            {
                _detectedItem = null;
                return;
            }
            Ray ray = new Ray(_camera.transform.position, _camera.transform.forward * _raycastDistance);
            if (Physics.Raycast(ray, out var hit, _raycastDistance, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore))
            {
                _detectedItem = hit.transform.GetComponentInParent<IInteractable>();

                if (_detectedItem != null && Input.GetMouseButtonDown(0))
                {
                    _detectedItem.Interact();
                }
            }
            else
            {
                _detectedItem = null;
            }
        }
    }
}