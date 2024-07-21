using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recursive : Maze
{
    public override void Generate()
    {
        Generate(5, 5, 5);
    }

    void Generate(float x, float y, float z)// I think I need to make this 3d here.
    {
        if (CountSquareNeighbours((int)x, (int)y, (int)z) >= 2) return;
        map[(int)x, (int)y, (int)z] = 0;

        directions.Shuffle();

        Generate(x + directions[0].x, y + directions[0].y, z + directions[0].z);
        Generate(x + directions[1].x, y + directions[1].y, z + directions[1].z);
        Generate(x + directions[2].x, y + directions[2].y, z + directions[2].z);
        Generate(x + directions[3].x, y + directions[3].y, z + directions[3].z);

        Generate(x + directions[2].x, y + directions[4].y, z + directions[4].z);
        Generate(x + directions[3].x, y + directions[5].y, z + directions[5].z);
    }

}