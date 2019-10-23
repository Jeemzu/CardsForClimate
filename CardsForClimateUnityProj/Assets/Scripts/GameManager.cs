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
    //Current player card
    public ActionCard currentCard;
    public int currentCardIndex = 0;

    //Boolean for if turn is finished
    bool turnActive = false;
    bool hopeValid = false;
    bool playingCard = false;

    //negative hope counter
    public int negativeHope = 0;
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

        DrawCard();
        /*
        for(int j = 0; j < MasterActionDeckRef.Count; j++)
        {
            Debug.Log(MasterActionDeckRef[j].cardName + ":" + MasterActionDeckRef[j].cardDesc + ":" + MasterActionDeckRef[j].costCarbon + ":" + MasterActionDeckRef[j].hope + ":" + MasterActionDeckRef[j].momentum + ":" + MasterActionDeckRef[j].costMoney);
        }
        Debug.Log("_________________________________");

        for (int k = 0; k < MasterEventDeckRef.Count; k++)
        {
            Debug.Log(MasterEventDeckRef[k].cardName + ":" + MasterEventDeckRef[k].cardDesc + ":" + MasterEventDeckRef[k].costCarbon + ":" + MasterEventDeckRef[k].hope + ":" + MasterEventDeckRef[k].momentum + ":" + MasterEventDeckRef[k].costMoney);
        }

        Debug.Log("_________________________________");*/
        
       /* for (int i = 0; i < CurrentActionDeck.Count; i++)
        {
            Debug.Log(CurrentActionDeck[i].cardName + ":" + CurrentActionDeck[i].cardDesc + ":" + CurrentActionDeck[i].costCarbon + ":" + CurrentActionDeck[i].hope + ":" + CurrentActionDeck[i].momentum + ":" + CurrentActionDeck[i].costMoney);
        }

        Debug.Log("_________________________________");

        for (int h = 0; h < CurrentEventDeck.Count; h++)
        {
            Debug.Log(CurrentEventDeck[h].cardName + ":" + CurrentEventDeck[h].cardDesc + ":" + CurrentEventDeck[h].costCarbon + ":" + CurrentEventDeck[h].hope + ":" + CurrentEventDeck[h].momentum + ":" + CurrentEventDeck[h].costMoney);
        }*/
    }

    // Update is called once per frame
    void Update()
    {
        //Testing key for turn beginning
        if (Input.GetKeyDown(KeyCode.P))
        {
            //Call begin turn to intialize event card
            BeginTurn();
            
            
            
        }
        PlayCard();
    }

    public void PlayCard()
    {
        if (turnActive)
        {
            if (!hopeValid) {
                //Check to see if the player hit any of the keys to play cards
                if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Alpha4) || Input.GetKeyDown(KeyCode.Alpha5))
                {
                    //Check which key is pressed to determine the card number played
                    if (Input.GetKeyDown(KeyCode.Alpha1))
                    {
                        currentCard = PlayerHand[0];
                        currentCardIndex = 0;
                    }
                    else if (Input.GetKeyDown(KeyCode.Alpha2))
                    {
                        currentCard = PlayerHand[1];
                        currentCardIndex = 1;
                    }
                    else if (Input.GetKeyDown(KeyCode.Alpha3))
                    {
                        currentCard = PlayerHand[2];
                        currentCardIndex = 2;

                    }
                    else if (Input.GetKeyDown(KeyCode.Alpha4))
                    {
                        //If the card number has decreased then check if there are enough cards for this key to be valid
                        if (PlayerHand.Count > 3)
                        {
                            currentCard = PlayerHand[3];
                            currentCardIndex = 3;
                        }
                        else
                        {
                            Debug.Log("No card is this slot, try again");
                            Debug.Log("_________________________________");
                            Debug.Log("Player Hand");
                            for (int i = 0; i < PlayerHand.Count; i++)
                            {
                                Debug.Log("Card Number: " + (i + 1) + "Name: " + PlayerHand[i].cardName + " Description: " + PlayerHand[i].cardDesc + " Money: " + PlayerHand[i].costMoney + " CO2:" + PlayerHand[i].costCarbon + " Hope:" + PlayerHand[i].hope + " Momentum:" + PlayerHand[i].momentum);

                            }
                        }


                    }
                    else if (Input.GetKeyDown(KeyCode.Alpha5))
                    {
                        //If the card number has decreased then check if there are enough cards for this key to be valid
                        if (PlayerHand.Count > 4)
                        {
                            currentCard = PlayerHand[4];
                            currentCardIndex = 4;
                        }
                        else
                        {
                            Debug.Log("No card is this slot, try again");
                            Debug.Log("_________________________________");
                            Debug.Log("Player Hand");
                            for (int i = 0; i < PlayerHand.Count; i++)
                            {
                                Debug.Log("Card Number: " + (i + 1) + "Name: " + PlayerHand[i].cardName + " Description: " + PlayerHand[i].cardDesc + " Money: " + PlayerHand[i].costMoney + " CO2:" + PlayerHand[i].costCarbon + " Hope:" + PlayerHand[i].hope + " Momentum:" + PlayerHand[i].momentum);

                            }
                        }
                    }
                    ValidHopeCard(currentCard);

                    //Check to see if the player hit the key to pay money
                }
                else if (Input.GetKeyDown(KeyCode.Alpha6))
                {
                    //Check if the player has enough money
                    if (Money > 5)
                    {
                        hopeValid = true;
                    }
                    else
                    {
                        Debug.Log("You do not have enough money to skip");
                    }
                }
            }
            //If the card played is valid based on hope then continue with turn
            if (hopeValid)
            {
                //Add card to player's active cards
                Debug.Log("Card played Name: " + currentCard.cardName + " Description: " + currentCard.cardDesc + " Money: " + currentCard.costMoney + " CO2:" + currentCard.costCarbon + " Hope:" + currentCard.hope + " Momentum:" + currentCard.momentum);
                activePlayerCards[activePlayerCardCount] = currentCard;
                activePlayerCardCount++;
                hopeValid = false;
                PlayerHand.RemoveAt(currentCardIndex);
                //Check if the card has momentum
                if (currentCard.momentum == 0)
                {
                    turnActive = false;
                    Debug.Log("Turn Ended");
                    EndTurn();
                }
            }
        }
                
             

        
    }

    public void BeginTurn()
    {
        //Temp testing info
        Debug.Log("Game Stats: Money: " + Money + " | C02: " + Carbon + " | Hope : " + negativeHope);

        //Pull an event card and remove it from the deck
        activeEventCard = CurrentEventDeck[0];
        CurrentEventDeck.RemoveAt(0);

        //Display Card info
        Debug.Log("Card Event: " + activeEventCard.cardName);
        Debug.Log("Card Description: " + activeEventCard.cardDesc);
        Debug.Log("Card Stats Money: " + activeEventCard.costMoney + " | CO2: " + activeEventCard.costCarbon + " | Hope: " + activeEventCard.hope);
        //Update Money and Carbon
        Money -= activeEventCard.costMoney;
        Carbon -= activeEventCard.costCarbon;

        //Update Hope stat
        negativeHope -= activeEventCard.negativeHope;

        Debug.Log("Game Stats: Money: " + Money + " | C02: " + Carbon + " | Hope : " + negativeHope);
        turnActive = true;
        Debug.Log("A positive hope card must be played, that card is not valid try again");
        Debug.Log("_________________________________");
        Debug.Log("Player Hand");
        for (int i = 0; i < PlayerHand.Count; i++)
        {
            Debug.Log("Card Number: " + (i + 1) + "Name: " + PlayerHand[i].cardName + " Description: " + PlayerHand[i].cardDesc + " Money: " + PlayerHand[i].costMoney + " CO2:" + PlayerHand[i].costCarbon + " Hope:" + PlayerHand[i].hope + " Momentum:" + PlayerHand[i].momentum);

        }
    }

    public void EndTurn()
    {
        //Check if the card has a super event card
        if(SuperEventCards.ContainsKey(activeEventCard.cardName))
        {
            //Increment the cards supercastrophe potential
            SuperEventCards[activeEventCard.cardName].IncrementCard();
        }

        
        //Update based on player cards
        for (int i = 0; i < activePlayerCardCount; i++)
        {
            //update money
            Money += activePlayerCards[i].costMoney;
            //update carbon cost
            Carbon += activePlayerCards[i].costCarbon;
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

        //Replenish player hand
        DrawCard();

        Debug.Log("Card Event: " + activeEventCard.cardName);
        Debug.Log("Card Description: " + activeEventCard.cardDesc);
        Debug.Log("Card Stats Money: " + activeEventCard.costMoney + " | CO2: " + activeEventCard.costCarbon + " | Hope: " + activeEventCard.hope);
    }

    public void DrawCard()
    {
        //Draw cards until player hand is full
        do
        {
            //Draw a card and add it to the player hand
            PlayerHand.Add(CurrentActionDeck[0]);
            CurrentActionDeck.RemoveAt(0);
        } while (PlayerHand.Count < 5);
        
    }

    public void ValidHopeCard(ActionCard playedCard)
    {
        if(((negativeHope < 0) && playedCard.hope > 0 || playedCard.hope < 0) || negativeHope == 0 )
        {
            hopeValid = true;
        } else
        {
            Debug.Log("A positive hope card must be played, that card is not valid try again");
            Debug.Log("_________________________________");
            Debug.Log("Player Hand");
            for (int i = 0; i < PlayerHand.Count; i++)
            {
                Debug.Log("Card Number: " + (i + 1) + "Name: " + PlayerHand[i].cardName + " Description: " + PlayerHand[i].cardDesc + " Money: " + PlayerHand[i].costMoney + " CO2:" + PlayerHand[i].costCarbon + " Hope:" + PlayerHand[i].hope + " Momentum:" + PlayerHand[i].momentum);

            }
        }
    }

    public void MoneyCarbonTick()
    {

    }

    public void FillMasterActionDeck()
    {
        //Create text asset for the csv file
        TextAsset cardData = Resources.Load<TextAsset>("ActionCards");

        //split the cards into different areas based on lines
        string[] data = cardData.text.Split(new char[] { '\n' });

        //Loop through card data
        for(int i = 1; i < data.Length; i++)
        {
            //split the whole data array into indiviual columns from the row
            string[] row = data[i].Split(new char[] { '|' });
            //Temp action card for creation
            ActionCard aCard = new ActionCard();
            int.TryParse(row[0], out aCard.cardNumber);
            aCard.cardName = row[1];
            aCard.cardDesc = row[2];
            int.TryParse(row[3], out aCard.costCarbon);
            int.TryParse(row[4], out aCard.hope);
            int.TryParse(row[5], out aCard.momentum);
            int.TryParse(row[6], out aCard.costMoney);
            MasterActionDeckRef.Add(aCard);
        }
    }

    public void FillMasterEventDeck()
    {
        //Create text asset for the csv file
        TextAsset cardData = Resources.Load<TextAsset>("EventCards");

        //split the cards into different areas based on lines
        string[] data = cardData.text.Split(new char[] { '\n' });

        //Loop through card data
        for (int i = 1; i < data.Length; i++)
        {
            //split the whole data array into indiviual columns from the row
            string[] row = data[i].Split(new char[] { '|' });
            //Temp action card for creation
            EventCard eCard = new EventCard();
            int.TryParse(row[0], out eCard.cardNumber);
            eCard.cardName = row[1];
            eCard.cardDesc = row[2];
            int.TryParse(row[3], out eCard.costCarbon);
            int.TryParse(row[4], out eCard.hope);
            int.TryParse(row[5], out eCard.momentum);
            int.TryParse(row[6], out eCard.costMoney);
            MasterEventDeckRef.Add(eCard);
        }
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
