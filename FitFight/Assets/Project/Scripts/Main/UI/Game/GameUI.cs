using System;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace _Project.Scripts.Main.UI
{
    public class GameUI : MonoBehaviour
    {
        [Header("Text")]
        [SerializeField] private TMP_Text _txtDrawPile;
        [SerializeField] private TMP_Text _txtDiscardPile;

        [Header("Buttons")]
        public Button btnStartExercise;
        public Button btnEndTurnNoSelected;
        public Button btnEndTurnForExercise;
        
        [Header("Panels")]
        public GameWinPanel winPanel;
        public GameCongratsPanel congratsPanel;
        
        public void SetDrawPileCount(int count) => _txtDrawPile.text = $"{count}";
        public void SetDiscardPileCount(int count) => _txtDiscardPile.text = $"{count}";


        private void Start()
        {
            ShowEndTurnForExerciseButton(false);
        }

        public void ShowStartExerciseButton(bool isShow)
        {
            btnStartExercise.gameObject.SetActive(isShow);
            btnEndTurnNoSelected.gameObject.SetActive(!isShow);
        }
        
        public void HideExerciseWithEndTurnButton()
        {
            btnStartExercise.gameObject.SetActive(false);
            btnEndTurnNoSelected.gameObject.SetActive(false);
        }
        
        public void ShowEndTurnForExerciseButton(bool isShow) => btnEndTurnForExercise.gameObject.SetActive(isShow);
        
    }
}