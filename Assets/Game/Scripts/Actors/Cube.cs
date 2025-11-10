#region _____________________________/ INFOS
//  AUTHOR : Nathan THEOPHILE (2025)
//  Engine : Unity
//  Note : MY_CONST, myPublic, m_MyProtected, _MyPrivate, lMyLocal, MyFunc(), pMyParam, onMyEvent, OnMyCallback, MyStruct
#endregion

using System;
using System.Collections.Generic;
using UnityEngine;

namespace Rush.Game
{
    public class Cube : MonoBehaviour, ITickDependant
    {
        #region _________________________/ MAIN VALUES
        [Header("Main")]
        private Transform _Self;
        private float _GridSize = 1f;
        private Action doAction;

        #endregion

        #region _________________________/ TIME VALUES
        [Header("Time")]
        public float currentTickStep { get; set; }

        #endregion

        #region _________________________/ MOVEMENT VALUES
        [Header("Movement")]
        private float   _BaseAngle = 90f;
        private Vector3 _PivotPoint;
        private Vector3 _Direction;
        public Vector3  direction { set { _Direction = value; } }
        private Quaternion  _StartRotation, _EndRotation;
        private Vector3     _StartPosition, _EndPosition;

        #endregion

        #region _________________________/ COLLISION VALUES
        [Header("Collisions")]
        [SerializeField] private LayerMask _GroundLayer;
        [SerializeField] private LayerMask _TilesLayer;

        // On utilise des directions logiques pr ne pas avoir à rotate le transform, on stocke les 4 directions dans une liste,
        // on bouclera sur les 4 directions à partir de la direction actuelle en modulant par par 4 poru rester entre 0 et 3
        // et pouvoir check facilement les 3 directions peut importe la direction actuel du cube
        static readonly Vector3Int[] DIRECTIONS = { Vector3Int.forward, Vector3Int.right, Vector3Int.back, Vector3Int.left };
        static int RightOf(int i) => (i + 1) % DIRECTIONS.Length;
        static int BackOf(int i) => (i + 2) % DIRECTIONS.Length;
        static int LeftOf(int i) => (i + 3) % DIRECTIONS.Length;
        private int DirectionIndexOf(Vector3Int pDirection) => System.Array.IndexOf(DIRECTIONS, pDirection);

        #endregion

        #region _________________________/ INIT
        private void Awake()
        {
            _Self = transform;
            doAction = Pause;
        }

        #endregion

        #region _________________________/ GAME FLOW METHODS

        private void Update() => doAction();

        public void TickUpdate(int pTickIndex)
        {
            doAction(); // Petite execution pour appliquer la dernière step du tick ajustée à 1 dans le TImeManger
            SetNextMode();
        }

        private void SetNextMode()
        {
            Debug.Log($"Current tick step = {currentTickStep}. Position = {_Self.position}");

            if (!TryFindGround()) { SetModeFall(); return; }

            var lDirsCheckingOrder = SetSidesCheckingOrder();    
            FindNewDirection(lDirsCheckingOrder); // Je set une nouvelle direction et dedans je gère la pause

            SetModeRoll();
        }

        #endregion

        #region _________________________/ PHYSIC METHODS

        /// <returns>retourne si le raycast a détecté qq chose et si tile retourne tile sinon null</returns>
        private bool TryFindGround()
        {
            if (Physics.Raycast(_Self.position, Vector3.down, out var hit, _GridSize, _GroundLayer | _TilesLayer)) return true;
            else return false;
        }

        /// <summary>
        /// on récupère l'index des directions correpsondant à la driection actuelle du cube, puis on définit depuis
        /// cette index les directiction du transform correspondant aux trois directions à checker dans l'ordre d'apres la direction actuelle
        /// </summary>
        /// <param name="currentOrientation">l'orientation LOGIQUE actuelle du cube</param>
        /// <returns></returns>
        private Vector3Int[] SetSidesCheckingOrder()
        {
            int i = DirectionIndexOf(Vector3Int.RoundToInt(_Direction));
            return new[] { DIRECTIONS[i], DIRECTIONS[RightOf(i)], DIRECTIONS[LeftOf(i)], DIRECTIONS[BackOf(i)] };
        }

        private bool CheckForWall(Vector3Int pDirection) => Physics.Raycast(_Self.position, pDirection, _GridSize, _GroundLayer); //oeoe le raycast

        /// <summary>
        /// on se base sur la liste pour check les 4 directions depuis la direction actuelle et on sort si un checkwall renvoie false
        /// </summary>
        /// <param name="pCheckingOrder"></param>
        private void FindNewDirection(IEnumerable<Vector3Int> pCheckingOrder)
        {
            foreach (var lDirection in pCheckingOrder)
                if (!CheckForWall(lDirection)) //là il a trouvé une direction ou il prend rien dans la goule
                {
                    _Direction = lDirection;
                    return;
                }
                else SetModePause(); //là il a mangé un truc dans la goule mdrrr
        }

        #endregion

        #region _________________________/ STATE MACHINE SETTERS

        private void SetModePause() { doAction = Pause; }

        public void SetModeRoll()
        {
            Vector3 lAxis = Vector3.Cross(Vector3.up, _Direction);
            _PivotPoint = _Self.position + (Vector3.down + _Direction) * (_GridSize / 2f);

            _StartRotation = _Self.rotation;
            _EndRotation = Quaternion.AngleAxis(_BaseAngle, lAxis) * _StartRotation;

            GetLerpMovement(_Self.position - _PivotPoint, _Direction);

            doAction = Roll;
        }

        private void SetModeFall()
        {
            GetLerpMovement(_Self.position, Vector3.down);
            doAction = Fall;
        }

        #endregion

        #region _________________________/ STATES

        void Roll()
        {
            _Self.rotation = Quaternion.Slerp(_StartRotation, _EndRotation, currentTickStep);
            _Self.position = _PivotPoint + Vector3.Slerp(_StartPosition, _EndPosition, currentTickStep);
        }


        private void Fall() => _Self.position = Vector3.Lerp(_StartPosition, _EndPosition, currentTickStep);
        
        private void Pause() { }

        #endregion

        #region _________________________/ MISC METHODS

        private void GetLerpMovement(Vector3 pOrigin, Vector3 pDirection)
        {
            _StartPosition = pOrigin;
            _EndPosition = _StartPosition + pDirection * _GridSize;
        }

        #endregion
    }
}