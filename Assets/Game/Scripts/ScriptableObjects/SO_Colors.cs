#region _____________________________/ INFOS
//  AUTHOR : Nathan THEOPHILE (2025)
//  Engine : Unity
//  Note : MY_CONST, myPublic, m_MyProtected, _MyPrivate, lMyLocal, MyFunc(), pMyParam, onMyEvent, OnMyCallback, MyStruct
#endregion

using UnityEngine;

namespace Rush.Game
{
    [CreateAssetMenu(fileName = "SO_Colors", menuName = "Scriptable Objects/Colors")]
    public class SO_Colors : ScriptableObject
    {
        [SerializeField] private Color _Color;

        public Color Color { get { return _Color; } set { _Color = value; } }
    }
}
