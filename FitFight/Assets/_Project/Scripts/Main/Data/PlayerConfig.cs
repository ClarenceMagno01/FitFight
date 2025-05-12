using Sirenix.OdinInspector;
using UnityEngine;

namespace _Project.Scripts.Main.Data
{
    [CreateAssetMenu(fileName = "PlayerConfig", menuName = "PlayerConfig/PlayerConfig")]
    public class PlayerConfig : ScriptableObject
    {
        public string characterName = "";
        public int initialHealth = 100;
        public int initialGold;
        [Required] public Cards initialCards;
        public RelicData starterRelic;
    }
}