using System.Collections.Generic;
using System.Linq;
using _Project.Scripts.Main.Data;
using _Project.Scripts.Main.Extension;

namespace _Project.Scripts.Main.Game.PlayerTurn.Systems
{
    public class CardSystem
    {
        public List<CardInstance> DrawPile { get; } = new();
        public List<CardInstance> DrawnCards  { get; } = new();
        public List<CardInstance> SelectedCards { get; } = new();
        public List<CardInstance> DiscardPile { get; } = new();
        
        private List<CardInstance> _recentDrawnCards  { get; } = new();
        
        private int _drawIDCount = 0;
        
        public CardSystem(List<CardData> currentDeck)
        {
            InitWithDeck(currentDeck);
        }
        
        private void InitWithDeck(List<CardData> currentDeck)
        {
            DrawPile.Clear();
            foreach (var data in currentDeck)
                DrawPile.Add(new CardInstance(data));
        }

        public void ShuffleDrawPile()
        {
            DrawPile.Shuffle();
        }
        
        public bool IsDrawPileEmpty() => DrawPile.Count <= 0;
   
        
        /// <summary>
        /// Draw cards from the drawPile
        /// </summary>
        /// <param name="drawCount"></param>
        /// <returns> the recent drawn cards </returns>
        public List<CardInstance> DrawCards(int drawCount)
        {
            _recentDrawnCards.Clear();
            for (int i = 0; i < drawCount; i++)
            {
                CardInstance drawnCardInstance = DrawPile[i];
                drawnCardInstance.DrawID = _drawIDCount;
                DrawnCards.Add(drawnCardInstance);
                _recentDrawnCards.Add(drawnCardInstance);
                _drawIDCount++;
            }
            DrawPile.RemoveRange(0, drawCount); //remove last to ensure drawPile is not modified during loop
            return _recentDrawnCards;
        }
        
        /// <summary>
        /// Select a card with the given drawID from the drawn cards
        /// </summary>
        public bool SelectDrawnCard(int drawID)
        {
            if(DrawnCards.Count <= 0)
                return false;
            
            CardInstance chosenCardInstance = DrawnCards.FirstOrDefault(card => card.DrawID == drawID);
            if (chosenCardInstance != null)
            {
                if (!chosenCardInstance.IsSelected)
                {
                    //Debug.Log($"Draw ID: {chosenCardInstance.DrawID} selected");
                    chosenCardInstance.IsSelected = true;
                    SelectedCards.Add(chosenCardInstance);
                    return true;
                }
            }
            return false;
        }
        
        public bool IsCardSelected(int drawID) => SelectedCards.Any(card => card.DrawID == drawID && card.IsSelected);

        /// <summary>
        /// Deselect a card with the given drawID from the drawn cards
        /// </summary>
        public bool DeselectDrawnCard(int drawID)
        {
            if(DrawnCards.Count <= 0)
                return false;
            
            CardInstance chosenCardInstance = DrawnCards.FirstOrDefault(card => card.DrawID == drawID);
            if (chosenCardInstance != null)
            {
                if (chosenCardInstance.IsSelected)
                {
                   // Debug.Log($"Draw ID: {chosenCardInstance.DrawID} unselected");
                    chosenCardInstance.IsSelected = false;
                    SelectedCards.Remove(chosenCardInstance);
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Remove a selected card with the given drawID permanently during combat
        /// It will never be recycled from the draw pile or place in discard pile
        /// </summary>
        /// <param name="drawID"></param>
        public void RemoveSelectedCardPermanently(int drawID)
        {
            CardInstance cardInstance = SelectedCards.FirstOrDefault(card => card.DrawID == drawID);
            if (cardInstance != null)
            {
                DrawPile.Remove(cardInstance);
                DrawnCards.Remove(cardInstance);
                SelectedCards.Remove(cardInstance);
                DiscardPile.Remove(cardInstance);
            }
        }
        
        /// <summary>
        /// Discard a selected card with the given drawID
        /// </summary>
        public void DiscardSelectedCard(int drawID)
        {
            CardInstance cardInstance = SelectedCards.FirstOrDefault(card => card.DrawID == drawID);
            DiscardSelectedCard(cardInstance);
        }
        
        public void CopyCardToDiscardPile(CardData cardData)
        {
            DiscardPile.Add(new CardInstance(cardData));
        }
        
        /// <summary>`
        /// Discard a selected card with cardInstance
        /// </summary>
        public void DiscardSelectedCard(CardInstance cardInstance)
        {
            SelectedCards.Remove(cardInstance);
            DiscardPile.Add(cardInstance);
        }
        
        public void DiscardRemainingDrawnAndAllSelectedCards()
        {
            DiscardAllSelectedCards();
            DiscardRemainingDrawnCards();
        }
        
        // Can call to ensure that remaining selected cards will be discarded
        private void DiscardAllSelectedCards()
        {
            if(SelectedCards.Count <= 0) return;
            DiscardPile.AddRange(SelectedCards);
            SelectedCards.Clear();
        }
        
        private void DiscardRemainingDrawnCards()
        {
            foreach (var card in DrawnCards)
            {
                if(!card.IsSelected)
                    DiscardPile.Add(card);
                card.IsSelected = false;
            }
            DrawnCards.Clear();
        }
        
        public void RecycleDiscardedCards()
        {
            DrawPile.AddRange(DiscardPile);
            DiscardPile.Clear();
        }
        
        public void ClearDrawPile()
        {
            DrawPile.Clear();
        }

        public void ClearAll()
        {
            DrawPile.Clear();
            DrawnCards.Clear();
            SelectedCards.Clear();
            DiscardPile.Clear();
        }
        
    }
}
