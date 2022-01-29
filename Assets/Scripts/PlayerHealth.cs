using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField]
    private Image _fillImage = null;

    public void SetFillAmount(float fillAmount)
    {
        _fillImage.fillAmount = fillAmount;
    }
}
