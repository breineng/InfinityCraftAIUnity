using System;
using InfinityCraft.Interaction;
using UnityEngine;

namespace InfinityCraft.Generation
{
    public class ItemContainer : MonoBehaviour
    {
        public event Action OnChange;
        public PickupableItem Item { get; private set; }
        
        private void OnTriggerEnter(Collider other)
        {
            var pickupableItem = other.GetComponentInParent<PickupableItem>();
            if(pickupableItem == null)
                return;

            if (Item == null)
                Item = pickupableItem;
            
            OnChange?.Invoke();
        }

        private void OnTriggerExit(Collider other)
        {
            var pickupableItem = other.GetComponentInParent<PickupableItem>();
            if(pickupableItem == null)
                return;

            if (Item == pickupableItem)
                Item = null;
            
            OnChange?.Invoke();
        }
    }
}