using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIController : MonoBehaviour
{
    [Header("UI Prefabs")]
    public GameObject buildingButtonPrefab;
    public GameObject cameraButtonPrefab;

    [Header("Parents")]
    public Transform buildingButtonParent;
    public Transform cameraButtonParent;

    [Header("Panels")]
    public GameObject buildingPanel;
    public GameObject cameraPanel;

    [Header("YinYang UI")]
    public Image yinFill;
    public Image yangFill;
    public TextMeshProUGUI yinText;
    public TextMeshProUGUI yangText;

    [Header("Chaos UI")]
    public Slider chaosSlider;
    public TextMeshProUGUI chaosText;

    [Header("Energy UI")]
    public Slider energySlider;
    public TextMeshProUGUI energyText;

    private void Start()
    {
        SetupBuildingButtons();
        SetupCameraButtons();

        GameModeManager.Instance.OnModeChanged += HandleModeChanged;
        HandleModeChanged(GameModeManager.Instance.CurrentMode);

        if (ChaosSystem.Instance != null)
        {
            ChaosSystem.Instance.OnChaosChanged.AddListener(UpdateChaosUI);
        }

        // »нициализировать UI при старте
        UpdateYinYangUI();
        UpdateChaosUI(ChaosSystem.Instance?.Chaos ?? 0f);
        UpdateEnergyUI();
    }

    private void Update()
    {
        UpdateYinYangUI();
        UpdateEnergyUI();
    }

    private void OnDestroy()
    {
        if (GameModeManager.Instance != null)
            GameModeManager.Instance.OnModeChanged -= HandleModeChanged;

        if (ChaosSystem.Instance != null)
            ChaosSystem.Instance.OnChaosChanged.RemoveListener(UpdateChaosUI);
    }

    private void HandleModeChanged(GameMode mode)
    {
        buildingPanel.SetActive(mode == GameMode.Building);
        cameraPanel.SetActive(mode == GameMode.Editing || mode == GameMode.Building);
    }

    private void SetupBuildingButtons()
    {
        var buildings = BuildingSystem.Instance.buildingOptions;

        foreach (Transform child in buildingButtonParent)
            Destroy(child.gameObject);

        for (int i = 0; i < buildings.Length; i++)
        {
            int index = i;
            GameObject btnObj = Instantiate(buildingButtonPrefab, buildingButtonParent);
            Image icon = btnObj.GetComponentInChildren<Image>();
            if (icon != null)
                icon.sprite = buildings[i].icon;

            Button btn = btnObj.GetComponent<Button>();
            if (btn != null)
                btn.onClick.AddListener(() =>
                {
                    BuildingSystem.Instance.SetBuildingIndex(index);
                });
        }
    }

    private void SetupCameraButtons()
    {
        var presets = CameraController.Instance.presets;

        foreach (Transform child in cameraButtonParent)
            Destroy(child.gameObject);

        for (int i = 0; i < presets.Length; i++)
        {
            int index = i;
            GameObject btnObj = Instantiate(cameraButtonPrefab, cameraButtonParent);
            Text label = btnObj.GetComponentInChildren<Text>();
            if (label != null)
                label.text = presets[i].name;

            Button btn = btnObj.GetComponent<Button>();
            if (btn != null)
                btn.onClick.AddListener(() =>
                {
                    CameraController.Instance.ApplyPresetByIndex(index);
                });
        }
    }

    private void UpdateYinYangUI()
    {
        if (EnergyBalanceSystem.Instance == null) return;

        float yin = EnergyBalanceSystem.Instance.yin;
        float yang = EnergyBalanceSystem.Instance.yang;
        float total = Mathf.Max(1f, yin + yang);

        float yinPercent = yin / total;
        float yangPercent = yang / total;

        if (yinFill) yinFill.fillAmount = yinPercent;
        if (yangFill) yangFill.fillAmount = yangPercent;

        if (yinText) yinText.text = $"»нь: {(yinPercent * 100f):F0}%";
        if (yangText) yangText.text = $"ян: {(yangPercent * 100f):F0}%";
    }

    private void UpdateChaosUI(float chaos)
    {
        float max = ChaosSystem.Instance != null ? ChaosSystem.Instance.MaxChaos : 100f;

        if (chaosSlider) chaosSlider.value = chaos / max;
        if (chaosText) chaosText.text = $"’аос: {chaos:F1}%";
    }

    private void UpdateEnergyUI()
    {
        if (EnergyManager.Instance == null) return;

        float energy = EnergyManager.Instance.currentEnergy;

        if (energySlider) energySlider.value = energy / EnergyManager.maxEnergy;
        if (energyText) energyText.text = $"Ёнерги€: {energy:F0}";
    }
}
