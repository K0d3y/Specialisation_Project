using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialPromtManager : MonoBehaviour
{
    [SerializeField] List<GameObject> tutorialPanels;
    [SerializeField] GameObject prevPanelBtn;
    [SerializeField] GameObject nextPanelBtn;

    private int currPanel = 0;

    private void Start()
    {
        HideAllPanels();
        tutorialPanels[currPanel].SetActive(true);
        prevPanelBtn.SetActive(false);
        nextPanelBtn.SetActive(true);
    }

    private void HideAllPanels()
    {
        foreach (GameObject panel in tutorialPanels)
        {
            panel.SetActive(false);
        }
    }

    public void ShowPrevPanel()
    {
        currPanel--;
        HideAllPanels();
        tutorialPanels[currPanel].SetActive(true);
        nextPanelBtn.SetActive(true);
        if (currPanel == 0)
        {
            prevPanelBtn.SetActive(false);
        }
    }

    public void ShowNextPanel()
    {
        currPanel++;
        HideAllPanels();
        tutorialPanels[currPanel].SetActive(true);
        prevPanelBtn.SetActive(true);
        if (currPanel == tutorialPanels.Count - 1)
        {
            nextPanelBtn.SetActive(false);
        }
    }
}
