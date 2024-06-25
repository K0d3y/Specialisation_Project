using UnityEngine;

public class RushCA : MonoBehaviour, ICardAbility
{
    public void OnSummon()
    {
        GetComponent<Card>().canAttack = true;
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
