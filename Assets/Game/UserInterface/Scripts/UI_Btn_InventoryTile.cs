#region _____________________________/ INFOS
//  AUTHOR : Nathan THEOPHILE (2025)
//  Engine : Unity
//  Note : MY_CONST, myPublic, m_MyProtected, _MyPrivate, lMyLocal, MyFunc(), pMyParam, onMyEvent, OnMyCallback, MyStruct
#endregion

using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Rush.Game;

[RequireComponent(typeof(Button))]
public class UI_Btn_InventoryTile : MonoBehaviour
{
    #region _____________________________/ REFS

    [Header("References")]
    [SerializeField] private TMP_Text _TileName;
    [SerializeField] private TMP_Text _TileAmount;
    [SerializeField] private Image _TileImage;

    [SerializeField] private Transform _TileOrientationVisual;

    #endregion

    #region _____________________________/ VALUES

    public int TileAmount { get; private set; }

    private Button _Button;
    private SO_LevelData.InventoryTile _InventoryTile;

    private static UI_Btn_InventoryTile _CurrentSelectedTile;
    private bool _IsSelected;

    private TilePlacer TilePlacerInstance => TilePlacer.Instance;

    #endregion

    #region _____________________________| PUBLIC

    public void Initialize(SO_LevelData.InventoryTile pInventoryTile)
    {
        if (_Button == null) _Button = GetComponent<Button>();
        if (_TileImage == null) _TileImage = GetComponent<Image>();

        _InventoryTile = pInventoryTile;

        TileAmount = pInventoryTile.quantity;

        _TileName.text = pInventoryTile.type.ToString();
        _TileAmount.text = TileAmount.ToString();
        ApplyTileSprite(pInventoryTile.image);

        ApplyTileOrientation(pInventoryTile.orientation);

        _Button.onClick.RemoveAllListeners();
        _Button.onClick.AddListener(() =>
        {
            if (TilePlacerInstance == null)
            {
                Debug.LogWarning("TilePlacer reference missing. Cannot set tile prefabs from inventory button.");
                return;
            }

            if (_InventoryTile.tilePrefab == null)
            {
                Debug.LogWarning($"No tile prefab assigned for {_InventoryTile.type} in level data.");
                return;
            }

            if (_CurrentSelectedTile != null && _CurrentSelectedTile != this) _CurrentSelectedTile._IsSelected = false;

            _CurrentSelectedTile = this;
            _IsSelected = true;

            TilePlacerInstance.OnTilePlaced -= HandleTilePlaced;
            TilePlacerInstance.OnTilePlaced += HandleTilePlaced;
            TilePlacerInstance.StartHandlingTile();

            TilePlacerInstance.SetTilePrefabs(_InventoryTile.tilePrefab, _InventoryTile.previewPrefab, _InventoryTile.orientation);
        });
    }

    public bool ConsumeTile()
    {
        TileAmount--;

        if (TileAmount <= 0)
        {
            Destroy(gameObject);
            return false;
        }

        _TileAmount.text = TileAmount.ToString();
        return true;
    }

    #endregion

    #region _____________________________| UNITY

    private void OnDestroy()
    {
        if (TilePlacerInstance != null) TilePlacerInstance.OnTilePlaced -= HandleTilePlaced;
    }

    #endregion

    #region _____________________________| PRIVATE

    private void HandleTilePlaced()
    {
        if (!_IsSelected) return;

        if (!ConsumeTile())
        {
            _IsSelected = false;

            if (_CurrentSelectedTile == this) _CurrentSelectedTile = null;

            if (TilePlacerInstance != null) TilePlacerInstance.ClearSelection();
        }
    }

    private void ApplyTileOrientation(Rush.Game.Tile.TileOrientations pOrientation)
    {
        Transform lTargetTransform = _TileOrientationVisual != null && _TileOrientationVisual != transform
            ? _TileOrientationVisual
            : _TileImage != null
                ? _TileImage.rectTransform
                : null;

        if (lTargetTransform == null) return;

        lTargetTransform.localRotation = pOrientation switch
        {
            Tile.TileOrientations.Right => Quaternion.Euler(0f, 0f, -90f),
            Tile.TileOrientations.Left => Quaternion.Euler(0f, 0f, 90f),
            Tile.TileOrientations.Down => Quaternion.Euler(0f, 0f, 180f),
            _ => Quaternion.identity
        };
    }

    private void ApplyTileSprite(Sprite pSprite)
    {
        if (_TileImage == null || pSprite == null) return;

        _TileImage.sprite = pSprite;
    }

    #endregion

    #region _____________________________| STATIC

    public static void ResetSelection()
    {
        if (_CurrentSelectedTile != null)
        {
            _CurrentSelectedTile._IsSelected = false;
            _CurrentSelectedTile = null;
        }

        TilePlacer.Instance?.ClearSelection();
    }

    #endregion
}