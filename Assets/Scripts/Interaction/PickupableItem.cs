using UnityEngine;

namespace InfinityCraft.Interaction
{
    public class PickupableItem : MonoBehaviour, IInteractable
    {
        [field:SerializeField]
        public string Name { get; set; }

        public void Interact() { }

        public bool IsDestroyable { get; set; }
        public Rigidbody Rigidbody => _rigidbody ??= GetComponent<Rigidbody>();

        private Rigidbody _rigidbody;
    }
}