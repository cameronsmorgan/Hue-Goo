using UnityEngine;

public class HelpPanel : MonoBehaviour
{
    private void Awake()
    {
        Debug.Log("start......");
        Time.timeScale = 0f;
    }

    public void Close()
    {
        Time.timeScale = 1f;
        gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        Debug.Log("disable");
        Time.timeScale = 1f;
        gameObject.SetActive(false);
    }
}
