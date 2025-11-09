#region _____________________________/ INFOS
//  AUTHOR : Nathan THEOPHILE (2025)
//  Engine : Unity
//  Singleton
//  Note : MY_CONST, myPublic, m_MyProtected, _MyPrivate, lMyLocal, MyFunc(), pMyParam, onMyEvent, OnMyCallback, MyStruct
#endregion

using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEngine.Events;

namespace Rush.Game
{
    public class TimeManager : MonoBehaviour
    {
        public List<ITickDependant> objectsAffectedByTime = new List<ITickDependant>();

        [Header("Speed Values")]
        [SerializeField, Range(1f, 5f)] private float _MaxSpeed = 3f;

        public event Action<int> onTickFinished;

        private float _GlobalTickSpeed = 1f;
        public float GlobalTickSpeed { get => _GlobalTickSpeed; set => _GlobalTickSpeed = Mathf.Clamp(value, 0f, _MaxSpeed); }

        private float _CurrentTickRatio = 0f;

        public bool pause = false;
        private float _ElapsedTime = 0f;
        private float _TickDuration = 1f;
        private int _TickIndex = 0;

        public static TimeManager Instance { get; private set; }

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

        private void OnDestroy()
        {
            if (Instance == this) Instance = null;
        }

        private void Update()
        {
            if (pause) return;

            _ElapsedTime += Time.deltaTime * _GlobalTickSpeed;


            if (_ElapsedTime >= _TickDuration)
            {
                _ElapsedTime = 0f;
                _TickIndex++;
                onTickFinished.Invoke(_TickIndex);
            }
            
            _CurrentTickRatio = _ElapsedTime / _TickDuration;

            AdministrateTime();

        }

        private void AdministrateTime() {
            foreach (ITickDependant lObject in objectsAffectedByTime) lObject.currentTickStep = _CurrentTickRatio; }
    }
}