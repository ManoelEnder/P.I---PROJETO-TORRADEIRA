using UnityEngine;
using UnityEngine.UI;

public class CameraHUD : MonoBehaviour {
    public Text crosshairInfoText;
    public Image crosshairProgressBar;

    private void Update() {
        UpdateCrosshair();
    }

    private void UpdateCrosshair() {
        // Check for target object under crosshair
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit)) {
            // Assuming the target has a Collider component
            var targetObject = hit.collider.gameObject;
            crosshairInfoText.text = targetObject.name;
            // Update the crosshair progress bar based on some condition
            UpdateProgressBar(targetObject);
        } else {
            crosshairInfoText.text = "";
            crosshairProgressBar.fillAmount = 0;
        }
    }

    private void UpdateProgressBar(GameObject targetObject) {
        // Dummy example condition to fill the progress bar
        // You can replace this with your own logic
        float progress = 0.5f; // Example progress value (0 to 1)
        crosshairProgressBar.fillAmount = progress;
    }
}