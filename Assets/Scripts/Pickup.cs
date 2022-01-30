using UnityEngine;
using UnityEngine.Events;

public class Pickup : MonoBehaviour
{
    public PickupEvent OnPickup = new PickupEvent();

    public bool CanPickUp { get; set; } = true;

    public void IssuePickupEvent()
    {
        OnPickup?.Invoke(this);
    }
}
