using UnityEngine;

public class EnergyManager : MonoBehaviour
{
    public static EnergyManager Instance { get; private set; }

    public float currentEnergy = 50f;
    public const float maxEnergy = 100f;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (DayNightCycle.Instance.IsNight)
        {
            AddEnergy(10f * Time.deltaTime); // восстановление ночью
        }
    }

    public bool SpendEnergy(float amount)
    {
        if (currentEnergy >= amount)
        {
            currentEnergy -= amount;
            return true;
        }

        if (currentEnergy <= 0)
            DayNightCycle.Instance.ForceNight();

        return false;
    }

    public void AddEnergy(float amount)
    {
        currentEnergy = Mathf.Min(currentEnergy + amount, maxEnergy);
    }
}
