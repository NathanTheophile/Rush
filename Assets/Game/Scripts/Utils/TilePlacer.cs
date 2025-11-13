using UnityEngine;
using UnityEngine.EventSystems;

public class TilePlacer : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] private Transform _TileToSpawn;
    [SerializeField] private Transform _TilePreviewPrefab;

    [Header("Physics")]
    [SerializeField] private float _RaycastDistance = 20f;
    [SerializeField] private LayerMask _GroundLayer, _UiLayer;

    private Vector3 InstantiatePos;
    public static Transform previewTile;

    void Start()
    {
        InstantiatePos = new Vector3(1000, 1000, 1000);
        InstantiatePreviewTile(_TilePreviewPrefab, InstantiatePos);
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
        if (Physics.Raycast(lRay, out lHitObject, _RaycastDistance, _GroundLayer)) previewTile.position = Vector3Int.RoundToInt(lHitObject.point);
        if (Input.GetMouseButtonUp(0)) Instantiate(_TileToSpawn, previewTile.position, Quaternion.identity);
    }
}