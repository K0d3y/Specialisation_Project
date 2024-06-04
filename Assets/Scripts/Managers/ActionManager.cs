using System;
using UnityEngine;

public class ActionManager : MonoBehaviour
{
    private static ActionManager _instance;

    public static ActionManager Instance
    {
        get
        {
            if (_instance == null)
            {
                // Find existing instance in scene
                _instance = FindObjectOfType<ActionManager>();

                if (_instance == null)
                {
                    // Create new GameObject
                    GameObject obj = new("ActionManager");

                    // Add EntityManager component to the GameObject
                    _instance = obj.AddComponent<ActionManager>();
                }
            }
            return _instance;
        }
    }

    public Action DrawCardEvent;

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
}