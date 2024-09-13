using UnityEngine;

namespace InfinityCraft.Generation
{
    public class ProgressBar : MonoBehaviour
    {
        private static readonly int _blink = Animator.StringToHash("blink");

        [SerializeField]
        private Animator[] _points;
        [SerializeField, Range(0f, 1f)]
        private float _progress;

        private void OnValidate()
        {
            SetProgress(_progress);
        }

        public void SetProgress(float progress)
        {
            _progress = progress;
            int currentIndex = Mathf.RoundToInt(_progress * (_points.Length - 1));
            for (var i = 0; i < _points.Length; i++)
            {
                _points[i].gameObject.SetActive(i <= currentIndex);
                if(_points[i].gameObject.activeInHierarchy)
                    _points[i].SetBool(_blink, i == currentIndex && i < _points.Length - 1);
            }
        }
    }
}