using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;

public class CameraController : MonoBehaviour
{
    public float sensitivity = 2f;
    private float yaw = 0f;
    private float pitch = 0f;

    public GraphicRaycaster raycaster;
    public EventSystem eventSystem;
    public Camera mainCamera;

    private GameObject lastHoveredObject;

    void Update()
    {
        HandleLook();

        HoverUI();

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
        pitch = Mathf.Clamp(pitch, -45f, 45f);

        transform.eulerAngles = new Vector3(pitch, yaw, 0f);
    }

    void TryClickUI()
    {
        PointerEventData pointerData = new PointerEventData(eventSystem);
        pointerData.position = new Vector2(Screen.width / 2, Screen.height / 2);

        List<RaycastResult> allResults = RaycastAllUI(pointerData);

        foreach (RaycastResult result in allResults)
        {
            Button button = result.gameObject.GetComponent<Button>();
            if (button != null)
            {
                button.onClick.Invoke();
                Debug.Log("Clicked on: " + result.gameObject.name);
                break;
            }
        }
    }

    void HoverUI()
    {
        PointerEventData pointerData = new PointerEventData(eventSystem);
        pointerData.position = new Vector2(Screen.width / 2, Screen.height / 2);

        List<RaycastResult> allResults = RaycastAllUI(pointerData);

        GameObject currentHoveredObject = null;

        foreach (RaycastResult result in allResults)
        {
            if (result.gameObject.GetComponent<Button>() != null)
            {
                currentHoveredObject = result.gameObject;
                break;
            }
        }

        if (currentHoveredObject != lastHoveredObject)
        {
            if (lastHoveredObject != null)
            {
                ExecuteEvents.Execute<IPointerExitHandler>(lastHoveredObject, pointerData, ExecuteEvents.pointerExitHandler);

                Transform oldHoverText = lastHoveredObject.transform.Find("HoverText");
                if (oldHoverText != null)
                    oldHoverText.gameObject.SetActive(false);
            }

            if (currentHoveredObject != null)
            {
                ExecuteEvents.Execute<IPointerEnterHandler>(currentHoveredObject, pointerData, ExecuteEvents.pointerEnterHandler);

                Transform newHoverText = currentHoveredObject.transform.Find("HoverText");
                if (newHoverText != null)
                    newHoverText.gameObject.SetActive(true);
            }

            lastHoveredObject = currentHoveredObject;
        }
    }

    List<RaycastResult> RaycastAllUI(PointerEventData pointerData)
    {
        List<RaycastResult> allResults = new List<RaycastResult>();

        GraphicRaycaster[] raycasters = FindObjectsOfType<GraphicRaycaster>();
        foreach (GraphicRaycaster rc in raycasters)
        {
            List<RaycastResult> results = new List<RaycastResult>();
            rc.Raycast(pointerData, results);
            allResults.AddRange(results);
        }

        allResults.Sort((a, b) => a.distance.CompareTo(b.distance));
        return allResults;
    }
}
