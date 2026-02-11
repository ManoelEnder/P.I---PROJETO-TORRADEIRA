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


    RenderTexture rt;
    Texture2D photo;

    void Start()
    {
        rt = new RenderTexture(512, 512, 24);
        photoCam.targetTexture = rt;
        photoCam.enabled = false;
        photoPreview.gameObject.SetActive(false);
    }

    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            TakePhoto();
        }
    }

    void TakePhoto()
    {
        StopAllCoroutines();

        transform.SetPositionAndRotation(
            playerCam.transform.position,
            playerCam.transform.rotation
        );

        photoCam.fieldOfView = playerCam.fieldOfView;

        photoCam.enabled = true;
        photoCam.Render();
        photoCam.enabled = false;

        RenderTexture.active = rt;
        photo = new Texture2D(rt.width, rt.height, TextureFormat.RGB24, false);
        photo.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
        photo.Apply();
        RenderTexture.active = null;

        photoPreview.texture = photo;
        photoPreview.gameObject.SetActive(true);

        foreach (GameObject obj in temporalObjects)
            obj.SetActive(true);

        StartCoroutine(HidePreview());
    }


    IEnumerator HidePreview()
    {
        yield return new WaitForSeconds(2f);
        photoPreview.gameObject.SetActive(false);
    }
}
