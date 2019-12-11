using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionCard : Card
{
    public GameManager gm;


    // Start is called before the first frame update
    void Start()
    {
        //Instantiate new ActionCard/EventCard object when Draw() method is called, use getComponent to fill in individual card data fields
        this.getComponent(TextMesh).Text = gm.CurrentActionDeck[0].cardName;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
