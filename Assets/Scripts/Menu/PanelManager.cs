using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;

public class PanelManager : MonoBehaviour
{
    [SerializeField] List<GameObject> panels;

    private void Start()
    {
        PhotonNetwork.Disconnect();
        HideAllPanels();
        panels[0].SetActive(true);
    }

    private void HideAllPanels()
    {
        foreach (GameObject panel in panels)
        {
            panel.SetActive(false);
        }
    }

    public void ShowPanel(string panelName)
    {
        HideAllPanels();
        foreach(GameObject panel in panels)
        {
            if (panel.name.Contains(panelName))
            {
                panel.SetActive(true);
            }
        }
    }
}
