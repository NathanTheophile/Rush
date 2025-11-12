#region _____________________________/ INFOS
//  AUTHOR : Nathan THEOPHILE (2025)
//  Engine : Unity
//  Independant
//  Note : MY_CONST, myPublic, m_MyProtected, _MyPrivate, lMyLocal, MyFunc(), pMyParam, onMyEvent, OnMyCallback, MyStruct
#endregion

using UnityEngine;

namespace Rush.Game
{
    [RequireComponent(typeof(Camera))]
    public class PreviewCamera : MonoBehaviour
    {
        #region ___________________________/ TARGET
        [Header("Target")]
        [SerializeField] private Vector3 _TargetPosition = Vector3.zero;
        #endregion

        #region ___________________________/ SPHERICAL COORDINATES
        [Header("Spherical Coordinates")]
        [SerializeField] private float _Radius = 20;
        [SerializeField] private float _MinRadius = 5f;
        [SerializeField] private float _MaxRadius = 50;
        [SerializeField, Range(0f, 180f)] private float _Colatitude = 60f;
        #endregion

        #region ___________________________/ INPUT SETTINGS
        [Header("Input")]
        [SerializeField] private float _RotationSensitivity = 0.1f;   // Degrees per pixel
        [SerializeField] private float _ScrollZoomSensitivity = 0.2f; // Units per scroll step
        [SerializeField] private float _PinchZoomSensitivity = 0.02f; // Units per pixel
        [SerializeField] private float _InertiaDamping = 4f;

                [SerializeField] private float _RotateSpeed = 45f;     
        #endregion

        #region ___________________________/ STATE
        private float      _Theta;
        private float      _RotationVelocity;
        private float      _ZoomVelocity;
        private bool       _IsDragging;
        private bool       _IsPinching;
        private bool       _HasZoomInput;
        private Vector2    _LastPointerPosition;
        private float _PreviousPinchDistance;

        public bool canRotate;
                
        #endregion

        void Start()
        {
            UpdateCameraPosition();
        }

        void Update()
        {
            ApplyInertia(Time.deltaTime);
                        if (canRotate)
            {
                RotatePreview(Time.deltaTime);
            }
            UpdateCameraPosition();
        }

        public void AddTargetWorldOffset(Vector3 pWorldOffset) => _TargetPosition += pWorldOffset;

        #region ___________________________| INPUTS

        void HandleHover()
        {

        }

        #endregion

        void ApplyInertia(float pDeltaTime)
        {
            if (Mathf.Approximately(pDeltaTime, 0f))
                return;

            if (!_IsDragging && Mathf.Abs(_RotationVelocity) > 0.0001f)
            {
                _Theta += _RotationVelocity * pDeltaTime;
                float lDampingFactor = Mathf.Exp(-_InertiaDamping * pDeltaTime);
                _RotationVelocity *= lDampingFactor;

                if (Mathf.Abs(_RotationVelocity) < 0.0001f)
                    _RotationVelocity = 0f;
            }

            if (!_IsPinching && !_HasZoomInput && Mathf.Abs(_ZoomVelocity) > 0.0001f)
            {
                AdjustRadius(_ZoomVelocity * pDeltaTime);

                float lDampingFactor = Mathf.Exp(-_InertiaDamping * pDeltaTime);
                _ZoomVelocity *= lDampingFactor;

                if (Mathf.Abs(_ZoomVelocity) < 0.0001f)
                    _ZoomVelocity = 0f;
            }
        }

        void AdjustRadius(float pDelta)
        {
            _Radius = Mathf.Clamp(_Radius + pDelta, _MinRadius, _MaxRadius);
        }

        void UpdateCameraPosition()
        {
            float lPhi = Mathf.Deg2Rad * _Colatitude;
            float lSinPhi = Mathf.Sin(lPhi);

            Vector3 lOffset = new Vector3
            (
                _Radius * lSinPhi * Mathf.Cos(_Theta),
                _Radius * Mathf.Cos(lPhi),
                _Radius * lSinPhi * Mathf.Sin(_Theta)
            );

            transform.position = _TargetPosition + lOffset;
            transform.LookAt(_TargetPosition, Vector3.up);
        }

                void RotatePreview(float pDeltaTime)
        {
            if (Mathf.Approximately(pDeltaTime, 0f))
            {
                return;
            }

            _Theta += _RotateSpeed * Mathf.Deg2Rad * pDeltaTime;
        }
    }
}