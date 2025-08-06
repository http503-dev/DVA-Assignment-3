using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.Video;
using UnityEditor;
using TMPro;

public class GameManager : MonoBehaviour
{
    private bool inStudio = false;
    public GameObject crosshair;
    public GameObject mainMenuUI;
    public GameObject outTouchpoints;
    public GameObject inTouchpoints;
    public GameObject objectiveText;
    public GameObject introText;

    public GameObject stillImage;
    public GameObject videoSphere;
    public VideoPlayer videoPlayer;
    
    public Material introVideoMaterial;
    public Material VideoMaterial;
    public VideoClip bassVideoMaterial;
    public VideoClip rhythmVideoMaterial;
    public VideoClip leadVideoMaterial;
    public VideoClip drumVideoMaterial;
    
    public AudioSource bgmSource;
    public AudioSource sfxSource;
    public AudioClip menuButtonSFX;
    public AudioClip hotSpotButtonSFX;
    public float maxVolume = 0.2f;

    private Coroutine fadeCoroutine;
    
    public Material outStillImageMaterial;
    public Material inStillImageMaterial;

    public CanvasGroup fadeCanvasGroup;
    public float fadeDuration;

    private MeshRenderer sphereRenderer;

    void Start()
    {
        sphereRenderer = videoSphere.GetComponent<MeshRenderer>();

        videoSphere.SetActive(false);
        videoPlayer.loopPointReached += OnVideoEnd;

        ShowMainMenu();
    }
    
    public void PlayBGM()
    {
        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);

        bgmSource.volume = 0f;
        if (!bgmSource.isPlaying)
            bgmSource.Play();

        fadeCoroutine = StartCoroutine(FadeBGMVolume(maxVolume));
    }

    public void PauseBGM()
    {
        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);

        fadeCoroutine = StartCoroutine(FadeBGMVolume(0f, true));
    }
    
    public void PlayMenuButtonSFX()
    {
        if (menuButtonSFX != null)
        {
            sfxSource.PlayOneShot(menuButtonSFX);
        }
    }

    public void PlayHotSpotButtonSFX()
    {
        if (hotSpotButtonSFX != null)
        {
            sfxSource.PlayOneShot(hotSpotButtonSFX);
        }
    }
    
    private IEnumerator FadeBGMVolume(float targetVolume, bool pauseAfter = false)
    {
        float startVolume = bgmSource.volume;
        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            bgmSource.volume = Mathf.Lerp(startVolume, targetVolume, elapsed / fadeDuration);
            yield return null;
        }

        bgmSource.volume = targetVolume;

        if (pauseAfter)
        {
            bgmSource.Pause();
        }
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
        PauseBGM();
        crosshair.SetActive(true);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        mainMenuUI.SetActive(false);
        outTouchpoints.SetActive(false);
        videoSphere.SetActive(true);
        introText.SetActive(true);

        sphereRenderer.material = introVideoMaterial;
        Play360Video();
    }
    
    public void PlayBass(){
        videoPlayer.clip = bassVideoMaterial;
        sphereRenderer.material = VideoMaterial;
        Play360Video();
    }
    public void PlayRhythm(){
        videoPlayer.clip = rhythmVideoMaterial;
        sphereRenderer.material = VideoMaterial;
        Play360Video();
    }
    public void PlayLead(){
        videoPlayer.clip = leadVideoMaterial;
        sphereRenderer.material = VideoMaterial;
        Play360Video();
    }
    public void PlayDrum(){
        videoPlayer.clip = drumVideoMaterial;
        sphereRenderer.material = VideoMaterial;
        Play360Video();
    }

    void Play360Video()
    {
        PauseBGM();
        objectiveText.SetActive(false);
        videoPlayer.Play();
    }

    public void Stop360Video()
    {
        videoPlayer.Stop();
    }

    void OnVideoEnd(VideoPlayer vp)
    {
        videoPlayer.Stop();
        if (!inStudio)
        {
            objectiveText.GetComponent<TextMeshProUGUI>().text = "Objective: Into the Studio!";
            sphereRenderer.material = outStillImageMaterial;
            outTouchpoints.SetActive(true);
            introText.SetActive(false);

        }
        else
        {
            sphereRenderer.material = inStillImageMaterial;
            inTouchpoints.SetActive(true);
        }
        objectiveText.SetActive(true);
        PlayBGM();
    }

    public void ToStudio()
    {
        StartCoroutine(FadeToStudio());
        inStudio = true;
    }

    public void OpenURL(string url)
    {
        Application.OpenURL(url);
    }

    private IEnumerator FadeToStudio()
    {
        // Fade to black
        yield return StartCoroutine(Fade(0f, 1f, fadeDuration));

        sphereRenderer.material = inStillImageMaterial;
        outTouchpoints.SetActive(false);
        inTouchpoints.SetActive(true);

        // Fade back
        yield return StartCoroutine(Fade(1f, 0f, fadeDuration));
        objectiveText.GetComponent<TextMeshProUGUI>().text = "Objective: Explore the Studio";
    }
    
    private IEnumerator Fade(float startAlpha, float endAlpha, float duration)
    {
        float elapsed = 0f;
        fadeCanvasGroup.alpha = startAlpha;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            fadeCanvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, t);
            yield return null;
        }

        fadeCanvasGroup.alpha = endAlpha;
    }

    public void QuitGame()
    {
        #if UNITY_EDITOR
        EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }
}