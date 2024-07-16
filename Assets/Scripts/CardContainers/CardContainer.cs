using System.Collections.Generic;
using UnityEngine;

public class CardContainer : MonoBehaviour
{
    [SerializeField] protected GameObject cardPrefab;
    [SerializeField] protected float cardOffsetX = 0.5f;
    [SerializeField] protected float cardOffsetY = 0.5f;
    [SerializeField] public List<GameObject> cardList;
    
    protected Vector3 containerStartPos;
    protected Vector3 cardPosOffset;

    private void Awake()
    {
        containerStartPos = transform.localPosition;
    }

    public virtual void AddCardToTop(GameObject card)
    {
        // change card's container
        card.transform.SetParent(transform);
        if (!card.GetComponentInChildren<Card>().isFlippedDown)
        {
            card.GetComponentInChildren<Card>().FlipCard();
        }
        card.transform.localEulerAngles = Vector3.zero;

        // add card to cardList
        cardList.Add(card);

        ResetCardOffset();
    }
    public virtual void AddCardToBottom(GameObject card)
    {
        if (cardList.Count == 0)
        {
            AddCardToTop(card);
            return;
        }

        // change card's container
        card.transform.SetParent(transform);
        if (!card.GetComponentInChildren<Card>().isFlippedDown)
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

    public GameObject TakeTopCard()
    {
        GameObject temp = cardList[cardList.Count - 1];
        cardList.RemoveAt(cardList.Count - 1);

        ResetCardOffset();

        return temp;
    }

    public GameObject TakeCard(GameObject card)
    {
        cardList.Remove(card);

        //int k = 0;
        //List<GameObject> tempCardList = new List<GameObject>();
        //for (int i = 0;  i < cardList.Count; i++)
        //{
        //    if (cardList[i] != null)
        //    {
        //        tempCardList[k] = cardList[i];
        //        k++;
        //    }
        //}

        ResetCardOffset();

        return card;
    }

    public void ShuffleContainer()
    {
        for (int i = 0; i < cardList.Count; i++)
        {
            GameObject temp = cardList[i];
            int randomCard = Random.Range(i, cardList.Count);
            cardList[i] = cardList[randomCard];
            cardList[randomCard] = temp;
        }
        ResetCardOffset();
    }

    public virtual void ResetCardOffset()
    {
        // offset each card
        for (int i = 0; i < cardList.Count; i++)
        {
            cardPosOffset = new Vector3(cardOffsetX * i, 0.005f * i, cardOffsetY * i);
            cardList[i].transform.localPosition = cardPosOffset;
        }

        // center cards
        //transform.position = containerStartPos + new Vector3(((-cardOffsetX * (cardList.Count + 1)) / 2), ((-cardOffsetY * (cardList.Count + 1)) / 2), 0);
    }
}
