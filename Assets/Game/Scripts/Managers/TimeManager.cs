#region _____________________________/ INFOS
//  AUTHOR : Nathan THEOPHILE (2025)
//  Engine : Unity
//  Singleton
//  Note : MY_CONST, myPublic, m_MyProtected, _MyPrivate, lMyLocal, MyFunc(), pMyParam, onMyEvent, OnMyCallback, MyStruct
#endregion

using UnityEngine;
using System.Collections.Generic;

namespace Rush.Game
{
    public class TimeManager : MonoBehaviour
    {
        public List<ITickDependant> objectsAffectedByTime = new List<ITickDependant>();

        [Header("Speed Values")]
        [SerializeField, Range(1f, 5f)] private float _MaxSpeed = 3f;


        private float _GlobalTickSpeed = 1f;
        public float GlobalTickSpeed { get => _GlobalTickSpeed; set => _GlobalTickSpeed = Mathf.Clamp(value, 0f, _MaxSpeed); }

        private float _CurrentTickRatio = 0f;

        public bool pause = false;
        private float _ElapsedTime = 0f;
        private float _TickDuration = 1f;

        public static TimeManager instance { get; private set; }

        private void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(gameObject);
                return;
            }
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        private void OnDestroy()
        {
            if (instance == this) instance = null;
        }

        private void Update()
        {
            if (pause) return;

            _ElapsedTime += Time.deltaTime * _GlobalTickSpeed;
            _CurrentTickRatio = _ElapsedTime / _TickDuration;

            AdministrateTime();

            if (_ElapsedTime >= _TickDuration) _ElapsedTime = 0f;
        }

        private void AdministrateTime() {
            foreach (ITickDependant lObject in objectsAffectedByTime) lObject.currentTickRatio = _CurrentTickRatio; }
    }
}