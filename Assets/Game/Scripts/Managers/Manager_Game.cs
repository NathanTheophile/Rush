#region _____________________________/ INFOS
//  AUTHOR : Nathan THEOPHILE (2025)
//  Engine : Unity
//  Singleton
//  Note : MY_CONST, myPublic, m_MyProtected, _MyPrivate, lMyLocal, MyFunc(), pMyParam, onMyEvent, OnMyCallback, MyStruct
#endregion

using System;
using UnityEditor;
using UnityEngine;

namespace Rush.Game
{
    public class Manager_Game : MonoBehaviour
    {
        #region _____________________________/ SINGLETON

        public static Manager_Game Instance { get; private set; }

        #endregion

        #region _____________________________/ GAME STATES

        public enum GameStates { Cards, Setup, Play, Pause }
        
        private GameStates _InitialState = GameStates.Cards;
        public  GameStates CurrentState { get; private set; }

        public event Action<GameStates> onGameStateChanged;
        public event Action onLevelFinished;

        #endregion

        #region _____________________________/ LEVEL DATA

        public SO_LevelData CurrentLevel { get; private set; }

        private int _CubesToComplete;
        private int _CubesArrived;

        #endregion

        #region _____________________________| INIT

        private void Awake()
        {
            if (Instance != null && Instance != this) { Destroy(gameObject); return; }
            Instance = this;
            DontDestroyOnLoad(gameObject);

            CurrentState = _InitialState;
            ApplyState(CurrentState);
        }

        #endregion

        #region _____________________________| GAME STATES

        public void SetState(GameStates pNewState)
        {
            if (CurrentState == pNewState) return;
            CurrentState = pNewState;
            ApplyState(CurrentState);
            onGameStateChanged?.Invoke(CurrentState);
        }

        private static void ApplyState(GameStates state)
        {
            var timeManager = Manager_Time.Instance;
            if (timeManager == null) return;

            timeManager.pause = state == GameStates.Pause;
        }

        public void UpdateCubesAmountoComplete(int pAmount) => _CubesToComplete += pAmount; 

        public void UpdateCubeArrived()
        {
            _CubesArrived++;
            if (_CubesArrived >= _CubesToComplete)
            {            
                onLevelFinished?.Invoke();
                SetState(GameStates.Cards);
            }
        }
        
        #endregion

        #region _____________________________/ LEVEL DATA

        public void SpawnCurrentLevel(SO_LevelData pLevelData)
        {
            Debug.Log($"{pLevelData.levelName} has {pLevelData.levelPrefab.name} prefab.");
            CurrentLevel = pLevelData;
            Instantiate(CurrentLevel.levelPrefab, Vector3.zero, Quaternion.identity);
        }




        #endregion

        #region _____________________________/ DESTROY

        private void OnDestroy() {
            if (Instance == this) Instance = null; }

        #endregion
    }
}