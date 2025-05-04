using _Project.Scripts.Main.Data;

namespace _Project.Scripts.Main.Game.PlayerTurn.Systems
{
    public class CardInstance
    {
        public readonly CardData Data;
        public bool IsSelected;
        public int DrawID;

        public CardInstance(CardData data, bool pIsSelected = false)
        {
            Data = data;
            IsSelected = pIsSelected;
        }
    }
}