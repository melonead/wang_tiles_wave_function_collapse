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

        Console.WriteLine("============================================");
        Console.WriteLine(Convert.ToString(tilesConnectingUpMask, 2).PadLeft(32, '0'));
        Console.WriteLine(Convert.ToString(tilesConnectingDownMask, 2).PadLeft(32, '0'));    
        Console.WriteLine(Convert.ToString(tilesConnectingRightMask, 2).PadLeft(32, '0'));
        Console.WriteLine(Convert.ToString(tilesConnectingLeftMask, 2).PadLeft(32, '0'));  

        Console.WriteLine("xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx");

        Console.WriteLine(Convert.ToString(tilesNotConnectingUpMask, 2).PadLeft(32, '0'));
        Console.WriteLine(Convert.ToString(tilesNotConnectingDownMask, 2).PadLeft(32, '0'));    
        Console.WriteLine(Convert.ToString(tilesNotConnectingRightMask, 2).PadLeft(32, '0'));
        Console.WriteLine(Convert.ToString(tilesNotConnectingLeftMask, 2).PadLeft(32, '0'));  
        Console.WriteLine("++++++++++++++++++++++++++++++++++++++++++++++");  

        int x = tilesConnectingUpMask & tilesNotConnectingLeftMask;

        Console.WriteLine(ConnectsDown(0));
        Console.WriteLine(ConnectsUp(0));
        Console.WriteLine(ConnectsLeft(0));
        Console.WriteLine(ConnectsRight(0));
        Console.WriteLine(Convert.ToString(x, 2).PadLeft(32, '0'));

        

        for (int i = 0; i < 32; ++i)
        {
            Console.WriteLine(isBitSet(tilesConnectingUpMask & tilesConnectingDownMask, i));
        }

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








    public bool isConnectedUp(int i)
    {
        int n = getIndexOfSetBit(i);
        return isBitSet(n, (int) ConnectionBits.UP);
    }

    public bool isConnectedRight(int i)
    {
        int n = getIndexOfSetBit(i);
        return isBitSet(n, (int) ConnectionBits.RIGHT);
    }

    public bool isConnectedDown(int i)
    {
        int n = getIndexOfSetBit(i);
        return isBitSet(n, (int) ConnectionBits.DOWN);
    }

    public bool isConnectedLeft(int i)
    {
        int n = getIndexOfSetBit(i);
        return isBitSet(n, (int) ConnectionBits.LEFT);
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