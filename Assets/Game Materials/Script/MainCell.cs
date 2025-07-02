using UnityEngine;

public class MainCell : MonoBehaviour
{
    public Vector2Int coordinates;

    [Header("Materials")]
    public Material defaultMat;
    public Material hoverMat;
    public Material selectedMat;

    private Renderer rend;
    public bool isBuilt = false;
    public bool isSelected = false;
    public bool isHovered = false;

    public BuildingData currentBuilding;

    void Awake()
    {
        rend = GetComponent<Renderer>();
        rend.material = defaultMat;
    }

    public void Init(Vector2Int coords)
    {
        coordinates = coords;
    }

    void OnMouseEnter()
    {
        if (GameModeManager.Instance.CurrentMode == GameMode.Default)
        {
            HighlightAll(true);
        }
        else if (GameModeManager.Instance.CurrentMode == GameMode.Editing && !isBuilt)
        {
            isHovered = true;
            UpdateMaterial();
        }
    }

    void OnMouseExit()
    {
        if (GameModeManager.Instance.CurrentMode == GameMode.Default)
        {
            HighlightAll(false);
        }
        else if (GameModeManager.Instance.CurrentMode == GameMode.Editing && !isBuilt)
        {
            isHovered = false;
            UpdateMaterial();
        }
    }

    void OnMouseDown()
    {
        if (GameModeManager.Instance.CurrentMode == GameMode.Default)
        {
            GameModeManager.Instance.EnterEditMode();
            CameraController.Instance.ApplyPresetByIndex(0); // Вид сверху
        }
        else if (GameModeManager.Instance.CurrentMode == GameMode.Editing && !isSelected && !isBuilt)
        {
            BuildingSystem.Instance.SetSelectedCell(this);
            Select();
            GameModeManager.Instance.EnterBuildMode();
        }
    }

    public void Select()
    {
        isSelected = true;
        UpdateMaterial();
    }

    public void Deselect()
    {
        isSelected = false;
        UpdateMaterial();
    }

    public void UpdateMaterial()
    {
        if (isSelected)
            rend.material = selectedMat;
        else if (isHovered)
            rend.material = hoverMat;
        else
            rend.material = defaultMat;
    }

    private void HighlightAll(bool highlight)
    {
        foreach (var cell in FindObjectsOfType<MainCell>())
        {
            if (!cell.isBuilt && !cell.isSelected)
            {
                cell.isHovered = highlight;
                cell.UpdateMaterial();
            }
        }
    }

    public static void ResetAllVisuals()
    {
        foreach (var cell in FindObjectsOfType<MainCell>())
        {
            cell.isHovered = false;
            cell.isSelected = false;
            cell.UpdateMaterial();
        }
    }
}
