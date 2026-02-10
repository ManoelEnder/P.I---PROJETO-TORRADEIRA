using UnityEngine;
using System.Collections;

public class ObjectRevealEffect : MonoBehaviour
{
    public ParticleSystem revealParticles;
    public Material objectMaterial;
    public Color revealColor = Color.white;
    public float revealDuration = 1.0f;

    void Start()
    {
        // Start the reveal effect when the object is instantiated
        StartCoroutine(RevealObject());
    }

    private IEnumerator RevealObject()
    {
        // Play the particle effect
        if (revealParticles != null)
        {
            revealParticles.Play();
        }

        // Change the material color over time
        Color initialColor = objectMaterial.color;
        float elapsedTime = 0f;

        while (elapsedTime < revealDuration)
        {
            float t = elapsedTime / revealDuration;
            objectMaterial.color = Color.Lerp(initialColor, revealColor, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure final color is set
        objectMaterial.color = revealColor;
    }
}