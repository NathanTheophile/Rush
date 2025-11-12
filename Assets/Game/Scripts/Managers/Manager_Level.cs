#region _____________________________/ INFOS
//  AUTHOR : Nathan THEOPHILE (2025)
//  Engine : Unity
//  Singleton
//  Note : MY_CONST, myPublic, m_MyProtected, _MyPrivate, lMyLocal, MyFunc(), pMyParam, onMyEvent, OnMyCallback, MyStruct
#endregion

using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;

namespace Rush.Game
{
    public class Manager_Level : MonoBehaviour
    {
        public static Manager_Level Instance { get; private set; }

        private int _CubesToSpawn;
        private int _CubesValidated;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        public void CubeValidated() { _CubesValidated++; } 

        private void OnDestroy()
        {
            if (Instance == this) Instance = null;
        }
    }
}