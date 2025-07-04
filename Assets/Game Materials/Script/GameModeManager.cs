using UnityEngine;

public enum GameMode
{
    Default,
    Editing,
    Building
}

public class GameModeManager : MonoBehaviour
{
    public static GameModeManager Instance { get; private set; }

    public GameMode CurrentMode { get; private set; } = GameMode.Default;
    public System.Action<GameMode> OnModeChanged;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void SetMode(GameMode mode)
    {
        CurrentMode = mode;
        Debug.Log("Режим изменён на: " + mode);

        MainCell.ResetAllVisuals();

        if (mode == GameMode.Building)
            BuildingSystem.Instance.RestoreSelectedCellVisual();

        // 🔥 ВАЖНО: уведомляем всех, кто подписан
        OnModeChanged?.Invoke(mode);
    }

    public void BackToDefault() => SetMode(GameMode.Default);
    public void EnterEditMode() => SetMode(GameMode.Editing);
    public void EnterBuildMode() => SetMode(GameMode.Building);
}
