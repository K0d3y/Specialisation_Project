using Unity.VisualScripting;
using UnityEngine;

public class PlayingAreaContainer : CardContainer
{
    [SerializeField] public string areaType;
    [SerializeField] public string areaCardType;

    public override void AddCardToTop(GameObject card)
    {
        // change card's container
        card.transform.SetParent(transform);
        if (card.GetComponentInChildren<Card>().isFlippedDown)
        {
            card.GetComponentInChildren<Card>().FlipCard();
        }
        card.transform.localEulerAngles = Vector3.zero;

        // add card to cardList
        cardList.Add(card);

        ResetCardOffset();
    }
    public override void AddCardToBottom(GameObject card)
    {
        if (cardList.Count == 0)
        {
            AddCardToTop(card);
            return;
        }

        // change card's container
        card.transform.SetParent(transform);
        if (card.GetComponentInChildren<Card>().isFlippedDown)
        {
            card.GetComponentInChildren<Card>().FlipCard();
        }
        card.transform.localEulerAngles = Vector3.zero;

        // add card to cardList
        cardList.Add(cardList[cardList.Count - 1]);
        for (int i = cardList.Count - 2; i >= 0; i--)
        {
            cardList[i + 1] = cardList[i];
        }
        cardList[0] = card;

        ResetCardOffset();
    }

    public void RestCard()
    {
        if (cardList.Count > 0)
        {
            cardList[0].transform.localEulerAngles = new Vector3(0, 0, 90);
            cardList[0].GetComponentInChildren<Card>().canAttack = false;
        }
    }

    public void StandCard()
    {
        if (cardList.Count > 0)
        {
            cardList[0].transform.localEulerAngles = new Vector3(0, 0, 0);
            cardList[0].GetComponentInChildren<Card>().canAttack = true;
        }
    }
}
