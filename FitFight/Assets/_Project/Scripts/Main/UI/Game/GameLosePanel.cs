using TMPro;
using UnityEngine;

namespace _Project.Scripts.Main.UI
{
    public struct GameSummary
    {
        public int TotalCards;
        public int TotalRelics;
        public int Gold;
    }
    
    public class GameLosePanel : MonoBehaviour
    {
        [SerializeField] private TMP_Text _txtTotalCards;
        [SerializeField] private TMP_Text _txtTotalRelics;
        [SerializeField] private TMP_Text _txtTotalGold;
        
        public void SetGameSummary(GameSummary summary)
        {
            _txtTotalCards.text = $"Total Cards: {summary.TotalCards}";
            _txtTotalRelics.text = $"Total Relics: {summary.TotalRelics}";
            _txtTotalGold.text = $"Gold: {summary.Gold}";
        }
    }
}