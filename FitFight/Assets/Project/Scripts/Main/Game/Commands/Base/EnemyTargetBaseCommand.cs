using System;
using System.Collections.Generic;
using System.Linq;
using _Project.Scripts.Main.Game.Entity;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Project.Scripts.Main.Game.Commands.Base
{
    [Serializable]
    public enum TargetType
    {
        Selected,
        Random,
        All,
    }
    
    public abstract class EnemyTargetBaseCommand : BaseCommand
    {
        [SerializeField] private TargetType _targetType;
        private readonly List<Enemy> _finalTargets = new();
        private List<Enemy> _targets;
        private int _targetIndex = -1;
        
        protected TargetType GetTargetType => _targetType;
        
        public void SetPriority(int spawnIndex)
        {
            _targetIndex = spawnIndex;
        }
        
        public void SetTargets(List<Enemy> targets)
        {
            _targets = targets;
        }

        protected List<Enemy> GetTargets()
        {
            if(_targets == null)
                throw new NullReferenceException($"{GetType().Name}: Targets should be assigned first");
            
            _finalTargets.Clear();
            if (_targets.Count > 0)
            {
                switch (_targetType)
                {
                    case TargetType.Selected:
                        Enemy selectedEnemy = _targets.FirstOrDefault(e => e.spawnIndex == _targetIndex);
                        if (selectedEnemy == null)
                            selectedEnemy = _targets.FirstOrDefault(); //should select the front target base on index when selected enemy is already defeated or null
                        _finalTargets.Add(selectedEnemy);
                        break;
                    case TargetType.Random:
                        int rand = Random.Range(0,_targets.Count);
                        _finalTargets.Add(_targets[rand]);
                        break;
                    case TargetType.All:
                        _finalTargets.AddRange(_targets);
                        break;
                }
            }
            return _finalTargets;
        }
        
    }
}