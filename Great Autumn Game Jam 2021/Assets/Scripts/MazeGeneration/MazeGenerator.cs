using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Assets.Scripts.Spawners;
using Assets.Scripts.UI.InGameOverlay;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Assets.Scripts.MazeGeneration
{
    public class MazeGenerator : MonoBehaviour
    {
        /// <summary>
        /// Wall Prefab
        /// </summary>
        [SerializeField]
        private TileBase _wall;

        /// <summary>
        /// Floor Prefab
        /// </summary>
        [SerializeField]
        private TileBase _floor;

        /// <summary>
        /// Goal Prefab
        /// </summary>
        [SerializeField]
        private GameObject _goal;

        /// <summary>
        /// Floor Tile Map
        /// </summary>
        [SerializeField]
        private Tilemap _floorTileMap;

        /// <summary>
        /// Wall Tile map
        /// </summary>
        [SerializeField]
        private Tilemap _wallTileMap;

        /// <summary>
        /// Wall Locations
        /// </summary>
        private HashSet<Vector3Int> _visited = new HashSet<Vector3Int>();

        /// <summary>
        /// Player Game Object
        /// </summary>
        [SerializeField]
        private GameObject _player;

        /// <summary>
        /// Width of Maze
        /// </summary>
        [SerializeField]
        private int _width = 10;

        /// <summary>
        /// Height of Maze
        /// </summary>
        [SerializeField]
        private int _height = 10;

        /// <summary>
        /// Next Scene to load.
        /// </summary>
        [SerializeField] 
        private string _nextSceneName = "Maze_Easy_1";

        /// <summary>
        /// Start is called before the first frame update
        /// </summary>
        [UsedImplicitly]
        private void Start()
        {
            GenerateMaze();
        }

        /// <summary>
        /// Randomized Prim's algorithm from Wikipedia:
        /// https://en.wikipedia.org/wiki/Maze_generation_algorithm
        /// </summary>
        private void GenerateMaze()
        {
            var maxX = _width;
            var maxY = _height;
            var level = InGameViewModel.Level;

            if (level >= 5)
            {
                maxX = 10 + (2 * level);
                maxY = 10 + (2 * level);
                UpdateZombieParameters();
            }

            var startPos = new Vector3Int(maxX / 2, 1, 0);
            var portalGoalPos = new Vector3Int(maxX/2, maxY, 0);

            var closestPortalPos = startPos;
            _visited.Add(startPos);
            FillWallMap();

            var pathCandidates = SetPath(startPos).ToList();
            var random = new System.Random();
            while (pathCandidates.Any())
            {
                var candidate = pathCandidates[random.Next(0, pathCandidates.Count)];
                if (_visited.Contains(candidate))
                {
                    pathCandidates.Remove(candidate);
                    continue;
                }

                pathCandidates.AddRange(SetPath(candidate));
                pathCandidates = pathCandidates.Distinct().ToList();
                pathCandidates.Remove(candidate);

                var distanceFromPortal = Vector3Int.Distance(portalGoalPos, candidate);
                var currentClosest = Vector3Int.Distance(closestPortalPos, portalGoalPos);
                closestPortalPos = currentClosest < distanceFromPortal ? closestPortalPos : candidate;
            }

            _player.transform.position = startPos + new Vector3(.5f, .5f, 0);
            ConfigurePortal(closestPortalPos);
        }

        /// <summary>
        /// Fill tile map. 
        /// </summary>
        private void FillWallMap()
        {
            for (var i = -20; i < _width + 20; i++)
            {
                for (var j = -20; j < _height + 20; j++)
                {
                    _wallTileMap.SetTile(new Vector3Int(i, j, 0), _wall);
                }
            }
        }

        /// <summary>
        /// Set Position to a path instead of a wall
        /// </summary>
        /// <param name="pos">Path position</param>
        /// <returns>New Path candidates</returns>
        private IEnumerable<Vector3Int> SetPath(Vector3Int pos)
        {
            _wallTileMap.SetTile(pos, null);
            _floorTileMap.SetTile(pos, _floor);
            return GetPossiblePathNeighbors(pos);
        }

        /// <summary>
        /// Get checks all four sides of tile position. Returns all possible paths.
        /// </summary>
        /// <param name="pos"></param>
        /// <returns>Neighbors eligible to become a path.</returns>
        private IEnumerable<Vector3Int> GetPossiblePathNeighbors(Vector3Int pos)
        {
            var candidatePositions = GetSurroundingTilePos(pos);
            return candidatePositions.Where(IsValidCandidate).ToList();
        }

        /// <summary>
        /// Is supplied tile position able to become a path?
        /// </summary>
        /// <param name="pos">Tile position</param>
        /// <returns>True if it can be a path.</returns>
        private bool IsValidCandidate(Vector3Int pos)
        {
            var candidates = GetSurroundingTilePos(pos);
            var neighborWallCount = 0;
            foreach (var candidate in candidates)
            {
                if (IsEdgePos(candidate)) return false;
                var tile = _wallTileMap.GetTile(candidate);
                if (tile == _wall)
                {
                    neighborWallCount++;
                }
            }

            var isValid = neighborWallCount >= 3;
            if (!isValid) _visited.Add(pos);

            return isValid;
        }

        /// <summary>
        /// Get all direct surrounding positions. This is only
        /// north, east, south, and west. Does not grab diagonals.
        /// </summary>
        /// <param name="pos">Position to grab neighbors of.</param>
        /// <returns>Tile neighbors. Be sure to check if these neighbors are in bounds.</returns>
        private IEnumerable<Vector3Int> GetSurroundingTilePos(Vector3Int pos)
        {
            var northPos = new Vector3Int(pos.x, pos.y + 1, pos.z);
            var eastPos = new Vector3Int(pos.x + 1, pos.y, pos.z);
            var southPos = new Vector3Int(pos.x, pos.y - 1, pos.z);
            var westPos = new Vector3Int(pos.x - 1, pos.y, pos.z);
            var candidatePositions = new List<Vector3Int>
            {
                northPos,
                eastPos,
                southPos,
                westPos
            };

            return candidatePositions;
        }

        /// <summary>
        /// Checks to see if this position is an edge position. An edge position should never be a
        /// path.
        /// </summary>
        /// <param name="pos">Tile position to check</param>
        /// <returns>True if provided position is on the perimeter of valid play.</returns>
        private bool IsEdgePos(Vector3Int pos)
        {
            return (pos.x < 0 || pos.x >= _width) || (pos.y < 0 || pos.y >= _height);
        }

        /// <summary>
        /// Create portal, then place it in the top center of the maze.
        /// </summary>
        /// <param name="pos">Position to place the portal. Be sure that this is a walkable tile.</param>
        private void ConfigurePortal(Vector3Int pos)
        {
            var portal = Instantiate(_goal, pos + new Vector3(.5f, .5f, 0), Quaternion.identity);
            var portalScript = portal.GetComponent<Portal>() ?? portal.GetComponentInChildren<Portal>();
            portalScript.SetNextScene(_nextSceneName);
        }

        private void UpdateZombieParameters()
        {
            var zombieSpawnerGO = GameObject.Find("MazeEnvironment/ZombieSpawner");
            var zombieSpawner = zombieSpawnerGO?.GetComponent<ZombieSpawner>();
            zombieSpawner?.SetMaxCreatureCount(InGameViewModel.Level);
        }
    }
}
