using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Grid : MonoBehaviour
{
    const int Size = 3;
    [SerializeField] List<Transform> GridObjects = new List<Transform>();
    public void Awake()
    {

       ApplyChanges();

        Test(GridObjects[0], Direction.Row);
        Test(GridObjects[0], Direction.Collum);

    }

    public void Test(Transform obj, Direction dir)
    {
        var testlist = GetList(obj, dir);

        string result = string.Empty;

        foreach (Transform t in testlist)
        {
            result += t.name + "\n";
        }

        Debug.Log(result);
    }

    public List<Transform> GetList(Transform t, Direction d)
    {
        int index = GridObjects.IndexOf(t);

        if (index == -1)
            throw new System.Exception($"object {t} is not part of the grid");

        if (d == Direction.Collum)
        {

            index = FloorTo(index, 3);

            return new List<Transform> {
                GridObjects[index],
                GridObjects[index + 1],
                GridObjects[index + 2]
            };
        }
        else
        {
            index %= 3;

            return new List<Transform> {
                GridObjects[index],
                GridObjects[index + 3],
                GridObjects[index + 6]
            };
        }
    }

    private int FloorTo(int source, int to)
    {
        return source - (source % to);
    }

    internal (int VectorIndex, int crossIndex, Vector3 Dir,Vector3 Cross) GetInfo(Direction direction)
    {
        if (direction != Direction.Collum)
            return (0,2, Vector3.right, Vector3.forward);
        return (2,0, Vector3.forward, Vector3.right);
    }

    internal void ApplyChanges()
    {
        GridObjects = GridObjects.OrderBy(x => x.position.x).ThenBy(x => x.position.z).ToList();
    }
}

public enum Direction
{
    Row,
    Collum
}
