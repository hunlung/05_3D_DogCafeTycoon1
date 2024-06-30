using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementManager : MonoBehaviour
{
    public GameObject gridCellPrefab;
    public GameObject furniturePrefab;
    public int gridWidth = 10;
    public int gridHeight = 10;

    private GameObject[,] grid;
    private GameObject currentFurniture;

    public GameObject CreateGridCellPrefab()
    {
        GameObject cellPrefab = GameObject.CreatePrimitive(PrimitiveType.Quad);
        cellPrefab.name = "GridCell";
        cellPrefab.transform.rotation = Quaternion.Euler(90, 0, 0); // 바닥에 평평하게 놓이도록 회전

        // 셀의 크기 조정 (예: 1x1 유닛)
        cellPrefab.transform.localScale = new Vector3(1, 1, 1);

        // 머티리얼 설정
        Renderer renderer = cellPrefab.GetComponent<Renderer>();
        renderer.material = new Material(Shader.Find("Standard"));
        renderer.material.color = new Color(1f, 1f, 1f, 0.5f); // 반투명한 흰색

        // 콜라이더 추가 (선택적)
        cellPrefab.AddComponent<BoxCollider>();

        return cellPrefab;
    }

    void Start()
    {
        gridCellPrefab = CreateGridCellPrefab();
        CreateGrid();
    }

    void Update()
    {
        if (currentFurniture != null)
        {
            UpdateFurniturePlacement();
        }
    }

    public void CreateGrid()
    {
        GameObject gridParent = new GameObject("Grid");
        grid = new GameObject[gridWidth, gridHeight];

        for (int x = 0; x < gridWidth; x++)
        {
            for (int z = 0; z < gridHeight; z++)
            {
                Vector3 position = new Vector3(x, 0.01f, z); // 바닥 위에 살짝 띄움
                GameObject cell = Instantiate(gridCellPrefab, position, Quaternion.identity, gridParent.transform);
                cell.name = $"Cell_{x}_{z}";
                grid[x, z] = cell;

                // 그리드 셀 스크립트 추가
                GridCell gridCell = cell.AddComponent<GridCell>();
                gridCell.Initialize(x, z);
            }
        }
    }

    void UpdateFurniturePlacement()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            int x = Mathf.RoundToInt(hit.point.x);
            int z = Mathf.RoundToInt(hit.point.z);

            if (x >= 0 && x < gridWidth && z >= 0 && z < gridHeight)
            {
                currentFurniture.transform.position = new Vector3(x, 0, z);

                bool canPlace = CanPlaceFurniture(x, z);
                SetFurnitureColor(canPlace);
                SetGridCellColor(x, z, canPlace);

                if (Input.GetMouseButtonDown(0) && canPlace)
                {
                    PlaceFurniture(x, z);
                }
            }
        }
    }

    bool CanPlaceFurniture(int x, int z)
    {
        // 그리드 범위 확인
        if (x < 0 || x >= gridWidth || z < 0 || z >= gridHeight)
            return false;

        // 셀이 비어있는지 확인
        GridCell cell = grid[x, z].GetComponent<GridCell>();
        return !cell.IsOccupied;
    }

    void SetFurnitureColor(bool canPlace)
    {
        Renderer renderer = currentFurniture.GetComponent<Renderer>();
        renderer.material.color = canPlace ? Color.green : Color.red;
    }

    void SetGridCellColor(int x, int z, bool canPlace)
    {
        GridCell cell = grid[x, z].GetComponent<GridCell>();
        cell.SetColor(canPlace ? Color.green : Color.red);
    }

    void PlaceFurniture(int x, int z)
    {
        GridCell cell = grid[x, z].GetComponent<GridCell>();
        cell.SetOccupied(true);
        currentFurniture = null;
    }
public void StartPlacingFurniture()
    {
        currentFurniture = Instantiate(furniturePrefab);
    }
}