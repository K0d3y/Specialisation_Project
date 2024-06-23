using UnityEngine;

public class RushCA : MonoBehaviour, ICardAbility
{
    public void OnPlay()
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
