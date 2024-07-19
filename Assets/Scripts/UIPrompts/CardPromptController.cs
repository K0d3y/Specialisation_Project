using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;

public class CardPromptController : MonoBehaviour
{
    // UI panels
    [SerializeField] private GameObject turnOrderGroup;
    [SerializeField] public GameObject playCardPrompt;
    [SerializeField] public GameObject playCardAreasPrompt;
    [SerializeField] public GameObject cardActionGroup;
    [SerializeField] private GameObject attackTargetGroup;
    [SerializeField] private GameObject handTargetGroup;
    // for card preview
    public List<GameObject> player = new List<GameObject>();
    [SerializeField] private GameObject cardPrefab;
    public GameObject previewCard;
    private GameObject cardRef;
    private bool myTurn;
    // for card playing areas
    private List<HandContainer> playerHand = new List<HandContainer>();
    [SerializeField] private PlayingAreaButtonManager pabManager;
    [SerializeField] private TargetAreaButtonManager tabManager;
    private List<PlayingAreaContainer> playingAreas = new List<PlayingAreaContainer>();

    public void Init()
    {
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Player"))
        {
            player.Add(obj);
        }
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Hand"))
        {
            playerHand.Add(obj.GetComponent<HandContainer>());
        }
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("PlayingArea"))
        {
            playingAreas.Add(obj.GetComponent<PlayingAreaContainer>());
        }

        myTurn = player[0].GetComponent<PlayerController>().isMyTurn;
        UpdateTurnOrderButton();
    }

    public void ShowPlayCardPreview(RaycastHit hit, bool isMyTurn)
    {
        if (playCardAreasPrompt.activeSelf)
        {
            return;
        }
        if (previewCard != null)
        {
            HideCardPrompts();
        }
        turnOrderGroup.SetActive(false);
        myTurn = isMyTurn;
        cardRef = hit.collider.transform.parent.gameObject;
        previewCard = Instantiate(cardPrefab, player[0].transform);
        previewCard.name = hit.collider.transform.parent.gameObject.name;
        previewCard.GetComponentInChildren<Card>().cardData = hit.collider.GetComponent<Card>().cardData;
        previewCard.GetComponentInChildren<Card>().usebaseStats = false;
        previewCard.GetComponentInChildren<Card>().atk = hit.collider.GetComponent<Card>().atk;
        previewCard.GetComponentInChildren<Card>().def = hit.collider.GetComponent<Card>().def;
        previewCard.GetComponentInChildren<Card>().UpdateCardText();
        previewCard.transform.localPosition = new Vector3(10, 5, -5);
        previewCard.transform.localScale *= 5;
        if (isMyTurn)
        {
            playCardPrompt.SetActive(true);
        }
        player[0].GetComponent<PlayerController>().heldCard = cardRef;
    }

    public void ShowCardActions(RaycastHit hit, bool isMyTurn)
    {
        if (playCardAreasPrompt.activeSelf)
        {
            return;
        }
        if (previewCard != null)
        {
            HideCardPrompts();
        }
        turnOrderGroup.SetActive(false);
        myTurn = isMyTurn;
        cardRef = hit.collider.transform.parent.gameObject;
        previewCard = Instantiate(cardPrefab, player[0].transform);
        previewCard.name = hit.collider.transform.parent.gameObject.name;
        previewCard.GetComponentInChildren<Card>().cardData = hit.collider.GetComponent<Card>().cardData;
        previewCard.GetComponentInChildren<Card>().usebaseStats = false;
        previewCard.GetComponentInChildren<Card>().atk = hit.collider.GetComponent<Card>().atk;
        previewCard.GetComponentInChildren<Card>().def = hit.collider.GetComponent<Card>().def;
        previewCard.GetComponentInChildren<Card>().UpdateCardText();
        previewCard.transform.localPosition = new Vector3(10, 5, -5);
        previewCard.transform.localScale *= 5;
        if (isMyTurn)
        {
            for (int i = 0; i < 6; i++)
            {
                if (playingAreas[i].cardList.Count != 0 && playingAreas[i].cardList[0] == cardRef)
                {
                    cardActionGroup.SetActive(true);
                    break;
                }
            }
        }
        player[0].GetComponent<PlayerController>().heldCard = cardRef;
    }

    public void ShowPlayingAreas()
    {
        if (myTurn && GameplayManager.Instance.currPhase == "Main")
        {
            // can play
            if (GameplayManager.Instance.GetCurrMana() >= cardRef.GetComponentInChildren<Card>().cardData.CardCost)
            {
                // set various panels
                turnOrderGroup.SetActive(false);
                previewCard.SetActive(false);
                playCardPrompt.SetActive(false);
                playCardAreasPrompt.SetActive(true);

                // can promote
                if (cardRef.GetComponentInChildren<Card>().cardData.CardCost2 > 0 &&
                    GameplayManager.Instance.GetCurrMana() >= cardRef.GetComponentInChildren<Card>().cardData.CardCost2)
                {
                    pabManager.UpdateValidPlayingAreas(previewCard.GetComponentInChildren<Card>().cardData, playingAreas, true, true);
                }
                // cannot promote
                else
                {
                    pabManager.UpdateValidPlayingAreas(previewCard.GetComponentInChildren<Card>().cardData, playingAreas, true, false);
                }
            }
            // can promote
            else if (cardRef.GetComponentInChildren<Card>().cardData.CardCost2 > 0 &&
                    GameplayManager.Instance.GetCurrMana() >= cardRef.GetComponentInChildren<Card>().cardData.CardCost2)
            {
                // set various panels
                turnOrderGroup.SetActive(false);
                previewCard.SetActive(false);
                playCardPrompt.SetActive(false);
                playCardAreasPrompt.SetActive(true);
                pabManager.UpdateValidPlayingAreas(previewCard.GetComponentInChildren<Card>().cardData, playingAreas, false, true);
            }
        }
    }

    public void ShowHandGroup()
    {
        HideCardPrompts();
        turnOrderGroup.SetActive(false);
        handTargetGroup.SetActive(true);
    }

    public void PlaceCardInArea(int i)
    {
        int x = 0;
        for (int j = 0; j < playerHand[0].cardList.Count; j++)
        {
            if (playerHand[0].cardList[j] == cardRef)
            {
                x = j;
                break;
            }
        }

        PlayCard(i, x, true);
        player[0].GetComponent<PhotonView>().RPC("PlayCard", RpcTarget.Others, i, x, false);
    }

    public void PlayCard(int i, int x, bool isSelf)
    {
        if (isSelf && player[0].GetComponent<PlayerController>().view.IsMine)
        {
            // promote
            if (playingAreas[i].cardList.Count > 0)
            {
                GameplayManager.Instance.SetCurrMana(GameplayManager.Instance.GetCurrMana() - cardRef.GetComponentInChildren<Card>().cardData.CardCost2);
                playingAreas[i].AddCardToBottom(playerHand[0].TakeCard(cardRef));
                playingAreas[i].cardList[0].GetComponentInChildren<Card>().OnPromote(0);
                player[0].GetComponent<PlayerController>().isPromoting = true;
            }
            // summon
            else
            {
                GameplayManager.Instance.SetCurrMana(GameplayManager.Instance.GetCurrMana() - cardRef.GetComponentInChildren<Card>().cardData.CardCost);
                playingAreas[i].AddCardToBottom(playerHand[0].TakeCard(cardRef));
                playingAreas[i].cardList[0].GetComponentInChildren<Card>().OnSummon(0);
                if (playingAreas[i].areaCardType == "SPELL")
                {
                    player[0].GetComponent<PlayerController>().SendToDiscard(i);
                }
            }
            GameplayManager.Instance.UpdateManaText();
            HideCardPrompts();
        }
        else if (!isSelf)
        {
            cardRef = playerHand[1].cardList[x];
            i += 6;

            // promote
            if (playingAreas[i].cardList.Count > 0)
            {
                GameplayManager.Instance.SetOtherMana(GameplayManager.Instance.GetOtherMana() - cardRef.GetComponentInChildren<Card>().cardData.CardCost2);
                playingAreas[i].AddCardToBottom(playerHand[1].TakeCard(cardRef));
                playingAreas[i].cardList[0].GetComponentInChildren<Card>().OnPromote(1);
                player[1].GetComponent<PlayerController>().isPromoting = true;
            }
            // summon
            else
            {
                GameplayManager.Instance.SetOtherMana(GameplayManager.Instance.GetOtherMana() - cardRef.GetComponentInChildren<Card>().cardData.CardCost);
                playingAreas[i].AddCardToBottom(playerHand[1].TakeCard(cardRef));
                playingAreas[i].cardList[0].GetComponentInChildren<Card>().OnSummon(1);
                if (playingAreas[i].areaCardType == "SPELL")
                {
                    player[1].GetComponent<PlayerController>().SendToDiscard(i);
                }
            }
        }
    }

    public void ShowTargets()
    {
        if (myTurn && GameplayManager.Instance.currPhase == "Attack" && cardRef.GetComponentInChildren<Card>().canAttack)
        {
            turnOrderGroup.SetActive(false);
            previewCard.SetActive(false);
            cardActionGroup.SetActive(false);
            attackTargetGroup.SetActive(true);
            tabManager.UpdateValidPlayingAreas(playingAreas);
        }
    }

    public void Attack(int attackedSpace)
    {
        int x = 0;
        for (int j = 0; j < playingAreas.Count - 6; j++)
        {
            if (playingAreas[j].cardList.Count > 0 && playingAreas[j].cardList[0] == cardRef)
            {
                x = j;
                break;
            }
        }

        AttackCard(x, attackedSpace, true);
        player[0].GetComponent<PhotonView>().RPC("AttackCard", RpcTarget.Others, x, attackedSpace, false);
    }

    public void AttackCard(int attackingSpace, int attackedSpace, bool isSelf)
    {
        if (isSelf && player[0].GetComponent<PlayerController>().view.IsMine)
        {
            attackedSpace += 6;
            player[0].GetComponent<PlayerController>().isAttacking = true;
        }
        if (!isSelf)
        {
            attackingSpace += 6;
            player[0].GetComponent<PlayerController>().isAttacking = true;
        }

        playingAreas[attackingSpace].RestCard();
        playingAreas[attackingSpace].cardList[0].GetComponentInChildren<Card>().OnAttack(1);
        // if not leader space
        if (attackedSpace != 0 && attackedSpace != 6)
        {
            // do damage code
            playingAreas[attackedSpace].cardList[0].GetComponentInChildren<Card>().def -= playingAreas[attackingSpace].cardList[0].GetComponentInChildren<Card>().atk;
            playingAreas[attackingSpace].cardList[0].GetComponentInChildren<Card>().def -= playingAreas[attackedSpace].cardList[0].GetComponentInChildren<Card>().atk;
            if (playingAreas[attackedSpace].cardList[0].GetComponentInChildren<Card>().def <= 0)
            {
                if (isSelf)
                {
                    player[1].GetComponent<PlayerController>().SendToDiscard(attackedSpace);
                }
                else
                {
                    player[0].GetComponent<PlayerController>().SendToDiscard(attackedSpace);
                }
            }
            else
            {
                playingAreas[attackedSpace].cardList[0].GetComponentInChildren<Card>().UpdateCardText();
            }
            if (playingAreas[attackingSpace].cardList[0].GetComponentInChildren<Card>().def <= 0)
            {
                if (isSelf)
                {
                    player[0].GetComponent<PlayerController>().SendToDiscard(attackingSpace);
                }
                else
                {
                    player[1].GetComponent<PlayerController>().SendToDiscard(attackingSpace);
                }
            }
            else
            {
                playingAreas[attackingSpace].cardList[0].GetComponentInChildren<Card>().UpdateCardText();
            }
        }
        // if leader space
        else if (attackedSpace == 0 || attackedSpace == 6)
        {
            // do player damage code

            // attacking card damage code
            if (attackingSpace != 0 && attackingSpace != 6)
            {
                playingAreas[attackingSpace].cardList[0].GetComponentInChildren<Card>().def -= playingAreas[attackedSpace].cardList[0].GetComponentInChildren<Card>().atk;
                if (playingAreas[attackingSpace].cardList[0].GetComponentInChildren<Card>().def <= 0)
                {
                    if (isSelf)
                    {
                        player[0].GetComponent<PlayerController>().SendToDiscard(attackingSpace);
                    }
                    else
                    {
                        player[1].GetComponent<PlayerController>().SendToDiscard(attackingSpace);
                    }
                }
                else
                {
                    playingAreas[attackingSpace].cardList[0].GetComponentInChildren<Card>().UpdateCardText();
                }
            }
        }

        if (isSelf && player[0].GetComponent<PlayerController>().view.IsMine)
        {
            GameplayManager.Instance.UpdateManaText();
            HideCardPrompts();
        }
    }

    public void HideCardPrompts()
    {
        player[0].GetComponent<PlayerController>().isCLickToPreview = false;
        turnOrderGroup.SetActive(myTurn);
        Destroy(previewCard.gameObject);
        playCardPrompt.SetActive(false);
        playCardAreasPrompt.SetActive(false);
        cardActionGroup.SetActive(false);
        attackTargetGroup.SetActive(false);
        handTargetGroup.SetActive(false);
    }

    public void UpdateTurnOrderButton()
    {
        myTurn = player[0].GetComponent<PlayerController>().isMyTurn;
        turnOrderGroup.SetActive(myTurn);
    }
}
