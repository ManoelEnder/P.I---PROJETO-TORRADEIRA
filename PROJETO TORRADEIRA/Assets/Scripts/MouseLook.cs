using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public float sensibilidadeMouse = 100f;
    public Transform corpoDoJogador;
    float rotacaoX = 0f;

    void Start()
    {
        
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        
        float mouseX = Input.GetAxis("Mouse X") * sensibilidadeMouse * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * sensibilidadeMouse * Time.deltaTime;

      
        rotacaoX -= mouseY;
        rotacaoX = Mathf.Clamp(rotacaoX, -90f, 90f);

        transform.localRotation = Quaternion.Euler(rotacaoX, 0f, 0f);

        
        corpoDoJogador.Rotate(Vector3.up * mouseX);
    }
}