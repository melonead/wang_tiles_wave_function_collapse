
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
    public int[] waveFunction;
    private RuleSet ruleSet;
    public int leastEntropyTile = 0;
    int allPossibilitiesBitMask = 0;
    bool waveState = false;

    private int[, ] adjacentTileDisplacements = {{0, -1}, {1, 0}, {0, 1}, {-1, 0}};

    Random randomGen = new Random();

    public Wfc()
    {
        gd = new Grid();
        ruleSet = new RuleSet();
        waveFunction = new int[gd.size];

        

        allPossibilitiesBitMask |= (1 << 15);
        for (int i = 0; i < 15; ++i)
        {
            allPossibilitiesBitMask |= (1 << i);
        }

        for (int i = 0; i < gd.size; ++i)
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
                float y = currentPosition.Y + adjacentTileDisplacements[i, 1] * gd.cellSize;
                Vector2 neighborPosition = new Vector2(x, y);

                if (neighborPosition.X < 0 || neighborPosition.X >= gd.worldWidth || neighborPosition.Y < 0 || neighborPosition.Y >= gd.worldHeight)
                {
                    continue;
                }

                int cellTile = getIndexOfSetBit(waveFunction[index]);
                bool a = ruleSet.isConnectedDown(waveFunction[index]);
                bool b = ruleSet.isConnectedLeft(waveFunction[index]);
                bool c = ruleSet.isConnectedRight(waveFunction[index]);
                bool d = ruleSet.isConnectedUp(waveFunction[index]);


                    

                int neighborIndex = getIndexOfPosition(neighborPosition);

                int beforePossibilities = waveFunction[neighborIndex];
                // Evaluate the neighbor possibilities

                if (i == 0) // up
                {
                    if (ruleSet.ConnectsUp(getIndexOfSetBit(waveFunction[index])))
                    {
                        // neighbor should connect down
                        waveFunction[neighborIndex] = waveFunction[neighborIndex] & ruleSet.tilesConnectingDownMask;
                    } else
                    {
                        // waveFunction[neighborIndex] = waveFunction[neighborIndex] & ruleSet.tilesNotConnectingDownMask;
                    }
                    
                } else if (i == 1) // right
                {
                    if (ruleSet.ConnectsRight(getIndexOfSetBit(waveFunction[index])))
                    {
                        // neighbor should connect left
                        waveFunction[neighborIndex] = waveFunction[neighborIndex] & ruleSet.tilesConnectingLeftMask;
                    } else
                    {
                        // waveFunction[neighborIndex] = waveFunction[neighborIndex] & ruleSet.tilesNotConnectingLeftMask;
                    }
                    
                } else if (i == 2) // down
                {
                    if (ruleSet.ConnectsDown(getIndexOfSetBit(waveFunction[index])))
                    {
                        // neighbor should connect up
                        waveFunction[neighborIndex] = waveFunction[neighborIndex] & ruleSet.tilesConnectingUpMask;
                    } else
                    {
                        // waveFunction[neighborIndex] = waveFunction[neighborIndex] & ruleSet.tilesNotConnectingUpMask;
                    }
                    
                } else if (i == 3) // left
                {
                    if (ruleSet.ConnectsLeft(getIndexOfSetBit(waveFunction[index])))
                    {
                        // neighbor should connnect right
                        waveFunction[neighborIndex] = waveFunction[neighborIndex] & ruleSet.tilesConnectingRightMask;
                    } else
                    {
                        // waveFunction[neighborIndex] = waveFunction[neighborIndex] & ruleSet.tilesNotConnectingRightMask;
                    }
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
        if (isTileCollapsed(index) || waveFunction[index] == 0)
        {
            return waveFunction[index];
        }

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
        waveFunction[index] = 1 << possibleTiles[ind];

        return possibleTiles[ind];
    } 

    public bool isWaveCollapsed()
    {
        return waveState;
    }

    public bool isTileCollapsed(int number)
    {
        return countSetBits(number) == 1;
    }

    public int countSetBits(int number)
    {
        return System.Numerics.BitOperations.PopCount((uint) number);
    }

    public void visualizeWaveFunction(SpriteBatch sp, Texture2D tileSet)
    {
        
        int leastEntropy = 9999;

        for (int i = 0; i < gd.size; ++i)
        {

            waveState = isTileCollapsed(waveFunction[i]);

            int possibilitiesCount = countSetBits(waveFunction[i]);

            if ((possibilitiesCount <= leastEntropy) && !isTileCollapsed(waveFunction[i]) && !(waveFunction[i] == 0))
            {
                leastEntropyTile = i;
                leastEntropy = possibilitiesCount;
            }

            leastEntropyTile = randomGen.Next(gd.size);
            

            int cellTile = 0;
            if (possibilitiesCount == 1) // This tile has collapsed get the value
            {
                cellTile = getIndexOfSetBit(waveFunction[i]);
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

    private void printBitValueAt(int number , int position)
    {
        bool bitValue = ((number >> position) & 1) == 1;
        Console.WriteLine($"Bit at position {position}: {bitValue}");
    }

    // If a tile is collapsed this gets the index of the set bit
    // the index of the tile.
    private int getIndexOfSetBit(int mask)
    {
        return System.Numerics.BitOperations.TrailingZeroCount(mask);
    }


}