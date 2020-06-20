using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents the square in the upper-middle of the screen where cards go to be played.
/// </summary>
public class CardCatcher : MonoBehaviour
{
    public static CardCatcher Instance;

    public Vector3 caughtCardScale = new Vector3(.165f, .165f, .165f);

    private void Awake()
    {
        if (Instance != null) Debug.LogError("More than one CardCatcher found in scene");
        Instance = this;
    }

    /// <summary>
    /// Called when a UI action card is moved over the card catcher square. Visually queues the card up to be used.
    /// </summary>
    public void CatchCard(ActionCardDisplay card)
    {
        GameObject caughtCard = Instantiate<GameObject>(card.gameObject, transform);
        caughtCard.transform.localScale = caughtCardScale;
    }

    /// <summary>
    /// At the end of the turn, deletes all the card objects queued up in the card catcher square.
    /// </summary>
    public void ClearCaughtCards()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
    }
}
