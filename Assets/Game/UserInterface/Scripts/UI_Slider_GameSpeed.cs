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
    public class UI_Slider_GameSpeed : MonoBehaviour
    {
        private Slider _GameSpeedSlider;

        void Start() { _GameSpeedSlider = GetComponentInParent<Slider>(); _GameSpeedSlider.onValueChanged.AddListener(OnSliderValueChanged); }

        public void OnSliderValueChanged(float pValue) => Manager_Time.Instance.GlobalTickSpeed = pValue;
    }
}

