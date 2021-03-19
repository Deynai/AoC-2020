using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MainController : MonoBehaviour
{
    public GameObject actionBox;
    public GameObject actionBoxPanel;

    public GameObject acc_text;
    public GameObject index_text;

    private List<int> nop_index_list = new List<int>();
    private List<int> jmp_index_list = new List<int>();

    private IEnumerator Main()
    {
        string[] input = System.IO.File.ReadAllLines("./Assets/Input/day8input.txt");

        yield return Part1(input);

        yield return Part2(input);
    }

    private IEnumerator Part1(string[] input)
    {
        int count = 0;
        int loop_break = 100000;
        float speed = 0.1f;

        jcode computer = new jcode(input, 0);
        acc_text.GetComponent<TextMeshProUGUI>().text = computer.GetAccumulator().ToString();
        actionBoxPanel.GetComponent<actionBoxController>().Init(input);

        byte[] flags = new byte[computer.Size()];
        for(int i = 0; i<flags.Length; i++)
        {
            flags[i] = 0;
        }

        while (flags[computer.GetIndex()].Equals(0))
        {
            flags[computer.GetIndex()] = 1;
            actionBoxPanel.GetComponent<actionBoxController>().Colour(computer.GetIndex(), 1);

            string action = computer.GetLine().Split(' ')[0];
            if (action.Equals("nop"))
            {
                nop_index_list.Add(computer.GetIndex());
            }
            else if (action.Equals("jmp"))
            {
                jmp_index_list.Add(computer.GetIndex());
            }

            yield return new WaitForSeconds(speed);
            computer.RunLine();
            acc_text.GetComponent<TextMeshProUGUI>().text = computer.GetAccumulator().ToString();

            count++;
            if(count > loop_break)
            {
                break;
            }
        }
        
        if (flags[computer.GetIndex()].Equals(1))
        {
            actionBoxPanel.GetComponent<actionBoxController>().Colour(computer.GetIndex(), 2);
        }

        yield return new WaitForSeconds(2.0f);

        foreach(int index in nop_index_list)
        {
            actionBoxPanel.GetComponent<actionBoxController>().Colour(index, 4);
        }
        foreach (int index in jmp_index_list)
        {
            actionBoxPanel.GetComponent<actionBoxController>().Colour(index, 4);
        }

        yield return new WaitForSeconds(4.0f);
    
        Debug.Log("Final Accumulator Value: " + computer.GetAccumulator());
        Debug.Log("Final Steps Count: " + count);
    }

    private IEnumerator Part2(string[] input)
    {
        // jmp replacement
        foreach (int index in jmp_index_list)
        {
            string[] inputCopy = CopyInputTerminate(input);
            inputCopy[index] = "nop " + inputCopy[index].Split(' ')[1];
            actionBoxPanel.GetComponent<actionBoxController>().Destroy();
            actionBoxPanel.GetComponent<actionBoxController>().Init(inputCopy);
            actionBoxPanel.GetComponent<actionBoxController>().Colour(index, 4);
            jcode newComputer = new jcode(inputCopy, 0);
            yield return Part2RunCode(newComputer);

            if (newComputer.terminated)
            {
                Debug.Log("Final Accumulator Value: " + newComputer.GetAccumulator());
                yield break;
            }
        }

        // nop replacement
        foreach (int index in nop_index_list)
        {
            string[] inputCopy = CopyInputTerminate(input);
            inputCopy[index] = "jmp " + inputCopy[index].Split(' ')[1];
            actionBoxPanel.GetComponent<actionBoxController>().Destroy();
            actionBoxPanel.GetComponent<actionBoxController>().Init(inputCopy);
            actionBoxPanel.GetComponent<actionBoxController>().Colour(index, 4);
            jcode newComputer = new jcode(inputCopy, 0);

            yield return Part2RunCode(newComputer);

            if (newComputer.terminated)
            {
                actionBoxPanel.GetComponent<actionBoxController>().Colour(newComputer.GetIndex(), 3);
                Debug.Log("Final Accumulator Value: " + newComputer.GetAccumulator());
                yield break;
            }
        }

        yield break;
    }

    private IEnumerator Part2RunCode(jcode computer)
    {
        int count = 0;
        int loop_break = 100000;
        float speed = 0.0f;
        acc_text.GetComponent<TextMeshProUGUI>().text = computer.GetAccumulator().ToString();

        byte[] flags = new byte[computer.Size()];
        for (int i = 0; i < flags.Length; i++)
        {
            flags[i] = 0;
        }

        yield return new WaitForSeconds(0.5f);

        while (!computer.terminated && flags[computer.GetIndex()].Equals(0))
        {
            flags[computer.GetIndex()] = 1;
            actionBoxPanel.GetComponent<actionBoxController>().Colour(computer.GetIndex(), 1);

            yield return new WaitForSeconds(speed);
            computer.RunLine();
            acc_text.GetComponent<TextMeshProUGUI>().text = computer.GetAccumulator().ToString();

            count++;
            if (count > loop_break)
            {
                break;
            }
        }

        if (flags[computer.GetIndex()].Equals(1))
        {
            actionBoxPanel.GetComponent<actionBoxController>().Colour(computer.GetIndex(), 2);
        }

        if (computer.terminated)
        {
            actionBoxPanel.GetComponent<actionBoxController>().Colour(computer.GetIndex(), 3);
        }

        yield return new WaitForSeconds(0.2f);
    }

    private string[] CopyInputTerminate(string[] input)
    {
        string[] inputCopy = new string[input.Length + 1];
        for(int i = 0; i < input.Length; i++)
        {
            inputCopy[i] = input[i];
        }
        inputCopy[input.Length] = "end 0";
        return inputCopy;
    }

    public void DisplayIndex(GameObject caller)
    {
        index_text.GetComponent<TextMeshProUGUI>().text = caller.GetComponent<ButtonController>().index.ToString();
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("Main");
    }
}
