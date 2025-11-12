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
        [SerializeField]
        private TMP_Text _PrefabNameText;

        [SerializeField]
        private RawImage _PreviewImage;

        private Camera _PreviewCamera;

        private PreviewCamera _PreviewCameraController;
        private RenderTexture _PreviewTexture;

        public void Initialize(string prefabName, Camera previewCamera, Vector2Int previewResolution)
        {
            if (_PrefabNameText != null)
            {
                _PrefabNameText.text = prefabName;
            }

            if (_PreviewTexture != null)
            {
                CleanupTexture();
            }

            _PreviewCamera = previewCamera;
            _PreviewCameraController = _PreviewCamera != null ? _PreviewCamera.GetComponent<PreviewCamera>() : null;
            if (_PreviewCamera == null)
            {
                if (_PreviewImage != null)
                {
                    _PreviewImage.texture = null;
                }
                return;
            }

            if (previewResolution.x <= 0 || previewResolution.y <= 0)
            {
                previewResolution = new Vector2Int(256, 256);
            }

            _PreviewTexture = new RenderTexture(previewResolution.x, previewResolution.y, 24)
            {
                name = $"RT_{prefabName}_Preview"
            };

            _PreviewTexture.Create();

            _PreviewCamera.targetTexture = _PreviewTexture;
            _PreviewCamera.enabled = true;

            if (_PreviewImage != null)
            {
                _PreviewImage.texture = _PreviewTexture;
            }
        }

        private void CleanupTexture()
        {
            if (_PreviewCamera != null)
            {
                _PreviewCamera.targetTexture = null;
            }

            if (_PreviewTexture != null)
            {
                if (_PreviewTexture.IsCreated())
                {
                    _PreviewTexture.Release();
                }

                Destroy(_PreviewTexture);
                _PreviewTexture = null;
            }

            if (_PreviewImage != null)
            {
                _PreviewImage.texture = null;
            }

            if (_PreviewCameraController != null)
            {
                _PreviewCameraController.canRotate = false;
            }

            _PreviewCamera = null;
            _PreviewCameraController = null;
        }

        private void OnDestroy()
        {
            CleanupTexture();
        }

                public void OnPointerEnter(PointerEventData eventData)
        {
            if (_PreviewCameraController != null)
            {
                _PreviewCameraController.canRotate = true;
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (_PreviewCameraController != null)
            {
                _PreviewCameraController.canRotate = false;
            }
        }
    }
}