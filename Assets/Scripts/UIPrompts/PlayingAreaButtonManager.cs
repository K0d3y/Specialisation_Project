using System.Collections.Generic;
using UnityEngine;

public class PlayingAreaButtonManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> playingAreaButtons;

    public void UpdateValidPlayingAreas(CardData cardData, List<PlayingAreaContainer> areas, bool canPlay, bool canPromote)
    {
        HideAllButtons();
        for (int i = 0; i < playingAreaButtons.Count; i++)
        {
            if (areas[i].areaCardType == cardData.CardType &&
                areas[i].cardList.Count == 0 && 
                canPlay)
            {
                playingAreaButtons[i].SetActive(true);
            }
        }
        for (int i = 0; i < playingAreaButtons.Count; i++)
        {
            if (areas[i].areaCardType == cardData.CardType &&
                areas[i].cardList.Count > 0 && 
                canPromote)
            {
                playingAreaButtons[i].SetActive(true);
            }
        }
    }
    private void HideAllButtons()
    {
        for (int i = 0; i < playingAreaButtons.Count; i++)
        {
            playingAreaButtons[i].SetActive(false);
        }
    }
}
