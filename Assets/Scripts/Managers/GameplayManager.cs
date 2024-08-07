using Photon.Pun;
using Photon.Pun.Demo.Asteroids;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameplayManager : MonoBehaviour
{
    // instance creation
    private static GameplayManager _instance;
    public static GameplayManager Instance
    {
        get
        {
            if (_instance == null)
            {
                // Find existing instance in scene
                _instance = FindObjectOfType<GameplayManager>();

                if (_instance == null)
                {
                    // Create new GameObject
                    GameObject obj = new("GameplayManager");

                    // Add EntityManager component to the GameObject
                    _instance = obj.AddComponent<GameplayManager>();
                }
            }
            return _instance;
        }
    }
    private void CheckInstance()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject); // Optional: Keeps the EntityManager alive between scene changes
        }
        else
        {
            Destroy(gameObject); // Ensure only one instance exists
        }
    }

    private List<PlayingAreaContainer> playingAreas = new List<PlayingAreaContainer>();
    [SerializeField] private TMP_Text phase_text;
    [SerializeField] private TurnOrderController turnOrderController;
    [SerializeField] private CardPromptController cardPromptController;
    [SerializeField] private List<Image> playerColors;
    [SerializeField] private TMP_Text manaCount_text;

    public List<PlayerController> players;
    public string currPhase;
    public int currPlayer = 0;


    private void Awake()
    {
        CheckInstance();
    }
    public void StartGame()
    {
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Player"))
        {
            players.Add(obj.GetComponent<PlayerController>());
        }
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("PlayingArea"))
        {
            playingAreas.Add(obj.GetComponent<PlayingAreaContainer>());
        }

        if (PhotonNetwork.IsMasterClient)
        {
            int seed1 = Random.Range(0, 999999999);
            int seed2 = Random.Range(0, 999999999);
            players[1].view.RPC("ShuffleDeck", RpcTarget.All, seed1, seed2);

            PlayerController temp = players[0];
            players[0] = players[1];
            players[1] = temp;
            players[0].view.RPC("StartTurn", RpcTarget.All);
        }
    }

    public void ShuffleDeck(int s1, int s2)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            players[1].deck.ShuffleContainer(s1);
            players[0].deck.ShuffleContainer(s2);
            playerColors[0].color = AsteroidsGame.GetColor(0);
            playerColors[1].color = AsteroidsGame.GetColor(1);
        }
        else
        {
            players[0].deck.ShuffleContainer(s1);
            players[1].deck.ShuffleContainer(s2);
            playerColors[1].color = AsteroidsGame.GetColor(0);
            playerColors[0].color = AsteroidsGame.GetColor(1);
        }

        foreach (PlayerController player in players)
        {
            player.DrawFromDeck(5, "HAND");
            player.DrawFromDeck(5, "RESILIENCE");
            player.PlayLeaderCard();
        }
    }

    public void DrawCard(int playerNo, int amt)
    {
        players[currPlayer].DrawFromDeck(amt, "HAND");
    }

    public void DiscardCard(int playerNo)
    {
        players[playerNo].DiscardCardFromHand();
    }

    public void DoStartTurn()
    {
        // change player turn
        players[currPlayer].isMyTurn = false;
        currPlayer++;
        if (currPlayer >= PhotonNetwork.CurrentRoom.PlayerCount)
        {
            currPlayer = 0;
        }
        // increase mana
        if (players[currPlayer].maxManaCount < 10)
        {
            players[currPlayer].maxManaCount++;
        }
        players[currPlayer].manaCount = players[currPlayer].maxManaCount;
        if (players[currPlayer].view.IsMine)
        {
            UpdateManaText();
        }
        // draw card from deck
        players[currPlayer].DrawFromDeck(1, "HAND");
        // change phase
        players[currPlayer].isMyTurn = true;
        UpdatePhaseText("Main");
        // stand all cards
        for (int i = 0; i < playingAreas.Count; i++)
        {
            if (playingAreas[i].cardList.Count > 0)
            {
                playingAreas[i].StandCard();
            }
        }

        // update button
        cardPromptController.UpdateTurnOrderButton();
    }
    public void DoStartAttack()
    {
        // change phase
        UpdatePhaseText("Attack");
        turnOrderController.CycleButtons();
    }
    public void DoPassTurn()
    {
        // change phase
        players[currPlayer].isMyTurn = false;
        UpdatePhaseText("End");
        turnOrderController.CycleButtons();

        // activate all end of turn card abilites
        for (int i = 0; i < playingAreas.Count; i++)
        {
            if (playingAreas[i].cardList.Count > 0)
            {
                playingAreas[i].GetComponentInChildren<Card>().OnEndTurn(currPlayer);
            }
        }

        players[currPlayer].view.RPC("StartTurn", RpcTarget.All);
    }

    private void UpdatePhaseText(string phase)
    {
        currPhase = phase;
        phase_text.text = currPhase + " Phase";
    }
    public void UpdateManaText()
    {
        manaCount_text.text = players[currPlayer].manaCount.ToString() + '/' + players[currPlayer].maxManaCount.ToString();
    }

    public void BuffCardValues(int position, int atk, int def)
    {
        if (playingAreas[position].cardList.Count > 0)
        {
            foreach (Card card in playingAreas[position].GetComponentsInChildren<Card>())
            {
                card.atk += atk;
                card.def += def;
                card.UpdateCardText();
            }
        }
    }
    
    public int GetCurrMana()
    {
        return players[currPlayer].manaCount;
    }

    public void SetCurrMana(int m)
    {
        players[currPlayer].manaCount = m;
        if (m < 0)
        {
            players[currPlayer].manaCount = 0;
        }
        else if (m > 10)
        {
            players[currPlayer].manaCount = 10;
        }
    }

    public int GetOtherMana()
    {
        int i = currPlayer + 1;
        if (i >= PhotonNetwork.CurrentRoom.PlayerCount)
        {
            i = 0;
        }
        return players[i].manaCount;
    }

    public void SetOtherMana(int m)
    {
        int i = currPlayer + 1;
        if (i >= PhotonNetwork.CurrentRoom.PlayerCount)
        {
            i = 0;
        }
        players[i].manaCount = m;
        if (m < 0)
        {
            players[i].manaCount = 0;
        }
        else if (m > 10)
        {
            players[i].manaCount = 10;
        }
    }
}