using UnityEngine;

public class DepositSpot : MonoBehaviour
{
    public PickupEvent OnDeposit = new PickupEvent();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        OnDeposit?.Invoke(null);
    }
}