using UnityEngine;

public class ResilienceContainer : CardContainer
{
    public override void ResetCardOffset()
    {
        // offset each card
        for (int i = 0; i < cardList.Count; i++)
        {
            cardList[i].transform.localEulerAngles = new Vector3(0, 0, 180);
            cardPosOffset = new Vector3(cardOffsetX * i, cardOffsetY * i, 0.005f * i);
            cardList[i].transform.localPosition = cardPosOffset;
        }
    }
}
