using UnityEngine;

public class RaceHelper : MonoBehaviour
{

    bool isFirstRound;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Debug.Log($"Helper UI: {Level4GameManager.Instance.currentRound == 0}");

        isFirstRound = Level4GameManager.Instance.currentRound == 0;
        

        //gameObject.SetActive(false);
    }

    private void Update()
    {
        if (!isFirstRound)
            GetComponent<HelpPanel>().Close();
    }


}
