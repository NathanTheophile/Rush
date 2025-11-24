using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class TilePlacer : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] private Transform _TileToSpawn;
    [SerializeField] private Transform _TilePreviewPrefab;

    [Header("Physics")]
    [SerializeField] private float _RaycastDistance = 20f;
    [SerializeField] private LayerMask _GroundLayer, _UiLayer, _TilesLayer;
    private bool _HasGroundHit;
    public event Action OnTilePlaced;

    private Vector3 InstantiatePos;
    public static Transform previewTile;

    void Start()
    {
        InstantiatePos = new Vector3(1000, 1000, 1000);
        if (_TilePreviewPrefab != null)
            InstantiatePreviewTile(_TilePreviewPrefab, InstantiatePos);    }

    public void SetTilePrefabs(Transform pTileToSpawn, Transform pPreviewPrefab)
    {
        _TileToSpawn = pTileToSpawn;
        _TilePreviewPrefab = pPreviewPrefab != null ? pPreviewPrefab : pTileToSpawn;

        if (_TilePreviewPrefab == null) return;

        if (previewTile == null)
            InstantiatePreviewTile(_TilePreviewPrefab, InstantiatePos);
        else
            SwitchPreviewTile(_TilePreviewPrefab);
    }


    private static void InstantiatePreviewTile(Transform pPrefab, Vector3 pPosition) => previewTile = Instantiate(pPrefab, pPosition, Quaternion.identity);

    public static void SwitchPreviewTile (Transform pPrefab)
    {
        Vector3 lCurrentPos = previewTile.position;
        Destroy(previewTile);
        InstantiatePreviewTile(pPrefab, lCurrentPos);
    }

    // Update is called once per frame
    void Update()
    {
        if (EventSystem.current.IsPointerOverGameObject()) return;
        Ray lRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            Debug.DrawRay(lRay.origin, lRay.direction * 20, Color.white);
        RaycastHit lHitObject;
        _HasGroundHit = false;
        if (previewTile != null)
        {
            if (Physics.Raycast(lRay, out lHitObject, _RaycastDistance, _GroundLayer))
            {
                previewTile.position = Vector3Int.RoundToInt(lHitObject.point);
                _HasGroundHit = true;
            }
            else
                previewTile.position = InstantiatePos;
        }

        if (_TileToSpawn != null && previewTile != null && _HasGroundHit && Input.GetMouseButtonUp(0))
        {
            Instantiate(_TileToSpawn, previewTile.position, Quaternion.identity);

            OnTilePlaced?.Invoke();
        }
    }

    public void ClearSelection()
    {
        _TileToSpawn = null;
        _TilePreviewPrefab = null;

        if (previewTile != null)
            previewTile.position = InstantiatePos;
    }
}