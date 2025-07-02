using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyBalanceSystem : MonoBehaviour
{
    public static EnergyBalanceSystem Instance { get; private set; }

    public float yin = 0f;
    public float yang = 0f;

    void Awake() => Instance = this;

    void Start()
    {
        InvokeRepeating("UpdateBalance", 1f, 1f); // Вызываем UpdateBalance раз в секунду
    }

    public void AddYin(float amount) => yin += amount;
    public void AddYang(float amount) => yang += amount;

    public void UpdateBalance()
    {
        float total = yin + yang;
        if (total == 0) return;

        float imbalance = Mathf.Abs(yin - yang) / total;
        if (imbalance > 0.2f)
            ChaosSystem.Instance.AddChaos(imbalance * 5f * Time.deltaTime);
    }

    public bool IsBalanced() => Mathf.Abs(yin - yang) / (yin + yang) <= 0.2f;
}
