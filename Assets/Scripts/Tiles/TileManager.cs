using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileManager : MonoBehaviour
{
    public static TileManager Instance;
    
    [SerializeField] private int range = 3;
    [SerializeField] private Transform preview;
    [SerializeField] private Camera playerCamera;
    
    [SerializeField] private Tilemap tilemap;
    [SerializeField] private List<TileGroundData> tileGroundData;

    private bool inRange;
    private bool showPreview;
    

    private readonly Dictionary<TileBase, TileType> tiles = new Dictionary<TileBase, TileType>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        BuildTileDictionary();
    }

    private void BuildTileDictionary()
    {
        tiles.Clear();

        if (tileGroundData == null)
            return;

        foreach (TileGroundData ground in tileGroundData)
        {
            if (ground == null || ground.tiles == null)
                continue;

            foreach (TileBase tile in ground.tiles)
            {
                if (tile == null)
                    continue;

                if (!tiles.ContainsKey(tile))
                {
                    tiles.Add(tile, ground.tileType);
                }
            }
        }
    }

    private void Update()
    {
        if (playerCamera == null || preview == null || tilemap == null)
            return;

        Vector3 mouseWorldPosition = GetMouseWorldPosition();

        Vector3Int cellPosition = tilemap.WorldToCell(mouseWorldPosition);
        Vector3 cellWorldPosition = tilemap.GetCellCenterWorld(cellPosition);
        
        float distance = Vector2.Distance(playerCamera.transform.position, cellWorldPosition);

        inRange = distance <= range;

        preview.gameObject.SetActive(showPreview && inRange);

        if (!showPreview || !inRange)
            return;

        preview.position = cellWorldPosition;
    }

    private Vector3 GetMouseWorldPosition()
    {
        Vector3 mousePosition = Input.mousePosition;
        
        mousePosition.z = Mathf.Abs(playerCamera.transform.position.z - tilemap.transform.position.z);

        return playerCamera.ScreenToWorldPoint(mousePosition);
    }
    
    public Vector3Int GetMouseCellPosition()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = Mathf.Abs(playerCamera.transform.position.z - tilemap.transform.position.z);
        
        return tilemap.WorldToCell(playerCamera.ScreenToWorldPoint(mousePosition));
    }

    public void ShowPreview(bool show)
    {
        showPreview = show;

        if (preview != null && !show)
        {
            preview.gameObject.SetActive(false);
        }
    }

    public bool TryGetTileType(Vector3 worldPosition, out TileType tileType)
    {
        tileType = default;

        if (tilemap == null)
            return false;

        Vector3Int cellPosition = tilemap.WorldToCell(worldPosition);
        TileBase tile = tilemap.GetTile(cellPosition);

        if (tile == null)
            return false;

        return tiles.TryGetValue(tile, out tileType);
    }

    public TileType GetTileType(Vector3 worldPosition)
    {
        if (TryGetTileType(worldPosition, out TileType tileType))
            return tileType;

        return default;
    }

    public void ModifyTile(Vector3Int position, TileType tileType)
    {
        Vector3Int cellPosition = tilemap.WorldToCell(position);
        TileBase tile = tilemap.GetTile(cellPosition);
        
        if (tile == null) 
            return;
        
        tilemap.SetTile(cellPosition, tileGroundData.Find(x => x.tileType == tileType).tiles[0]);
        tiles[tile] = tileType;
    }

    public bool HoveredTileInRange()
    {
        return inRange;
    }
}