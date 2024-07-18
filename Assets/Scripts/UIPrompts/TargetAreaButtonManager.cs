using System.Collections.Generic;
using UnityEngine;

public class TargetAreaButtonManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> targetAreaButtons;

    public void UpdateValidPlayingAreas(List<PlayingAreaContainer> areas)
    {
        HideAllButtons();
        for (int i = 6; i < areas.Count - 1; i++)
        {
            if (areas[i].cardList.Count > 0)
            {
                targetAreaButtons[i - 6].SetActive(true);
            }
        }
    }
    private void HideAllButtons()
    {
        for (int i = 0; i < targetAreaButtons.Count; i++)
        {
            targetAreaButtons[i].SetActive(false);
        }
    }
}
