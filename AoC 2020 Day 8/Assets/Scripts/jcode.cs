using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class jcode
{
    private string[] code;
    private int curr_index = 0;
    private int prev_index = 0;
    private long accum = 0;

    private string curr_action_code;
    private int curr_action_value;

    public bool terminated = false;

    public jcode(string[] input, int start_index)
    {
        code = input; // may have issues if we change input or edit anything as this is a reference copy
        curr_index = start_index;
        prev_index = start_index;

        ParseAction();
    }

    public string GetLine()
    {
        return code[curr_index];
    }

    public string GetLine(int index)
    {
        return code[index];
    }

    public int GetIndex()
    {
        return curr_index;
    }

    public int GetPrevIndex()
    {
        return prev_index;
    }

    public int Size()
    {
        return code.Length;
    }

    public long GetAccumulator()
    {
        return accum;
    }

    public void RunLine()
    {
        Action(curr_action_code, curr_action_value);
    }

    private void ParseAction()
    {
        curr_action_code = GetLine().Split(' ')[0];
        curr_action_value = int.Parse(GetLine().Split(' ')[1]);
    }

    private void Action(string code, int val)
    {
        switch (code)
        {
            case "nop": NOP(val);
                break;
            case "acc": ACC(val);
                break;
            case "jmp": JMP(val);
                break;
            case "end": END(val);
                break;
            default: Debug.Log("Invalid Action");
                break;
        }

        ParseAction();
    }

    private void NOP(int val)
    {
        // do nothing
        prev_index = curr_index;
        curr_index++;
    }

    private void ACC(int val)
    {
        // add/subtract val
        accum += val;

        prev_index = curr_index;
        curr_index++;
    }

    private void JMP(int val)
    {
        prev_index = curr_index;
        curr_index += val;
    }
    
    private void END(int val)
    {
        terminated = true;
    }
}
