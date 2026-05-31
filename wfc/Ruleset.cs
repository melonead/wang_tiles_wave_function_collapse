using System;
using System.Collections.Generic;


namespace Ruleset;

public class RuleSet
{
    public int tilesConnectingUpMask = 0;
    public int tilesConnectingRightMask = 0;
    public int tilesConnectingDownMask = 0;
    public int tilesConnectingLeftMask = 0;

     public int tilesNotConnectingUpMask = 0;
    public int tilesNotConnectingRightMask = 0;
    public int tilesNotConnectingDownMask = 0;
    public int tilesNotConnectingLeftMask = 0;

    public enum ConnectionBits
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
            if (ConnectsUp(i))
            {
                tilesConnectingUpMask |= (1 << i);
            }

            if (ConnectsRight(i))
            {
                tilesConnectingRightMask |= (1 << i);   
            }

            if (ConnectsDown(i))
            {
                tilesConnectingDownMask |= (1 << i);
            }

            if (ConnectsLeft(i))
            {
                tilesConnectingLeftMask |= (1 << i);
            }
        }

        int numbBits = 16;

        tilesNotConnectingUpMask = tilesConnectingUpMask;
        tilesNotConnectingDownMask = tilesConnectingDownMask;
        tilesNotConnectingRightMask = tilesConnectingRightMask;
        tilesNotConnectingLeftMask = tilesConnectingLeftMask;

        tilesNotConnectingUpMask = flipBits(tilesNotConnectingUpMask, numbBits);
        tilesNotConnectingDownMask = flipBits(tilesNotConnectingDownMask, numbBits);
        tilesNotConnectingRightMask = flipBits(tilesNotConnectingRightMask, numbBits);
        tilesNotConnectingLeftMask = flipBits(tilesNotConnectingLeftMask, numbBits);

    }

    public bool isBitSet(int number, int position)
    {
        return (number & (1 << position)) != 0;
    }

    public bool ConnectsUp(int i)
    {
        return isBitSet(i, (int) ConnectionBits.UP);
    }

    public bool ConnectsRight(int i)
    {
        return isBitSet(i, (int) ConnectionBits.RIGHT);
    }

    public bool ConnectsDown(int i)
    {
        return isBitSet(i, (int) ConnectionBits.DOWN);
    }


    public bool ConnectsLeft(int i)
    {
        return isBitSet(i, (int) ConnectionBits.LEFT);
    }

    private int flipBits(int number, int bitCount)
    {
        int res = number;
        for (int i = 0; i < bitCount; ++i)
        {
            res ^= (1 << i);
        }
        return res;
    }

    private int getIndexOfSetBit(int mask)
    {
        return System.Numerics.BitOperations.TrailingZeroCount(mask);
    }
}