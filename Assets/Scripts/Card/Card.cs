using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Card : MonoBehaviour
{
    [SerializeField] private float hoverDistance = 0.5f;
    public bool isFlippedDown = false;
    public bool isHovering = false;
    public bool canAttack = false;
    public CardData cardData;
    public bool usebaseStats = true;

    // card details
    public TMP_Text type,
                    cost,
                    rank,
                    ability1,
                    ability2,
                    attribute,
                    cardName,
                    attack,
                    defence;
    // individual card stats
    public int atk,
               def;

    private void Start()
    {
        if (usebaseStats)
        {
            atk = cardData.CardAttack;
            def = cardData.CardDefense;
            UpdateCardText();
        }
    }

    private void Update()
    {
        if (transform.parent.parent.name == "Hand")
        {
            if (isHovering)
            {
                if (transform.parent.localPosition.y < hoverDistance)
                {
                    transform.parent.localPosition += Vector3.up * 2 * Time.deltaTime;
                }
            }
            else
            {
                if (transform.parent.localPosition.y > 0)
                {
                    transform.parent.localPosition -= Vector3.up * 2 * Time.deltaTime;
                }
            }
        }
    }

    public void UpdateCardText()
    {
        type.text = cardData.CardType;
        cost.text = cardData.CardCost.ToString();
        rank.text = cardData.CardRank.ToString();
        ability1.text = cardData.CardAbilty1;
        ability2.text = cardData.CardAbility2;
        attribute.text = cardData.CardRace;
        cardName.text = cardData.CardName;
        attack.text = atk.ToString();
        defence.text = def.ToString();
        Debug.Log("Text Updated.");
    }

    public void FlipCard()
    {
        transform.parent.localEulerAngles += new Vector3(180, 0, 0);
        if (isFlippedDown)
        {
            isFlippedDown = false;
        }
        else
        {
            isFlippedDown = true;
        }
    }

    public void OnSummon()
    {
        ICardAbility[] cardAbilities = GetComponents<ICardAbility>();
        foreach (var ability in cardAbilities)
        {
            ability.OnSummon();
        }
    }

    public void OnPromote()
    {
        ICardAbility[] cardAbilities = GetComponents<ICardAbility>();
        foreach (var ability in cardAbilities)
        {
            ability.OnPromote();
        }
    }

    public void OnAttack()
    {
        ICardAbility[] cardAbilities = GetComponents<ICardAbility>();
        foreach (var ability in cardAbilities)
        {
            ability.OnAttack();
        }
    }

    public void OnEndTurn()
    {
        ICardAbility[] cardAbilities = GetComponents<ICardAbility>();
        foreach (var ability in cardAbilities)
        {
            ability.OnEndTurn();
        }
    }
}
