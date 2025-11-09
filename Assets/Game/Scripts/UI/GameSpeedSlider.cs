#region _____________________________/ INFOS
//  AUTHOR : Nathan THEOPHILE (2025)
//  Engine : Unity
//  Note : MY_CONST, myPublic, m_MyProtected, _MyPrivate, lMyLocal, MyFunc(), pMyParam, onMyEvent, OnMyCallback, MyStruct
#endregion

using Rush.Game;
using UnityEngine;
using UnityEngine.UI;

namespace Rush.UI
{
    public class GameSpeedSlider : MonoBehaviour
    {
        private Slider _GameSpeedSlider;

        void Start() { _GameSpeedSlider = GetComponentInParent<Slider>(); Debug.Log(_GameSpeedSlider); _GameSpeedSlider.onValueChanged.AddListener(OnSliderValueChanged); }

        public void OnSliderValueChanged(float pValue) { Debug.Log(_GameSpeedSlider.value); TimeManager.instance.GlobalTickSpeed = pValue; }
    }
}

