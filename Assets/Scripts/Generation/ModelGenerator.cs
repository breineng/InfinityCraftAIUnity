using System;
using System.IO;
using System.Threading.Tasks;
using Dummiesman;
using InfinityCraft.Managers;
using InfinityCraft.OpenAI;
using LumaGenie;
using LumaGenie.Data.Convert;
using LumaGenie.Data.Creation;
using LumaGenie.Data.Status;
using Newtonsoft.Json;
using UnityEngine;

namespace InfinityCraft.Generation
{
    public class ModelGenerator : MonoBehaviour
    {
        [SerializeField]
        private Material _material;
        [SerializeField]
        private string _gptInstructions = "Come up with an item that would result from combining two items listed below. The response should only include the name of the item that makes sense as a prompt.\nChoose the most appropriate material from the list: 0 - Basketball, 1 - Brick, 2 - Metal, 3 - Plastic, 4 - Rock\nbounciness, dynamicFriction, staticFriction are properties of a physical material in Unity, float values from 0 to 1.\nThe response format should be valid JSON: {\"name\": \"<item name>\", \"soundMaterial\": <material index>, \"bounciness\": <bounciness>, \"dynamicFriction\": <dynamicFriction>, \"staticFriction\": <staticFriction>}";

        public event Action<GameObject> OnModelLoaded;
        public bool IsGenerating { get; private set; }
        
        private GenieClient _genieClient;
        private GptClient _gptClient;
        private Material _instanceMaterial;
        
        private void Awake()
        {
            string token = SettingsManager.GenieToken;
            _genieClient = new GenieClient(token);

            string apiKey = SettingsManager.OpenAIKey;
            _gptClient = new GptClient(apiKey);
            
            SettingsManager.OnSettingsChanged += OnSettingsChanged;
        }

        private void OnDestroy()
        {
            SettingsManager.OnSettingsChanged -= OnSettingsChanged;
        }

        private void OnSettingsChanged()
        {
            string token = SettingsManager.GenieToken;
            _genieClient.UpdateToken(token);
            
            string apiKey = SettingsManager.OpenAIKey;
            _gptClient.UpdateKey(apiKey);
        }
        
        public async Task<bool> TryGenerate(string prompt)
        {
            IsGenerating = true;

            string uuid = string.Empty;
            try
            {
                var creationRequestAsync = await _genieClient.CreationRequestAsync(new CreationRequest(prompt));
                uuid = creationRequestAsync.Response[0];

                bool isSuccess = false;
                while (!isSuccess)
                {
                    await Task.Delay(3000);
                    var statusRequestAsync = await _genieClient.GetStatusRequestAsync(uuid);
                    isSuccess = statusRequestAsync.Response.Status == CreationStatus.Completed;
                }

                var convertResponse = await _genieClient.ConvertRequestAsync(new ConvertRequest(uuid, ExportFormat.Obj));
                var files = await _genieClient.DownloadAndExtractFileAsync(convertResponse.Response.UploadedFiles[0].FileUrl);
                
                _instanceMaterial = Instantiate(_material);
                foreach ((string Name, Stream Content) file in files)
                    OnFileExtracted(file.Name, file.Content);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                IsGenerating = false;
                return false;
            }
            
            _instanceMaterial = null;
            IsGenerating = false;
            return true;
        }
        
        public async Task<ItemResponse> CreatePromptAsync(string prompt)
        {
            ApiResponse apiResponse = await _gptClient.Completions(_gptInstructions, prompt);
            if(apiResponse == null)
                return null;
            var itemResponse = JsonConvert.DeserializeObject<ItemResponse>(apiResponse.Choices[0].Message.Content);
            return itemResponse;
        }

        private async void OnFileExtracted(string fullName, Stream stream)
        {
            string fileExtension = Path.GetExtension(fullName);
            string fileName = Path.GetFileNameWithoutExtension(fullName);

            if (fileExtension == ".obj")
            {
                var objLoader = new OBJLoader();
                var loadedObj = objLoader.Load(stream);
                var meshRenderers = loadedObj.GetComponentsInChildren<MeshRenderer>();
                foreach (MeshRenderer meshRenderer in meshRenderers)
                {
                    meshRenderer.material = _instanceMaterial;
                }
                
                OnModelLoaded?.Invoke(loadedObj);
            }
            else if (fileExtension == ".jpg")
            {
                using MemoryStream memoryStream = new MemoryStream();
                await stream.CopyToAsync(memoryStream);
                byte[] fileData = memoryStream.ToArray();
                Texture2D tex = new Texture2D(2048, 2048);
                tex.name = fileName;
                tex.LoadImage(fileData);
                tex.Compress(false);
                
                if (fileName.EndsWith("texture_kd"))
                {
                    _instanceMaterial.SetTexture("_BaseMap", tex);
                }
                else if(fileName.EndsWith("roughness"))
                {
                    _instanceMaterial.SetTexture("_RoughnessMap", tex);
                }
                else if (fileName.EndsWith("metallic"))
                {
                    _instanceMaterial.SetTexture("_MetallicMap", tex);
                }
            }
        }
    }
}