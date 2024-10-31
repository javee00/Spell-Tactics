using System.Collections.Generic;
using UnityEngine;
using Grid;

public class EnemyLocationController : MonoBehaviour
{
    public List<Vector2Int> enemyLocations;
    public GameObject enemyPrefab;
    [SerializeField] private ObjectPlacer objectPlacer;

    void Start()
    {
        foreach (var location in enemyLocations)
        {
            PlaceEnemyOnGrid(location);
        }
    }

    private void PlaceEnemyOnGrid(Vector2Int location)
    {
        Vector2 worldPosition = HexTile.CoordenadasAxis(location.x, location.y, objectPlacer.radio);
        Vector3 adjustedPosition = new Vector3(worldPosition.x, worldPosition.y, 0);
        GameObject enemyObj = Instantiate(enemyPrefab, adjustedPosition, Quaternion.identity, transform);
        enemyObj.GetComponent<AttackEnemy>().SetName("Pichurrin23454");
    }

    public bool IsEnemyInCell(Vector2Int cell)
    {
        return enemyLocations.Contains(cell);
    }
}
