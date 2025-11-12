#region _____________________________/ INFOS
//  AUTHOR : Nathan THEOPHILE (2025)
//  Engine : Unity
//  Note : MY_CONST, myPublic, m_MyProtected, _MyPrivate, lMyLocal, MyFunc(), pMyParam, onMyEvent, OnMyCallback, MyStruct
#endregion

using Rush.Game;
using UnityEngine;

namespace Rush.UI
{
    public class Grid_LevelSelector : MonoBehaviour
    {
        [SerializeField]
        private Transform _GridRoot;

        [SerializeField]
        private LevelCollection _LevelCollection;

        private void Awake()
        {

        }
    }
}