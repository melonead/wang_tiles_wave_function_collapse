using System;
using System.Collections.Generic;


namespace Ruleset;

public class RuleSet
{
    int tilesConnectingUpMask = 0;
    int tilesConnectingRightMask = 0;
    int tilesConnectingDownMask = 0;
    int tilesConnectingLeftMask = 0;

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

    bool isConnectedUp(int i)
    {
        return isBitSet(i, (int) ConnectionBits.UP);
    }

    bool isConnectedRight(int i)
    {
        return isBitSet(i, (int) ConnectionBits.RIGHT);
    }

    bool isConnectedDown(int i)
    {
        return isBitSet(i, (int) ConnectionBits.DOWN);
    }

    bool isConnectedLeft(int i)
    {
        return isBitSet(i, (int) ConnectionBits.LEFT);
    }
}