#region _____________________________/ INFOS
//  AUTHOR : Nathan THEOPHILE (2025)
//  Engine : Unity
//  Singleton
//  Note : MY_CONST, myPublic, m_MyProtected, _MyPrivate, lMyLocal, MyFunc(), pMyParam, onMyEvent, OnMyCallback, MyStruct
#endregion

using Unity.VisualScripting;
using UnityEngine;

namespace Rush.Game
{
    public class Manager_Ui : MonoBehaviour
    {
        public static Manager_Ui Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
        }

        public void ClosePanel(Transform pPanel) => Destroy(pPanel);

        public void ShowPanel(Transform pPanel) => Instantiate(pPanel, Vector3.zero, Quaternion.identity);

        public void SwitchPanel(Transform pPanelToShow, Transform lPanelToHide) {
            Instantiate(pPanelToShow, transform);
            Destroy(lPanelToHide.GameObject()); }

        private void OnDestroy()
        {
            if (Instance == this) Instance = null;
        }
    }
}