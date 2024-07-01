using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // containers
    [SerializeField] private DeckContainer deck;
    [SerializeField] private DiscardContainer discard;
    [SerializeField] private HandContainer hand;
    [SerializeField] private ResilienceContainer resilience;
    [SerializeField] private List<PlayingAreaContainer> playingAreas;
    // click detection
    [SerializeField] private LayerMask cardLayerMask;
    [SerializeField] private float clickDetectionTime;
    private float clickTime;
    // preview & drag cards
    [SerializeField] private CardPromptController cardPromptController;
    [SerializeField] private GameObject cardHolder;
    private GameObject heldCard;
    // player values
    public bool isMyTurn = false;
    public int manaCount = 0;
    public int maxManaCount = 0;
    [SerializeField] private TMP_Text manaCount_text;
    // others
    private bool discardingCard = false;
    public bool isPromoting = false;

    private void Update()
    {
        cardHolder.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        cardHolder.transform.localPosition = new Vector3(cardHolder.transform.localPosition.x, 0, cardHolder.transform.localPosition.z);

        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector3.down, out hit, 100, cardLayerMask))
        {
            // handle detection of click or hold
            clickTime -= Time.deltaTime;
            if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2))
            {
                clickTime = clickDetectionTime;
            }

            if (isMyTurn)
            {
                HandleHoldInput(hit);
                HandleClickInput(hit);
                HandleActionInput(hit);
            }
            HandlePreviewInput(hit);
        }
    }

    private void HandlePreviewInput(RaycastHit hit)
    {
        // click (do click code here)
        if (Input.GetMouseButtonUp(0) && clickTime > 0) // left click
        {
            if (hit.collider.gameObject.transform.parent.parent.name == "Hand") // click on hand
            {
                // discarding card from hand
                if (discardingCard)
                {
                    discard.AddCardToTop(hit.collider.transform.parent.gameObject);
                    discardingCard = false;
                }
                // preview card & ask if the player wants to play it
                else
                {
                    cardPromptController.ShowPlayCardPreview(hit, isMyTurn);
                }
            }
        }
    }
    private void HandleActionInput(RaycastHit hit)
    {
        // click (do click code here)
        if (Input.GetMouseButtonUp(0) && clickTime > 0) // left click
        {
            if (hit.collider.gameObject.transform.parent.parent.name.Contains("Space")) // click on playind area
            {
                // show attack prompt
                if (!isPromoting)
                {
                    cardPromptController.ShowCardActions(hit, isMyTurn);
                }
            }
        }
    }
    private void HandleClickInput(RaycastHit hit)
    {
        // click (do click code here)
        if (Input.GetMouseButtonUp(0) && clickTime > 0) // left click
        {
            isPromoting = false;
        }
        else if (Input.GetMouseButtonUp(1) && clickTime > 0) // right click
        {

        }
        else if (Input.GetMouseButtonUp(2) && clickTime > 0) // middle click
        {

        }
    }
    private void HandleHoldInput(RaycastHit hit)
    {
        // hold (do hold code here)
        if (Input.GetMouseButton(0) && clickTime <= 0) // left click
        {
            if (hit.collider.gameObject.transform.parent.parent.name == "Hand") // click on hand
            {
                if (heldCard == null) // hold card
                {
                    heldCard = hand.TakeCard(hit.collider.gameObject.transform.parent.gameObject);
                    heldCard.transform.SetParent(cardHolder.transform);
                    heldCard.transform.localPosition = Vector3.zero;
                }
            }
        }
        // stop hold (undo any hold code here)
        if (Input.GetMouseButtonUp(0) && clickTime <= 0) // left click
        {
            if (heldCard != null) // release card
            {
                hand.AddCardToTop(heldCard);

                heldCard = null;
            }
        }
    }

    public void DrawFromDeck(int noOfTimes, string location)
    {
        for (int i = 0; i < noOfTimes; i++)
        {
            switch(location)
            {
                case "HAND":
                    hand.AddCardToTop(deck.TakeTopCard());
                    break;
                case "RESILIENCE":
                    resilience.AddCardToTop(deck.TakeTopCard());
                    break;
                case "DISCARD":
                    discard.AddCardToTop(deck.TakeTopCard());
                    break;
            }
        }
    }

    public void DiscardCardFromHand()
    {
        discardingCard = true;
        cardPromptController.ShowHandGroup();
    }

    public void SendToDiscard(int playingAreaNo)
    {
        while (playingAreas[playingAreaNo].cardList.Count > 0)
        {
            discard.AddCardToTop(playingAreas[playingAreaNo].TakeTopCard());
        }
    }

    public void UpdateManaText()
    {
        manaCount_text.text = manaCount.ToString() + '/' + maxManaCount.ToString();
    }
}
