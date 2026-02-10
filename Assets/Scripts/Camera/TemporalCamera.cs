using UnityEngine;

public class TemporalCamera : MonoBehaviour
{
    public float revealDistance = 10f;
    public LayerMask revealLayerMask;

    private Camera cam;

    void Start()
    {
        cam = GetComponent<Camera>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            RevealHiddenObjects();
        }
    }

    void RevealHiddenObjects()
    {
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0)); // Ray from the center of the screen
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, revealDistance, revealLayerMask))
        {
            GameObject hiddenObject = hit.collider.gameObject;
            if (IsInViewFrustum(hiddenObject))
            {
                // Logic to reveal the hidden object
                hiddenObject.SetActive(true);
                Debug.Log("Revealed: " + hiddenObject.name);
            }
        }
    }

    bool IsInViewFrustum(GameObject obj)
    {
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(cam);
        Collider objCollider = obj.GetComponent<Collider>();

        return GeometryUtility.TestPlanesAABB(planes, objCollider.bounds);
    }
}