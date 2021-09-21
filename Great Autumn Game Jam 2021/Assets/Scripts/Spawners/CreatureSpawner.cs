using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Constants;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Tilemaps;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

namespace Assets.Scripts.Spawners
{
    public class CreatureSpawner : MonoBehaviour
    {
        /// <summary>
        /// Tile map that the creature can spawn on
        /// </summary>
        [SerializeField] private Tilemap _tileMap;

        /// <summary>
        /// Creature to spawn.
        /// </summary>
        [SerializeField] private GameObject _creature;

        /// <summary>
        /// Max number of creatures to spawn.
        /// </summary>
        [SerializeField] private int _maxCreatureCount = 3;

        /// <summary>
        /// Max duration a creature has to wait in order to be spawned in.
        /// </summary>
        [SerializeField] private int _maxSpawnDelay = 7;

        /// <summary>
        /// Min duration a creature has to wait in order to be spawned in.
        /// </summary>
        [SerializeField] private int _minSpawnDelay = 3;

        /// <summary>
        /// Minimum distance that a creature can spawn to a player
        /// </summary>
        [SerializeField] private float _minDistanceToSpawnFromPlayer = 5;

        /// <summary>
        /// Last time a creature was spawned.
        /// </summary>
        private float _lastSpawnTime;

        /// <summary>
        /// How long the current spawn delay is
        /// </summary>
        private int _currentSpawnDelay;

        /// <summary>
        /// Player game object
        /// </summary>
        private GameObject _player;

        /// <summary>
        /// Collection of spawned creatures.
        /// </summary>
        private List<GameObject> _creatureCollection = new List<GameObject>();

        /// <summary>
        /// Set Max Creature Count
        /// </summary>
        /// <param name="count">Max number of creatures that can spawn</param>
        public void SetMaxCreatureCount(int count) => _maxCreatureCount = count;

        /// <summary>
        /// Called at object creation
        /// </summary>
        [UsedImplicitly]
        private void Start()
        {
            UpdateSpawnTime();
        }

        /// <summary>
        /// Called after start and when object is enabled.
        /// </summary>
        [UsedImplicitly]
        private void Awake()
        {
            _player ??= GameObject.Find(PlayerConstants.Name);
            UpdateSpawnTime();
        }

        /// <summary>
        /// Update is called once per frame
        /// </summary>
        [UsedImplicitly]
        private void Update()
        {
            if (!CanSpawn() || !ShouldSpawn()) return;
            SpawnCreature();
        }

        /// <summary>
        /// Spawn A creature
        /// </summary>
        private void SpawnCreature()
        {
            var tilePositions = GetValidSpawnPoints();
            var playerPos = _player.transform.position;
            var random = new System.Random();

            Vector3 spawnPoint;
            while (true)
            {
                spawnPoint = tilePositions[random.Next(0, tilePositions.Count)];
                if (Vector3.Distance(spawnPoint, playerPos) > _minDistanceToSpawnFromPlayer) break;
            }

            var creature = Instantiate(_creature, spawnPoint, Quaternion.identity);
            _creatureCollection.Add(creature);
        }

        /// <summary>
        /// Can the creature spawn?
        /// </summary>
        /// <returns>True if creature can spawn.</returns>
        protected virtual bool CanSpawn()
        {
            return true;
        }

        /// <summary>
        /// Has it been a long enough duration for a creature to spawn?
        /// </summary>
        /// <returns>True if creature should spawn</returns>
        protected bool ShouldSpawn()
        {
            // Check if creature count is at max capacity. Reset spawn time if it is so there won't be an immediate spawn.
            _creatureCollection = _creatureCollection.Where(creature => creature != null).ToList();
            if (_creatureCollection.Count == _maxCreatureCount)
            {
                _lastSpawnTime = Time.timeSinceLevelLoad;
                return false;
            }

            // Not time for a new spawn
            if (Time.timeSinceLevelLoad < _lastSpawnTime + _currentSpawnDelay) return false;

            UpdateSpawnTime();
            return true;
        }

        /// <summary>
        /// Reset the spawn delay while also setting the new spawn time.
        /// </summary>
        private void UpdateSpawnTime()
        {
            var random = new System.Random();
            _currentSpawnDelay = random.Next(_minSpawnDelay, _maxSpawnDelay);
            _lastSpawnTime = Time.timeSinceLevelLoad;
        }

        /// <summary>
        /// Get Valid spawn points from provided tile map
        /// </summary>
        /// <returns>Spawn locations for creatures</returns>
        protected virtual List<Vector3> GetValidSpawnPoints()
        {
            var bounds = _tileMap.cellBounds;
            var tilePositions = new List<Vector3>();
            for (var i = bounds.xMin; i < bounds.xMax; i++)
            {
                for (var j = bounds.yMin; j < bounds.yMax; j++)
                {
                    if (_tileMap.GetTile(new Vector3Int(i, j, 0)) != null)
                        tilePositions.Add(new Vector3(i + .5f, j + .5f, 0));
                }
            }

            return tilePositions;
        }
    }
}
