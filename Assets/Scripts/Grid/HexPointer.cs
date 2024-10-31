using UnityEngine;
using System;
using Unity.VisualScripting;

namespace Grid
{
    [RequireComponent(typeof(ObjectPlacer))]
    public class HexPointer : MonoBehaviour
    {
        [SerializeField]
        private GameObject punteroHexagonal;
        [SerializeField]
        private EnemyLocationController enemyLocationController;
        private ObjectPlacer objectPlacer;
        private string este = ";";

        private void Start()
        {
            objectPlacer = GetComponent<ObjectPlacer>();
        }

        private void Update()
        {
            UpdatePointerPosition();
        }

        public void UpdatePointerPosition()
        {
            //Calc�la las posiciones convirtiendolas en pixeles hexagonales
            Vector2 PosisionDeRaton = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 axial = HexTile.WorldToHexGridPosition(PosisionDeRaton, objectPlacer.radio);
            Vector3 Redondeo = HexTile.RoundHexPosition(axial);
            Vector2 NuevaPosision = HexTile.CoordenadasAxis((int)Redondeo.x, (int)Redondeo.z, objectPlacer.radio);
            punteroHexagonal.transform.position = NuevaPosision;


            //Activa y descativa el Puntero
            string TipoDeTile = objectPlacer.GetTileTypeAtPosition(NuevaPosision).ToString();
            ActualizarTipoDeTile(TipoDeTile);
            bool isGround = TipoDeTile == "GROUND" && !enemyLocationController.IsEnemyInCell(new Vector2Int((int)Redondeo.x, (int)Redondeo.z));
            Puntero(isGround);

        }

        public void Puntero(bool active)
        {
            punteroHexagonal.SetActive(active);
        }
        public void ActualizarTipoDeTile(string tipoDeTile)
        {

            if (string.IsNullOrEmpty(tipoDeTile))
            {
                return;
            }

            if (tipoDeTile.Equals(este))
            {
                return;
            }

            este = tipoDeTile;
            Debug.Log(tipoDeTile);
        }

    }
}