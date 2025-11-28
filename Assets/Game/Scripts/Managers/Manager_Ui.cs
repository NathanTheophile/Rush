#region _____________________________/ INFOS
//  AUTHOR : Nathan THEOPHILE (2025)
//  Engine : Unity
//  Singleton
//  Note : MY_CONST, myPublic, m_MyProtected, _MyPrivate, lMyLocal, MyFunc(), pMyParam, onMyEvent, OnMyCallback, MyStruct
#endregion

using System.Collections.Generic;
using System.Linq;
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
            DontDestroyOnLoad(gameObject);
        }

        #endregion

        #region _____________________________/ VALUES

        [SerializeField] private List<Transform> _UiCards = new();
        [SerializeField] private List<Transform> _ActiveCards = new();
        [SerializeField] private Transform _CurrentCard;

        #endregion

        #region _____________________________| INIT

        private void Awake() => CheckForInstance();

        #endregion

        #region _____________________________| CARDS

        public void AddActivePanel(Transform pCard)
        {
            if (pCard == null || _UiCards.Contains(pCard))
                return;

            _UiCards.Add(pCard);
        }

        public void Show(Transform pCard)
        {
            if (pCard == null) return;

            AddActivePanel(pCard);

            pCard.gameObject.SetActive(true);

            if (!_ActiveCards.Contains(pCard))
                _ActiveCards.Add(pCard);

            _CurrentCard = pCard;
        }

        public void Hide(Transform pCard)
        {
            if (pCard == null) return;

            pCard.gameObject.SetActive(false);

            _ActiveCards.Remove(pCard);

            if (_CurrentCard == pCard)
                _CurrentCard = _ActiveCards.LastOrDefault();
        }

        public void Switch(Transform pCardToShow, Transform pCardToHide)
        {
            Hide(pCardToHide);
            Show(pCardToShow);
        }

        #endregion
    }
}