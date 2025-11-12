#region _____________________________/ INFOS
//  AUTHOR : Nathan THEOPHILE (2025)
//  Engine : Unity
//  Note : MY_CONST, myPublic, m_MyProtected, _MyPrivate, lMyLocal, MyFunc(), pMyParam, onMyEvent, OnMyCallback, MyStruct
#endregion

using Rush.Game;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Rush.UI
{
    public class Item_LevelItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private TMP_Text _PrefabNameText;

        [SerializeField] private RawImage _PreviewImage;

        private Camera _PreviewCamera;

        private PreviewCamera _PreviewCameraController;
        private RenderTexture _PreviewTexture;

        public void Initialize(string prefabName, Camera previewCamera, Vector2Int previewResolution)
        {
            _PrefabNameText.text = prefabName;

            CleanupTexture();

            _PreviewCamera = previewCamera;
            _PreviewCameraController = _PreviewCamera.GetComponent<PreviewCamera>();

            _PreviewTexture = new RenderTexture(previewResolution.x, previewResolution.y, 24)
            {
                name = $"RT_{prefabName}_Preview"
            };

            _PreviewTexture.Create();

            _PreviewCamera.targetTexture = _PreviewTexture;
            _PreviewCamera.enabled = true;

            _PreviewImage.texture = _PreviewTexture;
        }

        private void CleanupTexture()
        {
            if (_PreviewTexture == null)
            {
                return;
            }

            _PreviewCamera.targetTexture = null;

            if (_PreviewTexture.IsCreated())
            {
                _PreviewTexture.Release();
            }

            Destroy(_PreviewTexture);
            _PreviewTexture = null;

            _PreviewImage.texture = null;
            _PreviewCameraController.canRotate = false;

            _PreviewCamera = null;
            _PreviewCameraController = null;
        }

        private void OnDestroy()
        {
            CleanupTexture();
        }

        public void OnPointerEnter(PointerEventData eventData) => _PreviewCameraController.canRotate = true;

        public void OnPointerExit(PointerEventData eventData) => _PreviewCameraController.canRotate = false;
    }
}