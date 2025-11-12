#region _____________________________/ INFOS
//  AUTHOR : Nathan THEOPHILE (2025)
//  Engine : Unity
//  Singleton
//  Note : MY_CONST, myPublic, m_MyProtected, _MyPrivate, lMyLocal, MyFunc(), pMyParam, onMyEvent, OnMyCallback, MyStruct
#endregion

using UnityEngine;

namespace Rush.Game
{
    public class Manager_Game : MonoBehaviour
    {
        public static Manager_Game Instance { get; private set; }

        public enum GameStates { Cards, Setup, Play, Pause }

        public GameStates gameState = GameStates.Cards;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
        }


        private void OnDestroy()
        {
            if (Instance == this) Instance = null;
        }
    }
}