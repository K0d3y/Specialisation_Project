using TMPro;
using UnityEngine;

public class Card : MonoBehaviour
{
    public CardData cardData;
    public bool isFlippedDown = false;
    public bool isHovering = false;
    [SerializeField] float hoverDistance = 0.5f;

    // card details
    [SerializeField]
    private TMP_Text type,
                    cost,
                    rank,
                    ability1,
                    ability2,
                    attribute,
                    cardName,
                    attack,
                    defence;

    private void Start()
    {
        type.text = cardData.CardType;
        cost.text = cardData.CardCost.ToString();
        rank.text = cardData.CardRank.ToString();
        ability1.text = cardData.CardAbilty1;
        ability2.text = cardData.CardAbility2;
        attribute.text = cardData.CardRace;
        cardName.text = cardData.CardName;
        attack.text = cardData.CardAttack.ToString();
        defence.text = cardData.CardDefense.ToString();
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
}
