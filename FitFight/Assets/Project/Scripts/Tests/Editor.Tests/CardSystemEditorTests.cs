using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using _Project.Scripts.Main.Data;
using _Project.Scripts.Main.Game.Card;
using _Project.Scripts.Main.Game.PlayerTurn;
using _Project.Scripts.Main.Game.PlayerTurn.Systems;
using NUnit.Framework;
using VContainer;

// 1st level Is/Has/Does/Contains
// 2nd level All/Not/Some/Exactly
// Or/And/Not
// IsUnique / IsOrdered
// Assert.IsTrue

namespace _Project.Scripts.Tests.Editor.Tests
{
    public class CardSystemEditorTests
    {
        private CardSystem _cardSystem;
        private List<CardData> _currentDeck = new();
        private const int MinDeckSize = 12;
        private const int DefaultDrawCount = 5;

        [SetUp]
        [SuppressMessage("ReSharper", "Unity.IncorrectScriptableObjectInstantiation")]
        public void Setup()
        {
            //Ensure is clean for every test run
            _currentDeck.Clear();
            if(_cardSystem != null)
                _cardSystem.ClearAll();
            
            for (int i = 0; i < MinDeckSize; ++i)
            {
                CardData data = new CardData {info = new CardInfo {name = $"Card_{i + 1}"}};
                _currentDeck.Add(data);
            }

            //Build test container
            var builder = new ContainerBuilder();
            builder.RegisterInstance(_currentDeck).AsSelf();
            builder.Register<CardSystem>(Lifetime.Singleton);
            var container = builder.Build();
        
            //Resolve instance for testing
            _cardSystem = container.Resolve<CardSystem>();
        }

        [Test]
        public void IsDeckEnough()
        {
            Assert.That(_currentDeck.Count, Is.GreaterThanOrEqualTo(MinDeckSize));
        }
    
        [Test]
        public void IsDrawPileEnough()
        {
            Assert.That(_cardSystem.DrawPile.Count, Is.GreaterThanOrEqualTo(MinDeckSize));
        }

        [Test]
        public void IsDrawPileShuffled()
        {
            List<string> originalOrder = _cardSystem.DrawPile.Select(card => card.Data.info.name).ToList();
            _cardSystem.ShuffleDrawPile();
            List<string> shuffledOrder = _cardSystem.DrawPile.Select(card => card.Data.info.name).ToList();
            Assert.That(originalOrder, Is.Not.EqualTo(shuffledOrder));
        }
    
        [Test]
        public void HasDrawnFiveCards()
        {
            int beforeDrawCount = _cardSystem.DrawPile.Count;
            List<CardInstance> drawnCards = _cardSystem.DrawCards(DefaultDrawCount);
            Assert.That(drawnCards.Count, Is.EqualTo(5));
            Assert.That(_cardSystem.DrawPile.Count, Is.EqualTo( beforeDrawCount - 5));
        }

        [Test]
        public void SelectDrawnCard_ShouldMarkCardAsSelected()
        {
            _cardSystem.DrawCards(DefaultDrawCount);
            bool selected = _cardSystem.SelectDrawnCard(0);

            Assert.IsTrue(selected, "Card should be selectable.");
            Assert.IsTrue(_cardSystem.DrawnCards[0].IsSelected, "Card is already selected.");
        }

        [Test]
        public void DeselectDrawnCard_ShouldUnmarkCardAsSelected()
        {
            _cardSystem.DrawCards(DefaultDrawCount);
            _cardSystem.SelectDrawnCard(0);
            bool deselected = _cardSystem.DeselectDrawnCard(0);

            Assert.IsTrue(deselected, "Card should be deselectable.");
            Assert.IsFalse(_cardSystem.DrawnCards[0].IsSelected, "Card should be unmarked as selected.");
        }
    
        [Test]
        public void DiscardSelectedCard_ShouldMoveCardToDiscardPile()
        {
            _cardSystem.DrawCards(DefaultDrawCount);
            _cardSystem.SelectDrawnCard(0);
            _cardSystem.DiscardSelectedCard(0);

            Assert.AreEqual(1, _cardSystem.DiscardPile.Count, "Discard pile should contain 1 card.");
        }
    
        [Test]
        public void DiscardRemainingAndAllSelectedCards_ShouldMoveAllDrawnAndSelectedCardsToDiscardPile()
        {
            int minDrawCount = 5;
        
            // Arrange
            _cardSystem.DrawCards(DefaultDrawCount); // draw 5 cards
            _cardSystem.SelectDrawnCard(0); // Select one card
        
            // Act
            _cardSystem.DiscardRemainingDrawnAndAllSelectedCards();

            // Assert
            Assert.AreEqual(0, _cardSystem.DrawnCards.Count, "All drawn cards should be discarded.");
            Assert.AreEqual(0, _cardSystem.SelectedCards.Count, "All selected cards should be cleared.");
            Assert.AreEqual(minDrawCount, _cardSystem.DiscardPile.Count, "All drawn cards should be in discard pile.");
        }
    
        [Test]
        public void RecycleDiscardedCards_ShouldMoveAllDiscardedCardsBackToDrawPile()
        {
            int initialPileCount = _cardSystem.DrawPile.Count();
        
            // Arrange
            _cardSystem.DrawCards(DefaultDrawCount); // Draw 5 cards
            _cardSystem.SelectDrawnCard(1); 
            _cardSystem.SelectDrawnCard(3); 
            _cardSystem.DiscardRemainingDrawnAndAllSelectedCards(); // Discard drawn and selected cards
        
            // Ensure the discard pile is not empty before recycling
            Assert.Greater(_cardSystem.DiscardPile.Count, 0, "Discard pile should have cards before recycling.");

            // Act
            _cardSystem.RecycleDiscardedCards();

            // Assert
            Assert.AreEqual(0, _cardSystem.DiscardPile.Count, "Discard pile should be empty after recycling.");
            Assert.AreEqual(initialPileCount, _cardSystem.DrawPile.Count, "Draw pile should contain the recycled cards.");
        }

        [Test]
        public void Simulate_DrawLessFiveCards()
        {
            _cardSystem.DrawCards(10); // draw 10 and remain 2 cards in draw pile
            Assert.That(_cardSystem.DrawPile.Count, Is.EqualTo(2));
            _cardSystem.DiscardRemainingDrawnAndAllSelectedCards(); 
            Assert.That(_cardSystem.DiscardPile.Count, Is.EqualTo(10));

            if (_cardSystem.DrawPile.Count >= 5)
            {
                //Do normal draw
            }
            else if(!_cardSystem.IsDrawPileEmpty())
            {
                //Draw all remaining cards and recycle
                int drawCount = _cardSystem.DrawPile.Count;
                _cardSystem.DrawCards(drawCount);
                Assert.That(_cardSystem.DrawPile.Count, Is.EqualTo(0));
                
                _cardSystem.RecycleDiscardedCards();
                Assert.That(_cardSystem.DrawPile.Count, Is.EqualTo(10));
                
                _cardSystem.DrawCards(5 - drawCount);
                Assert.That(_cardSystem.DrawnCards.Count, Is.EqualTo(5));
                
            }
            
        }
    }
}
