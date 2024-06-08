using System.Collections.Generic;
using UnityEngine;

public class PlayingAreaButtonManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> playingArea;

    public void UpdateValidPlayingAreas(CardData cardData, List<PlayingAreaContainer> areas, bool canPlay, bool canPromote)
    {
        HideAllButtons();
        for (int i = 0; i < playingArea.Count; i++)
        {
            if (areas[i].areaCardType == cardData.CardType &&
                areas[i].cardList.Count == 0 && 
                canPlay)
            {
                playingArea[i].SetActive(true);
            }
        }
        for (int i = 0; i < playingArea.Count; i++)
        {
            if (areas[i].areaCardType == cardData.CardType &&
                areas[i].cardList.Count > 0 && 
                canPromote)
            {
                playingArea[i].SetActive(true);
            }
        }
    }
    private void HideAllButtons()
    {
        for (int i = 0; i < playingArea.Count; i++)
        {
            playingArea[i].SetActive(false);
        }
    }
}
