using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int Money { get; set; }
    public int Carbon { get; set; }
    public int Momentum { get; set; }
    public int HopeKill { get; set; }

    //Reference list of cards for queues to pull from
    public List<EventCard> MasterEventDeckRef = new List<EventCard>();
    public List<ActionCard> MasterActionDeckRef = new List<ActionCard>();

    //Current cards in player's hand and previously played cards
    public List<ActionCard> PlayerHand = new List<ActionCard>();
    public List<Card> PlayedCards = new List<Card>();

    //Keeps track of what cards come next from the decks, receives data from master lists
    public Queue<EventCard> CurrentEventDeck = new Queue<EventCard>();
    public Queue<ActionCard> CurrentActionDeck = new Queue<ActionCard>();

    // Start is called before the first frame update
    void Start()
    {
        //Money and Carbon start at 20 every game
        Money = 20;
        Carbon = 20;

        //Start the game by generating the decks players will draw from
        FillMasterActionDeck();
        GenerateActionDeck();
        FillMasterEventDeck();
        GenerateEventDeck();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DrawCard()
    {

    }

    public void MoneyCarbonTick()
    {

    }

    public void FillMasterActionDeck()
    {

    }

    public void FillMasterEventDeck()
    {

    }

    public void GenerateActionDeck()
    {
        do
        {
            //Generate random number to choose a card from master queue
            int ranNum = Random.Range(0, MasterActionDeckRef.Count);
            //Add chosen card to queue of action deck that player draws from
            CurrentActionDeck.Enqueue(MasterActionDeckRef[ranNum]);
            //Remove added card from master queue to avoid duplicates
            MasterActionDeckRef.RemoveAt(ranNum);
        } while (MasterActionDeckRef.Count > 0);
    }

    public void GenerateEventDeck()
    {
        do
        {
            //Generate random number to choose a card from master queue
            int ranNum = Random.Range(0, MasterEventDeckRef.Count);
            //Add chosen card to queue of action deck that player draws from
            CurrentEventDeck.Enqueue(MasterEventDeckRef[ranNum]);
            //Remove added card from master queue to avoid duplicates
            MasterEventDeckRef.RemoveAt(ranNum);
        } while (MasterEventDeckRef.Count > 0);
    }
}
