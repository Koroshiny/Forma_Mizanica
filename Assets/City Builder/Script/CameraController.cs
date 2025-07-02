using UnityEngine;

[System.Serializable]
public class CameraPreset
{
    public string name;
    public Vector3 position;
    public Vector3 rotation;
    public KeyCode hotkey = KeyCode.None;
}

public class CameraController : MonoBehaviour
{
    public static CameraController Instance { get; private set; }

    public CameraPreset[] presets;
    public int defaultPresetIndex = 0;

    private Camera cam;

    private void Awake()
    {
        Instance = this;
        cam = Camera.main;
    }

    private void Start()
    {
        ApplyDefaultPreset();
    }

    public void ApplyDefaultPreset()
    {
        ApplyPresetByIndex(defaultPresetIndex);
    }

    private void Update()
    {
        if (GameModeManager.Instance.CurrentMode != GameMode.Building)
        {
            // Клавиши 1–5
            for (int i = 0; i < presets.Length; i++)
            {
                if (Input.GetKeyDown(presets[i].hotkey))
                {
                    ApplyPreset(presets[i]);
                    return;
                }
            }

            // Стрелки ? ?
            if (Input.GetKeyDown(KeyCode.LeftArrow))
                CyclePreset(-1);
            else if (Input.GetKeyDown(KeyCode.RightArrow))
                CyclePreset(1);
        }
    }

    private int currentIndex = 0;

    public void ApplyPreset(CameraPreset preset)
    {
        cam.transform.position = preset.position;
        cam.transform.rotation = Quaternion.Euler(preset.rotation);
        Debug.Log($"Камера: {preset.name}");
    }

    public void ApplyPresetByIndex(int index)
    {
        if (index < 0 || index >= presets.Length) return;
        currentIndex = index;
        ApplyPreset(presets[currentIndex]);
    }

    public void CyclePreset(int direction)
    {
        currentIndex += direction;
        if (currentIndex < 0) currentIndex = presets.Length - 1;
        else if (currentIndex >= presets.Length) currentIndex = 0;

        ApplyPreset(presets[currentIndex]);
    }
}
