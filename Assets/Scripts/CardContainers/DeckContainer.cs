using System.Collections.Generic;
using UnityEngine;

public class DeckContainer : CardContainer
{
    [SerializeField] public CardData leader;
    [SerializeField] public List<GameObject> deck;
    [SerializeField] public List<CardData> tokens;

    private void Awake()
    {
        // initialise objects into deck list
        Init();
    }

    private void Init()
    {
        for (int i = 0; i < deck.Count; i++)
        {
            GameObject obj = Instantiate(deck[i]);
            //obj.name = deck[i].name;
            //obj.GetComponentInChildren<Card>().cardData = deck[i];
            obj.GetComponentInChildren<Card>().FlipCard();
            obj.transform.SetParent(transform);
            cardList.Add(obj);
        }
    }
}
