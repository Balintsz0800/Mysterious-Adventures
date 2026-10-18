using System;
using JetBrains.Annotations;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;
using UnityEngine.Tilemaps;

public class FootSpeps : MonoBehaviour
{
    [SerializeField] private Tilemap tilemap;
    [SerializeField] private AudioSource audioSource;
    
    [SerializeField] private AudioClip GrassStep;
    [SerializeField] private AudioClip DirtStep;
    [SerializeField] private AudioClip StoneStep;
    
    [SerializeField] private float stepInterval;
    
    private float stepCounter;
    
    bool isMoving;
    private Vector2 lastPos;
    public PlayerMovement player;

    private void Update()
    {
        if (player == null)
        {
            return;
        }
        
        Vector2 currentPos = new Vector2(transform.position.x, transform.position.y);
        isMoving = currentPos != lastPos;
        lastPos = currentPos;
        
        if (!isMoving)
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
        Vector3Int tilePos = tilemap.WorldToCell(transform.position);
        TileBase tile = tilemap.GetTile(tilePos);

        if (tilemap == null)
        {
            return;
        }
        if (tile == null)
        {
            return;
        }
        
        CustomTile customTile = tile as CustomTile;
        if (customTile == null)
        {
            return;
        }
        
        switch (customTile.groundType)
        {
            case GroundType.groundType.Grass:
                stepInterval = 0.45f;
                audioSource.clip = GrassStep;
                break;
            case GroundType.groundType.Dirt:
                stepInterval = 0.6f;
                audioSource.clip = DirtStep;
                break;
            case GroundType.groundType.Stone:
                stepInterval = 0.5f;
                audioSource.clip = StoneStep;
                break;
            default:
                return;
        }
        
        audioSource.Play();
    }
}
