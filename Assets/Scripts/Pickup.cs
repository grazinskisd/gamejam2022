using UnityEngine;
using UnityEngine.Events;

public class Pickup : MonoBehaviour
{
    public PickupEvent OnPickup = new PickupEvent();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        OnPickup?.Invoke(this);
    }
}
