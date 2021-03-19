using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ImpactText : MonoBehaviour
{
    // Start is called before the first frame update
    public void SetText(string str)
    {
        GetComponent<TextMeshProUGUI>().text = str;
    }
}
