using System;
using UnityEngine;

namespace _Project.Scripts.Main.Game.Entity.Component
{
    [Serializable]
    public enum DebuffType
    {
        Weak,
        Vulnerable    
    }
    
    public class DebuffComponent : MonoBehaviour
    {
        private int _vulnerableCount = 0;
        private int _weakCount = 0;
        
        public bool IsVulnerable => _vulnerableCount > 0;
        public bool IsWeak => _weakCount > 0;
        public bool isPaperPhrog;
        
        public void Apply(DebuffType debuffType,int amount)
        {
            if (debuffType == DebuffType.Weak)
                _weakCount+=amount;
            else if(debuffType == DebuffType.Vulnerable)
                _vulnerableCount+=amount;
        }

        public void ApplyPaperPhrog() => isPaperPhrog = true;
        
        public void Decrease()
        {
            DecreaseWeak();
            DecreaseVulnerable();
        }

        private void DecreaseWeak()
        {
            if (_weakCount > 0)
            {
                _weakCount--;
                if (_weakCount < 0)
                    _weakCount = 0;
            }
        }

        private void DecreaseVulnerable()
        {
            if (_vulnerableCount > 0)
            {
                _vulnerableCount--;
                if (_vulnerableCount < 0)
                    _vulnerableCount = 0;
            }
        }
        
        
    }
}