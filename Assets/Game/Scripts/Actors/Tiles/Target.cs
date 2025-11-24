#region _____________________________/ INFOS
//  AUTHOR : Nathan THEOPHILE (2025)
//  Engine : Unity
//  Note : MY_CONST, myPublic, m_MyProtected, _MyPrivate, lMyLocal, MyFunc(), pMyParam, onMyEvent, OnMyCallback, MyStruct
#endregion

using System;
using Unity.VisualScripting;
using UnityEngine;

namespace Rush.Game
{
    public class Target : Tile
    {
        [SerializeField] private SO_Colors _ColorSO;
        private Color _Color;

        private Manager_Time     timeManager;
        private Manager_Tile     tileManager;
        private Manager_Game     gameManager;

        public event Action onCubeValidation;


        void Awake()
        {
            _Color = _ColorSO.Color;
            GetComponentInChildren<Renderer>().material.color = _Color;
        }

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        protected override void Start()
        {
            base.Start();
            timeManager = Manager_Time.Instance;
            tileManager = Manager_Tile.Instance;
            gameManager = Manager_Game.Instance;

            onCubeValidation += levelManager.CubeValidated;
        }

        public bool CheckColor(Cube pCube)
        {
            if (pCube.Color != _Color) return false;
            DestroyCube(pCube);
            onCubeValidation.Invoke();
            return true;
        }
        
        private void DestroyCube(Cube pCube)
        {
            timeManager.onTickFinished -= pCube.TickUpdate;
            timeManager.objectsAffectedByTime.Remove(pCube);
            pCube.onTileDetected -= tileManager.TryGetTile;
            gameManager.UpdateCubeArrived();

            Destroy(pCube.GameObject());
        }
    }
}