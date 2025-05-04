using System;
using _Project.Scripts.Main.Managers;
using Cysharp.Threading.Tasks;
using MoreMountains.Feedbacks;
using MoreMountains.Tools;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Project.Scripts.Main.Game
{
    public class Bootstrapper : MonoBehaviour
    {
        [SerializeField] private MMF_Player _bgmFeedback;
        
        private void Start()
        {
            LoadScenesAsync().Forget();
        }
        
        private async UniTaskVoid LoadScenesAsync()
        {
            await SceneManager.LoadSceneAsync("OverlayUI",LoadSceneMode.Additive);
            await SceneManager.LoadSceneAsync("CardSystem",LoadSceneMode.Additive);
            await SceneManager.LoadSceneAsync("GameController",LoadSceneMode.Additive);
            await GameManager.Instance.LoadInputConfig();
            _bgmFeedback.PlayFeedbacks();
        }

        private void OnDestroy()
        {
            MMSoundManagerAllSoundsControlEvent.Trigger(MMSoundManagerAllSoundsControlEventTypes.Free);
        }
    }
}