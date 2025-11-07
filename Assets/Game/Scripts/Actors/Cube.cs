#region _____________________________/ INFOS
//  AUTHOR : Nathan THEOPHILE (2025)
//  Engine : Unity
//  Independant
//  Note : MY_CONST, myPublic, m_MyProtected, _MyPrivate, lMyLocal, MyFunc(), pMyParam, onMyEvent, OnMyCallback, MyStruct
#endregion

using UnityEngine;

namespace Rush.Game
{
    public class Cube : MonoBehaviour, ITickDependant
    {
        #region _________________________/ TIME VALUES
        [Header("Time")]
        public float currentTickStep { get; set; }

        #endregion

        #region _________________________/ MOVEMENT VALUES
        [Header("Time")]
        private float       _BaseAngle = 90f;
        private Vector3     _Pivot;
        private Quaternion  _StartRotation, _EndRotation;
        private Vector3     _StartPosition, _EndPosition;

        #endregion

        #region _________________________/ RAYCAST VARS
        [Header("Raycasts")]
        [SerializeField] private LayerMask _GroundLayer;
        [SerializeField] private LayerMask _TilesLayer;

        #endregion

    

        void Start()
        {
            if (TimeManager.Instance != null)
                TimeManager.Instance.objectsAffectedByTime.Add(this);

            SetModeRoll();
        }

        void Update()
        {
            Roll(currentTickStep);
        }

        public void TickUpdate(int pTickIndex)
        {
            Snap();
            SetModeRoll();
        }

        public void SetModeRoll()
        {
            Vector3 lAxis = Vector3.Cross(Vector3.up, Vector3.forward);
            _Pivot = transform.position + (Vector3.down + Vector3.forward) * (1f / 2f);

            _StartRotation = transform.rotation;
            _EndRotation = Quaternion.AngleAxis(_BaseAngle, lAxis) * _StartRotation;

            _StartPosition = transform.position - _Pivot;
            _EndPosition = _StartPosition + Vector3.forward * 1f;
        }

        void Roll(float pCurrentTickStep)
        {
            transform.rotation = Quaternion.Slerp(_StartRotation, _EndRotation, pCurrentTickStep);
            transform.position = _Pivot + Vector3.Slerp(_StartPosition, _EndPosition, pCurrentTickStep);
        }

        void Snap()
        {
            transform.rotation = _EndRotation;
            transform.position = _Pivot + _EndPosition;
        }
    
    }
}

