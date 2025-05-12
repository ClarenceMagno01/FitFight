using UnityEngine;

namespace _Project.Scripts.Main.Game.Entity.Component
{
    public class BuffComponent : MonoBehaviour
    {
        public bool isFossilizedHelix;
        public bool isLizardTail;
        public bool isBarricade;
        public bool isRedSkull;
        
        public void ApplyFossilizedHelix() => isFossilizedHelix = true;
        public void ApplyLizardTail() => isLizardTail = true;
        public void ApplyBarricade() => isBarricade = true;
        public void ApplyRedSkull() => isRedSkull = true;
        
        public int TryAbsorbDamageWithFossilizedHelix(int damage)
        {
            if (isFossilizedHelix)
            {
                isFossilizedHelix = false;
                return 0;
            }
            return damage;
        }
    }
}