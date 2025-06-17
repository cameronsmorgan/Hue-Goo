using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class VideoManager : MonoBehaviour
{
    public VideoPlayer videoPlayer; 
    public string sceneToLoad; 

    void Start()
    {
        videoPlayer.loopPointReached += OnVideoFinished; 
    }

    void OnVideoFinished(VideoPlayer vp)
    {
        SceneManager.LoadScene(sceneToLoad);
    }
}

