using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SOColors", menuName = "Scriptable Objects/SOColors")]
public class SOColors : ScriptableObject
{
    [SerializeField] private Color _Color;

    public Color Color
    {
        get { return _Color;}
        set { _Color = value; }
    }
}
