using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RushOnPromote : MonoBehaviour, ICardAbility
{
    public void OnSummon()
    {
    }
    public void OnPromote()
    {
        GetComponent<Card>().canAttack = true;
    }
    public void OnAttack()
    {
    }
    public void OnEndTurn()
    {
    }
}
