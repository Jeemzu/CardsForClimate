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
    //Boolean for if hope requirements met
    bool hopeValid = true;
    //Boolean for if momentum is active
    bool hasMomemtum = false;
    //Boolean for if slot is a valid slot
    bool validPos = true;
    //Boolean for game lost status
    bool gameOver = false;

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

        Debug.Log("Cards For Climate!");
        Debug.Log("Press P to start your turn and advance further");
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
        if (!gameOver && Input.GetKeyDown(KeyCode.P))
        {
            //Call begin turn to intialize event card
            BeginTurn();
            
            
            
        }
        PlayCard();
    }

    public void BeginTurn()
    {
        //Turn has begun
        Debug.Log("Turn has begun");

        //Temp testing info
        Debug.Log("Global Stats: Money: " + Money + " | C02: " + Carbon + " | Hope : " + negativeHope);

        //Pull an event card and remove it from the deck
        activeEventCard = CurrentEventDeck[0];
        CurrentEventDeck.RemoveAt(0);

        //Display Card info
        Debug.Log("Card Event: " + activeEventCard.cardName);
        Debug.Log("Card Description: " + activeEventCard.cardDesc);
        Debug.Log("Card Stats Money: " + activeEventCard.costMoney + " | CO2: " + activeEventCard.costCarbon + " | Hope: " + activeEventCard.hope);
        //Update Money, Carbon and Hope
        Money += activeEventCard.costMoney;
        Carbon += activeEventCard.costCarbon;
        negativeHope += activeEventCard.hope;
        //Hope cannot be greater than 1
        if(negativeHope > 0)
        {
            negativeHope = 0;
        }
        //Display updated stats
        Debug.Log("Global Stats: Money: " + Money + " | C02: " + Carbon + " | Hope : " + negativeHope);
        //Check if hope has been affected
        if (negativeHope < 0)
        {
            Debug.Log("A positive hope card must be played");
            hopeValid = false;
        }
        turnActive = true;

        Debug.Log("_________________________________");
        Debug.Log("Player Hand");
        for (int i = 0; i < PlayerHand.Count; i++)
        {
            Debug.Log("Card Number: " + (i + 1) + "Name: " + PlayerHand[i].cardName + " Description: " + PlayerHand[i].cardDesc + " Money: " + PlayerHand[i].costMoney + " CO2:" + PlayerHand[i].costCarbon + " Hope:" + PlayerHand[i].hope + " Momentum:" + PlayerHand[i].momentum);

        }
    }

    public void PlayCard()
    {
        if (turnActive)
        {
            if (activePlayerCardCount == 0 || (activePlayerCardCount < 3 && hasMomemtum)) {
                //Check to see if the player hit any of the keys to play cards
                if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Alpha4) || Input.GetKeyDown(KeyCode.Alpha5))
                {
                    //Check which key is pressed to determine the card number played
                    if (Input.GetKeyDown(KeyCode.Alpha1))
                    {
                        currentCard = PlayerHand[0];
                        currentCardIndex = 0;
                        validPos = true;
                    }
                    else if (Input.GetKeyDown(KeyCode.Alpha2))
                    {
                        currentCard = PlayerHand[1];
                        currentCardIndex = 1;
                        validPos = true;
                    }
                    else if (Input.GetKeyDown(KeyCode.Alpha3))
                    {
                        currentCard = PlayerHand[2];
                        currentCardIndex = 2;
                        validPos = true;

                    }
                    else if (Input.GetKeyDown(KeyCode.Alpha4))
                    {
                        //If the card number has decreased then check if there are enough cards for this key to be valid
                        if (PlayerHand.Count > 3)
                        {
                            currentCard = PlayerHand[3];
                            currentCardIndex = 3;
                            validPos = true;
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
                            //This position is not valid
                            validPos = false;
                        }


                    }
                    else if (Input.GetKeyDown(KeyCode.Alpha5))
                    {
                        Debug.Log("Pressed 5");
                        //If the card number has decreased then check if there are enough cards for this key to be valid
                        if (PlayerHand.Count > 4)
                        {
                            currentCard = PlayerHand[4];
                            currentCardIndex = 4;
                            validPos = true;
                        }
                        else
                        {
                            //No card is in this position of the hand display a message and redisplay hand
                            Debug.Log("No card is this slot, try again");
                            Debug.Log("_________________________________");
                            Debug.Log("Player Hand");
                            for (int i = 0; i < PlayerHand.Count; i++)
                            {
                                Debug.Log("Card Number: " + (i + 1) + "Name: " + PlayerHand[i].cardName + " Description: " + PlayerHand[i].cardDesc + " Money: " + PlayerHand[i].costMoney + " CO2:" + PlayerHand[i].costCarbon + " Hope:" + PlayerHand[i].hope + " Momentum:" + PlayerHand[i].momentum);

                            }
                            //This position is not valid
                            validPos = false;
                        }
                    }
                    //If this turn has a negative hope event card then it will check if the card played has a valid hope value to be played and if the position is a valid one to check
                    if (validPos && !hopeValid)
                    {
                        ValidHopeCard(currentCard);
                    }

                    //Either the card has passed the appropriate hope checks to be played or the player currently has momentum then check what card was played and if the position is a valid one to check
                    if (validPos && (hopeValid || hasMomemtum))
                    {
                        //Display the name of the card the plyaer has played
                        Debug.Log("Card played Name: " + currentCard.cardName + " Description: " + currentCard.cardDesc + " Money: " + currentCard.costMoney + " CO2:" + currentCard.costCarbon + " Hope:" + currentCard.hope + " Momentum:" + currentCard.momentum);
                        //Add that card to the activePlayedCards array for stat checking after turn completion
                        activePlayerCards[activePlayerCardCount] = currentCard;
                        //Increment the activePlayerCardCount
                        activePlayerCardCount++;
                        PlayerHand.RemoveAt(currentCardIndex);

                        Debug.Log("Playerhand count = " + PlayerHand.Count);

                        //Check if the card has momentum
                        if (currentCard.momentum == 0 || activePlayerCardCount == 3)
                        {
                            //if the card does not have momentum or if the max number of allowed cards to be played is reached then end the turn
                            turnActive = false;
                            EndTurn();
                        } else
                        {
                            //Update has momentum bool
                            hasMomemtum = true;
                            //Display updated hand and message
                            Debug.Log("Card played has momentum, play another card!");
                            Debug.Log("_________________________________");
                            Debug.Log("Player Hand");
                            for (int i = 0; i < PlayerHand.Count; i++)
                            {
                                Debug.Log("Card Number: " + (i + 1) + "Name: " + PlayerHand[i].cardName + " Description: " + PlayerHand[i].cardDesc + " Money: " + PlayerHand[i].costMoney + " CO2:" + PlayerHand[i].costCarbon + " Hope:" + PlayerHand[i].hope + " Momentum:" + PlayerHand[i].momentum);

                            }
                        }
                    }
                }
            }            
        }                      
    }

    public void EndTurn()
    {
        //Display that the turn has ended
        Debug.Log("Turn Ended");
        
        //Check if the card has a super event card
        if (SuperEventCards.ContainsKey(activeEventCard.cardName))
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
            //Awaiting super positive cards to test feature
            //Insert positive event
            //CurrentEventDeck.Insert(Random.Range(0, CurrentEventDeck.Count), PositiveCards[Random.Range(0, PositiveCards.Count)]);
        }

        //Clear activeplayercards and reset counter
        for(int j = 0; j < activePlayerCards.Length; j++)
        {
            activePlayerCards[j] = null;
        }
        //reset counter
        activePlayerCardCount = 0;
        //reset values
        turnActive = false;
        hopeValid = true;
        hasMomemtum = false;
        validPos = true;

        //Temporarily display stats
        Debug.Log("Global Stats: Money: " + Money + " | C02: " + Carbon + " | Hope : " + negativeHope);

        //reset currentCard values
        currentCard = null;
        currentCardIndex = 0;
        //Replenish player hand
        DrawCard();

        //Check the game loss conditions
        GameEnd();

        /*Debug.Log("Card Event: " + activeEventCard.cardName);
        Debug.Log("Card Description: " + activeEventCard.cardDesc);
        Debug.Log("Card Stats Money: " + activeEventCard.costMoney + " | CO2: " + activeEventCard.costCarbon + " | Hope: " + activeEventCard.hope);*/
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
        //Checks if the played card has either a positive or negative hope value that would allow it to be played
        if((playedCard.hope > 0 || playedCard.hope < 0))
        {
            //If valid change the boolean
            hopeValid = true;
        } else
        {
            //Else if the card played does not meet the requirements then display an indicating message
            Debug.Log("A positive hope card must be played, that card is not valid try again");

            /*Debug.Log("_________________________________");
            Debug.Log("Player Hand");
            for (int i = 0; i < PlayerHand.Count; i++)
            {
                Debug.Log("Card Number: " + (i + 1) + "Name: " + PlayerHand[i].cardName + " Description: " + PlayerHand[i].cardDesc + " Money: " + PlayerHand[i].costMoney + " CO2:" + PlayerHand[i].costCarbon + " Hope:" + PlayerHand[i].hope + " Momentum:" + PlayerHand[i].momentum);

            }*/
        }
    }

    //Method for ending the game
    public void GameEnd()
    {
        //Check the loss conditions for the game
        //If Carbon reaches 30 then gameover
        //if money reaches 0 then gameover
        //if hope reaches -3 then gameover
        if(Carbon >= 30 || Money <= 0 || negativeHope <= -3)
        {
            //Change status of gameover
            gameOver = true;

            //Display message based on loss condition
            if(Carbon >= 30)
            {
                Debug.Log("Carbon levels are too high now! Game Over");
            } else if (Money <= 0)
            {
                Debug.Log("We have run out of money for further action! Game Over");
            } else if(negativeHope <= -3)
            {
                Debug.Log("People have lost too much to continue! Game Over");
            }
        //Check game win conditions
        } else if(Carbon <= 0)
        {
            Debug.Log("The Planet is Saved! We have begun reducing carbon emmisions and are track to help the world! You Win!");
        }
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
