using System;
using System.Collections.Generic;
using System.Linq;
using _Project.Scripts.Main.Game.Card;
using _Project.Scripts.Main.Game.PlayerTurn.Systems;
using _Project.Scripts.Main.Utilities.Scripts;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace _Project.Scripts.Main.Game.PlayerTurn
{
    public class HeldCards : MonoBehaviour
    {
        private struct CardTransform
        {
            public Vector2[] positions;
            public Quaternion[] rotations;
        }
        
        [SerializeField] private GameCard _cardPrefab; 
        private float _angleStep = 12f;
        private float _yDropStep = 0.15f;
        private float _spacing = 1.8f;
        private int _defaultSortingOrder = 0;
        
        private List<GameCard> _cards = new();
        private Stack<GameCard> _selectedCards = new();
        
        /// <summary>
        /// Spawn a new set of cards from the given list
        /// </summary>
        /// <param name="cards"></param>
        /// <param name="onSpawn"></param>
        public async UniTask SpawnNewSet(List<CardInstance> cards,Action onSpawn = null)
        {
            CardTransform cardTransform = GetCardTransform(cards.Count);
            for (int i = 0; i < cards.Count; ++i)
            {
                await SpawnCard(cards[i],cardTransform,i);
                onSpawn?.Invoke();
            }
        }
        
        private async UniTask SpawnCard(CardInstance card,CardTransform dest, int index)
        {
            GameCard gc = CreateCard(card);
            await MoveAndRotate(gc, dest.positions[index], dest.rotations[index].eulerAngles);
        }

        /// <summary>
        /// Add a card and rearrange its position and rotation
        /// </summary>
        /// <param name="card"></param>
        /// <param name="onSpawn"></param>
        /// <returns></returns>
        public async UniTask<GameCard> AddCard(CardInstance card,Action<GameCard> onSpawn = null)
        {
            GameCard gc = CreateCard(card);
            await RearrangeCards();
            onSpawn?.Invoke(gc);
            return gc;
        }
        
        public void RemoveCard(int drawID)
        {
            GameCard card = _cards.FirstOrDefault(c => c.drawID == drawID);
            if (card)
                RemoveCard(card);
        }
        
        public void RemoveCard(GameCard card)
        {
            _cards.Remove(card);
            PrefabPoolManager.Instance.ReturnObjectToPool(card.gameObject);
        }

        public void ClearCards()
        {
            PrefabPoolManager.Instance.RemoveAllFromParent(transform);
            _selectedCards.Clear();
            _cards.Clear();
        }
        
        private async UniTask RearrangeCards()
        {
            CardTransform dest = GetCardTransform(_cards.Count);
            for (int i = 0; i < _cards.Count; ++i)
            {
                GameCard gc = _cards[i];
                gc.ResetToArrange();
                await MoveAndRotate(gc, dest.positions[i], dest.rotations[i].eulerAngles,0.1f);
            }
        }
 
        private GameCard CreateCard(CardInstance card)
        {
            GameCard gc = PrefabPoolManager.Instance.GetObjectFromPool(_cardPrefab.gameObject).GetComponent<GameCard>();
            gc.gameObject.transform.SetParent(transform);
            // gc.gameObject.transform.localPosition = Vector3.zero;
            // gc.gameObject.transform.rotation = Quaternion.identity;
            
            gc.Data = card.Data;
            gc.drawID = card.DrawID;
            gc.InvalidateUI();
            gc.SetSortingOrder(_defaultSortingOrder++);
            _cards.Add(gc);
            return gc;
        }
        
        private async UniTask MoveAndRotate(GameCard card, Vector3 localPos, Vector3 angle,float duration = 0.25f)
        {
            Tween moveTween = card.transform.DOLocalMove(localPos, duration);
            Tween rotateTween = card.transform.DORotate(angle, duration);
            await UniTask.WhenAll(
                moveTween.ToUniTask(),
                rotateTween.ToUniTask()
            );
        }
        
        
        public async UniTask<Stack<GameCard>> ArrangeSelectedCards(List<CardInstance> selectedCards,float duration = 0.25f)
        {
            _selectedCards.Clear();
            Vector2 worldPos = new Vector2(0, 0.5f);
            Vector2 localPos = transform.InverseTransformPoint(worldPos);
    
            int len = selectedCards.Count - 1;
            for (int i = len; i >= 0; --i)
            {
                GameCard card = _cards.FirstOrDefault(c => c.drawID == selectedCards[i].DrawID);
                if (card)
                {
                    card.ResetToArrange();
                    card.SetSortingOrder(len - i);
                    _selectedCards.Push(card);
           
                    Tween moveTween = card.transform.DOLocalMove(localPos, duration);
                    Tween rotateTween = card.transform.DORotate(Vector3.zero, duration);
                    await UniTask.WhenAll(
                        moveTween.ToUniTask(),
                        rotateTween.ToUniTask()
                    );
                }
            }
            return _selectedCards;
        }
        
        private CardTransform GetCardTransform(int totalCards)
        {
            CardTransform cardTransform = new CardTransform
            {
                positions = new Vector2[totalCards],
                rotations = new Quaternion[totalCards]
            };
            
            int middleIndex = totalCards / 2;
            
            for (int i = 0; i < totalCards; i++)
            {
                int offsetFromCenter = i - middleIndex;
                float angle = -offsetFromCenter * _angleStep; // Negative to match left/right
                float edgeOffset = totalCards >= 3 && (i == 0 || i == totalCards - 1) ? 0.5f : 0;
          
                float x = transform.position.x;
                x+= (offsetFromCenter * _spacing);
                float y = -Mathf.Abs(offsetFromCenter) * _yDropStep;
                
                cardTransform.positions[i] = new Vector2(x , y - edgeOffset);
                cardTransform.rotations[i] = Quaternion.AngleAxis(angle, Vector3.forward);
            }
            return cardTransform;
        }

    }
    
}