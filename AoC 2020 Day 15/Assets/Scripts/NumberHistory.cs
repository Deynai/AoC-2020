using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumberHistory
{
    List<long> mention_history = new List<long>();

    public long GetLastMention(long curr)
    {
        return mention_history.Count.Equals(1) ? 0 : curr - mention_history[mention_history.Count - 2] - 1;
    }

    public void AddMention(long curr)
    {
        mention_history.Add(curr);
    }
}
