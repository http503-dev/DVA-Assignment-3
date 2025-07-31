using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;

public class CameraController : MonoBehaviour
{
    public float sensitivity = 2f;
    private float yaw = 0f;
    private float pitch = 0f;

    [Header("UI Interaction")]
    public GraphicRaycaster raycaster;
    public EventSystem eventSystem;
    public Camera mainCamera;

    void Update()
    {
        HandleLook();

        if (Cursor.lockState == CursorLockMode.Locked && Input.GetMouseButtonDown(0))
        {
            TryClickUI();
        }
    }

    void HandleLook()
    {
        if (Cursor.lockState != CursorLockMode.Locked)
            return;

        yaw += Input.GetAxis("Mouse X") * sensitivity;
        pitch -= Input.GetAxis("Mouse Y") * sensitivity;
        pitch = Mathf.Clamp(pitch, -90f, 90f);

        transform.eulerAngles = new Vector3(pitch, yaw, 0f);
    }

    void TryClickUI()
    {
        PointerEventData pointerData = new PointerEventData(eventSystem);
        pointerData.position = new Vector2(Screen.width / 2, Screen.height / 2);

        List<RaycastResult> results = new List<RaycastResult>();
        raycaster.Raycast(pointerData, results);

        foreach (RaycastResult result in results)
        {
            Debug.Log(result.gameObject.name);
            Button button = result.gameObject.GetComponent<Button>();
            if (button != null)
            {
                button.onClick.Invoke();
                Debug.Log("Clicked on: " + result.gameObject.name);
                break;
            }
        }
    }
}