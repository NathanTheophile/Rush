#region _____________________________/ INFOS
//  AUTHOR : Nathan THEOPHILE (2025)
//  Engine : Unity
//  Note : MY_CONST, myPublic, m_MyProtected, _MyPrivate, lMyLocal, MyFunc(), pMyParam, onMyEvent, OnMyCallback, MyStruct
#endregion

using System.Collections.Generic;
using UnityEditor;
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

        #region _________________________/ RAYCAST VARS
        [Header("Raycasts")]
        [SerializeField] private LayerMask _GroundLayer;
        [SerializeField] private LayerMask _TilesLayer;

        #endregion


    // On utilise des directions logiques pr ne pas avoir à rotate le transform, on stocke les 4 directions dans une liste,
    // on bouclera sur les 4 directions à partir de la direction actuelle en modulant par par 4 poru rester entre 0 et 3
    // et pouvoir check facilement les 3 directions peut importe la direction actuel du cube
    static readonly Vector3Int[] DIRECTIONS = { Vector3Int.forward, Vector3Int.right, Vector3Int.back, Vector3Int.left };
    static int RightOf(int i) => (i + 1) % DIRECTIONS.Length;
    static int BackOf(int i) => (i + 2) % DIRECTIONS.Length;
    static int LeftOf(int i) => (i + 3) % DIRECTIONS.Length;
    private int DirectionIndexOf(Vector3Int pDirection) => System.Array.IndexOf(DIRECTIONS, pDirection);



    


        void Awake()
        {
            selfTransform = transform;
            direction = selfTransform.forward;
            doAction = Wait;
        }


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


    private bool CheckForWall(Vector3Int pDirection) => Physics.Raycast(selfTransform.position, pDirection, _GridSize, _GroundLayer); //oeoe le raycast


        /// <summary>
        /// on se base sur la liste pour check les 4 directions depuis la direction actuelle et on sort si un checkwall renvoie false
        /// </summary>
        /// <param name="pCheckingOrder"></param>
        private void FindNewDirection(IEnumerable<Vector3Int> pCheckingOrder)
        {
            foreach (var lDirection in pCheckingOrder)
                if (!CheckForWall(lDirection)) //là il a trouvé une direction ou il prend rien dans la goule
                {
                    direction = lDirection;
                    return;
                }
                else SetModePause(); //là il a mangé un truc dans la goule mdrrr
        }


        public void TickUpdate(int pTickIndex)
        {
            Snap();
            SetModeRoll();
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
    void SetModePause() { doAction = Pause; }
        void Pause() { }
    }
}