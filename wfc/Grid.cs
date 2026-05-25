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
    Vector2[] cells = new Vector2[900];

    public Grid()
    {
        worldWidth = 600;
        worldHeight = 600;
        cellSize = 20;
        gridWidth = worldWidth / cellSize;
        gridHeight = worldHeight / cellSize;

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