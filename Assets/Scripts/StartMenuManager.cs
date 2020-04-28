using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartMenuManager : MonoBehaviour
{
    // Public Variables.
    public Transform blueTrans;
    public Transform orangeTrans;
    public string nextLevel;
    public CanvasGroup imgCanvasGroup;
    public CanvasGroup[] textCanvasGroup;

    // Private Variables.
    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        GameObject blue = Instantiate(PortalManager.instance.bluePrefab);
        blue.transform.position = blueTrans.position;
        blue.transform.rotation = blueTrans.rotation;

        RaycastHit hit;

        Physics.Raycast(blue.transform.position, -blue.transform.forward, out hit);
        blue.GetComponent<Portal>().surface = hit.collider.gameObject;

        GameObject orange = Instantiate(PortalManager.instance.orangePrefab);
        orange.transform.position = orangeTrans.position;
        orange.transform.rotation = orangeTrans.rotation;
        Physics.Raycast(orange.transform.position, -orange.transform.forward, out hit);
        orange.GetComponent<Portal>().surface = hit.collider.gameObject;

        StartCoroutine(FadeInCO());
    }

    private IEnumerator FadeInCO()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        float duration = 1f;

        imgCanvasGroup.alpha = 1f;
        foreach (CanvasGroup canvasGroup in textCanvasGroup)
            canvasGroup.alpha = 0f;

        yield return new WaitForSeconds(1f);

        // Fade in from black.
        float elapsedTime = 0f;
        float startVal = 1f;
        float targetVal = 0f;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.unscaledDeltaTime;
            imgCanvasGroup.alpha = Mathf.SmoothStep(startVal, targetVal, (elapsedTime / duration));
            yield return null;
        }
        imgCanvasGroup.alpha = targetVal;

        // Fade buttons in.
        elapsedTime = 0f;
        startVal = 0f;
        targetVal = 1f;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.unscaledDeltaTime;
            foreach (CanvasGroup canvasGroup in textCanvasGroup)
                canvasGroup.alpha = Mathf.SmoothStep(startVal, targetVal, (elapsedTime / duration));
            yield return null;
        }
        foreach (CanvasGroup canvasGroup in textCanvasGroup)
            canvasGroup.alpha = targetVal;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void Play()
    {
        StartCoroutine(PlayCO());
        SceneManager.LoadScene(nextLevel, LoadSceneMode.Single);
    }

    private IEnumerator PlayCO()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        float duration = 1f;

        // Fade buttons out.
        float elapsedTime = 0f;
        float startVal = 1f;
        float targetVal = 0f;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.unscaledDeltaTime;
            foreach (CanvasGroup canvasGroup in textCanvasGroup)
                canvasGroup.alpha = Mathf.SmoothStep(startVal, targetVal, (elapsedTime / duration));
            yield return null;
        }
        foreach (CanvasGroup canvasGroup in textCanvasGroup)
            canvasGroup.alpha = targetVal;

        // Fade to black.
        elapsedTime = 0f;
        startVal = 0f;
        targetVal = 1f;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.unscaledDeltaTime;
            imgCanvasGroup.alpha = Mathf.SmoothStep(startVal, targetVal, (elapsedTime / duration));
            yield return null;
        }
        imgCanvasGroup.alpha = targetVal;

        SceneManager.LoadScene(nextLevel, LoadSceneMode.Single);
    }

    public void Itch()
    {
        Application.OpenURL("https://nwlsmith.itch.io/portal-15");
    }

    public void Quit()
    {
        Application.Quit();
    }
}
