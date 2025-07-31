using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.Video;

public class GameManager : MonoBehaviour
{
    public GameObject crosshair;
    public GameObject mainMenuUI;
    public Canvas HUDCanvas;
    public GameObject outTouchpoints;
    public GameObject inTouchpoints;

    public GameObject videoSphere;
    public VideoPlayer videoPlayer;
    
    public Material introVideoMaterial;
    public Material outStillImageMaterial;
    public Material inStillImageMaterial;

    private MeshRenderer sphereRenderer;

    void Start()
    {
        sphereRenderer = videoSphere.GetComponent<MeshRenderer>();

        videoSphere.SetActive(false);
        videoPlayer.loopPointReached += OnVideoEnd;

        ShowMainMenu();
        StartSimulation();
    }

    public void ShowMainMenu()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        crosshair.SetActive(false);
        outTouchpoints.SetActive(false);
        inTouchpoints.SetActive(false);
        mainMenuUI.SetActive(true);
        videoSphere.SetActive(false);
    }

    public void StartSimulation()
    {
        crosshair.SetActive(true);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        mainMenuUI.SetActive(false);
        outTouchpoints.SetActive(false);
        videoSphere.SetActive(true);

        sphereRenderer.material = introVideoMaterial;
        Play360Video();
    }

    void Play360Video()
    {
        videoPlayer.Play();
    }

    void OnVideoEnd(VideoPlayer vp)
    {
        videoPlayer.Stop();
        sphereRenderer.material = outStillImageMaterial;
        outTouchpoints.SetActive(true);
        // HUDClicks(false);

        // Optionally unlock cursor again if interaction is needed
        // Cursor.lockState = CursorLockMode.None;
        // Cursor.visible = true;
    }
                
    // void HUDClicks(bool state)
    // {
    //     // Disable raycast target on HUD elements
    //     foreach (Graphic g in HUDCanvas.GetComponentsInChildren<Graphic>())
    //     {
    //         g.raycastTarget = state;
    //     }
    // }

    public void ToStudio()
    {
        Debug.Log("To Studio");
        sphereRenderer.material = inStillImageMaterial;
        outTouchpoints.SetActive(false);
        inTouchpoints.SetActive(true);
    }
}