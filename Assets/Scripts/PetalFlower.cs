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
    private Color player1Color = Color.red;
    private Color player2Color = Color.blue;

    private bool isCompleted = false;

    void Start()
    {
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

            yield return new WaitForSeconds(petalChangeInterval);

            // Interrupt check
            if (currentPlayer == "" || isCompleted)
                yield break;
        }

        // All petals colored — complete the flower
        isCompleted = true;
        GameManager.Instance.AwardPoint(currentPlayer);
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
