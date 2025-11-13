#region _____________________________/ INFOS
//  AUTHOR : Nathan THEOPHILE (2025)
//  Engine : Unity
//  Note : MY_CONST, myPublic, m_MyProtected, _MyPrivate, lMyLocal, MyFunc(), pMyParam, onMyEvent, OnMyCallback, MyStruct
#endregion

using Rush.Game;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Rush.UI
{
    public class UI_Btn_Level : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        #region _____________________________/ ELEMENTS

        [SerializeField] private TMP_Text _PrefabNameText;
        [SerializeField] private RawImage _PreviewImage;
        [SerializeField] private Button _BtnLevel;
        [SerializeField] private Transform _PanelToShow;
        private RenderTexture _PreviewTexture;
        private Transform _GridSelector;

        #endregion

        #region _____________________________/ DATA

        private SO_LevelData _LevelData;

        #endregion

        #region _____________________________/ CAMERA

        private Camera _PreviewCamera;
        private PreviewCamera _PreviewCameraController;

        #endregion

        #region _____________________________/ DYNAMICS

        private float _HoveringScale = 1.1f;

        #endregion

        #region _____________________________| INIT


        private void Awake() {
            _BtnLevel = GetComponent<Button>();
            _BtnLevel.onClick.AddListener(OnButtonClicked); }

        public void Initialize(Transform pGridSelector, SO_LevelData pLevelData, Camera previewCamera, Vector2Int previewResolution, Vector3 pPosition)
        {
            _GridSelector = pGridSelector;
            _LevelData = pLevelData;
            _PrefabNameText.text = _LevelData.levelName;

            CleanupTexture();

            _PreviewCamera = previewCamera;
            _PreviewCameraController = _PreviewCamera.GetComponent<PreviewCamera>();

            _PreviewCameraController.AddTargetWorldOffset(pPosition);


            _PreviewTexture = new RenderTexture(previewResolution.x, previewResolution.y, 24)
            {
                name = $"RT_{_LevelData.name}_Preview"
            };

            _PreviewTexture.Create();

            _PreviewCamera.targetTexture = _PreviewTexture;
                        _PreviewCamera.forceIntoRenderTexture = true;

            _PreviewCamera.enabled = true;
                        _PreviewCamera.Render();

            _PreviewImage.texture = _PreviewTexture;

        }

        private void CleanupTexture()
        {
            ReleasePreviewCamera();

            if (_PreviewTexture != null)
            {
                if (_PreviewTexture.IsCreated()) _PreviewTexture.Release();

                Destroy(_PreviewTexture);
                _PreviewTexture = null;
            }

            _PreviewImage.texture = null;
            _PreviewCamera = null;
            _PreviewCameraController = null;
        }

        private void ReleasePreviewCamera()
        {
            if (_PreviewCamera == null) return;

            _PreviewCamera.targetTexture = null;

            if (_PreviewCameraController != null)
            {
                _PreviewCameraController.canRotate = false;
            }
        }

        private void OnDestroy()
        {
            CleanupTexture();
        }

        #endregion        

        #region _____________________________| MOUSE EVENTS

        public void OnPointerEnter(PointerEventData eventData) {
            _PreviewCameraController.canRotate = true;
            transform.localScale = Vector3.one * _HoveringScale; }

        public void OnPointerExit(PointerEventData eventData) {
            _PreviewCameraController.canRotate = false;
            transform.localScale = Vector3.one; }

        private void OnButtonClicked() {
            CleanupTexture();
            Instantiate(_LevelData.levelPrefab, Vector3.zero, Quaternion.identity);
            Debug.Log(transform.root.name);
            Instantiate(_PanelToShow, transform.root);
            Destroy(_GridSelector.parent.GameObject());
            Manager_Game.Instance?.SetState(Manager_Game.GameStates.Setup);

        }

        #endregion
    }
}