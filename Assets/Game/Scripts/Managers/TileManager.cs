#region _____________________________/ INFOS
//  AUTHOR : Nathan THEOPHILE (2025)
//  Engine : Unity
//  Singleton
//  Note : MY_CONST, myPublic, m_MyProtected, _MyPrivate, lMyLocal, MyFunc(), pMyParam, onMyEvent, OnMyCallback, MyStruct
#endregion

using UnityEditor.Rendering;
using UnityEngine;

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
                    case Tile.TileVariants.Stopper:     Stopper(pCube);                         break;
                    case Tile.TileVariants.Arrow:       Arrow(pCube, lTile);                    break;
                    case Tile.TileVariants.Convoyer:    Convoyer(pCube, lTile);                 break;
                    case Tile.TileVariants.Dispatcher:  Dispatcher(pCube, (Dispatcher)lTile);   break;
                    case Tile.TileVariants.Teleporter:  Teleporter(pCube, (Teleporter)lTile);   break;
                    case Tile.TileVariants.Target:      Target(pCube, (Target)lTile);                   break;
                    default: pCube.SetModeRoll(); break;
                }
            }
        }

        private void Stopper(Cube pCube) => pCube.SetModePause();
        private void Arrow(Cube pCube, Tile pTile) => pCube.SetModeRoll(pTile.direction);
        private void Convoyer(Cube pCube, Tile pTile) => pCube.SetModeSlide(pTile.direction);
        private void Dispatcher(Cube pCube, Dispatcher pDispatcher) { pCube.SetModeRoll(pDispatcher.direction); pDispatcher.Switch(); }
        private void Teleporter(Cube pCube, Teleporter pTeleporter)
        {
            if (pCube.justTeleported) { pCube.SetModeRoll(); pCube.justTeleported = false; }
            else
            {
                pCube.justTeleported = true;
                Vector3 lTarget = pTeleporter.pairedTeleporter.position;
                pCube.SetModeTeleportation(lTarget);
            }
        }
        private void Target(Cube pCube, Target pTile) => pTile.CheckColor(pCube);

        private void OnDestroy()
        {
            if (Instance == this) Instance = null;
        }
    }
}