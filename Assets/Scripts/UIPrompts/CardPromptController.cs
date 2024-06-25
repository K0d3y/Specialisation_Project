using System.Collections.Generic;
using UnityEngine;

public class CardPromptController : MonoBehaviour
{
    // UI panels
    [SerializeField] private GameObject turnOrderGroup;
    [SerializeField] private GameObject playCardPrompt;
    [SerializeField] private GameObject playCardAreasPrompt;
    [SerializeField] private GameObject cardActionGroup;
    [SerializeField] private GameObject attackTargetGroup;
    // for card preview
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject cardPrefab;
    private GameObject previewCard;
    private GameObject cardRef;
    private bool myTurn;
    // for card playing areas
    [SerializeField] private HandContainer hand;
    [SerializeField] private PlayingAreaButtonManager pabManager;
    [SerializeField] private List<PlayingAreaContainer> playingArea;

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
        previewCard.transform.localPosition = new Vector3(10, 0, 0);
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
        previewCard.transform.localPosition = new Vector3(10, 0, 0);
        previewCard.transform.localScale *= 5;
        cardActionGroup.SetActive(true);
    }
    public void ShowPlayingAreas()
    {
        if (myTurn && GameplayManager.Instance.currPhase == "Main")
        {
            // can play
            if (player.GetComponent<PlayerController>().manaCount >= cardRef.GetComponentInChildren<Card>().cardData.CardCost)
            {
                // set various panels
                turnOrderGroup.SetActive(false);
                previewCard.SetActive(false);
                playCardPrompt.SetActive(false);
                playCardAreasPrompt.SetActive(true);

                // can promote
                if (cardRef.GetComponentInChildren<Card>().cardData.CardCost2 > 0 &&
                    player.GetComponent<PlayerController>().manaCount >= cardRef.GetComponentInChildren<Card>().cardData.CardCost2)
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
                    player.GetComponent<PlayerController>().manaCount >= cardRef.GetComponentInChildren<Card>().cardData.CardCost2)
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

    public void PlaceCardInArea(int i)
    {
        playingArea[i].AddCardToBottom(hand.TakeCard(cardRef));
        if (playingArea[i].cardList.Count > 0)
        {
            player.GetComponent<PlayerController>().manaCount -= cardRef.GetComponentInChildren<Card>().cardData.CardCost2;
            cardRef.GetComponentInChildren<Card>().OnPromote();
        }
        else
        {
            player.GetComponent<PlayerController>().manaCount -= cardRef.GetComponentInChildren<Card>().cardData.CardCost;
            cardRef.GetComponentInChildren<Card>().OnSummon();
        }
        player.GetComponent<PlayerController>().UpdateManaText();
        HideCardPrompts();
    }
    public void AttackEnemy()
    {
        GameplayManager.Instance.EnemyTakeDamage(cardRef.GetComponentInChildren<Card>().cardData.CardAttack);
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
    }
}
