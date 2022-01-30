using UnityEngine;

public class ShootBehaviourLevelController : MonoBehaviour
{
    public BaseShootBehaviour[] levels;
    public int currentLevel;

    private void Awake()
    {
        DisableAll();
        levels[currentLevel].enabled = true;
    }

    public void IncrementLevel()
    {
        DisableAll();
        currentLevel = Mathf.Clamp(currentLevel + 1, 0, levels.Length -1);
        levels[currentLevel].enabled = true;
    }

    private void DisableAll()
    {
        for (int i = 0; i < levels.Length; i++)
        {
            levels[i].enabled = false;
        }
    }
}
