using UnityEngine;

public class MapLoader : MonoBehaviour
{
    public enum TileType
    {
        WALL,
        GROUND
    }

    private TileType[,] mapTiles;
    private Vector2[,] tilePositions;
    /// ///////////////////////////////////////////////////////
    public TextAsset mapData;  // Archivo de datos del mapa

    private int ancho;
    private int alto;


    void Awake()
    {
        // Carga el mapa solo si el TextAsset está asignado
        if (mapData != null)
        {
            LoadMap();
        }
        else
        {
            Debug.LogError("mapData no está asignado. Por favor, asigna un archivo de texto con los datos del mapa.");
        }
    }

    void LoadMap()
    {
        string mapText = mapData.text;

        // Verifica si el mapa está vacío
        if (string.IsNullOrEmpty(mapText))
        {
            Debug.LogError("Los datos del mapa están vacíos.");
            return;
        }

        // Separa las líneas del mapa
        string[] lines = mapText.Split(new[] { '\r', '\n' }, System.StringSplitOptions.RemoveEmptyEntries);
        ancho = lines[0].Length;
        alto = lines.Length;

        // Inicializa las matrices
        mapTiles = new TileType[ancho, alto];
        tilePositions = new Vector2[ancho, alto];

        // Rellena las matrices con datos del mapa
        for (int y = 0; y < alto; y++)
        {
            for (int x = 0; x < ancho; x++)
            {
                char tileChar = lines[alto - 1 - y][x];
                mapTiles[x, y] = CharToTileType(tileChar);
                tilePositions[x, y] = new Vector2(x, y);
            }
        }
    }

    TileType CharToTileType(char c)
    {
        switch (c)
        {
            case '#':
                return TileType.WALL;
            case '.':
                return TileType.GROUND;
            default:
                Debug.LogWarning($"Carácter no reconocido en el mapa: {c}");
                return TileType.GROUND; // Valor por defecto
        }
    }

    public TileType[,] GetMapTiles()
    {
        return mapTiles;
    }

    public Vector2[,] GetTilePositions()
    {
        return tilePositions;
    }
    //Orientación

    public Vector2 Rectangular(int x, int y, float radio)
    {
        Vector2 basePosition = GetTilePositions()[x, y];
        return basePosition * radio;
    }

    public int width => ancho;
    public int height => alto;
}