using Photon.Pun;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

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
    [SerializeField] TMP_Text phase_text;
    [SerializeField] TurnOrderController turnOrderController;
    [SerializeField] private CardPromptController cardPromptController;
    private EnemyHealthManager enemy;

    public List<PlayerController> players;
    public string currPhase;
    public int currPlayer = 0;

    public int manaCount = 0;
    public int maxManaCount = 0;
    [SerializeField] private TMP_Text manaCount_text;

    private void Awake()
    {
        CheckInstance();
        enemy = GameObject.FindGameObjectWithTag("Enemy").GetComponent<EnemyHealthManager>();
    }
    public void StartGame(int playerID)
    {
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Player"))
        {
            players.Add(obj.GetComponent<PlayerController>());
        }
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("PlayingArea"))
        {
            playingAreas.Add(obj.GetComponent<PlayingAreaContainer>());
        }
        foreach (PlayerController player in players)
        {
            player.deck.ShuffleContainer();
            player.DrawFromDeck(5, "HAND");
            player.DrawFromDeck(8, "RESILIENCE");
        }

        if (PhotonNetwork.IsMasterClient)
        {
            currPlayer = playerID;
            players[currPlayer].view.RPC("StartTurn", RpcTarget.All);
        }
        else
        {
            currPlayer = playerID;
            PlayerController temp = players[0];
            players[0] = players[1];
            players[1] = temp;
        }
    }

    public void DrawCard(int playerNo, int amt)
    {
        players[playerNo].DrawFromDeck(amt, "HAND");
    }

    public void DiscardCard(int playerNo)
    {
        players[playerNo].DiscardCardFromHand();
    }

    public void DoStartTurn()
    {
        currPlayer++;
        if (currPlayer >= PhotonNetwork.CurrentRoom.PlayerCount)
        {
            currPlayer = 0;
        }
        // increase mana
        if (maxManaCount < 10)
        {
            maxManaCount++;
        }
        manaCount = maxManaCount;
        UpdateManaText();
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
        cardPromptController.HideCardPrompts();
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
        manaCount_text.text = manaCount.ToString() + '/' + maxManaCount.ToString();
    }
    public void EnemyTakeDamage(int damage)
    {
        enemy.health -= damage;
        enemy.health_text.text = enemy.health.ToString();
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
}