#region _____________________________/ INFOS
//  AUTHOR : Nathan THEOPHILE (2025)
//  Engine : Unity
//  Independant
//  Note : MY_CONST, myPublic, m_MyProtected, _MyPrivate, lMyLocal, MyFunc(), pMyParam, onMyEvent, OnMyCallback, MyStruct
#endregion

using System.Collections.Generic;
using UnityEditor;
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

    // On utilise des directions logiques pr ne pas avoir à rotate le transform, on stocke les 4 directions dans une liste,
    // on bouclera sur les 4 directions à partir de la direction actuelle en modulant par par 4 poru rester entre 0 et 3
    // et pouvoir check facilement les 3 directions peut importe la direction actuel du cube
    static readonly Vector3Int[] DIRECTIONS = { Vector3Int.forward, Vector3Int.right, Vector3Int.back, Vector3Int.left };
    static int RightOf(int i) => (i + 1) % DIRECTIONS.Length;
    static int BackOf(int i) => (i + 2) % DIRECTIONS.Length;
    static int LeftOf(int i) => (i + 3) % DIRECTIONS.Length;
    private int DirectionIndexOf(Vector3Int pDirection) => System.Array.IndexOf(DIRECTIONS, pDirection);



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
    void SetModePause() { doAction = Pause; }
        void Pause() { }
    }
}

