using UnityEngine;

public class EnergyBalanceSystem : MonoBehaviour
{
    public static EnergyBalanceSystem Instance { get; private set; }

    public float yin = 0f;
    public float yang = 0f;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        InvokeRepeating("UpdateBalance", 1f, 1f); // раз в секунду
    }

    public void AddYin(float amount) => yin += amount;
    public void AddYang(float amount) => yang += amount;

    void UpdateBalance()
    {
        float total = yin + yang;
        if (total == 0) total = 1f;

        // Добавляем мягко в зависимости от времени суток
        if (DayNightCycle.Instance.IsNight)
            yin += 0.05f;
        else
            yang += 0.05f;

        float imbalance = Mathf.Abs(yin - yang) / total;
        if (imbalance > 0.2f)
        {
            ChaosSystem.Instance.AddChaos(imbalance * 5f);
        }
    }

    public bool IsBalanced()
    {
        float total = yin + yang;
        if (total == 0f) return true;
        return Mathf.Abs(yin - yang) / total <= 0.2f;
    }
}
