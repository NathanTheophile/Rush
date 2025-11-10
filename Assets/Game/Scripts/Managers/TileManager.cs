#region _____________________________/ INFOS
//  AUTHOR : Nathan THEOPHILE (2025)
//  Engine : Unity
//  Singleton
//  Note : MY_CONST, myPublic, m_MyProtected, _MyPrivate, lMyLocal, MyFunc(), pMyParam, onMyEvent, OnMyCallback, MyStruct
#endregion

using UnityEngine;
using System.Collections.Generic;
using System;
using Unity.VisualScripting;

namespace Rush.Game
{
    public class TileManager : MonoBehaviour
    {
        public static TileManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        public void TryGetTile(Cube pCube, RaycastHit pHit)
        {
            if (pHit.collider != null && pHit.collider.TryGetComponent(out Tile lTile))
            {
                switch (lTile.tileVariant)
                {
                    case Tile.TileVariants.Stopper:
                        pCube.SetModePause(); break;
                    case Tile.TileVariants.Arrow:
                        pCube.SetModeRoll(lTile.direction); break;
                    case Tile.TileVariants.Convoyer:
                        pCube.SetModeSlide(lTile.direction); break;
                    case Tile.TileVariants.Dispatcher:
                        pCube.SetModeRoll(lTile.direction);
                        Dispatcher lDispatcher = (Dispatcher)lTile;
                        lDispatcher.Switch(); break;
                    case Tile.TileVariants.Teleporter:
                        Teleporter lTeleporter = (Teleporter)lTile;
                        Vector3 lTarget = lTeleporter.pairedTeleporter.position;
                        pCube.SetModeTeleportation(lTarget); break;
                    default: pCube.SetModeRoll(pCube._Direction); break;

                }
            }
        }

        private void OnDestroy()
        {
            if (Instance == this) Instance = null;
        }
    }
}