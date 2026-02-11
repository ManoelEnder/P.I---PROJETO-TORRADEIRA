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
    public GameObject flashObject;

    public float cooldown = 2f;

    RenderTexture rt;
    Texture2D photo;
    bool canShoot = true;

    void Start()
    {
        rt = new RenderTexture(512, 512, 24);
        photoCam.targetTexture = rt;
        photoCam.enabled = false;
        photoPreview.gameObject.SetActive(false);

        if (flashObject != null)
            flashObject.SetActive(false);
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

        if (flashObject != null)
            flashObject.SetActive(true);

        yield return new WaitForSeconds(0.05f);

        photoCam.enabled = true;
        photoCam.Render();
        photoCam.enabled = false;

        if (flashObject != null)
            flashObject.SetActive(false);

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

        foreach (GameObject obj in temporalObjects)
            obj.SetActive(false);

        yield return new WaitForSeconds(cooldown);
        canShoot = true;
    }
}
