using Rush.Game;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class UI_Btn_InventoryTile : MonoBehaviour
{
    [SerializeField] public TMP_Text _TileName;
    [SerializeField] public TMP_Text _TileAmount;
    [SerializeField] private Transform _TileOrientationVisual;

    public int TileAmount { get; private set; }

    private Button _Button;
    private TilePlacer _TilePlacer;
    private SO_LevelData.InventoryTile _InventoryTile;

    private static UI_Btn_InventoryTile _CurrentSelectedTile;
    private bool _IsSelected;

    public void Initialize(SO_LevelData.InventoryTile pInventoryTile, TilePlacer pTilePlacer)
    {
        if (_Button == null)
            _Button = GetComponent<Button>();

        _TilePlacer = pTilePlacer;
        _InventoryTile = pInventoryTile;

        TileAmount = pInventoryTile.quantity;

        _TileName.text = pInventoryTile.type.ToString();
        _TileAmount.text = TileAmount.ToString();
        ApplyTileOrientation(pInventoryTile.orientation);

        _Button.onClick.RemoveAllListeners();
        _Button.onClick.AddListener(() =>
        {
            if (_TilePlacer == null)
            {
                Debug.LogWarning("TilePlacer reference missing. Cannot set tile prefabs from inventory button.");
                return;
            }

            if (_InventoryTile.tilePrefab == null)
            {
                Debug.LogWarning($"No tile prefab assigned for {_InventoryTile.type} in level data.");
                return;
            }

            if (_CurrentSelectedTile != null && _CurrentSelectedTile != this)
                _CurrentSelectedTile._IsSelected = false;

            _CurrentSelectedTile = this;
            _IsSelected = true;

            _TilePlacer.OnTilePlaced -= HandleTilePlaced;
            _TilePlacer.OnTilePlaced += HandleTilePlaced;
            _TilePlacer.StartHandlingTile();

            _TilePlacer.SetTilePrefabs(_InventoryTile.tilePrefab, _InventoryTile.previewPrefab, _InventoryTile.orientation);
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

    void OnDestroy()
    {
        if (_TilePlacer != null)
            _TilePlacer.OnTilePlaced -= HandleTilePlaced;
    }

    private void HandleTilePlaced()
    {
        if (!_IsSelected)
            return;

        if (!ConsumeTile())
        {
            _IsSelected = false;

            if (_CurrentSelectedTile == this)
                _CurrentSelectedTile = null;

            if (_TilePlacer != null)
                _TilePlacer.ClearSelection();
        }
    }

    private void ApplyTileOrientation(Rush.Game.Tile.TileOrientations pOrientation)
    {
        if (_TileOrientationVisual == null)
            return;

        _TileOrientationVisual.localRotation = pOrientation switch
        {
            Rush.Game.Tile.TileOrientations.East => Quaternion.Euler(0f, 0f, -90f),
            Rush.Game.Tile.TileOrientations.West => Quaternion.Euler(0f, 0f, 90f),
            Rush.Game.Tile.TileOrientations.South => Quaternion.Euler(0f, 0f, 180f),
            _ => Quaternion.identity
        };
    }

        public static void ResetSelection(TilePlacer pTilePlacer)
    {
        if (_CurrentSelectedTile != null)
        {
            _CurrentSelectedTile._IsSelected = false;
            _CurrentSelectedTile = null;
        }

        pTilePlacer?.ClearSelection();
    }
}