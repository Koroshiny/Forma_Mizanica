using UnityEngine;
using UnityEngine.Events;

public class ChaosSystem : MonoBehaviour
{
    public static ChaosSystem Instance { get; private set; }

    [SerializeField] private float _maxChaos = 100f;
    private float _chaos = 0f;

    public UnityEvent<float> OnChaosChanged;
    public UnityEvent OnMaxChaosReached;

    public float Chaos => _chaos;
    public float MaxChaos => _maxChaos;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void AddChaos(float amount)
    {
        _chaos = Mathf.Min(_chaos + amount, _maxChaos);
        OnChaosChanged?.Invoke(_chaos);

        if (_chaos >= _maxChaos)
        {
            OnMaxChaosReached?.Invoke();
            Debug.LogError("Поражение: Хаос достиг 100%!");
        }
    }

    public void ResetChaos()
    {
        _chaos = 0f;
        OnChaosChanged?.Invoke(_chaos);
    }
}