using UnityEngine;
using System.Collections;
using UnityEngine.Video;
using UnityEngine.SceneManagement; 

public class EndHome : MonoBehaviour
{

    public VideoPlayer videoPlayer;
    public string sceneToLoad;

    public GameObject button; 

    void Start()
    {
        videoPlayer.loopPointReached += OnVideoFinished;
    }

    void OnVideoFinished(VideoPlayer vp)
    {
        button.SetActive(true); 
        StartCoroutine(FadeIn());
    }
    IEnumerator FadeIn()
    {
        button.GetComponent<TitlePulse>().enabled = false;

        float elapsedTime = 0f;
        Vector3 endScale = button.transform.localScale;
        float duration = 1f; 

        while (elapsedTime < duration)
        {
            button.transform.localScale = Vector3.Lerp(Vector3.zero, endScale, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        button.GetComponent<TitlePulse>().enabled = true; 
    }

    public void Home()
    {
        SceneManager.LoadScene(sceneToLoad); 
    }
}
