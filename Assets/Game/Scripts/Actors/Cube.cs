#region _____________________________/ INFOS
//  AUTHOR : Nathan THEOPHILE (2025)
//  Engine : Unity
//  Note : MY_CONST, myPublic, m_MyProtected, _MyPrivate, lMyLocal, MyFunc(), pMyParam, onMyEvent, OnMyCallback, MyStruct
#endregion

using System;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.UIElements;

namespace Rush.Game
{
    public class Cube : MonoBehaviour, ITickDependant
    {
        #region _________________________/ TIME VALUES
        [Header("Time")]
        public float currentTickStep { get; set; }

        private float _GridSize = 1f;

        #endregion

        #region _________________________/ MOVEMENT VALUES
        [Header("Time")]
        private float       _BaseAngle = 90f;
        private Vector3     pivotPoint;
        private Quaternion  _StartRotation, _EndRotation;
        private Vector3 fromPosition, toPosition;
        private Vector3 direction;
        private Transform selfTransform;

        private Action doAction;

        #endregion

        #region _________________________/ RAYCAST VARS
        [Header("Raycasts")]
        [SerializeField] private LayerMask _GroundLayer;
        [SerializeField] private LayerMask _TilesLayer;

        #endregion


        void Awake()
        {
            selfTransform = transform;
            direction = selfTransform.forward;
            doAction = Wait;
        }

        void Start()
        {

        }

        void Update()
        {
            doAction();
        }

        public void TickUpdate(int pTickIndex)
        {
            //Snap();
            Debug.Log("Tick update reçu");
            SetNextMode();
        }


        private void SetNextMode()
        {
            if (TryFindGround()) { Debug.Log("Sol detecté"); SetModeRoll(); }
            else { Debug.Log("Sol non detecté"); SetModeFall(); }
        }

        #region _________________________| COLLISIONS
        /// <returns>retourne si le raycast a détecté qq chose et si tile retourne tile sinon null</returns>
        private bool TryFindGround()
        {
            Debug.DrawRay(selfTransform.position, Vector3.down);
            if (Physics.Raycast(selfTransform.position, Vector3.down, out var hit, _GridSize, _GroundLayer | _TilesLayer))
            {
                return true;
            }
            return false;
        }

        private void Wait()
        {
            
        }

        #endregion

        private void GetLerpMovement(Vector3 pOrigin, Vector3 pDirection)
        {
            fromPosition = pOrigin;
            toPosition = fromPosition + pDirection * _GridSize;
        }
    

        public void SetModeRoll()
        {
            Debug.Log("Setting roll mode");

            Vector3 lAxis = Vector3.Cross(Vector3.up, Vector3.forward);
            pivotPoint = selfTransform.position + (Vector3.down + Vector3.forward) * (1f / 2f);

            _StartRotation = selfTransform.rotation;
            _EndRotation = Quaternion.AngleAxis(_BaseAngle, lAxis) * _StartRotation;

            GetLerpMovement(selfTransform.position - pivotPoint, direction);

            Debug.Log($"Start position = {fromPosition}. To Position = {toPosition}.");
            doAction = Roll;
        }

        void Roll()
        {
            selfTransform.rotation = Quaternion.Slerp(_StartRotation, _EndRotation, currentTickStep);
            selfTransform.position = pivotPoint + Vector3.Slerp(fromPosition, toPosition, currentTickStep);
        }

        private void SetModeFall()
        {
            GetLerpMovement(selfTransform.position, Vector3.down);
            doAction = Fall;
        }

        void Fall()
        {
            selfTransform.position = Vector3.Lerp(fromPosition, toPosition, currentTickStep);
        }

        void Snap()
        {
            selfTransform.rotation = _EndRotation;
            selfTransform.position = pivotPoint + toPosition;
        }
    
    }
}