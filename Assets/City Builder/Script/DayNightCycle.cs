using UnityEngine;
using UnityEngine.Events;

public class DayNightCycle : MonoBehaviour
{
    public static DayNightCycle Instance { get; private set; }

    [Header("Timing")]
    [SerializeField] private float _dayDuration = 90f;
    [SerializeField] private float _nightDuration = 90f;

    [Header("Lighting")]
    [SerializeField] private Light _directionalLight;
    [SerializeField] private Color _dayLightColor = new Color(1f, 0.9f, 0.6f);
    [SerializeField] private Color _nightLightColor = new Color(0.4f, 0.6f, 1f);

    public UnityEvent OnDayStart;
    public UnityEvent OnNightStart;

    private float _timer;
    private bool _isDay = true;

    public bool IsDay => _isDay;
    public bool IsNight => !_isDay;
    public float DayProgress => _timer / _dayDuration;
    public float NightProgress => _timer / _nightDuration;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    void Update()
    {
        _timer += Time.deltaTime;
        float cycleDuration = _isDay ? _dayDuration : _nightDuration;

        if (_timer >= cycleDuration)
        {
            SwitchPhase();
        }

        UpdateLighting();
    }

    private void SwitchPhase()
    {
        _isDay = !_isDay;
        _timer = 0f;

        if (_isDay) OnDayStart?.Invoke();
        else OnNightStart?.Invoke();
    }

    private void UpdateLighting()
    {
        if (_directionalLight == null) return;

        Color targetColor = _isDay ? _dayLightColor : _nightLightColor;
        _directionalLight.color = Color.Lerp(_directionalLight.color, targetColor, Time.deltaTime * 2f);
    }

    public void ForceNight()
    {
        if (_isDay)
        {
            _isDay = false;
            _timer = 0f;
            ChaosSystem.Instance.AddChaos(20f);
            OnNightStart?.Invoke();
        }
    }
}