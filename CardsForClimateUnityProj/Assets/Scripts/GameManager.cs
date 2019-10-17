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

    //Separate reference decks for super events
    public List<EventCard> MasterSuperBadEventDeckRef = new List<EventCard>();
    public List<EventCard> MasterSuperGoodEventDeckRef = new List<EventCard>();

    //Current cards in player's hand and previously played cards
    public List<ActionCard> PlayerHand = new List<ActionCard>();
    public List<Card> PlayedCards = new List<Card>();

    //Keeps track of what cards come next from the decks, receives data from master lists
    public List<EventCard> CurrentEventDeck = new List<EventCard>();
    public List<ActionCard> CurrentActionDeck = new List<ActionCard>();

    //Dictionary for possible catastrophe cards
    public Dictionary<string, SuperCard> SuperEventCards = new Dictionary<string, SuperCard>();
    //List of positive event cards
    public List<EventCard> PositiveCards = new List<EventCard>();

    //Active event card
    public EventCard activeEventCard;
    //Active player cards
    public ActionCard[] activePlayerCards = new ActionCard[3];
    public int activePlayerCardCount = 0;

    //negative hope counter
    public int negativeHope = 0;
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

        //draw five cards
        for(int i = 0; i < 5; i++)
        {
            DrawCard();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BeginTurn()
    {
        //Pull an event card and remove it from the deck
        activeEventCard = CurrentEventDeck[0];
        CurrentEventDeck.RemoveAt(0);

        //Update Money and Carbon
        Money -= activeEventCard.costMoney;
        Carbon -= activeEventCard.costCarbon;

        //Update Hope stat
        negativeHope -= activeEventCard.negativeHope;
    }

    public void EndTurn()
    {
        //Check if the card has a super event card
        if(SuperEventCards[activeEventCard.cardName])
        {
            //Increment the cards supercastrophe potential
            SuperEventCards[activeEventCard.cardName].IncrementCard();
        }

        
        //Update based on player cards
        for (int i = 0; i < activePlayerCardCount; i++)
        {
            //update money
            Money -= activePlayerCards[i].costMoney;
            //update carbon cost
            Carbon -= activePlayerCards[i].costCarbon;
            //update hope card
            negativeHope += activePlayerCards[i].hope;
        }

        //Check if hope is negative
        if(negativeHope > 0)
        {
            negativeHope = 0;
        }
        //check momentum for super positive event
        if(activePlayerCardCount >= 3)
        {
            //Insert positive event
            CurrentEventDeck.Insert(Random.Range(0, CurrentEventDeck.Count), PositiveCards[Random.Range(0, PositiveCards.Count)]);
        }

        //Clear activeplayercards and reset counter
        for(int j = 0; j < activePlayerCards.Length; j++)
        {
            activePlayerCards[j] = null;
        }
        //reset counter
        activePlayerCardCount = 0;

    }

    public void DrawCard()
    {
        //Draw a card and add it to the player hand
        PlayerHand.Add(CurrentActionDeck[0]);
        CurrentActionDeck.RemoveAt(0);
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
            CurrentActionDeck.Add(MasterActionDeckRef[ranNum]);
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
            CurrentEventDeck.Add(MasterEventDeckRef[ranNum]);
            //Remove added card from master queue to avoid duplicates
            MasterEventDeckRef.RemoveAt(ranNum);
        } while (MasterEventDeckRef.Count > 0);
    }
}
