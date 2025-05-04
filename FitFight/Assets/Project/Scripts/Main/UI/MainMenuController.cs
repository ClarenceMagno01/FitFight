using System;
using _Project.Scripts.Main.InputSystem;
using Cysharp.Threading.Tasks;
using MoreMountains.Tools;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace _Project.Scripts.Main
{
    public class MainMenuController : MonoBehaviour
    {
        [SerializeField] private Button _btnStart;
        [SerializeField] private Button _btnExit;
        
        [Header("Input Config")]
        [SerializeField] private InputConfig _inputConfig;

        private void Start()
        {
            MMSoundManagerAllSoundsControlEvent.Trigger(MMSoundManagerAllSoundsControlEventTypes.Free);
            LoadInputConfig().Forget();
        }
        
        private async UniTask LoadInputConfig()
        {
            if(_inputConfig.isRingconAndGamepadEnabled)
                await SceneManager.LoadSceneAsync("RingconInputs",LoadSceneMode.Additive);
            else
                await SceneManager.LoadSceneAsync("DefaultInputs",LoadSceneMode.Additive);
        }

        private void OnEnable()
        {
            _btnStart.onClick.AddListener(OnClickStart);
            _btnExit.onClick.AddListener(OnClickExit);
        }

        private void OnDisable()
        {
            _btnStart.onClick.RemoveListener(OnClickStart);
            _btnExit.onClick.RemoveListener(OnClickExit);
        }

        private void OnClickStart()
        {
            SceneManager.LoadScene("MapScene");
        }

        private void OnClickExit()
        {
            Application.Quit();
        }
    }
}