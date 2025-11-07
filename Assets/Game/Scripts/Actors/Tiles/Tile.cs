#region _____________________________/ INFOS
//  AUTHOR : Nathan THEOPHILE (2025)
//  Engine : Unity
//  Note : MY_CONST, myPublic, m_MyProtected, _MyPrivate, lMyLocal, MyFunc(), pMyParam, onMyEvent, OnMyCallback, MyStruct
#endregion

using UnityEngine;

namespace Rush.Game
{
    public class Tile : MonoBehaviour
    {
        [SerializeField] public TileVariants tileVariant = TileVariants.Default;
        public Vector3 direction;

        public enum TileVariants
        {
            Default,
            Arrow,
            Convoyer,
            Dispatcher,
            Teleporter,
            Stopper,
            Spawner,
            Target
        }


        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start() { direction = transform.forward; }
    }
}