using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TilePlacer : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] private Transform _TileToSpawn;
    [SerializeField] private Transform _TilePreviewPrefab;
    [SerializeField, Range(0f, 1f)] private float _SurfaceNormalThreshold = 0.3f;
    [Header("Physics")]
    [SerializeField] private float _RaycastDistance = 20f;
    [SerializeField] private LayerMask _GroundLayer, _UiLayer, _TilesLayer;
    private bool _HasGroundHit;
    public event Action OnTilePlaced;
    public bool HandlingTile { get; private set; }
    private readonly List<Transform> _PlacedTiles = new();

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
        Destroy(previewTile.gameObject);
        InstantiatePreviewTile(pPrefab, lCurrentPos);
    }

    // Update is called once per frame
    void Update()
    {
        if (!HandlingTile) return;

        if (Input.GetMouseButtonDown(1))
        {
            if (previewTile != null)
            {
                Destroy(previewTile.gameObject);
                previewTile = null;
            }

            HandlingTile = false;
            return;
        }
        if (EventSystem.current.IsPointerOverGameObject()) return;
        Ray lRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            Debug.DrawRay(lRay.origin, lRay.direction * 20, Color.white);
        RaycastHit lHitObject;
        _HasGroundHit = false;
        if (previewTile != null)
        {
            if (TryGetPlacementHit(lRay, out lHitObject))
            {
                if (lHitObject.transform.gameObject.layer == 7) return;

                float lUpDot = Vector3.Dot(lHitObject.normal, Vector3.up);

                if (lUpDot > 0.9f)
                {
                    Vector3 lOffsetPoint = lHitObject.point + lHitObject.normal * 0.5f;

                    previewTile.position = Vector3Int.RoundToInt(lOffsetPoint);
                    _HasGroundHit = true;
                }
                else
                {
                    previewTile.position = InstantiatePos;
                }
            }
            else
                previewTile.position = InstantiatePos;
        }

        if (_TileToSpawn != null && previewTile != null && _HasGroundHit && Input.GetMouseButtonUp(0))
        {
            Transform lNewTile = Instantiate(_TileToSpawn, previewTile.position, Quaternion.identity);
            _PlacedTiles.Add(lNewTile);            Destroy(previewTile.gameObject);
            previewTile = null;
            HandlingTile = false;
            OnTilePlaced?.Invoke();
        }
    }

    private bool TryGetPlacementHit(Ray pRay, out RaycastHit pHit)
    {
        var lHits = Physics.RaycastAll(pRay, _RaycastDistance, _GroundLayer | _TilesLayer);
        Array.Sort(lHits, (a, b) => a.distance.CompareTo(b.distance));

        foreach (var lHit in lHits)
        {
            if (previewTile != null && (lHit.transform == previewTile || lHit.transform.IsChildOf(previewTile)))
                continue;

            pHit = lHit;
            return true;
        }

        pHit = default;
        return false;
    }

    public void StartHandlingTile()
    {
        HandlingTile = true;
    }
    
    public void ClearSelection()
    {
        _TileToSpawn = null;
        _TilePreviewPrefab = null;

        if (previewTile != null)
            previewTile.position = InstantiatePos;
    }

    
    public void ResetPlacedTiles()
    {
        foreach (Transform lTile in _PlacedTiles)
        {
            if (lTile != null)
                Destroy(lTile.gameObject);
        }

        _PlacedTiles.Clear();
        HandlingTile = false;
        ClearSelection();
    }
}