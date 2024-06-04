using System.Collections.Generic;
using UnityEngine;

public class PlayingAreaButtonManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> playingArea;

    public void UpdateValidPlayingAreas(CardData cardData, List<PlayingAreaContainer> areas)
    {
        HideAllButtons();
        for (int i = 0; i < playingArea.Count; i++)
        {
            if (areas[i].GetComponent<PlayingAreaContainer>().areaCardType == cardData.CardType &&
                areas[i].GetComponent<PlayingAreaContainer>().cardList.Count == 0)
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
