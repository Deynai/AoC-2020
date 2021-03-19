using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Numerics;

public class MainController : MonoBehaviour
{
    private void Main()
    {
        BigInteger input1 = 10604480;
        BigInteger input2 = 4126658;

        Part1(input1, input2);
    }

    private void Part1(BigInteger pkey1, BigInteger pkey2)
    {
        BigInteger modulo = 20201227;
        BigInteger card_loop_size = 0;
        BigInteger door_loop_size = 0;
        BigInteger encryption_key = 0;
        BigInteger value = 1;
        BigInteger subject_number = 7;

        int bp = 0;

        for(int i = 1; i < (int) pkey1; i++)
        {
            bp++;
            if (bp > 10000000) { Debug.Log("Loop break hit."); break; }

            value = (value * subject_number) % modulo;

            if (value.Equals(pkey1))
            {
                Debug.Log($"Found Card Public Key at loop: {i}");
                card_loop_size = i;
                encryption_key = BigInteger.ModPow(pkey2, card_loop_size, modulo);
                break;
            }
            else if(value.Equals(pkey2))
            {
                Debug.Log($"Found Door Public Key at loop: {i}");
                door_loop_size = i;
                encryption_key = BigInteger.ModPow(pkey1, door_loop_size, modulo);
                break;
            }
        }

        Debug.Log($"Encryption key: {encryption_key.ToString()}");
    }

    // Start is called before the first frame update
    void Start()
    {
        Main();
    }
}
