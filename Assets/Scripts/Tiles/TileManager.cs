using UnityEngine;
using UnityEngine.Tilemaps;

public class TileManager : MonoBehaviour
{
    public static TileManager Instance;

    private bool showPreview;
    [SerializeField] private int range;

    [SerializeField] private Transform preview;
    [SerializeField] private Camera playerCamera;
    [SerializeField] private Tilemap tilemap;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
    }

    private void Update()
    {
        if (playerCamera == null || preview == null || tilemap == null)
            return;

        Vector3 mousePos = Input.mousePosition;
        mousePos.z = Mathf.Abs(tilemap.transform.position.z - playerCamera.transform.position.z);

        Vector3 worldPos = playerCamera.ScreenToWorldPoint(mousePos);
        Vector3Int cellPos = tilemap.WorldToCell(worldPos);

        int cellPlayerDistance = Mathf.FloorToInt(Vector3.Distance((Vector2)playerCamera.transform.position, new Vector2(cellPos.x, cellPos.y)));

        if (cellPlayerDistance > range)
        {
            preview.gameObject.SetActive(false);
            return;
        }

        preview.gameObject.SetActive(showPreview);

        preview.position = tilemap.GetCellCenterWorld(cellPos);
    }

    public void ShowPreview(bool show)
    {
        showPreview = show;
    }
}
