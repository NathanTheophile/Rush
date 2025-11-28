#region _____________________________/ INFOS
//  AUTHOR : Nathan THEOPHILE (2025)
//  Engine : Unity
//  Singleton
//  Note : MY_CONST, myPublic, m_MyProtected, _MyPrivate, lMyLocal, MyFunc(), pMyParam, onMyEvent, OnMyCallback, MyStruct
#endregion

using System.Collections.Generic;
using UnityEngine;

namespace Rush.Game.Core
{
    public class Manager_Ui : MonoBehaviour
    {
        #region _____________________________/ SINGLETON

        public static Manager_Ui Instance { get; private set; }

        private void CheckForInstance()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;  
        }

        #endregion

        #region _____________________________/ VALUES
        [SerializeField] private Transform _MainCanvas;
        [SerializeField] private Transform _WinScreen, _LoseScreen;
        [SerializeField] private List<Transform> _UiCards = new();
        
        private Transform _CurrentCard;

        [SerializeField] Manager_Game lManager;

        #endregion

        #region _____________________________| INIT

        private void Awake() => CheckForInstance();

        /// <summary>
        /// Start is called on the frame when a script is enabled just before
        /// any of the Update methods is called the first time.
        /// </summary>
        void Start()
        {
            Debug.Log(Manager_Game.Instance == null);
            Manager_Game.Instance.onGameOver += SwitchToLose;
            Manager_Game.Instance.onGameWon += SwitchToWin;
                        Debug.Log(Manager_Game.Instance == null);
        }

        #endregion

        #region _____________________________| CARDS

        public void AddCardToScene(Transform pCard)
        {
            if (pCard == null || _UiCards.Contains(pCard))
                return;

            Transform lCard = Instantiate(pCard, _MainCanvas);
            _UiCards.Add(lCard);
        }

        public void Show(Transform pCard)
        {
            if (pCard == null) return;

            AddCardToScene(pCard);
            _CurrentCard = pCard;
            pCard.gameObject.SetActive(true);
        }

        public void Hide(Transform pCard)
        {
            if (pCard == null) return;
            
            pCard.gameObject.SetActive(false);
            _CurrentCard = null;
        }

        public void Switch(Transform pCardToShow, Transform pCardToHide)
        {
            Hide(pCardToHide);
            Show(pCardToShow);
        }

        private void SwitchToWin()  { Debug.Log("OUOUOUOUOU " + _CurrentCard.name); Switch(_WinScreen, _CurrentCard);}
        private void SwitchToLose() { Debug.Log("OUOUOUOUOU " + _CurrentCard.name);  Switch(_LoseScreen, _CurrentCard);}


        #endregion
    }
}