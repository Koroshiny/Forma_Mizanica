using UnityEngine;
using UnityEngine.UI;
using TMPro; // если используешь TextMeshPro

public class UIController : MonoBehaviour
{
    [Header("UI Prefabs")]
    public GameObject buildingButtonPrefab;
    public GameObject cameraButtonPrefab;

    [Header("Parents")]
    public Transform buildingButtonParent;
    public Transform cameraButtonParent;

    private void Start()
    {
        SetupBuildingButtons();
        SetupCameraButtons();
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
