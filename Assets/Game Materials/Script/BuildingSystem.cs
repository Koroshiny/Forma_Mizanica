using UnityEngine;

public class BuildingSystem : MonoBehaviour
{
    public static BuildingSystem Instance { get; private set; }

    [Header("Settings")]
    public BuildingData[] buildingOptions;
    public Transform buildingParent;

    private int selectedIndex = 0;
    private MainCell selectedCell;

    [SerializeField] private float destroyRefund = 5f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Update()
    {
        // Кнопка Q работает в любом режиме
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (selectedCell != null)
            {
                selectedCell.Deselect();
                selectedCell = null;
            }
            GameModeManager.Instance.BackToDefault();
            return;
        }

        if (GameModeManager.Instance.CurrentMode != GameMode.Building)
            return;

        HandleControls();
    }

    private void HandleControls()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            selectedIndex = (selectedIndex - 1 + buildingOptions.Length) % buildingOptions.Length;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            selectedIndex = (selectedIndex + 1) % buildingOptions.Length;
        }

        if (Input.GetKeyDown(KeyCode.B)) // Build
        {
            TryBuild();
        }

        if (Input.GetKeyDown(KeyCode.D)) // Destroy
        {
            TryDestroy();
        }

        if (Input.GetKeyDown(KeyCode.Z)) // Back to Editing
        {
            GameModeManager.Instance.EnterEditMode();
        }

        if (Input.GetKeyDown(KeyCode.Q)) // Back to Default
        {
            if (selectedCell != null)
            {
                selectedCell.Deselect();
                selectedCell = null;
            }
            GameModeManager.Instance.BackToDefault();
        }
    }

    public void SetSelectedCell(MainCell cell)
    {
        if (selectedCell != null)
            selectedCell.Deselect();

        selectedCell = cell;
        cell.Select();
    }

    public void RestoreSelectedCellVisual()
    {
        if (selectedCell != null)
        {
            selectedCell.Select();
        }
    }

    private void TryBuild()
    {
        if (selectedCell == null || selectedCell.isBuilt)
            return;

        var data = buildingOptions[selectedIndex];
        GameObject obj = Instantiate(data.prefab, selectedCell.transform.position, Quaternion.identity, buildingParent);

        obj.transform.localScale = Vector3.one * 0.85f;
        selectedCell.currentBuilding = data;
        selectedCell.isBuilt = true;

        EnergyBalanceSystem.Instance.AddYin(data.yinAmount);
        EnergyBalanceSystem.Instance.AddYang(data.yangAmount);

        Debug.Log($"Построено: {data.type} (+инь: {data.yinAmount}, +ян: {data.yangAmount})");

        // 💥 ПРОВЕРКА энергии
        if (!EnergyManager.Instance.SpendEnergy(data.energyCost))
        {
            Debug.Log("Недостаточно энергии");
            return;
        }

    }

    private void TryDestroy()
    {
        if (selectedCell == null || !selectedCell.isBuilt)
            return;

        foreach (Transform child in buildingParent)
        {
            if (Vector3.Distance(child.position, selectedCell.transform.position) < 0.1f)
            {
                Destroy(child.gameObject);
                break;
            }
        }

        if (selectedCell.currentBuilding != null)
        {
            EnergyBalanceSystem.Instance.AddYin(-selectedCell.currentBuilding.yinAmount);
            EnergyBalanceSystem.Instance.AddYang(-selectedCell.currentBuilding.yangAmount);
        }

        selectedCell.isBuilt = false;
        selectedCell.currentBuilding = null;

        Debug.Log("Здание удалено.");

        EnergyManager.Instance.AddEnergy(5f); // Фиксированный возврат

    }

    public void SetBuildingIndex(int index)
    {
        if (index < 0 || index >= buildingOptions.Length) return;
        selectedIndex = index;
        Debug.Log("Выбрано здание: " + buildingOptions[index].type);
    }
}
