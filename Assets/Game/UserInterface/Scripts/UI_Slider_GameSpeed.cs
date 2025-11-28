using Rush.Game.Core;
using UnityEngine;
using UnityEngine.UI;

namespace Rush.UI
{
    public class UI_Slider_GameSpeed : MonoBehaviour
    {
        #region _____________________________/ VALUES

        [SerializeField] private Slider _GameSpeedSlider;

        #endregion

        #region _____________________________| UNITY

        private void Awake() => _GameSpeedSlider ??= GetComponentInParent<Slider>();

        private void Start()
        {
            if (_GameSpeedSlider == null)
                return;

            _GameSpeedSlider.onValueChanged.AddListener(OnSliderValueChanged);
            Manager_Time.Instance.GlobalTickSpeed = _GameSpeedSlider.value;
        }

        #endregion

        #region _____________________________| CALLBACKS

        private void OnSliderValueChanged(float pValue) => Manager_Time.Instance.GlobalTickSpeed = pValue;

        #endregion
    }
}