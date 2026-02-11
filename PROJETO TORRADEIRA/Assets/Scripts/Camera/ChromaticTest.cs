using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class ChromaticTest : MonoBehaviour
{
    public Volume volume;
    ChromaticAberration chromatic;

    void Start()
    {
        volume.profile.TryGet(out chromatic);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            chromatic.intensity.value = 0.8f;
        }

        if (Input.GetKeyDown(KeyCode.Y))
        {
            chromatic.intensity.value = 0f;
        }
    }
}
