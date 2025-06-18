using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class PetalFlower : MonoBehaviour
{
    public List<SpriteRenderer> petals = new List<SpriteRenderer>();
    public float petalChangeInterval = 0.5f;

    private Coroutine colorCoroutine = null;
    private string currentPlayer = "";
    private int coloredPetals = 0;
    private Color player1Color;
    private Color player2Color;

    private bool isCompleted = false;

    void Start()
    {
        ColorUtility.TryParseHtmlString("#78DBD9", out player1Color);
        ColorUtility.TryParseHtmlString("#96DA83", out player2Color);
        
        // Automatically find all child petals
        petals.Clear();
        foreach (Transform child in transform)
        {
            SpriteRenderer sr = child.GetComponent<SpriteRenderer>();
            if (sr != null) petals.Add(sr);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isCompleted) return; // Ignore if flower is already completed

        if (other.name == "Player1" || other.name == "Player2")
        {
            if (currentPlayer != other.name)
            {
                ResetPetals();
                currentPlayer = other.name;

                if (colorCoroutine != null) StopCoroutine(colorCoroutine);
                colorCoroutine = StartCoroutine(ColorPetalsRoutine());
            }
        }
    }

    private IEnumerator ColorPetalsRoutine()
    {
        coloredPetals = 0;
        Color targetColor = (currentPlayer == "Player1") ? player1Color : player2Color;

        while (coloredPetals < petals.Count)
        {
            petals[coloredPetals].color = targetColor;
            coloredPetals++;

            // Immediately check if the flower is complete
            if (coloredPetals >= petals.Count)
            {
                isCompleted = true;
                GameManager.Instance.AwardPoint(currentPlayer);
                yield break; // Exit after awarding point
            }

            yield return new WaitForSeconds(petalChangeInterval);

            // Interrupt check (in case something resets mid-process)
            if (currentPlayer == "" || isCompleted)
                yield break;
        }
    }

    private void ResetPetals()
    {
        if (isCompleted) return; // No reset allowed if completed

        foreach (var petal in petals)
        {
            petal.color = Color.white;
        }
        coloredPetals = 0;
    }

}
