using System.Collections.Generic;
using System.Linq;
using _Project.Scripts.Main.Data;
using _Project.Scripts.Main.Extension;
using _Project.Scripts.Main.Utilities.Scripts.Singleton;
using UnityEngine;

namespace _Project.Scripts.Main.Managers
{
    public class RelicPoolManager: PersistentSingletonMonoBehaviour<RelicPoolManager>
    {
        [SerializeField] private RelicList _purchasableRelicsSO;
        [SerializeField] private RelicList _dropOnlyRelicsSO;
        
        private List<RelicData> _purchasableRelics = new();
        private List<RelicData> _dropOnlyRelics = new();
        private List<RelicData> _rewardRelics = new();

        protected override void Awake()
        {
            base.Awake();
            _purchasableRelics.AddRange(_purchasableRelicsSO.list);
            _dropOnlyRelics.AddRange(_dropOnlyRelicsSO.list);
        }
        
        public int PurchasableRelicsCount => _purchasableRelics.Count;
        
        public RelicData[] GetRandomPurchasableRelics(int max)
        {
            _purchasableRelics.Shuffle();
            int remaining = _purchasableRelics.Count;
            int totalToObtain = (remaining >= max) ? max : remaining;
            RelicData[] purchasable = new RelicData[totalToObtain];
            if (totalToObtain > 0)
            {
                for (int i = totalToObtain - 1; i >= 0; i--)
                {
                    purchasable[i] = _purchasableRelics[i];
                }
            }
            else
            {
                Debug.Log("Purchasable Relics Pool is Empty!");
            }
            return purchasable;
        }

        public RelicData GetRandomRelicReward()
        {
            RelicData rewardRelic = null;
            _rewardRelics.Clear();
            _rewardRelics.AddRange(_purchasableRelics);
            _rewardRelics.AddRange(_dropOnlyRelics);
            if (_rewardRelics.Count > 0)
            {
                rewardRelic = _rewardRelics.FirstOrDefault();
                if(rewardRelic)
                    RemoveFromPool(rewardRelic);
            }
            else
                Debug.Log("No more relics available in the pool");
            return rewardRelic;
        }

        public void RemoveFromPool(RelicData relicData)
        {
            _purchasableRelics.Remove(relicData);
            _dropOnlyRelics.Remove(relicData);
        }
        
        public void ResetPool()
        {
            _purchasableRelics.Clear();
            _dropOnlyRelics.Clear();
            _purchasableRelics.AddRange(_purchasableRelicsSO.list);
            _dropOnlyRelics.AddRange(_dropOnlyRelicsSO.list);
        }
        
        //Note: should be called when the game is over to ensure cleanup
        public void Destroy() => Destroy(gameObject);
    }
}