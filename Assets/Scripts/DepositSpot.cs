using UnityEngine;

public class DepositSpot : MonoBehaviour
{
    public PickupEvent OnDeposit = new PickupEvent();
    public Transform[] runeSlots;

    private int _slotIndex;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        InvokeOnDeposit();
    }

    private void InvokeOnDeposit()
    {
        OnDeposit?.Invoke(null);
    }

    public void AddRune(Pickup rune)
    {
        rune.transform.SetParent(runeSlots[_slotIndex]);
        rune.transform.localPosition = Vector3.zero;
        rune.gameObject.SetActive(true);
        _slotIndex++;
        OnDeposit?.Invoke(rune);
    }
}