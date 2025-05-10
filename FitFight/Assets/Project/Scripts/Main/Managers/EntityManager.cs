using System.Collections.Generic;
using _Project.Scripts.Main.Game.Entity;
using _Project.Scripts.Main.Game.Events;
using _Project.Scripts.Main.Utilities.Scripts.EventBus;
using _Project.Scripts.Main.Utilities.Scripts.Singleton;
using UnityEngine;

namespace _Project.Scripts.Main.Managers
{
    public class EntityManager : SingletonMonoBehaviour<EntityManager>
    {
        [Header("Prefabs")]
        [SerializeField] private Player _playerPrefab;
     
        [Header("Spawn Points")]
        [SerializeField] private Transform _playerSpawnPoint;
        [SerializeField] private Transform[] _enemySpawnPoints;
  
        public Player PlayerEntity { get; private set; }
        public List<Enemy> Enemies { get; private set; } = new();
        
        private void OnEnable()
        {
            EventBus<EnemyDiedEvent>.Register(OnEnemyDied);
        }

        private void OnDisable()
        {
            EventBus<EnemyDiedEvent>.Deregister(OnEnemyDied);
        }
        
        public void SpawnPlayer()
        {
            PlayerEntity = Instantiate(_playerPrefab, _playerSpawnPoint.position, Quaternion.identity).GetComponent<Player>();
        }

        public void SpawnEnemies()
        {
            GameObject[] enemiesPrefab = GameManager.Instance.GetCurrentLevelData().enemiesPrefab;
            if (enemiesPrefab.Length == 1)
            {
                Transform spawnPoint = _enemySpawnPoints[1]; // middle position
                Enemy enemy = Instantiate(enemiesPrefab[0], spawnPoint.position, Quaternion.identity).GetComponent<Enemy>();
                enemy.spawnIndex = 0;
                Enemies.Add(enemy);
            }
            else
            {
                int startPosIndex = _enemySpawnPoints.Length - enemiesPrefab.Length; 
                for (int i = startPosIndex; i < _enemySpawnPoints.Length; ++i)
                {
                    int zeroBaseIndex = i - startPosIndex;
                    Transform spawnPoint = _enemySpawnPoints[i];
                
                    Enemy enemy = Instantiate(enemiesPrefab[zeroBaseIndex], spawnPoint.position, Quaternion.identity).GetComponent<Enemy>();
                    enemy.spawnIndex = zeroBaseIndex;
                    Enemies.Add(enemy);
                }
            }
        }

        private void OnEnemyDied(EnemyDiedEvent ev)
        {
            Enemies.Remove(ev.Enemy);
            Destroy(ev.Enemy.gameObject);
        }
        
        private void OnDestroy()
        {
            PlayerEntity = null;
            Enemies.Clear();
        }
    }
}