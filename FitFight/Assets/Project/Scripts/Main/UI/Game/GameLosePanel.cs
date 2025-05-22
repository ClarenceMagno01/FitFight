using TMPro;
using UnityEngine;
using System.Text;
using System.Collections.Generic;
using _Project.Scripts.Main.Managers;
using _Project.Scripts.Main.Data;

namespace _Project.Scripts.Main.UI
{
    public struct GameSummary
    {
        public int TotalCards;
        public int TotalRelics;
        public int Gold;
        public string TimeElapsedFormatted;

        public Dictionary<ExerciseType, int> ActivatedExerciseCounts;
    }

    public class GameLosePanel : MonoBehaviour
    {
        [SerializeField] private TMP_Text _txtTotalCards;
        [SerializeField] private TMP_Text _txtTotalRelics;
        [SerializeField] private TMP_Text _txtTotalGold;
        [SerializeField] private TMP_Text _txtTime;
        [SerializeField] private TMP_Text _txtExerciseCounts;  // New UI text for exercise counts

        private void OnEnable()
        {
            TimerManager.Instance?.StopTimer();
        }

        public void SetGameSummary(GameSummary summary)
        {
            Debug.Log($"SetGameSummary called. Time: {summary.TimeElapsedFormatted}");

            _txtTotalCards.text = $"Total Cards: {summary.TotalCards}";
            _txtTotalRelics.text = $"Total Relics: {summary.TotalRelics}";
            _txtTotalGold.text = $"Gold: {summary.Gold}";
            _txtTime.text = $"Time: {summary.TimeElapsedFormatted}";

            // Build a string showing exercise counts
            if (summary.ActivatedExerciseCounts != null && summary.ActivatedExerciseCounts.Count > 0)
            {
                var exerciseText = "";
                foreach (var kvp in summary.ActivatedExerciseCounts)
                {
                    exerciseText += $"{kvp.Key}: {kvp.Value}\n";
                }
                _txtExerciseCounts.text = exerciseText;
            }
            else
            {
                _txtExerciseCounts.text = "No exercises activated.";
            }
        }
    }
}