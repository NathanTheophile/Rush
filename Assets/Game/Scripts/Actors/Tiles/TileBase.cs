#region _____________________________/ INFOS
//  AUTHOR : Nathan THEOPHILE (2025)
//  Engine : Unity
//  Note : MY_CONST, myPublic, m_MyProtected, _MyPrivate, lMyLocal, MyFunc(), pMyParam, onMyEvent, OnMyCallback, MyStruct
#endregion

using UnityEngine;

public class BaseTile : MonoBehaviour
{
    [SerializeField] public TileVariants tileVariant = TileVariants.Default;
    
    public enum TileVariants
    {
        Default,
        Arrow,
        Convoyer,
        Dispatcher,
        Teleporter,
        Stopper,
        Target
    }
    
    public Vector3 direction;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() { direction = transform.forward; }
}
