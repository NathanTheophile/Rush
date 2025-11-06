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
    public class SphericalOrbitCamera : MonoBehaviour
    {
        #region ___________________________/ TARGET
        [Header("Target")]
        [SerializeField] private Vector3 _TargetPosition = Vector3.zero;
        #endregion

        #region ___________________________/ SPHERICAL COORDINATES
        [Header("Spherical Coordinates")]
        [SerializeField] private float _Radius = 10f;
        [SerializeField] private float _MinRadius = 5f;
        [SerializeField] private float _MaxRadius = 30f;
        [SerializeField, Range(0f, 180f)] private float _Colatitude = 60f;
        #endregion

        #region ___________________________/ INPUT SETTINGS
        [Header("Input")]
        [SerializeField] private float _RotationSensitivity = 0.2f;
        [SerializeField] private float _ScrollZoomSensitivity = 1.5f;
        [SerializeField] private float _PinchZoomSensitivity = 0.02f;
        [SerializeField] private float _InertiaDamping = 4f;
        #endregion

        #region ___________________________/ STATE
        private float      _Theta;
        private float      _RotationVelocity;
        private bool       _IsDragging;
        private bool       _IsPinching;
        private Vector2    _LastPointerPosition;
        private float      _PreviousPinchDistance;
        #endregion

        void Start()
        {
            Vector3 lOffset = transform.position - _TargetPosition;
            if (lOffset.sqrMagnitude > Mathf.Epsilon)
            {
                _Radius = Mathf.Clamp(lOffset.magnitude, _MinRadius, _MaxRadius);
                Vector3 lNormalized = lOffset.normalized;
                _Theta = Mathf.Atan2(lNormalized.z, lNormalized.x);
            }
            else
            {
                _Theta = 0f;
            }

            UpdateCameraPosition();
        }

        void Update()
        {
            HandleInput();
            ApplyInertia(Time.deltaTime);
            UpdateCameraPosition();
        }

        void HandleInput()
        {
            if (Input.touchCount > 0)
            {
                HandleTouchInput();
            }
            else
            {
                _IsPinching = false;
                HandleMouseInput();
            }

            float lScrollDelta = Input.mouseScrollDelta.y;
            if (!Mathf.Approximately(lScrollDelta, 0f))
            {
                AdjustRadius(-lScrollDelta * _ScrollZoomSensitivity);
            }
        }

        void HandleMouseInput()
        {
            if (Input.GetMouseButtonDown(0))
            {
                _IsDragging = true;
                _LastPointerPosition = Input.mousePosition;
                _RotationVelocity = 0f;
            }
            else if (Input.GetMouseButtonUp(0))
            {
                _IsDragging = false;
            }

            if (_IsDragging && Input.GetMouseButton(0))
            {
                Vector2 lCurrentPosition = Input.mousePosition;
                Vector2 lDelta = lCurrentPosition - _LastPointerPosition;
                _LastPointerPosition = lCurrentPosition;

                float lDeltaTheta = lDelta.x * _RotationSensitivity * Mathf.Deg2Rad;
                _Theta += lDeltaTheta;

                if (Time.deltaTime > Mathf.Epsilon)
                    _RotationVelocity = lDeltaTheta / Time.deltaTime;
            }
        }

        void HandleTouchInput()
        {
            if (Input.touchCount == 1)
            {
                Touch lTouch = Input.GetTouch(0);

                if (lTouch.phase == TouchPhase.Began)
                {
                    _IsDragging = true;
                    _LastPointerPosition = lTouch.position;
                    _RotationVelocity = 0f;
                }
                else if (lTouch.phase == TouchPhase.Moved && _IsDragging)
                {
                    Vector2 lDelta = lTouch.deltaPosition;
                    float lDeltaTheta = lDelta.x * _RotationSensitivity * Mathf.Deg2Rad;
                    _Theta += lDeltaTheta;

                    if (Time.deltaTime > Mathf.Epsilon)
                        _RotationVelocity = lDeltaTheta / Time.deltaTime;
                }
                else if (lTouch.phase == TouchPhase.Ended || lTouch.phase == TouchPhase.Canceled)
                {
                    _IsDragging = false;
                }

                _IsPinching = false;
            }
            else if (Input.touchCount >= 2)
            {
                Touch lTouch0 = Input.GetTouch(0);
                Touch lTouch1 = Input.GetTouch(1);

                float lCurrentDistance = Vector2.Distance(lTouch0.position, lTouch1.position);

                if (!_IsPinching)
                {
                    _IsPinching = true;
                    _PreviousPinchDistance = lCurrentDistance;
                }
                else
                {
                    float lDeltaDistance = lCurrentDistance - _PreviousPinchDistance;
                    AdjustRadius(-lDeltaDistance * _PinchZoomSensitivity);
                    _PreviousPinchDistance = lCurrentDistance;
                }

                _IsDragging = false;
            }
        }

        void ApplyInertia(float pDeltaTime)
        {
            if (_IsDragging || Mathf.Approximately(pDeltaTime, 0f))
                return;

            if (Mathf.Abs(_RotationVelocity) > 0.0001f)
            {
                _Theta += _RotationVelocity * pDeltaTime;
                float lDampingFactor = Mathf.Exp(-_InertiaDamping * pDeltaTime);
                _RotationVelocity *= lDampingFactor;

                if (Mathf.Abs(_RotationVelocity) < 0.0001f)
                    _RotationVelocity = 0f;
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
    }
}