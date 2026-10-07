using System;
using JetBrains.Annotations;
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
    [CanBeNull] public PlayerMovement player;

    private void Update()
    {
        isMoving = player.movementVector.x != 0 || player.movementVector.y != 0;

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
        Vector3Int tilePos =  tilemap.WorldToCell(transform.position);
        
        TileBase tile = tilemap.GetTile(tilePos);

        if (tile == null)
        {
            return; 
        }
        
        string tileName = tile.name.ToLower();

        if (tileName.Contains("grass"))
        {
            stepInterval = 0.45f;
            audioSource.clip = GrassStep;
        }
        else if (tileName.Contains("dirt"))
        {
            stepInterval = 0.6f;
            audioSource.clip = DirtStep;
        }
        else if (tileName.Contains("stone"))
        {
            audioSource.clip = StoneStep;
        }
        else
        {
            return;
        }
        
        audioSource.Play();
    }
}
