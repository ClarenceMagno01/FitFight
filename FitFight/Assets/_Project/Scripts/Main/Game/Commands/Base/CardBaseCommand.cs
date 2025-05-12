using _Project.Scripts.Main.Data;

namespace _Project.Scripts.Main.Game.Commands.Base
{
    public class CardBaseCommand : BaseCommand
    {
        protected CardData CardDataVar;
        
        public void SetCardData(CardData cardData)
        {
            CardDataVar = cardData;
        }
        
    }
}