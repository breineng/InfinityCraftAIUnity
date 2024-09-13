using System.Threading.Tasks;
using InfinityCraft.Interaction;
using InfinityCraft.Managers;
using InfinityCraft.OpenAI;
using InfinityCraft.UI;
using PhysSound;
using UnityEngine;
using UnityMeshSimplifier;

namespace InfinityCraft.Generation
{
    public class Combinator : MonoBehaviour
    {
        private static readonly int _push = Animator.StringToHash("push");
        private static readonly int _error = Animator.StringToHash("error");
        private static readonly int _combining = Animator.StringToHash("combining");
        private static readonly int _emissionColor = Shader.PropertyToID("_EmissionColor");

        [SerializeField]
        private ModelGenerator _modelGenerator;
        [SerializeField]
        private ScreenManager _screenManager;
        [SerializeField]
        private Animator _animator;
        [SerializeField]
        private Transform _resultContainer;
        [SerializeField]
        private ItemContainer _blueItemContainer;
        [SerializeField]
        private ItemContainer _yellowItemContainer;
        [SerializeField]
        private MeshRenderer _blue;
        [SerializeField]
        private MeshRenderer _yellow;
        [SerializeField, ColorUsage(false, true)]
        private Color _emissionBlue;
        [SerializeField, ColorUsage(false, true)]
        private Color _emissionYellow;
        [SerializeField]
        private AudioSource _templateAudio;
        [SerializeField]
        private AudioSource _audioSource;
        [SerializeField]
        private AudioClip _errorSound;
        [SerializeField]
        private AudioClip _successSound;
        [SerializeField]
        private PhysSoundMaterial[] _physSoundMaterials;
        [SerializeField]
        private ProgressBar _progressBar;
        [SerializeField]
        private float _estimatedTime = 36f;

        private float _startCombineTime;
        private string _lastPrompt;
        private ItemResponse _lastGeneratedInfo;

        private void Awake()
        {
            _modelGenerator.OnModelLoaded += OnModelLoaded;
            _blueItemContainer.OnChange += OnChangeItemContainerBlue;
            _yellowItemContainer.OnChange += OnChangeItemContainerYellow;
        }

        private void OnDestroy()
        {
            _modelGenerator.OnModelLoaded -= OnModelLoaded;
            _blueItemContainer.OnChange -= OnChangeItemContainerBlue;
            _yellowItemContainer.OnChange -= OnChangeItemContainerYellow;
        }

        private void Update()
        {
            float progress = Mathf.Clamp01((Time.time - _startCombineTime) / _estimatedTime);
            _progressBar.SetProgress(progress);
        }

        private void OnChangeItemContainerBlue()
        {
            bool hasItem = _blueItemContainer.Item != null;
            if (_blue != null) _blue.material.SetColor(_emissionColor, hasItem ? _emissionBlue : Color.black);
        }
        
        private void OnChangeItemContainerYellow()
        {
            bool hasItem = _yellowItemContainer.Item != null;
            if (_yellow != null) _yellow.material.SetColor(_emissionColor, hasItem ? _emissionYellow : Color.black);
        }
        
        private void OnModelLoaded(GameObject obj)
        {
            SetupAsync(obj);
            float estimatedTime = Time.time - _startCombineTime;
            if(estimatedTime > 0)
                _estimatedTime = estimatedTime;
        }

        private async void SetupAsync(GameObject obj)
        {
            obj.transform.position = _resultContainer.position;
            obj.transform.localScale *= 0.4f;
            var meshFilter = obj.GetComponentInChildren<MeshFilter>();
            
            var meshSimplifier = new MeshSimplifier();
            meshSimplifier.Initialize(meshFilter.sharedMesh);
            await Task.Run(() => meshSimplifier.SimplifyMesh(0.1f));
            meshFilter.sharedMesh = meshSimplifier.ToMesh();
            
            var meshCollider = meshFilter.gameObject.AddComponent<MeshCollider>();
            meshCollider.convex = true;
            Rigidbody rigidbody = obj.AddComponent<Rigidbody>();
            rigidbody.interpolation = RigidbodyInterpolation.Interpolate;
            rigidbody.collisionDetectionMode = CollisionDetectionMode.Continuous;
            var pickupableItem = obj.AddComponent<PickupableItem>();
            var physSoundObject = obj.AddComponent<PhysSoundObject>();
            physSoundObject.ImpactAudio = _templateAudio;
            physSoundObject.SoundMaterial = _physSoundMaterials[3];
            if (_lastGeneratedInfo != null)
            {
                physSoundObject.SoundMaterial = _physSoundMaterials[_lastGeneratedInfo.SoundMaterial];
                meshCollider.sharedMaterial = new PhysicMaterial()
                {
                    bounceCombine = PhysicMaterialCombine.Average,
                    frictionCombine = PhysicMaterialCombine.Average,
                    bounciness = _lastGeneratedInfo.Bounciness,
                    dynamicFriction = _lastGeneratedInfo.DynamicFriction,
                    staticFriction = _lastGeneratedInfo.StaticFriction,
                };
            }

            pickupableItem.IsDestroyable = true;
            pickupableItem.Name = _lastPrompt;
            PlayCombining(false);
            if (_audioSource != null)
                _audioSource.PlayOneShot(_successSound);
        }

        public void PushDown()
        {
            if (_animator != null) _animator.SetTrigger(_push);
            Combine();
        }

        [ContextMenu("Combine")]
        public async void Combine()
        {
            if (_modelGenerator.IsGenerating ||
                _blueItemContainer.Item == null ||
                _yellowItemContainer.Item == null)
            {
                PlayError();
                return;
            }

            PlayCombining(true);
            _startCombineTime = Time.time;

            string prompt = _blueItemContainer.Item.Name + " + " + _yellowItemContainer.Item.Name;
            if (SettingsManager.IsOpenAIKeyValid)
            {
                ItemResponse generatedInfo = await _modelGenerator.CreatePromptAsync(prompt);
                _lastGeneratedInfo = generatedInfo;
                if (generatedInfo != null)
                    prompt = generatedInfo.Name;
            }
            else
            {
                _lastGeneratedInfo = null;
            }

            _lastPrompt = prompt;
            bool success = await _modelGenerator.TryGenerate(_lastPrompt);
            if (!success)
            {
                PlayCombining(false);
                PlayError();
                _screenManager.OpenScreen<StartupSequence>();
            }
        }

        private void PlayCombining(bool state)
        {
            if (_animator != null) _animator.SetBool(_combining, state);
            if (_progressBar != null) _progressBar.gameObject.SetActive(state);
            if (_audioSource != null)
            {
                if(state)
                    _audioSource.Play();
                else
                    _audioSource.Stop();
            }
        }

        private void PlayError()
        {
            if (_animator != null) _animator.SetTrigger(_error);
            if (_audioSource != null) _audioSource.PlayOneShot(_errorSound);
        }
    }
}