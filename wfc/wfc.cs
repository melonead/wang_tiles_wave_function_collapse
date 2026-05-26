
using System;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using grid;
using Ruleset;
using System.Linq.Expressions;
using System.Data;
using Microsoft.Xna.Framework.Graphics;
using System.Threading;


public class Wfc
{

    public Grid gd;
    public int[] waveFunction = new int[900];
    private RuleSet ruleSet;
    public int leastEntropyTile = 0;

    private int[, ] adjacentTileDisplacements = {{-1, 0}, {1, 0}, {0, 1}, {0, -1}};

    Random randomGen = new Random();

    public Wfc()
    {
        gd = new Grid();
        ruleSet = new RuleSet();

        int allPossibilitiesBitMask = 0;

        allPossibilitiesBitMask |= (1 << 15);
        for (int i = 0; i < 15; ++i)
        {
            allPossibilitiesBitMask |= (1 << i);
        }

        for (int i = 0; i < 900; ++i)
        {
            waveFunction[i] = allPossibilitiesBitMask; // At the beginning each cell has possibility of having any of the possible 16 tiles.
        }
    }

    public void propagate(Vector2 position)
    {
        Stack<Vector2> tileStack = new Stack<Vector2>();
        tileStack.Push(position);

        int index = getIndexOfPosition(position);

        collapseTile(index);

        while (tileStack.Count > 0)
        {
            Vector2 currentPosition = tileStack.Pop();
            index = getIndexOfPosition(currentPosition);

            for (int i = 0; i < 4; ++i)
            {
                float x = currentPosition.X + adjacentTileDisplacements[i, 0] * gd.cellSize;
              sdfsd  float y = currentPosition.Y + adjacentTileDisplacements[i, 1] * gd.cellSize;
                Vector2 neighborPosition = new Vector2(x, y);

                if (neighborPosition.X < 0 || neighborPosition.X > gd.worldWidth || neighborPosition.Y < 0 || neighborPosition.Y > gd.worldHeight)
                {
                    continue;
                }

                    

                int neighborIndex = getIndexOfPosition(neighborPosition);

                int beforePossibilities = waveFunction[neighborIndex];
                // Evaluate the neighbor possibilities

                if (ruleSet.isConnectedDown(waveFunction[index]))
                {
                    // neighbor should connect up
                    waveFunction[neighborIndex] = waveFunction[neighborIndex] & ruleSet.tilesConnectingUpMask;
                }

                if (ruleSet.isConnectedLeft(waveFunction[index]))
                {
                    // neighbor should connnect right
                    waveFunction[neighborIndex] = waveFunction[neighborIndex] & ruleSet.tilesConnectingRightMask;
                }

                if (ruleSet.isConnectedRight(waveFunction[index]))
                {
                    // neighbor should connect left
                    waveFunction[neighborIndex] = waveFunction[neighborIndex] & ruleSet.tilesConnectingLeftMask;
                }

                if (ruleSet.isConnectedUp(waveFunction[index]))
                {
                    // neighbor should connect down
                    waveFunction[neighborIndex] = waveFunction[neighborIndex] & ruleSet.tilesConnectingDownMask;
                }

                // If the neighbor possibilities have changed, push then to the stack
                int afterPossibilities = waveFunction[neighborIndex];

                if (afterPossibilities != beforePossibilities)
                {
                    tileStack.Push(neighborPosition);
                }
            }
        }
    }

    public int collapseTile(int index)
    {
        int possibilities = waveFunction[index];
        List<int> possibleTiles = new List<int>();

        for (int i = 0; i < 16; ++i)
        {
            if (ruleSet.isBitSet(possibilities, i))
            {
                possibleTiles.Add(i);
            }
        }

        int ind = randomGen.Next(possibleTiles.Count);

        waveFunction[index] = possibilities & (1 << possibleTiles[ind]);

        return possibleTiles[ind];
    } 

    public bool isWaveCollapsed()
    {
        return false;
    }

    public bool isTileCollapsed(int index)
    {
        return countSetBits(index) == 1;
    }

    public int countSetBits(int index)
    {
        return System.Numerics.BitOperations.PopCount((uint) waveFunction[index]);
    }

    public void visualizeWaveFunction(SpriteBatch sp, Texture2D tileSet)
    {
        int leastEntropy = 9999;
        for (int i = 0; i < gd.size; ++i)
        {
            int possibilitiesCount = countSetBits(i);

            if (possibilitiesCount < leastEntropy)
            {
                leastEntropyTile = i;
                leastEntropy = possibilitiesCount;
            }

            int cellTile = 0;
            if (possibilitiesCount == 1) // This tile has collapsed get the value
            {
                cellTile = waveFunction[i];
            }
            Rectangle sourceRect = new Rectangle((int) gd.cellSize * cellTile, 0, (int) gd.cellSize, (int) gd.cellSize);
            Rectangle destinationRect = new Rectangle((int) gd.cells[i].X, (int) gd.cells[i].Y, (int) gd.cellSize, (int) gd.cellSize);

            sp.Begin();
            sp.Draw(tileSet, destinationRect, sourceRect, Color.White);
            sp.End();
        }
    }

    public int getIndexOfPosition(Vector2 position)
    {
        int gridX = (int) (position.X / gd.cellSize);
        int gridY = (int) (position.Y / gd.cellSize);

        return (int) (gridY * gd.gridWidth + gridX);
    }


}