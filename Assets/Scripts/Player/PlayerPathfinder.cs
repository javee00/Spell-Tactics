using System.Collections.Generic;
using UnityEngine;
using Grid;

public class PlayerPathfinder : MonoBehaviour
{
    [SerializeField] private GameObject playerObject;
    [SerializeField] private GameObject destinationMarker;
    [SerializeField] private ObjectPlacer objectPlacer;
    [SerializeField] Vector2Int initPositionPlayer = new(0, 0);
    [SerializeField] private EnemyLocationController enemyLocationController;

    private List<Vector2Int> currentPath = new List<Vector2Int>();
    private Vector2Int playerPosition;
    private Coroutine pathFollowingCoroutine;

    private void Start()
    {
        if (PlayerPrefs.HasKey("playerPositionX") && PlayerPrefs.HasKey("playerPositionY"))
        {
            playerPosition = new Vector2Int(PlayerPrefs.GetInt("playerPositionX"), PlayerPrefs.GetInt("playerPositionY"));
        }
        else
        {
            playerPosition = initPositionPlayer;
        }
        MovePlayerToTile(playerPosition);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 hexPosition = HexTile.WorldToHexGridPosition(mousePosition, objectPlacer.radio);
            Vector3 roundedPosition = HexTile.RoundHexPosition(hexPosition);
            Vector2Int destination = new Vector2Int((int)roundedPosition.x, (int)roundedPosition.z);

            if (IsTileGround(destination))
            {
                if (pathFollowingCoroutine != null)
                {
                    StopCoroutine(pathFollowingCoroutine);
                }

                currentPath = CalculatePath(playerPosition, destination);

                if (currentPath.Count > 0)
                {
                    destinationMarker.transform.position = HexTile.CoordenadasAxis(destination.x, destination.y, objectPlacer.radio);
                    pathFollowingCoroutine = StartCoroutine(FollowPath(currentPath));
                }
                else
                {
                    Debug.Log("The destination is not reachable.");
                }
            }
        }
    }

    private void MovePlayerToTile(Vector2Int tilePosition)
    {
        playerObject.transform.position = HexTile.CoordenadasAxis(tilePosition.x, tilePosition.y, objectPlacer.radio);
        playerPosition = tilePosition;
        PlayerPrefs.SetInt("playerPositionX", playerPosition.x);
        PlayerPrefs.SetInt("playerPositionY", playerPosition.y);
    }

    private List<Vector2Int> CalculatePath(Vector2Int start, Vector2Int end)
    {
        List<Vector2Int> path = new List<Vector2Int>();
        HashSet<Vector2Int> closedSet = new HashSet<Vector2Int>();
        PriorityQueue<Vector2Int> openSet = new PriorityQueue<Vector2Int>();

        openSet.Enqueue(start, 0);
        Dictionary<Vector2Int, Vector2Int> cameFrom = new Dictionary<Vector2Int, Vector2Int>();
        Dictionary<Vector2Int, int> gScore = new Dictionary<Vector2Int, int> { [start] = 0 };

        while (openSet.Count > 0)
        {
            Vector2Int current = openSet.Dequeue();

            if (current == end)
            {
                return ReconstructPath(cameFrom, current);
            }

            closedSet.Add(current);

            foreach (Vector2Int neighbor in GetHexNeighbors(current))
            {
                if (closedSet.Contains(neighbor)) continue;

                if (IsTileGround(neighbor) && !enemyLocationController.IsEnemyInCell(neighbor))
                {
                    int tentativeGScore = gScore[current] + 1;

                    if (!gScore.ContainsKey(neighbor) || tentativeGScore < gScore[neighbor])
                    {
                        cameFrom[neighbor] = current;
                        gScore[neighbor] = tentativeGScore;
                        int fScore = tentativeGScore + Heuristic(neighbor, end);
                        openSet.Enqueue(neighbor, fScore);
                    }
                }
            }
        }

        return path;
    }

    private bool IsTileGround(Vector2Int tile)
    {
        Vector2 coords = HexTile.CoordenadasAxis(tile.x, tile.y, objectPlacer.radio);
        if (objectPlacer.GetTileTypeAtPosition(coords) == MapLoader.TileType.GROUND)
        {
            return true;
        }
        return false;
    }

    private List<Vector2Int> GetHexNeighbors(Vector2Int tile)
    {
        List<Vector2Int> potentialNeighbors = new List<Vector2Int>
        {
            new Vector2Int(tile.x + 1, tile.y),
            new Vector2Int(tile.x - 1, tile.y),
            new Vector2Int(tile.x, tile.y + 1),
            new Vector2Int(tile.x, tile.y - 1),
            new Vector2Int(tile.x + 1, tile.y - 1),
            new Vector2Int(tile.x - 1, tile.y + 1)
        };

        List<Vector2Int> validNeighbors = new List<Vector2Int>();
        foreach (var neighbor in potentialNeighbors)
        {
            if (IsTileGround(neighbor))
            {
                validNeighbors.Add(neighbor);
            }
        }

        return validNeighbors;
    }

    private int Heuristic(Vector2Int a, Vector2Int b)
    {
        return (Mathf.Abs(a.x - b.x) + Mathf.Abs(a.x + a.y - b.x - b.y) + Mathf.Abs(a.y - b.y)) / 2;
    }

    private List<Vector2Int> ReconstructPath(Dictionary<Vector2Int, Vector2Int> cameFrom, Vector2Int current)
    {
        List<Vector2Int> totalPath = new List<Vector2Int> { current };
        while (cameFrom.ContainsKey(current))
        {
            current = cameFrom[current];
            totalPath.Insert(0, current);
        }
        return totalPath;
    }

    private System.Collections.IEnumerator FollowPath(List<Vector2Int> path)
    {
        foreach (var tile in path)
        {
            MovePlayerToTile(tile);
            yield return new WaitForSeconds(0.2f);
        }
    }
}