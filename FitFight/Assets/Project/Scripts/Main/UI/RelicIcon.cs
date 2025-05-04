using System;
using _Project.Scripts.Main.Data;
using _Project.Scripts.Main.Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.Main
{
    public class RelicIcon : MonoBehaviour
    {
        [SerializeField] private Image img;
        
        [Header("Description")]
        [SerializeField] private GameObject _description;
        [SerializeField] private TMP_Text _txtTitle;
        [SerializeField] private TMP_Text _txtDescription;
        
        [Header("Price")]
        [SerializeField] private GameObject _price;
        [SerializeField] private TMP_Text _txtPrice;
        public RelicData Data { get; private set; }
        
        public void SetData(RelicData data)
        {
            Data = data;
            img.sprite = data.assets.image;
            _txtTitle.text = data.info.name;
            _txtDescription.text = data.info.description;
            
            int price = GameManager.Instance.DiscountedPrice(data.price);
            _txtPrice.text = price.ToString();
        }
        
        public void ShowPrice(bool isShow)
        {
            _price.SetActive(isShow);
        }

        public void ShowDescription() => _description.SetActive(true);
        public void HideDescription() => _description.SetActive(false);
    }
}