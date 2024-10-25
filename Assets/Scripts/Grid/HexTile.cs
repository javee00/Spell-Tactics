using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Grid
{
    public static class HexTile
    {
        // Convierte la posición del mundo a coordenadas de grilla hexagonal (axial).
        public static Vector2 WorldToHexGridPosition(Vector2 worldPosition, float hexRadius)
        {
            float q = (worldPosition.x * 2f / 3f) / hexRadius;
            float r = (-worldPosition.x / 3f + Mathf.Sqrt(3) / 3 * worldPosition.y) / hexRadius;
            return new Vector2(q, r);
        }

        // Redondea la posición axial a la coordenada entera más cercana.
        public static Vector3 RoundHexPosition(Vector2 axial)
        {
            float x = axial.x;
            float z = axial.y;
            float y = -x - z;

            int rx = Mathf.RoundToInt(x);
            int ry = Mathf.RoundToInt(y);
            int rz = Mathf.RoundToInt(z);

            float x_diff = Mathf.Abs(rx - x);
            float y_diff = Mathf.Abs(ry - y);
            float z_diff = Mathf.Abs(rz - z);

            if (x_diff > y_diff && x_diff > z_diff)
            {
                rx = -ry - rz;
            }
            else if (y_diff > z_diff)
            {
                ry = -rx - rz;
            }
            else
            {
                rz = -rx - ry;
            }

            return new Vector3(rx, ry, rz);
        }
        public static Vector2 CoordenadasAxis(int q, int r, float hexRadius)
        {
            float x = hexRadius * 3f / 2f * q;
            float y = hexRadius * Mathf.Sqrt(3) * (r + q / 2f);
            return new Vector2(x, y);
        }
        public static Vector2 HexagonalPosition(int primary, int secondary, float radio, bool isYXZ = true)
        {
            if (isYXZ)
            {
                float x = radio * 3f / 2f * primary;
                float y = radio * Mathf.Sqrt(3) * (secondary + (primary % 2) * 0.5f);
                return new Vector2(x, y);
            }
            else
            {
                float y = radio * 3f / 2f * secondary;
                float x = radio * Mathf.Sqrt(3) * (primary + (secondary % 2) * 0.5f);
                return new Vector2(x, y);
            }
        }

    }
}
