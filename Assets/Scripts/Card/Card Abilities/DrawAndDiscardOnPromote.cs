using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawAndDiscardOnPromote : MonoBehaviour, ICardAbility
{
    public void OnSummon()
    {
    }
    public void OnPromote()
    {
        GameplayManager.Instance.DrawCard(1);
    }
    public void OnAttack()
    {
    }
    public void OnEndTurn()
    {
    }
}
