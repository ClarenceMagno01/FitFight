using System;
using _Project.Scripts.Main.Managers;
using _Project.Scripts.Main.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.Main
{
    public class GameCongratsPanel : MonoBehaviour
    {
        [SerializeField] private TMP_Text _txtTotalCards;
        [SerializeField] private TMP_Text _txtTotalRelics;
        [SerializeField] private TMP_Text _txtTotalGold;
        [SerializeField] private TMP_Text _txtTime;


        [SerializeField] private Button _btnMainMenu;

        private void OnEnable()
        {
            TimerManager.Instance?.StopTimer();
            _btnMainMenu.onClick.AddListener(OnClickMainMenu);
        }

        private void OnDisable()
        {
            _btnMainMenu.onClick.RemoveListener(OnClickMainMenu);
        }

        public void SetGameSummary(GameSummary summary)
        {
            _txtTotalCards.text = $"Total Cards: {summary.TotalCards}";
            _txtTotalRelics.text = $"Total Relics: {summary.TotalRelics}";
            _txtTotalGold.text = $"Gold: {summary.Gold}";
            _txtTime.text = $"Time: {TimerManager.Instance?.GetFormattedTime() ?? "N/A"}";
        }
        
        private void OnClickMainMenu() => GameManager.Instance.GoToMainMenu();
    }
}