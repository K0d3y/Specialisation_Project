using UnityEngine;

public class DiscardContainer : CardContainer
{
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
}
