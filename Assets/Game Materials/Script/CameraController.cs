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

    [Header("Transition Settings")]
    public float moveSpeed = 5f;
    public float rotateSpeed = 5f;

    private Camera cam;
    private Vector3 targetPosition;
    private Quaternion targetRotation;
    private bool transitioning = false;

    private int currentIndex = 0;

    private void Awake()
    {
        Instance = this;
        cam = Camera.main;
    }

    private void Start()
    {
        ApplyPresetInstantly(defaultPresetIndex);
    }

    private void Update()
    {
        if (!transitioning)
        {
            for (int i = 0; i < presets.Length; i++)
            {
                if (Input.GetKeyDown(presets[i].hotkey))
                {
                    ApplyPreset(i);
                    return;
                }
            }

            if (GameModeManager.Instance.CurrentMode != GameMode.Building)
            {
                if (Input.GetKeyDown(KeyCode.LeftArrow)) CyclePreset(-1);
                if (Input.GetKeyDown(KeyCode.RightArrow)) CyclePreset(1);
            }
        }

        if (transitioning)
        {
            cam.transform.position = Vector3.Lerp(cam.transform.position, targetPosition, Time.deltaTime * moveSpeed);
            cam.transform.rotation = Quaternion.Slerp(cam.transform.rotation, targetRotation, Time.deltaTime * rotateSpeed);

            if (Vector3.Distance(cam.transform.position, targetPosition) < 0.01f &&
                Quaternion.Angle(cam.transform.rotation, targetRotation) < 0.5f)
            {
                cam.transform.position = targetPosition;
                cam.transform.rotation = targetRotation;
                transitioning = false;
            }
        }
    }

    public void ApplyPreset(int index)
    {
        if (index < 0 || index >= presets.Length) return;

        currentIndex = index;
        CameraPreset preset = presets[index];
        targetPosition = preset.position;
        targetRotation = Quaternion.Euler(preset.rotation);
        transitioning = true;

        Debug.Log($"Камера: {preset.name}");
    }

    public void ApplyPresetByIndex(int index)
    {
        ApplyPreset(index);
    }

    public void CyclePreset(int direction)
    {
        currentIndex += direction;
        if (currentIndex < 0) currentIndex = presets.Length - 1;
        else if (currentIndex >= presets.Length) currentIndex = 0;

        ApplyPreset(currentIndex);
    }

    public void ApplyPresetInstantly(int index)
    {
        if (index < 0 || index >= presets.Length) return;

        currentIndex = index;
        CameraPreset preset = presets[index];
        cam.transform.position = preset.position;
        cam.transform.rotation = Quaternion.Euler(preset.rotation);
    }
}
