using UnityEngine;

public class DrawAndDiscardOnPromote : MonoBehaviour, ICardAbility
{
    public void OnSummon(int i)
    {
    }
    public void OnPromote(int i)
    {
        GameplayManager.Instance.DrawCard(i, 1);
        GameplayManager.Instance.DiscardCard(i);
    }
    public void OnAttack(int i)
    {
    }
    public void OnEndTurn(int i)
    {
    }
}
