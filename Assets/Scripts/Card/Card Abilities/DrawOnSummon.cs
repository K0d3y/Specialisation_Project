using UnityEngine;

public class DrawOnSummon : MonoBehaviour, ICardAbility
{
    public void OnSummon()
    {
        GameplayManager.Instance.DrawCard(1);
    }
    public void OnPromote()
    {
    }
    public void OnAttack()
    {
    }
    public void OnEndTurn()
    {
    }
}
