using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperCard : MonoBehaviour
{
    //Counter to track until superevent
    public int cardCounter = 0;
    //reference to the super event card
    public EventCard superCard;
    //reference to the gamemanager
    public GameManager gm;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    //Add to the counter as card is played
    public void IncrementCard()
    {
        //Increment the counter on card played
        cardCounter++;

        //Check if the card is a catastrophe with lower counter threshold or if meets threshold
        if (cardCounter >= 2)
        {
            //Get a random position in the deck and shuffle in card
            gm.CurrentEventDeck.Insert(Random.Range(0, gm.CurrentEventDeck.Count), superCard);
            //reset card counter
            cardCounter = 0;
        }
    }
}

