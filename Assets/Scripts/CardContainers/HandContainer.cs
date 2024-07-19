using UnityEngine;

public class HandContainer : CardContainer
{
    [SerializeField] private LayerMask cardLayerMask;
    private GameObject tempCard;

    private void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector3.down, out hit, 100, cardLayerMask))
        {
            if (hit.collider.gameObject.transform.parent.parent.name == "Hand" &&
                hit.collider.gameObject.transform.parent.parent.parent.parent.GetComponent<PlayerController>().view.IsMine)
            {
                if (hit.collider.gameObject.transform.parent.gameObject != tempCard)
                {
                    if (tempCard != null)
                    {
                        tempCard.GetComponentInChildren<Card>().isHovering = false;
                    }
                    tempCard = hit.collider.gameObject.transform.parent.gameObject;
                    hit.collider.gameObject.GetComponent<Card>().isHovering = true;
                }
            }
        }
        else if (tempCard != null)
        {
            tempCard.GetComponentInChildren<Card>().isHovering = false;
        }
    }
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
    public override void ResetCardOffset()
    {
        // offset each card
        for (int i = 0; i < cardList.Count; i++)
        {
            cardPosOffset = new Vector3(cardOffsetX * i, cardOffsetY * i, -0.005f * i);
            cardList[i].transform.localPosition = cardPosOffset;
        }

        // center cards
        transform.localPosition = containerStartPos + new Vector3(((-cardOffsetX * (cardList.Count + 1)) / 2), ((-cardOffsetY * (cardList.Count + 1)) / 2), 0);
    }
}
