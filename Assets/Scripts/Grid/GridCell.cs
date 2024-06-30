using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCell : MonoBehaviour
{
    public int X { get; private set; }
    public int Z { get; private set; }
    public bool IsOccupied { get; set; }

    private Renderer cellRenderer;

    public void Initialize(int x, int z)
    {
        X = x;
        Z = z;
        IsOccupied = false;
        cellRenderer = GetComponent<Renderer>();
    }

    public void SetColor(Color color)
    {
        cellRenderer.material.color = color;
    }

    public void SetOccupied(bool occupied)
    {
        IsOccupied = occupied;
        SetColor(occupied ? Color.red : Color.green);
    }
}