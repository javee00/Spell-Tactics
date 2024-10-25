using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Grid
{

    [RequireComponent(typeof(MapLoader))]
    public class ObjectPlacer : MonoBehaviour
    {
        private MapLoader mapLoader;             // Referencia a MapLoader
        public float radio = 2f;    // Factor para aumentar la distancia entre instancias



        private List<PlacedObjectInfo> placedObjects = new List<PlacedObjectInfo>();

        //Tiles
        [SerializeField]
        private Tilemap tilemap;
        [SerializeField]
        private Tile wall;
        [SerializeField]
        private Tile ground;

        void Start()
        {
            mapLoader = GetComponent<MapLoader>();
            PlaceObjects();
        }

        void PlaceObjects()
        {

            if (mapLoader == null || mapLoader.GetMapTiles() == null)
            {
                Debug.LogError("MapLoader no está asignado o los datos del mapa no están cargados.");
                return;
            }


            int width = mapLoader.GetMapTiles().GetLength(0);
            int height = mapLoader.GetMapTiles().GetLength(1);

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    Vector2 adjustedPosition = HexTile.HexagonalPosition(x, y, radio, true);

                    if (mapLoader.GetMapTiles()[x, y] == MapLoader.TileType.GROUND)
                    {
                        tilemap.SetTile(new Vector3Int(y, x, 0), ground);
                        placedObjects.Add(new PlacedObjectInfo(adjustedPosition, MapLoader.TileType.GROUND));
                    }

                    else if (mapLoader.GetMapTiles()[x, y] == MapLoader.TileType.WALL)
                    {
                        tilemap.SetTile(new Vector3Int(y, x, 0), wall);
                        placedObjects.Add(new PlacedObjectInfo(adjustedPosition, MapLoader.TileType.WALL));
                    }
                }
            }
        }

        public MapLoader.TileType? GetTileTypeAtPosition(Vector2 position)
        {
            return placedObjects
                .FirstOrDefault(placedObject => Vector2.Distance(placedObject.position, position) < 0.01f)?
                .tileType;
        }
    }
}