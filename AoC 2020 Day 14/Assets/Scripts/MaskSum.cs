using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Numerics;

public class MaskSum
{
    private string current_mask;
    private List<int> ones = new List<int>();
    private long ones_sum = 0;
    private List<int> xs = new List<int>();
    private long xs_sum = 0;
    Dictionary<int, long> memory = new Dictionary<int, long>();
    private long sum = 0;

    private static int mask_length = 36;

    public void SetMask(string mask)
    {
        current_mask = mask;
        ones.Clear();
        xs.Clear();
        xs_sum = 0;
        ones_sum = 0;

        for(int i = 0; i < mask_length; i++)
        {
            if (mask[i].Equals('X'))
            {
                xs.Add(mask_length - i - 1);
                xs_sum += (long) BigInteger.Pow(2, mask_length - i - 1);
            }
            else if (mask[i].Equals('1'))
            {
                ones.Add(mask_length - i - 1);
                ones_sum += (long) BigInteger.Pow(2, mask_length - i - 1);
            }
        }
    }

    public void WriteMemory(int address, long value)
    {
        long maskedValue = Mask(value);
        memory.Remove(address);
        memory.Add(address, maskedValue);
    }

    public long GetSum()
    {
        foreach(long num in memory.Values)
        {
            sum += num;
        }

        return sum;
    }

    private long Mask(long value)
    {
        value = value & xs_sum; // bitwise AND the value and the X's (as 1's)
        value = value | ones_sum; // bitwise "OR" the value and the ones - should be the same as just adding both
        return value;
    }
}
