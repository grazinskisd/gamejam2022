using UnityEngine;
using UnityEngine.Events;

public class Pickup : MonoBehaviour
{
    public PickupEvent OnPickup = new PickupEvent();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        IssuePickupEvent();
    }

    public void IssuePickupEvent()
    {
        OnPickup?.Invoke(this);
    }
}
