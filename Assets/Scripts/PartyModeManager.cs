using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class PartyModeManager : MonoBehaviour
{
    // Static Variables for party mode
    public static List<string> toPlay;
    public static int numRounds;
    public static int numToWin;
    public static int player1WinPoints;
    public static int player2WinPoints;
    public static int lastRoundWinner;



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
        numRounds = n;
        numToWin = Mathf.CeilToInt(n/2f);
        player1WinPoints = 0;
        player2WinPoints = 0;
        lastRoundWinner = 0;


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
        //populateToPlay(n);

        LevelTypes(n);
    }

    public void LoadNext()
    {
        string sceneName = PartyModeManager.toPlay[0];
        toPlay.RemoveAt(0);

        foreach (var scene in toPlay)
        {
            Debug.Log("Lineup: " + scene);
        }


        SceneManager.LoadScene(sceneName);
    }

    //public void LoadNextScene(int currentSceneIndex)
    //{
    //    if (currentSceneIndex + 1 < selectedScenes.Count)
    //    {
    //        SceneManager.LoadScene(selectedScenes[currentSceneIndex + 1]);
    //    }
    //    else
    //    {
    //        // All rounds complete - you can load results or return to main menu
    //        //SceneManager.LoadScene("ResultsScene");

    //        Debug.Log("done"); 
    //    }
    //}


    public void LevelTypes(int n)
    {
        toPlay = new List<string>();
        List<string> selectedScenes;


        while (toPlay.Count < n)
        {
            selectedScenes = new List<string>
            {
                GetRandomNew(territory),
                GetRandomNew(collection),
                GetRandomNew(race),
                GetRandomNew(memory),
            };

            selectedScenes = ShuffleList(selectedScenes);

            while (toPlay.Count < n && selectedScenes.Count > 0)
            {
                toPlay.Add(selectedScenes[0]);
                selectedScenes.RemoveAt(0);
            }
        }

        

        


        foreach (var scene in toPlay)
        {
            Debug.Log("Randomized Scene: " + scene);
        }


        //SceneManager.LoadScene(selectedScenes[0]);
    }

    string GetRandomNew(List<string> list)
    {
        bool stop = false;
        string temp = "";
        while (!stop)
        {
            temp = list[Random.Range(0, list.Count)];

            if (!toPlay.Contains(temp)) stop = true;
        }

        return temp;
    }

    private List<string> ShuffleList(List<string> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            string temp = list[i];
            int randomIndex = Random.Range(i, list.Count);
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }

        return list;
    }
}
