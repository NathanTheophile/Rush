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

        [SerializeField] private GameStates _InitialState = GameStates.Cards;

        public GameStates CurrentState { get; private set; }

        public event System.Action<GameStates> onGameStateChanged;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            CurrentState = _InitialState;
            ApplyState(CurrentState);
        }

        private void OnDestroy()
        {
            if (Instance == this) Instance = null;
        }

        public void SetState(GameStates newState)
        {
            if (CurrentState == newState) return;
            Debug.Log("" + newState.ToString());
            CurrentState = newState;
            ApplyState(CurrentState);
            onGameStateChanged?.Invoke(CurrentState);
        }

        private static void ApplyState(GameStates state)
        {
            Debug.Log($"{state}");
            var timeManager = Manager_Time.Instance;
            if (timeManager == null) return;

            timeManager.pause = state == GameStates.Pause;
        }
    }
}