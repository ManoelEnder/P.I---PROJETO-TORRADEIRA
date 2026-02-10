using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemporalObject : MonoBehaviour
{
    private Renderer objectRenderer;
    private bool isVisible = false;

    void Start()
    {
        objectRenderer = GetComponent<Renderer>();
        SetVisibility(false);
    }

    void Update()
    {
        // Check for visibility based on camera position or any other logic
        if(CanReveal())
        {
            SetVisibility(true);
        }
        else
        {
            SetVisibility(false);
        }
    }

    private bool CanReveal()
    {
        // Logic to determine if the object should be revealed
        // For instance, we can check if the camera is within a certain distance
        return Vector3.Distance(Camera.main.transform.position, transform.position) < 10f;
    }

    private void SetVisibility(bool visible)
    {
        isVisible = visible;
        objectRenderer.enabled = isVisible;
    }
}