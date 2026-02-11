using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using System.Collections;

public class PhotoCamera : MonoBehaviour
{
    public Camera photoCam;
    public Camera playerCam;
    public RawImage photoPreview;
    public GameObject[] temporalObjects;
    public Image flashImage;

    public float cooldown = 2f;
    public float flashDuration = 0.2f;

    RenderTexture rt;
    Texture2D photo;
    bool canShoot = true;

    void Start()
    {
        rt = new RenderTexture(512, 512, 24);
        photoCam.targetTexture = rt;
        photoCam.enabled = false;
        photoPreview.gameObject.SetActive(false);

        if (flashImage != null)
        {
            Color c = flashImage.color;
            c.a = 0f;
            flashImage.color = c;
        }
    }

    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame && canShoot)
        {
            StartCoroutine(TakePhoto());
        }
    }

    IEnumerator TakePhoto()
    {
        canShoot = false;

        transform.SetPositionAndRotation(
            playerCam.transform.position,
            playerCam.transform.rotation
        );

        photoCam.fieldOfView = playerCam.fieldOfView;

        photoCam.enabled = true;
        photoCam.Render();
        photoCam.enabled = false;

        yield return null;

        StartCoroutine(FlashEffect());



        RenderTexture.active = rt;
        photo = new Texture2D(rt.width, rt.height, TextureFormat.RGB24, false);
        photo.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
        photo.Apply();
        RenderTexture.active = null;

        photoPreview.texture = photo;
        photoPreview.gameObject.SetActive(true);

        foreach (GameObject obj in temporalObjects)
            obj.SetActive(true);

        yield return new WaitForSeconds(2f);
        photoPreview.gameObject.SetActive(false);

        yield return new WaitForSeconds(cooldown);
        canShoot = true;
    }

    IEnumerator FlashEffect()
    {
        float t = 0f;

        if (flashImage == null)
            yield break;

        Color c = flashImage.color;
        c.a = 1f;
        flashImage.color = c;

        while (t < flashDuration)
        {
            t += Time.deltaTime;
            float lerp = t / flashDuration;

            c.a = Mathf.Lerp(1f, 0f, lerp);
            flashImage.color = c;

            yield return null;
        }

        c.a = 0f;
        flashImage.color = c;
    }
}
