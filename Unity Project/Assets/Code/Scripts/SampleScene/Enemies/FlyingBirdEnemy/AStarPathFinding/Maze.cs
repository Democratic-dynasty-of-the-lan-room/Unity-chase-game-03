using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapLocation
{
    public float x;
    public float y;
    public float z;

    public MapLocation(float _x, float _y, float _z)
    {
        x = _x;
        y = _y;
        z = _z;
    }

    public Vector3 ToVector()
    {
        return new Vector3(x, y, z);
    }

    public static MapLocation operator +(MapLocation a, MapLocation b)
       => new MapLocation(a.x + b.x, a.y + b.y, a.z + b.z);

}

public class Maze : MonoBehaviour
{
    public List<MapLocation> directions = new List<MapLocation>() {
                                       new MapLocation(1, 0, 0),
                                       new MapLocation(-1, 0, 0), 
                                       new MapLocation(0, 0, 1),  
                                       new MapLocation(0, 0, -1), 
                                       new MapLocation(0, 1, 0),  
                                       new MapLocation(0, -1, 0) };
    public int width = 30; //x length
    public int height = 30; //y length
    public int depth = 30; //z length 
    public byte[,,] map;
    public int scale = 6;

    // Start is called before the first frame update
    void Start()
    {
        InitialiseMap();
        Generate();
        DrawMap();
    }

    void InitialiseMap()
    {
        map = new byte[width, height, depth];
        for (int z = 0; z < depth; z++)
            for (int y = 0; y < height; y++)
                for (int x = 0; x < width; x++)
                {
                    map[x, y, z] = 1;     //1 = wall  0 = corridor
                }


    }

    public virtual void Generate()
    {
        for (int z = 0; z < depth; z++)
            for (int y = 0; y < height; y++)
                for (int x = 0; x < width; x++)
                {
                    if (Random.Range(0, 100) < 50)
                    map[x, y, z] = 0;     //1 = wall  0 = corridor
                }
    }

    void DrawMap()
    {
        for (int z = 0; z < depth; z++)
            for (int y = 0; y < height; z++)
                for (int x = 0; x < width; x++)                
                    if (map[x, y, z] == 1)
                    {
                        Vector3 pos = new Vector3(x * scale, y * scale, z * scale);
                        GameObject wall = GameObject.CreatePrimitive(PrimitiveType.Cube);
                        wall.transform.localScale = new Vector3(scale, scale, scale);
                        wall.transform.position = pos;
                    }
    }

    public int CountSquareNeighbours(int x, int y, int z)
    {
        int count = 0;
        if (x <= 0 || x >= width - 1 || y <= 0 || y >= height - 1 || z <= 0 || z >= depth - 1) return 5;
        if (map[x - 1, y, z] == 0) count++;
        if (map[x + 1, y, z] == 0) count++;
        if (map[x, y, z + 1] == 0) count++;
        if (map[x, y, z - 1] == 0) count++;
        if (map[x, y + 1, z] == 0) count++;
        if (map[x, y - 1, z] == 0) count++;
        return count;
    }

    
    public int CountDiagonalNeighbours(int x, int y, int z)
    {
        int count = 0;
        if (x <= 0 || x >= width - 1 || y <= 0 || y >= height - 1 || z <= 0 || z >= depth - 1) return 5;
        if (map[x - 1, y, z - 1] == 0) count++;
        if (map[x + 1, y, z + 1] == 0) count++;
        if (map[x - 1, y, z + 1] == 0) count++;
        if (map[x + 1, y, z - 1] == 0) count++;
        if (map[x - 1, y + 1, z] == 0) count++;
        if (map[x + 1, y - 1, z] == 0) count++;
        return count;
    }
    

    public int CountAllNeighbours(int x, int y, int z)
    {
        return CountSquareNeighbours(x, y, z) + CountDiagonalNeighbours(x, y, z);
    }
}