using System.Collections.Generic;
using UnityEngine;

public class TurnOrderController : MonoBehaviour
{
    [SerializeField] List<GameObject> buttons;

    private void Start()
    {
        for (int i = 0; i < buttons.Count; i++)
        {
            buttons[i].SetActive(false);
        }
        buttons[0].SetActive(true);
    }

    public void CycleButtons()
    {
        for (int i = 0; i < buttons.Count; i++)
        {
            if (buttons[i].activeSelf)
            {
                buttons[i].SetActive(false);
                if (i + 1 < buttons.Count)
                {
                    buttons[i + 1].SetActive(true);
                }
                else
                {
                    buttons[0].SetActive(true);
                }
                break;
            }
        }
    }
}
