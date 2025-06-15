using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class PartyScenes : MonoBehaviour
{
    static List<string> toPlay; 
    public List<string> sceneNames; // List of scene names to choose from
    public List<string> playedScenes = new List<string>(); // Keep track of played scenes

    public List<string> territory;
    public List<string> collection;
    public List<string> race;
    public List<string> memory;

    //public void LoadRandomScene()
    //{
    //    if (sceneNames.Count == 0)
    //    {
    //        Debug.LogWarning("No scenes in the list!");
    //        return;
    //    }

    //    // If all scenes have been played, reset the list
    //    if (sceneNames.Count == playedScenes.Count)
    //    {
    //        sceneNames.AddRange(playedScenes);
    //        playedScenes.Clear();
    //    }

    //    int randomIndex = Random.Range(0, sceneNames.Count);
    //    string sceneName = sceneNames[randomIndex];

    //    // Remove the scene from the available list and add to played list
    //    sceneNames.RemoveAt(randomIndex);
    //    playedScenes.Add(sceneName);

    //    SceneManager.LoadScene(sceneName);

    //}

    private void populateToPlay(int n)
    {
        toPlay = new List<string>();

        

        while (toPlay.Count != n)
        {
            int randomIndex = Random.Range(0, sceneNames.Count);
            string sceneName = sceneNames[randomIndex]; 
            if (!toPlay.Contains(sceneName))
            {
                toPlay.Add(sceneName);
            }
        }

        
    }

    public void StartParty(int n)
    {
        //toPlay.Clear();
        populateToPlay(n);
    }

    public void LoadNext()
    {
        string sceneName = PartyScenes.toPlay[0];
        toPlay.RemoveAt(0);
        SceneManager.LoadScene(sceneName);
    }
}
