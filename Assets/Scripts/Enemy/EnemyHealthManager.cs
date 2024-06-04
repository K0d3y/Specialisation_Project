using TMPro;
using UnityEngine;

public class EnemyHealthManager : MonoBehaviour
{
    public TMP_Text health_text;
    [SerializeField] public int health = 100;

    private void Start()
    {
        health_text = GetComponent<TMP_Text>();
        health_text.text = health.ToString();
    }
}
