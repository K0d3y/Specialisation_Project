using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;

public class CardPromptController : MonoBehaviour
{
    // UI panels
    [SerializeField] private GameObject turnOrderGroup;
    [SerializeField] private GameObject playCardPrompt;
    [SerializeField] public GameObject playCardAreasPrompt;
    [SerializeField] private GameObject cardActionGroup;
    [SerializeField] private GameObject attackTargetGroup;
    [SerializeField] private GameObject handTargetGroup;
    // for card preview
    public GameObject player;
    [SerializeField] private GameObject cardPrefab;
    public GameObject previewCard;
    private GameObject cardRef;
    private bool myTurn;
    // for card playing areas
    private HandContainer hand;
    [SerializeField] private PlayingAreaButtonManager pabManager;
    private List<PlayingAreaContainer> playingArea = new List<PlayingAreaContainer>();

    private void Start()
    {
        hand = GameObject.FindGameObjectWithTag("Hand").GetComponent<HandContainer>();
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("PlayingArea"))
        {
            playingArea.Add(obj.GetComponent<PlayingAreaContainer>());
        }
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Player"))
        {
            if (obj.GetComponent<PhotonView>().IsMine)
            {
                player = obj;
                break;
            }
        }
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
        previewCard = Instantiate(cardPrefab, player.transform);
        previewCard.name = hit.collider.transform.parent.gameObject.name;
        previewCard.GetComponentInChildren<Card>().cardData = hit.collider.GetComponent<Card>().cardData;
        previewCard.GetComponentInChildren<Card>().usebaseStats = false;
        previewCard.GetComponentInChildren<Card>().atk = hit.collider.GetComponent<Card>().atk;
        previewCard.GetComponentInChildren<Card>().def = hit.collider.GetComponent<Card>().def;
        previewCard.GetComponentInChildren<Card>().UpdateCardText();
        previewCard.transform.localPosition = new Vector3(10, 5, -5);
        previewCard.transform.localScale *= 5;
        playCardPrompt.SetActive(true);
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
        previewCard = Instantiate(cardPrefab, player.transform);
        previewCard.name = hit.collider.transform.parent.gameObject.name;
        previewCard.GetComponentInChildren<Card>().cardData = hit.collider.GetComponent<Card>().cardData;
        previewCard.GetComponentInChildren<Card>().usebaseStats = false;
        previewCard.GetComponentInChildren<Card>().atk = hit.collider.GetComponent<Card>().atk;
        previewCard.GetComponentInChildren<Card>().def = hit.collider.GetComponent<Card>().def;
        previewCard.GetComponentInChildren<Card>().UpdateCardText();
        previewCard.transform.localPosition = new Vector3(10, 5, -5);
        previewCard.transform.localScale *= 5;
        cardActionGroup.SetActive(true);
    }
    public void ShowPlayingAreas()
    {
        if (myTurn && GameplayManager.Instance.currPhase == "Main")
        {
            // can play
            if (GameplayManager.Instance.manaCount >= cardRef.GetComponentInChildren<Card>().cardData.CardCost)
            {
                // set various panels
                turnOrderGroup.SetActive(false);
                previewCard.SetActive(false);
                playCardPrompt.SetActive(false);
                playCardAreasPrompt.SetActive(true);

                // can promote
                if (cardRef.GetComponentInChildren<Card>().cardData.CardCost2 > 0 &&
                    GameplayManager.Instance.manaCount >= cardRef.GetComponentInChildren<Card>().cardData.CardCost2)
                {
                    pabManager.UpdateValidPlayingAreas(previewCard.GetComponentInChildren<Card>().cardData, playingArea, true, true);
                }
                // cannot promote
                else
                {
                    pabManager.UpdateValidPlayingAreas(previewCard.GetComponentInChildren<Card>().cardData, playingArea, true, false);
                }
            }
            // can promote
            else if (cardRef.GetComponentInChildren<Card>().cardData.CardCost2 > 0 &&
                    GameplayManager.Instance.manaCount >= cardRef.GetComponentInChildren<Card>().cardData.CardCost2)
            {
                // set various panels
                turnOrderGroup.SetActive(false);
                previewCard.SetActive(false);
                playCardPrompt.SetActive(false);
                playCardAreasPrompt.SetActive(true);
                pabManager.UpdateValidPlayingAreas(previewCard.GetComponentInChildren<Card>().cardData, playingArea, false, true);
            }
        }
    }
    public void ShowTargets()
    {
        if (myTurn && GameplayManager.Instance.currPhase == "Attack" && cardRef.GetComponentInChildren<Card>().canAttack)
        {
            Debug.Log("Can Attack");
            HideCardPrompts();
            turnOrderGroup.SetActive(false);
            attackTargetGroup.SetActive(true);
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
        // promote
        if (playingArea[i].cardList.Count > 0)
        {
            GameplayManager.Instance.manaCount -= cardRef.GetComponentInChildren<Card>().cardData.CardCost2;
            playingArea[i].AddCardToBottom(hand.TakeCard(cardRef));
            playingArea[i].cardList[0].GetComponentInChildren<Card>().OnPromote();
            player.GetComponent<PlayerController>().isPromoting = true;
        }
        // summon
        else
        {
            GameplayManager.Instance.manaCount -= cardRef.GetComponentInChildren<Card>().cardData.CardCost;
            playingArea[i].AddCardToBottom(hand.TakeCard(cardRef));
            playingArea[i].cardList[0].GetComponentInChildren<Card>().OnSummon();
            if (playingArea[i].areaCardType == "SPELL")
            {
                player.GetComponent<PlayerController>().SendToDiscard(i);
            }
        }
        GameplayManager.Instance.UpdateManaText();
        HideCardPrompts();
    }
    public void AttackEnemy()
    {
        GameplayManager.Instance.EnemyTakeDamage(cardRef.GetComponentInChildren<Card>().atk);
        cardRef.transform.parent.GetComponentInParent<PlayingAreaContainer>().RestCard();
        cardRef.GetComponentInChildren<Card>().OnAttack();
        HideCardPrompts();
    }

    public void HideCardPrompts()
    {
        turnOrderGroup.SetActive(true);
        Destroy(previewCard.gameObject);
        playCardPrompt.SetActive(false);
        playCardAreasPrompt.SetActive(false);
        cardActionGroup.SetActive(false);
        attackTargetGroup.SetActive(false);
        handTargetGroup.SetActive(false);
    }
}
