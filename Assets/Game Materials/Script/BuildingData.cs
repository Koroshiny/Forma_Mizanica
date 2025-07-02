using UnityEngine;

[CreateAssetMenu(fileName = "NewBuildingData", menuName = "CityBuilder/Building Data")]
public class BuildingData : ScriptableObject
{
    public BuildingType type;
    public GameObject prefab;
    public float yinAmount;
    public float yangAmount;
    public Sprite icon; // Для UI кнопки
}
