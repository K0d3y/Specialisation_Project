using Photon.Pun;
using Photon.Pun.Demo.Asteroids;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // containers
    [SerializeField] public DeckContainer deck;
    [SerializeField] private DiscardContainer discard;
    [SerializeField] private HandContainer hand;
    [SerializeField] private ResilienceContainer resilience;
    [SerializeField] private List<PlayingAreaContainer> playingAreas;
    // click detection
    [SerializeField] private LayerMask cardLayerMask;
    [SerializeField] private float clickDetectionTime;
    private float clickTime;
    // preview & drag cards
    private CardPromptController cardPromptController;
    [SerializeField] private GameObject cardHolder;
    public GameObject heldCard;
    // player values
    public bool isMyTurn = false;
    public int manaCount = 0;
    public int maxManaCount = 0;
    // others
    private bool discardingCard = false;
    public bool isPromoting = false;
    public bool isCLickToPreview = false;
    public bool isAttacking = false;
    // audio
    private AudioManager audioManager;
    // game end stuff
    [SerializeField] private GameObject gameEndPanel;
    private bool isGameEnd = false;
    // photon view
    public PhotonView view;

    private void Start()
    {
        view = GetComponent<PhotonView>();
        cardPromptController = GameObject.FindGameObjectWithTag("PromptController").GetComponent<CardPromptController>();
        audioManager = cardPromptController.audioManager;

        isMyTurn = PhotonNetwork.IsMasterClient;
        if (!view.IsMine)
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
            transform.position = new Vector3(0, 0, -10);
            cardPromptController.Init();
            GameplayManager.Instance.StartGame();
        }
    }

    private void Update()
    {
        if (view.IsMine && !isGameEnd)
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

                HandleActionInput(hit);
                HandlePreviewInput(hit);
                if (isMyTurn)
                {
                    HandleHoldInput(hit);
                    HandleClickInput(hit);
                }
            }
            else if (!isCLickToPreview && cardPromptController.previewCard != null)
            {
                cardPromptController.HideCardPrompts();
            }
        }
    }

    private void HandlePreviewInput(RaycastHit hit)
    {
        // click (do click code here)
        if (Input.GetMouseButtonUp(0) && clickTime > 0) // left click
        {
            if (hit.collider.gameObject.transform.parent.parent.name == "Hand" &&
            hit.collider.gameObject.transform.parent.parent.parent.parent.GetComponent<PlayerController>() == this &&
            isMyTurn) // click on hand
            {
                // discarding card from hand
                if (discardingCard)
                {
                    discard.AddCardToTop(hand.TakeCard(hit.collider.transform.parent.gameObject));
                    discardingCard = false;
                    cardPromptController.HideCardPrompts();
                }
                // preview card & ask if the player wants to play it
                else
                {
                    cardPromptController.ShowPlayCardPreview(hit, isMyTurn);
                    isCLickToPreview = true;
                }
            }
        }
        if (hit.collider.gameObject.transform.parent.parent.name == "Hand" &&
            hit.collider.gameObject.transform.parent.parent.parent.parent.GetComponent<PlayerController>() == this &&
            !isCLickToPreview)
        {
            if (!isPromoting && !discardingCard && !isAttacking)
            {
                cardPromptController.ShowPlayCardPreview(hit, isMyTurn);
            }
        }
    }
    private void HandleActionInput(RaycastHit hit)
    {
        // click (do click code here)
        if (Input.GetMouseButtonUp(0) && clickTime > 0) // left click
        {
            if (hit.collider.gameObject.transform.parent.parent.name.Contains("Space") && isMyTurn) // click on playind area
            {
                // show attack prompt
                if (!isPromoting && !discardingCard && !isAttacking)
                {
                    cardPromptController.ShowCardActions(hit, isMyTurn);
                    isCLickToPreview = true;
                }
            }
        }
        if (hit.collider.gameObject.transform.parent.parent.name.Contains("Space") &&
            !isCLickToPreview)
        {
            if (!isPromoting && !discardingCard && !isAttacking)
            {
                cardPromptController.ShowCardActions(hit, isMyTurn);
            }
        }
    }
    private void HandleClickInput(RaycastHit hit)
    {
        // click (do click code here)
        if (Input.GetMouseButtonUp(0) && clickTime > 0) // left click
        {
            isPromoting = false;
            isAttacking = false;
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
            //if (hit.collider.gameObject.transform.parent.parent.name == "Hand") // click on hand
            //{
            //    if (heldCard == null) // hold card
            //    {
            //        heldCard = hand.TakeCard(hit.collider.gameObject.transform.parent.gameObject);
            //        heldCard.transform.SetParent(cardHolder.transform);
            //        heldCard.transform.localPosition = Vector3.zero;
            //    }
            //}
        }
        // stop hold (undo any hold code here)
        if (Input.GetMouseButtonUp(0) && clickTime <= 0) // left click
        {
            //if (heldCard != null) // release card
            //{
            //    hand.AddCardToTop(heldCard);

            //    heldCard = null;
            //}
        }
    }

    public void DrawFromDeck(int noOfTimes, string location)
    {
        audioManager.PlaySound(1);
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
        for (int i = 0; i < hand.cardList.Count; i++)
        {
            if (!view.IsMine && !hand.cardList[i].GetComponentInChildren<Card>().isFlippedDown)
            {
                hand.cardList[i].GetComponentInChildren<Card>().FlipCard();
            }
        }
    }

    public void DrawResilienceToHand()
    {
        audioManager.PlaySound(1);
        hand.AddCardToTop(resilience.TakeTopCard());
        for (int i = 0; i < hand.cardList.Count; i++)
        {
            if (!view.IsMine && !hand.cardList[i].GetComponentInChildren<Card>().isFlippedDown)
            {
                hand.cardList[i].GetComponentInChildren<Card>().FlipCard();
            }
        }
        if (resilience.cardList.Count <= 0)
        {
            // do game end code
            view.RPC("DoGameEnd", RpcTarget.All);
        }
    }

    public void DiscardCardFromHand()
    {
        audioManager.PlaySound(1);
        discardingCard = true;
        cardPromptController.ShowHandGroup();
    }

    public void SendToDiscard(int playingAreaNo)
    {
        audioManager.PlaySound(1);
        if (playingAreaNo >= 6)
        {
            playingAreaNo -= 6;
        }
        while (playingAreas[playingAreaNo].cardList.Count > 0)
        {
            discard.AddCardToTop(playingAreas[playingAreaNo].TakeTopCard());
        }
    }

    public void PlayLeaderCard()
    {
        playingAreas[0].AddCardToBottom(deck.leader);
    }

    [PunRPC]
    public void ShuffleDeck(int s1, int s2)
    {
        GameplayManager.Instance.ShuffleDeck(s1, s2);
    }

    [PunRPC]
    public void StartTurn()
    {
        GameplayManager.Instance.DoStartTurn();
    }

    [PunRPC]
    public void PlayCard(int i, int x, bool isSelf)
    {
        cardPromptController.PlayCard(i, x, isSelf);
    }

    [PunRPC]
    public void AttackCard(int attackingSpace, int attackedSpace, bool isSelf)
    {
        cardPromptController.AttackCard(attackingSpace, attackedSpace, isSelf);
    }

    [PunRPC]
    public void TakeResilience(int i)
    {
        cardPromptController.TakeResilience(i);
    }

    [PunRPC]
    public void DoGameEnd()
    {
        isGameEnd = true;
        if (resilience.cardList.Count <= 0)
        {
            // change text if win/loss
            gameEndPanel.GetComponentInChildren<EndGamePanelManager>().UpdateTitleText("YOU WIN !");
        }
        gameEndPanel.SetActive(true);
    }
}
