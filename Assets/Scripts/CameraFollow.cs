using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float distanta = 5.0f;
    public float inaltime = 2.0f;
    public float netezireRotatie = 15.0f;
   


    private float currentX = 0f;
    private float currentY = 0f;
    private float smoothX = 0f;
    private float smoothY = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Vector3 angles = transform.eulerAngles;
        currentX = angles.y;
        currentY = angles.x;
        smoothX = currentX;
        smoothY = currentY;
    }

    void LateUpdate()
    {
        
        if (target == null) return;
        float knightsens = target.GetComponent<PlayerMovement>().mouseSensitivity;
        currentX += Input.GetAxis("Mouse X") * knightsens* 0.02f;
        currentY -= Input.GetAxis("Mouse Y") * knightsens * 0.02f;

        currentY = Mathf.Clamp(currentY, -15f, 60f);

        smoothX = Mathf.LerpAngle(smoothX, currentX, netezireRotatie * Time.deltaTime);
        smoothY = Mathf.LerpAngle(smoothY, currentY, netezireRotatie * Time.deltaTime);

        Quaternion rotation = Quaternion.Euler(smoothY,smoothX, 0);
        Vector3 direction =new Vector3(0, 0, -distanta);
        transform.position = target.position + rotation * direction + Vector3.up * inaltime;

      
        transform.LookAt(target.position + Vector3.up * inaltime);
    }

}
