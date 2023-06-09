using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Selector : MonoBehaviour
{
    private const float size = 19.5f;
    private const float CellSize = 6.5f;
    private const float HalfSize = CellSize / 2;
    [SerializeField] List<Transform> Cells = new List<Transform>();
    [SerializeField] List<float> CellOffsets = new List<float>();

    [SerializeField] float ScrollSpeed = 50;

    float Speed;
    bool Down;

    private Vector3 Dir;
    private Vector3 Cross;
    private int VectorIndex;
    private int CrossIndex;

    public Grid grid;

    private Direction dir;
    void Start()
    {
        grid = FindObjectOfType<Grid>();
    }



    // Update is called once per frame
    void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            dir = (Input.GetKey(KeyCode.LeftShift) ? Direction.Row : Direction.Collum);
            Save();
        }

        if (Down)
        {


            Apply();
        }

        if (Input.GetMouseButtonUp(0))
        {

            SetGrid();
        }

    }

    private void SetGrid()
    {
        if (!Down)
            return;

        Down = false;

        Cells = Cells.OrderBy(e => e.transform.position[VectorIndex]).ToList();

        for (int i = 0; i < Cells.Count; i++)
        {
            Cells[i].position = Dir * (i * CellSize) + Cross * Cells[i].position[CrossIndex];
        }
    }

    private void Apply()
    {
        Speed += Input.mouseScrollDelta.y * Time.deltaTime * ScrollSpeed;

        if (Speed < 0)
        {
            Speed = size;
        }
        for (int i = 0; i < Cells.Count; i++)
        {
            Cells[i].position = Dir * (( - HalfSize) + (Speed + CellOffsets[i]) % size) + Cross * Cells[i].position[CrossIndex];
        }

        grid.ApplyChanges();
    }

    private void Save()
    {

        (VectorIndex, CrossIndex, Dir, Cross) = grid.GetInfo(dir);
        Speed = HalfSize;

        if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition),out RaycastHit hit))
        {
            Cells = grid.GetList(hit.transform, dir);
            CellOffsets = Cells.Select(e => e.transform.position[VectorIndex]).ToList();
            Down = true;
        }

    }
}
