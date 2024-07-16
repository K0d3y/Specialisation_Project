using UnityEngine;

public class RushCA : MonoBehaviour, ICardAbility
{
    public void OnSummon(int i)
    {
        GetComponent<Card>().canAttack = true;
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
