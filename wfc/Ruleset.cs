using System;
using System.Collections.Generic;


namespace Ruleset;

public class RuleSet
{
    public int tilesConnectingUpMask = 0;
    public int tilesConnectingRightMask = 0;
    public int tilesConnectingDownMask = 0;
    public int tilesConnectingLeftMask = 0;

    enum ConnectionBits
    {
        UP = 0,
        RIGHT = 1,
        DOWN = 2,
        LEFT = 3
    }

    public RuleSet()
    {
        for (int i = 0; i < 16; ++i)
        {
            if (isConnectedUp(i))
            {
                tilesConnectingUpMask |= (1 << i);
            }

            if (isConnectedRight(i))
            {
                tilesConnectingRightMask |= (1 << i);   
            }

            if (isConnectedDown(i))
            {
                tilesConnectingDownMask |= (1 << i);
            }

            if (isConnectedLeft(i))
            {
                tilesConnectingLeftMask |= (1 << i);
            }
        }
    }

    public bool isBitSet(int number, int position)
    {
        return (number & (1 << position)) != 0;
    }

    public bool isConnectedUp(int i)
    {
        return isBitSet(i, (int) ConnectionBits.UP);
    }

    public bool isConnectedRight(int i)
    {
        return isBitSet(i, (int) ConnectionBits.RIGHT);
    }

    public bool isConnectedDown(int i)
    {
        return isBitSet(i, (int) ConnectionBits.DOWN);
    }

    public bool isConnectedLeft(int i)
    {
        return isBitSet(i, (int) ConnectionBits.LEFT);
    }
}