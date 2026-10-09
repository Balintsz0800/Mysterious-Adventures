using System;
using System.Collections.Generic;
using UnityEngine;

public class Shadow : MonoBehaviour
{
    private class ShadowParts
    {
        public SpriteRenderer spriteRenderer;
        public SpriteRenderer shadowRenderer;
    }
    
    [SerializeField] private float sunriseTime = 6f;
    [SerializeField] private float sunsetTime = 18f;
    
    [SerializeField] private float maxShadowLength = 1.5f;
    [SerializeField] private float minShadowLength = 0.1f;
    [SerializeField] private float shadowAlpha = .45f;
    [SerializeField] private Color shadowColor = Color.black;

    [SerializeField] private Vector2 morningDir = new Vector2(-1f, -0.5f);
    [SerializeField] private Vector2 eveningDir = new Vector2(1f, -0.5f);
    
    private readonly List<ShadowParts> shadowparts = new List<ShadowParts>();

    private void Awake()
    {
        SpriteRenderer[] renderers = GetComponentsInChildren<SpriteRenderer>(true);

        foreach (SpriteRenderer original in renderers)
        {
            if (original.transform.parent == null || original.gameObject.name == "Shadow")
            {
                continue;
            }
            
            GameObject shadowObj = new GameObject("Shadow");
            shadowObj.transform.SetParent(original.transform.parent, false);
            shadowObj.transform.localPosition = original.transform.localPosition;
            shadowObj.transform.localRotation = original.transform.localRotation;
            shadowObj.transform.localScale = original.transform.localScale;
            
            SpriteRenderer shadowRenderer = shadowObj.AddComponent<SpriteRenderer>();
            shadowRenderer.sprite = original.sprite;
            shadowRenderer.color = new Color(shadowColor.r, shadowColor.g, shadowColor.b, shadowAlpha);
            shadowRenderer.sortingLayerID = original.sortingLayerID;
            shadowRenderer.sortingOrder = original.sortingOrder -2;
            shadowRenderer.flipX = original.flipX;
            shadowRenderer.flipY = original.flipY;
            
            shadowparts.Add(new ShadowParts{spriteRenderer = original, shadowRenderer = shadowRenderer});
        }
    }

    private void LateUpdate()
    {
        if (DayNightTimer.Instance == null)
        {
            return;
        }
        
        float hour = DayNightTimer.Instance.CurrentHour;
        
        float dayProgress = Mathf.InverseLerp(sunriseTime, sunsetTime, hour);
        bool isDay = hour >= sunriseTime && hour <= sunsetTime;
        
        Vector2 dir = Vector2.Lerp(morningDir.normalized, eveningDir.normalized, dayProgress).normalized;
        
        float shadowLength = Mathf.Lerp(maxShadowLength, minShadowLength, Mathf.Sin(dayProgress * Mathf.PI));
        
        float alpha = isDay  ? shadowAlpha : 0f;
        Vector3 offset = isDay ? (Vector3)(dir * shadowLength) : Vector3.zero;

        foreach (ShadowParts part  in shadowparts)
        {
            if (part.spriteRenderer == null || part.shadowRenderer == null)
            {
                continue;
            }
            
            part.shadowRenderer.sprite = part.spriteRenderer.sprite;
            part.shadowRenderer.flipX = part.spriteRenderer.flipX;
            part.shadowRenderer.flipY = part.spriteRenderer.flipY;
            part.shadowRenderer.sortingLayerID = part.spriteRenderer.sortingLayerID;
            part.shadowRenderer.sortingOrder = part.spriteRenderer.sortingOrder -2;
            
            part.shadowRenderer.transform.position = part.spriteRenderer.transform.position + offset;
            part.shadowRenderer.transform.rotation = part.spriteRenderer.transform.rotation;
            part.shadowRenderer.transform.localScale = part.spriteRenderer.transform.localScale;
            
            part.shadowRenderer.color = new Color(shadowColor.r, shadowColor.g, shadowColor.b, alpha);
        }
    }
}
