using System.Collections;
using UnityEngine;

namespace InfinityCraft.Interaction
{
    public class TrashBin : MonoBehaviour
    {
        private static readonly int _dissolve = Shader.PropertyToID("_Dissolve");

        [SerializeField]
        private AudioSource _audioSource;
        [SerializeField]
        private AudioClip _audioClip;
        [SerializeField]
        private float _dissolveDuration = 2f;
        
        private void OnTriggerEnter(Collider other)
        {
            var item = other.GetComponentInParent<PickupableItem>();
            if(!item.IsDestroyable)
                return;
            StartCoroutine(DestroyAsset(item.gameObject));
        }

        private IEnumerator DestroyAsset(GameObject gameObject)
        {
            _audioSource.PlayOneShot(_audioClip);

            var meshRenderer = gameObject.GetComponentInChildren<MeshRenderer>();
            float elapsedTime = 0f;
            while (elapsedTime < _dissolveDuration)
            {
                elapsedTime += Time.deltaTime;
                float dissolveValue = Mathf.Lerp(0f, 1f, elapsedTime / _dissolveDuration);
                meshRenderer.material.SetFloat(_dissolve, dissolveValue);
                yield return null;
            }
            
            yield return null;
            DestroyImmediate(gameObject);
            Resources.UnloadUnusedAssets();
        }
    }
}