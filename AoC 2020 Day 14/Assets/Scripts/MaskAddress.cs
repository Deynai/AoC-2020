using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Numerics;
using System;

public class MaskAddress
{
    private string current_mask;
    private List<int> ones = new List<int>();
    private long ones_sum = 0;
    private List<int> xs = new List<int>();
    private long xs_sum = 0;
    Dictionary<long, long> memory = new Dictionary<long, long>();
    private long sum = 0;

    private static int mask_length = 36;

    public void SetMask(string mask)
    {
        current_mask = mask;
        ones.Clear();
        xs.Clear();
        xs_sum = 0;
        ones_sum = 0;

        for (int i = 0; i < mask_length; i++)
        {
            if (mask[i].Equals('X'))
            {
                xs.Add(mask_length - i - 1);
                xs_sum += (long)BigInteger.Pow(2, mask_length - i - 1);
            }
            else if (mask[i].Equals('1'))
            {
                ones.Add(mask_length - i - 1);
                ones_sum += (long)BigInteger.Pow(2, mask_length - i - 1);
            }
        }
    }

    public void WriteMemory(long address, long value)
    {
        long maskedaddress = Mask(address);
        int[] copy_xs = xs.ToArray();

        RecurseWriteToMemory(copy_xs, maskedaddress, value);
    }

    private void RecurseWriteToMemory(int[] copy_xs, long address, long value)
    {
        if (copy_xs.Length.Equals(0))
        {
            memory.Remove(address);
            memory.Add(address, value);
            return;
        }
        else
        {
            int[] cc_xs = new int[copy_xs.Length - 1];
            Array.ConstrainedCopy(copy_xs, 1, cc_xs, 0, copy_xs.Length - 1);
            long num = (long) BigInteger.Pow(2,copy_xs[0]);
            RecurseWriteToMemory(cc_xs, address, value);
            RecurseWriteToMemory(cc_xs, address + num, value);
        }
    }

    public long GetSum()
    {
        foreach (long num in memory.Values)
        {
            sum += num;
        }

        return sum;
    }

    private long Mask(long address)
    {
        address = address | ones_sum; 
        address = address & ~xs_sum;
        return address;
    }
}
