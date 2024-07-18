using UnityEngine;

public class BuffOnPromote : MonoBehaviour, ICardAbility
{
    public void OnSummon(int i)
    {
    }
    public void OnPromote(int i)
    {
        GameplayManager.Instance.BuffCardValues(0 + (i * 6), 1, 1);
        GameplayManager.Instance.BuffCardValues(1 + (i * 6), 1, 1);
        GameplayManager.Instance.BuffCardValues(2 + (i * 6), 1, 1);
        GameplayManager.Instance.BuffCardValues(3 + (i * 6), 1, 1);
        GameplayManager.Instance.BuffCardValues(4 + (i * 6), 1, 1);
    }
    public void OnAttack(int i)
    {
    }
    public void OnEndTurn(int i)
    {
    }
}
