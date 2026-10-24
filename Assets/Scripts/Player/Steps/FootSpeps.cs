using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class FootSpeps : MonoBehaviour
{
    [SerializeField] private Tilemap tilemap;
    [SerializeField] private AudioSource audioSource;
    
    [SerializeField] private AudioClip GrassStep;
    [SerializeField] private AudioClip DirtStep;
    [SerializeField] private AudioClip StoneStep;
    
    [SerializeField] private float stepInterval = 0.45f;
    
    [SerializeField] private List<TileGroundData> TileGroundDatas;
    
    private Dictionary<TileBase, GroundType> tileGroundType;
    
    private float stepCounter;
    private Vector2 lastPos;
    public Animation anim;
    public bool moving;

    private void Awake()
    {
        tileGroundType = new Dictionary<TileBase, GroundType>();

        foreach (var ground in TileGroundDatas)
        {
            if (ground == null || ground.tiles == null)
            {
                continue;
            }

            foreach (var tile in ground.tiles)
            {
                if (tile != null && !tileGroundType.ContainsKey(tile))
                {
                    tileGroundType.Add(tile, ground.groundType);
                }
            }
            
        }
    }

    private void Update()
    {

        moving = anim.isMoving;
        
        Debug.Log("player is moving: " + moving);
        
        if (!moving)
        {
            stepCounter = 0f;
            audioSource.Stop();
            return;
        }
        
        stepCounter -= Time.deltaTime;

        if (stepCounter <= 0f)
        {
            PlayStep();
            stepCounter = stepInterval;
        }
    }
    

    private void PlayStep()
    {
        
        if (tilemap == null)
        {
            return;
        }
        
        Vector3 worldPos = transform.position;
        Vector3Int  tilePos = tilemap.WorldToCell(worldPos);
        TileBase tile = tilemap.GetTile<TileBase>(tilePos);

        if (tile == null)
        {
            return;
        }

        if (!tileGroundType.TryGetValue(tile, out GroundType groundType))
        {
            return;
        }

        switch (groundType)
        {
            case  GroundType.Grass:
                stepInterval = 0.45f;
                audioSource.clip = GrassStep;
                break;
            case  GroundType.Dirt:
                stepInterval = 0.6f;
                audioSource.clip = DirtStep;
                break;
            case  GroundType.Stone:
                stepInterval = 0.5f;
                audioSource.clip = StoneStep;
                break;
            default:
                return;
        }
        
        audioSource.Play();
    }
}
