using System.Collections.Generic;
using UnityEngine;

public class DeckContainer : CardContainer
{
    [SerializeField] public GameObject leader;
    [SerializeField] public List<GameObject> deck;
    [SerializeField] public List<CardData> tokens;

    private void Awake()
    {
        // initialise objects into deck list
        Init();
    }

    private void Init()
    {
        GameObject obj = Instantiate(leader);
        obj.transform.SetParent(transform);
        leader = obj;

        for (int i = 0; i < deck.Count; i++)
        {
            obj = Instantiate(deck[i]);
            //obj.name = deck[i].name;
            //obj.GetComponentInChildren<Card>().cardData = deck[i];
            obj.GetComponentInChildren<Card>().FlipCard();
            obj.transform.SetParent(transform);
            cardList.Add(obj);
        }
    }
}
