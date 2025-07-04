using UnityEngine;
using UnityEngine.UI;

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

    private void Start()
    {
        SetupBuildingButtons();
        SetupCameraButtons();

        GameModeManager.Instance.OnModeChanged += HandleModeChanged;
        HandleModeChanged(GameModeManager.Instance.CurrentMode);
    }


    private void OnDestroy()
    {
        if (GameModeManager.Instance != null)
            GameModeManager.Instance.OnModeChanged -= HandleModeChanged;
    }

    private void HandleModeChanged(GameMode mode)
    {
        // BuildingPanel Ч только в режиме строительства
        buildingPanel.SetActive(mode == GameMode.Building);

        // CameraPanel Ч в редактировании или строительстве
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
}
