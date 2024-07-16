using UnityEngine;

public class RushOnPromote : MonoBehaviour, ICardAbility
{
    public void OnSummon(int i)
    {
    }
    public void OnPromote(int i)
    {
        GetComponent<Card>().canAttack = true;
    }
    public void OnAttack(int i)
    {
    }
    public void OnEndTurn(int i)
    {
    }
}
