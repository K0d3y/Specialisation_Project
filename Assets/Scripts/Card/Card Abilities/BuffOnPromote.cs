using UnityEngine;

public class BuffOnPromote : MonoBehaviour, ICardAbility
{
    public void OnSummon(int i)
    {
    }
    public void OnPromote(int i)
    {
        GameplayManager.Instance.BuffCardValues(0, 1, 1);
        GameplayManager.Instance.BuffCardValues(1, 1, 1);
        GameplayManager.Instance.BuffCardValues(2, 1, 1);
        GameplayManager.Instance.BuffCardValues(3, 1, 1);
        GameplayManager.Instance.BuffCardValues(4, 1, 1);
    }
    public void OnAttack(int i)
    {
    }
    public void OnEndTurn(int i)
    {
    }
}
