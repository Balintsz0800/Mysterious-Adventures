using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class FootSteps : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;

    [SerializeField] private AudioClip GrassStep;
    [SerializeField] private AudioClip DirtStep;
    [SerializeField] private AudioClip StoneStep;

    [SerializeField] private float stepInterval = 0.45f;

    private float stepCounter;
    private Vector2 lastPos;
    public Animation anim;
    public bool moving;

    private void Update()
    {

        moving = anim.isMoving;

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
        TileType groundType = TileManager.Instance.GetTileType(transform.position);
        
        float pitch = 0;

        switch (groundType)
        {
            case TileType.Grass:
                stepInterval = 0.45f;
                pitch = Random.Range(0.9f, 1.1f);
                audioSource.clip = GrassStep;
                break;
            case TileType.Dirt:
                stepInterval = 0.6f;
                pitch = Random.Range(0.85f, 1.05f);
                audioSource.clip = DirtStep;
                break;
            case TileType.Stone:
                stepInterval = 0.5f;
                pitch = Random.Range(0.8f, 1f);
                audioSource.clip = StoneStep;
                break;
            default:
                return;
        }

        audioSource.pitch = pitch;

        audioSource.Play();
    }
}
