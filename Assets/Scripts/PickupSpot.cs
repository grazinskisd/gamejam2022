using UnityEngine;

public class PickupSpot : MonoBehaviour
{
    public Pickup[] pickups;
    public PickupEvent OnPickup = new PickupEvent();

    private void Awake()
    {
        for (int i = 0; i < pickups.Length; i++)
        {
            pickups[i].OnPickup.AddListener(IssuePickupEvent);
        }
    }

    private void IssuePickupEvent(Pickup arg0)
    {
        OnPickup?.Invoke(arg0);
    }

    public bool IsPickupAllowed
    {
        set
        {
            for (int i = 0; i < pickups.Length; i++)
            {
                var pickup = pickups[i];
                if(pickup != null)
                {
                    pickup.CanPickUp = value;
                }
            }
        }
    }
}
