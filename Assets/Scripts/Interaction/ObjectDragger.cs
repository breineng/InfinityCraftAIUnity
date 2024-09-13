using AFPC;
using UnityEngine;

namespace InfinityCraft.Interaction
{
    public class ObjectDragger : MonoBehaviour
    {
        [SerializeField]
        private FPController _fpController;
        [SerializeField]
        private InteractionDetector _interactionDetector;
        [SerializeField]
        private float _moveSpeed = 20f;
        [SerializeField]
        private float _rotationSpeed = 300f;
        [SerializeField]
        private float _zoomSpeed = 2f;
        [SerializeField]
        private float _distance = 1.6f;
        [SerializeField]
        private float _distanceMin = 1.2f;
        [SerializeField]
        private float _distanceMax = 5f;

        private Rigidbody _currentBody;
        private bool _isHoldingObject;
        private Camera _camera;
        private Vector3 _targetPoint;

        public bool IsHoldingObject => _isHoldingObject;

        private void Start()
        {
            _camera = Camera.main;
        }

        private void Update()
        {
            HandleObjectInteraction();
        }

        private void HandleObjectInteraction()
        {
            if (Input.GetMouseButton(0))
            {
                if (!_isHoldingObject)
                    TryPickUpObject();
                else
                    UpdateObject();
            }
            else if (_isHoldingObject || _currentBody == null)
            {
                DropObject();
            }
        }

        private void TryPickUpObject()
        {
            if (_interactionDetector.DetectedItem is PickupableItem pickupableItem)
            {
                Rigidbody targetObj = pickupableItem.Rigidbody;
                if (targetObj) TakeObject(targetObj);
            }
        }

        private void UpdateObject()
        {
            if (_currentBody == null)
                return;

            _targetPoint = _camera.transform.position + _camera.transform.forward * _distance;
            DragObject();

            if (Input.GetMouseButton(1))
            {
                RotateObject();
                _fpController.Look.Control.Toggle = false;
            }
            else
            {
                _fpController.Look.Control.Toggle = true;
            }

            float scrollInput = Input.GetAxis("Mouse ScrollWheel");
            _distance = Mathf.Clamp(_distance + scrollInput * _zoomSpeed, _distanceMin, _distanceMax);
        }

        private void DragObject()
        {
            Vector3 direction = _targetPoint - _currentBody.transform.position;
            float distance = direction.magnitude;
            Vector3 objectVelocity = direction.normalized * Mathf.Min(distance * _moveSpeed, _moveSpeed);
            _currentBody.velocity = objectVelocity + _fpController.rigidbody.velocity;
        }

        private void RotateObject()
        {
            float rotationX = Input.GetAxis("Mouse X") * _rotationSpeed * Time.deltaTime;
            float rotationY = Input.GetAxis("Mouse Y") * _rotationSpeed * Time.deltaTime;

            _currentBody.transform.Rotate(Vector3.up, -rotationX, Space.World);
            _currentBody.transform.Rotate(_camera.transform.right, rotationY, Space.World);
        }

        private void DropObject()
        {
            if (_currentBody != null)
            {
                _currentBody.useGravity = true;
                _currentBody.freezeRotation = false;
            }
            _currentBody = null;
            _isHoldingObject = false;
            _fpController.Look.Control.Toggle = true;
        }

        private void TakeObject(Rigidbody rigidbody)
        {
            _currentBody = rigidbody;
            _currentBody.useGravity = false;
            _currentBody.isKinematic = false;
            _currentBody.freezeRotation = true;
            _isHoldingObject = true;
        }
    }
}
