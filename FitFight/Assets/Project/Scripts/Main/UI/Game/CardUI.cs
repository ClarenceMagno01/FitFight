using _Project.Scripts.Main.Data;
using _Project.Scripts.Main.Managers;
using TMPro;
using UnityEngine;

namespace _Project.Scripts.Main.UI
{
    public class CardUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text _txtRepCount;
        [SerializeField] private TMP_Text _txtExercise;
        [SerializeField] private TMP_Text _txtDesc;

        [Header("Price")] 
        [SerializeField] private GameObject _price;
        [SerializeField] private TMP_Text _txtPrice;
        public CardData Data { get; private set; }
        
        public void SetData(CardData data)
        {
            Data = data;
            _txtRepCount.text = data.info.requiredReps.ToString();
            _txtExercise.text = data.info.name;
            _txtDesc.text = data.info.description;
            
             int price = GameManager.Instance.DiscountedPrice(data.info.price);
            _txtPrice.text = price.ToString();
        }

        public void ShowPrice(bool isShow)
        {
            _price.gameObject.SetActive(isShow);
        }
    }
}