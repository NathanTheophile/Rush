#region _____________________________/ INFOS
//  AUTHOR : Nathan THEOPHILE (2025)
//  Engine : Unity
//  Singleton
//  Note : MY_CONST, myPublic, m_MyProtected, _MyPrivate, lMyLocal, MyFunc(), pMyParam, onMyEvent, OnMyCallback, MyStruct
#endregion

using System;
using Rush.Game.Core;
using UnityEngine;

namespace Rush.Game
{
    public class Manager_Game : MonoBehaviour
    {
        #region _____________________________/ SINGLETON

        public static Manager_Game Instance { get; private set; }

        private void CheckForInstance()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }

        #endregion

        public event Action onLevelFinished;
        public event Action onGameOver;
        public event Action onGameWon;

        #region _____________________________/ LEVEL DATA

        public SO_LevelData CurrentLevel { get; private set; }

        private int _CubesToComplete;
        private int _CubesArrived;

        #endregion

        #region _____________________________| INIT

        private void Awake() => CheckForInstance();

        #endregion

        public void UpdateCubesAmountoComplete(int pAmount) => _CubesToComplete += pAmount;

        public void UpdateCubeArrived()
        {
            _CubesArrived++;
            if (_CubesArrived >= _CubesToComplete)
            {
                onGameWon?.Invoke();
            }
        }
        
        public void GameOver()
        {
            Manager_Time.Instance.UpdatePauseStatus();
            onGameOver?.Invoke();
        }

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