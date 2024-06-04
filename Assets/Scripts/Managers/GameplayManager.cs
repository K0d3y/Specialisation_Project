using System;
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
    private void Awake()
    {
        CheckInstance();
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

    [SerializeField] TMP_Text phase_text;
    private EnemyHealthManager enemy;
    private PlayerController player;
    public string currPhase;

    private void Start()
    {
        enemy = GameObject.FindGameObjectWithTag("Enemy").GetComponent<EnemyHealthManager>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        StartGame();
    }
    private void StartGame()
    {
        player.DrawFromDeck(5, "HAND");
        player.DrawFromDeck(8, "RESILIENCE");
    }

    public void DoStartTurn()
    {
        // increase mana
        if (player.maxManaCount < 10)
        {
            player.maxManaCount++;
        }
        player.manaCount = player.maxManaCount;
        player.UpdateManaText();
        // draw card from deck
        player.DrawFromDeck(1, "HAND");
        player.isMyTurn = true;
        UpdatePhaseText("Main");
    }
    public void DoStartAttack()
    {
        UpdatePhaseText("Attack");
    }
    public void DoPassTurn()
    {
        player.isMyTurn = false;
        UpdatePhaseText("End");
    }

    private void UpdatePhaseText(string phase)
    {
        currPhase = phase;
        phase_text.text = currPhase + " Phase";
    }
    public void EnemyTakeDamage(int damage)
    {
        enemy.health -= damage;
        enemy.health_text.text = enemy.health.ToString();
    }
}
