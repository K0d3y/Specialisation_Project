using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffOnPromote : MonoBehaviour, ICardAbility
{
    public void OnSummon()
    {
    }
    public void OnPromote()
    {
        GameplayManager.Instance.BuffCardValues(0, 1, 1);
        GameplayManager.Instance.BuffCardValues(1, 1, 1);
        GameplayManager.Instance.BuffCardValues(2, 1, 1);
        GameplayManager.Instance.BuffCardValues(3, 1, 1);
        GameplayManager.Instance.BuffCardValues(4, 1, 1);
    }
    public void OnAttack()
    {
    }
    public void OnEndTurn()
    {
    }
}
