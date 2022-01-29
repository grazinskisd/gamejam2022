using UnityEngine;
using UnityEngine.Events;

public class Pickup : MonoBehaviour
{
    public PickupEvent OnPickup = new PickupEvent();

    public void IssuePickupEvent()
    {
        OnPickup?.Invoke(this);
    }
}
