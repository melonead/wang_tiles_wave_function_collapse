using System;
using Microsoft.Xna.Framework;

namespace grid;
public class Grid
{
    public float worldWidth;
    public float worldHeight;
    public float gridWidth;
    public float gridHeight;
    public float cellSize;
    public Vector2[] cells;
    public int size = 0;
    public Grid()
    {
        worldWidth = 896;
        worldHeight = 896;
        cellSize = 128;
        gridWidth = worldWidth / cellSize;
        gridHeight = worldHeight / cellSize;

        size = (int) (gridWidth * gridHeight);
        cells = new Vector2[size];

        createCells();
    }

    private void createCells()
    {
        for (float y = 0; y < gridHeight; y++ )
        {
            for (float x = 0; x < gridWidth; x++)
            {
                int index = (int) (y * gridWidth + x);
                Vector2 position = new Vector2(x * cellSize, y * cellSize);
                cells[index] = position;
            }
        }
    }

}