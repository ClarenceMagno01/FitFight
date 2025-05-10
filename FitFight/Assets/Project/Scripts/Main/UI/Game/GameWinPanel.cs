using System;
using _Project.Scripts.Main.Data;
using _Project.Scripts.Main.Managers;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace _Project.Scripts.Main.UI
{
    public class GameWinPanel : MonoBehaviour
    {
        [SerializeField] private TMP_Text _txtGold;
        [SerializeField] private GameObject _lootPopup;
        [SerializeField] private CardRewardPicker _cardRewardPicker;
        
        [Header("Buttons")]
        [SerializeField] private Button _btnAddCard;
        [SerializeField] private Button _btnProceed;
        
        [Header("Relic Loot")]
        [SerializeField] private GameObject _relicLoot;
        [SerializeField] private TMP_Text _txtRelic;
        [SerializeField] private Image _imgRelic;

        [Header("Card database")] 
        [SerializeField] private Cards _cardDatabase;

        private Rewards _rewards;

        private void OnEnable()
        {
            _btnAddCard.onClick.AddListener(ShowCardRewardPicker);
            _btnProceed.onClick.AddListener(OnClickProceed);
        }

        private void OnDisable()
        {
            _btnAddCard.onClick.RemoveListener(ShowCardRewardPicker);
            _btnProceed.onClick.RemoveListener(OnClickProceed);
        }

        public void SetRewards(Rewards rewards)
        {
            _rewards = rewards;
            ValidateGoldLoot(rewards);
            ValidateRelicLoot(rewards);
            ValidateCardLoot(rewards);
        }
        
        private void ValidateGoldLoot(Rewards rewards)
        {
            _txtGold.text = $"{rewards.gold} Gold";
            GameManager.Instance.AddGold(rewards.gold);
        }

        private void ValidateRelicLoot(Rewards rewards)
        {
            _relicLoot.SetActive(false);
            if (rewards.hasRandomRelic)
            {
                RelicData relic = RelicPoolManager.Instance.GetRandomRelicReward();
                if (relic)
                {
                    _relicLoot.SetActive(true);
                    _imgRelic.sprite = relic.assets.image;
                    _txtRelic.text = relic.info.name;
                    GameManager.Instance.AddRelic(relic);
                }
            }
        }

        private void ValidateCardLoot(Rewards rewards)
        {
            _btnAddCard.gameObject.SetActive(false);
            if (rewards.numberOfCardChoices > 0)
                _btnAddCard.gameObject.SetActive(true);
        }

        private void ShowCardRewardPicker()
        {
            _lootPopup.SetActive(false);
            
            CardData[] cardPool = (!_rewards.specificRandomCardPool) ? _cardDatabase.list : _rewards.cardPool;
            _cardRewardPicker.gameObject.SetActive(true);
            _cardRewardPicker.ShowChoices(cardPool, _rewards.numberOfCardChoices);
        }

        private void OnClickProceed()
        {
            SceneManager.LoadScene("MapScene");
        }
    }
}