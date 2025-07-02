using UnityEngine;
using System.Collections.Generic;

public class GridManager : MonoBehaviour
{
    public GameObject board;
    public GameObject cellPrefab;
    public int gridSize = 6;

    public Dictionary<Vector2Int, MainCell> cells = new();

    void Start()
    {
        GenerateGrid();
    }

    void GenerateGrid()
    {
        Vector3 boardSize = board.transform.localScale;
        Vector3 boardPos = board.transform.position;

        float cellSize = Mathf.Min(boardSize.x, boardSize.z) / gridSize;

        float startX = boardPos.x - (cellSize * gridSize) / 2 + cellSize / 2;
        float startZ = boardPos.z - (cellSize * gridSize) / 2 + cellSize / 2;
        float y = boardPos.y + boardSize.y / 2 + 0.03f;

        for (int x = 0; x < gridSize; x++)
        {
            for (int z = 0; z < gridSize; z++)
            {
                Vector3 pos = new Vector3(startX + x * cellSize, y, startZ + z * cellSize);
                GameObject cellObj = Instantiate(cellPrefab, pos, Quaternion.identity, transform);
                cellObj.transform.localScale = new Vector3(cellSize * 0.98f, 0.05f, cellSize * 0.98f);

                MainCell cell = cellObj.GetComponent<MainCell>();
                cell.Init(new Vector2Int(x, z));
                cells.Add(cell.coordinates, cell);
            }
        }
    }
}
