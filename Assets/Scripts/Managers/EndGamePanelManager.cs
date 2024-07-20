using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGamePanelManager : MonoBehaviour
{
    [SerializeField] private TMP_Text titleText;

    public void UpdateTitleText(string title)
    {
        titleText.text = title;
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene("LobbyScene");
    }
}
