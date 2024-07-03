using System;
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
    private EnemyHealthManager enemy;
    private PlayerController player;
    public string currPhase;

    public int manaCount = 0;
    public int maxManaCount = 0;
    [SerializeField] private TMP_Text manaCount_text;

    private void Awake()
    {
        CheckInstance();
        enemy = GameObject.FindGameObjectWithTag("Enemy").GetComponent<EnemyHealthManager>();
    }
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("PlayingArea"))
        {
            playingAreas.Add(obj.GetComponent<PlayingAreaContainer>());
        }
        StartGame();
    }
    private void StartGame()
    {
        player.DrawFromDeck(5, "HAND");
        player.DrawFromDeck(8, "RESILIENCE");
    }

    public void DrawCard(int amt)
    {
        player.DrawFromDeck(amt, "HAND");
    }

    public void DiscardCard(int amt)
    {
        player.DiscardCardFromHand();
    }

    public void DoStartTurn()
    {
        // increase mana
        if (maxManaCount < 10)
        {
            maxManaCount++;
        }
        manaCount = maxManaCount;
        UpdateManaText();
        // draw card from deck
        player.DrawFromDeck(1, "HAND");
        // change phase
        player.isMyTurn = true;
        UpdatePhaseText("Main");
        // stand all cards
        for (int i = 0; i < playingAreas.Count; i++)
        {
            if (playingAreas[i].cardList.Count > 0)
            {
                playingAreas[i].StandCard();
            }
        }
    }
    public void DoStartAttack()
    {
        // change phase
        UpdatePhaseText("Attack");
    }
    public void DoPassTurn()
    {
        // change phase
        player.isMyTurn = false;
        UpdatePhaseText("End");

        // activate all end of turn card abilites
        for (int i = 0; i < playingAreas.Count; i++)
        {
            if (playingAreas[i].cardList.Count > 0)
            {
                playingAreas[i].GetComponentInChildren<Card>().OnEndTurn();
            }
        }
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