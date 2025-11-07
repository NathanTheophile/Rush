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
        public float currentTickRatio { get; set; }

        private float _BaseAngle = 90f;

        private Vector3 _Pivot;
        private Quaternion _StartRotation, _EndRotation;
        private Vector3 _StartPosition, _EndPosition;

        void Start()
        {
            if (TimeManager.instance != null)
                TimeManager.instance.objectsAffectedByTime.Add(this);

            SetModeRoll();
        }

        void Update()
        {
            Roll(currentTickRatio);
        }

        public void TickUpdate()
        {
            
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

        void Roll(float t)
        {
            transform.rotation = Quaternion.Slerp(_StartRotation, _EndRotation, t);
            transform.position = _Pivot + Vector3.Slerp(_StartPosition, _EndPosition, t);

            if (t >= 1f)
            {
                transform.rotation = _EndRotation;
                transform.position = _Pivot + _EndPosition;
            }
        }
    
    }
}

