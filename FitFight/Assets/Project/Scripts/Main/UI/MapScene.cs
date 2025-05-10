using _Project.Scripts.Main.Managers;
using Cysharp.Threading.Tasks;
using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Project.Scripts.Main
{
    public class MapScene : MonoBehaviour
    {
        [SerializeField] private MMF_Player _bgmFeedback;
        
        private void Awake()
        {
            Time.timeScale = 1;
        }

        private void Start()
        {
            LoadScenesAsync().Forget();
        }
        
        private async UniTaskVoid LoadScenesAsync()
        {
            await SceneManager.LoadSceneAsync("OverlayUI",LoadSceneMode.Additive);
            await GameManager.Instance.LoadInputConfig();
            _bgmFeedback.PlayFeedbacks();
        }
        
    }
}