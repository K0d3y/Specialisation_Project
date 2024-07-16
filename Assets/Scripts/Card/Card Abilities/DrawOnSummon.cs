using UnityEngine;

public class DrawOnSummon : MonoBehaviour, ICardAbility
{
    public void OnSummon(int i)
    {
        GameplayManager.Instance.DrawCard(i, 1);
    }
    public void OnPromote(int i)
    {
    }
    public void OnAttack(int i)
    {
    }
    public void OnEndTurn(int i)
    {
    }
}
