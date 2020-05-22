using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// Displays information about action cards in UI form.
/// </summary>
public class ActionCardDisplay : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    /// <summary>
    /// The ActionCard data object that underpins all this card displayer's information.
    /// </summary>
    public ActionCard MyCard { get; private set; }

    [Header("Information Slots and Icons")]
    public GameObject Title;
    public GameObject Description;
    public GameObject Money;
    public GameObject MoneyIcon;
    public GameObject CarbonIcon;
    public GameObject Carbon;
    public GameObject HopeIcon;
    public GameObject MomentumIcon;
    public GameObject CardArt;

    [Header("Other")]
    public int DisplayCardIndex;

    /// <summary>
    /// Used to calculate where the card should be based on where the mouse is dragging it from
    /// </summary>
    private Vector3 mouseDiff;

    /// <summary>
    /// This card displayer's default screen position in the player's hand
    /// </summary>
    private Vector3 restingPos;

    private void Start()
    {
        restingPos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
    }

    /// <summary>
    /// Fills in the card information and turns on the proper icons on the card displayer.
    /// </summary>
    /// <param name="thisCard"></param>
    public void SetCardAndDisplay(ActionCard thisCard)
    {
        MyCard = thisCard;
        Title.GetComponent<TextMeshProUGUI>().text = thisCard.cardName;
        Description.GetComponent<TextMeshProUGUI>().text = thisCard.cardDesc;
        Money.GetComponent<TextMeshProUGUI>().text =
                                                (thisCard.costMoney > 0 ? "+" : "") +  thisCard.costMoney.ToString();
        Carbon.GetComponent<TextMeshProUGUI>().text = 
                                                (thisCard.costCarbon > 0 ? "+" : "") + thisCard.costCarbon.ToString();
        CardArt.GetComponent<Image>().sprite = thisCard.cardImage;
        MomentumIcon.SetActive(thisCard.momentum > 0);
        HopeIcon.SetActive(thisCard.hope > 0);
    }

    /// <summary>
    /// Turns on/off the money and carbon icons when the card focus is switched.
    /// </summary>
    public void ToggleMoneyAndCarbonDisplays(bool on)
    {
        MoneyIcon.SetActive(on);
        CarbonIcon.SetActive(on);
    }

    /// <summary>
    /// Implemented by the IBeginDragHandler interface. Called when the user begins dragging the card with the mouse.
    /// </summary>
    public void OnBeginDrag(PointerEventData eventData)
    {
        mouseDiff = 
            new Vector3(Input.mousePosition.x - transform.position.x, Input.mousePosition.y - transform.position.y, 0);
    }

    /// <summary>
    /// Implemented by the IDragHandler interface. Called each frame the user is dragging the card with the mouse.
    /// </summary>
    public void OnDrag(PointerEventData eventData)
    {
        if (DisplayCardIndex == HandManager.Instance.ActiveCardIndex)
        {
            transform.position = Input.mousePosition - mouseDiff;
        }
    }

    /// <summary>
    /// Implemented by the IEndDragHandler interface. Called when the user stops dragging the card with the mouse.
    /// </summary>
    public void OnEndDrag(PointerEventData eventData)
    {
        int currentTurn = GameManager.Instance.CurrentTurnNumber;
        // put card back in its hand position
        transform.position = restingPos;

        PointerEventData pointerData = new PointerEventData(EventSystem.current);
        pointerData.position = Input.mousePosition;
        // Test to see if the user dropped the card in the card play square - if so, the user wants to play this card.
        // We test by casting a ray on all UI objects under the mouse position, and seeing if the ray hits the square.
        List<RaycastResult> raycastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, raycastResults);
        for (int i = 0; i < raycastResults.Count; i++)
        {
            if (raycastResults[i].gameObject.CompareTag("Card Play Square"))
            {
                // First, we make sure that this card is valid to be played
                if (GameManager.Instance.PlayerCardsHope() && GameManager.Instance.ValidCard(MyCard))
                {
                    // If valid, we add the card to the card catcher square's collection, move focus to a different
                    // adjacent card, and tell the GameManager to play this card.
                    CardCatcher.Instance.CatchCard(this);
                    GameManager.Instance.UseCardByUI(MyCard);
                    HandManager.Instance.ScrollRandom();
                    // only turn the card off if a new turn hasn't been started, triggered by the last card; 
                    // otherwise this will cause issues with the hand refilling between turns.
                    if (currentTurn == GameManager.Instance.CurrentTurnNumber)
                    {
                        gameObject.SetActive(false);
                    }
                }
                break;
            }
        }
    }
}
