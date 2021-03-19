using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour
{
    public int index;
    public GameObject mainController;

    public void Init(string line, int index)
    {
        this.GetComponentInChildren<TextMeshProUGUI>().text = line;
        this.index = index;
        mainController = GameObject.Find("MainController");
        GetComponent<Button>().onClick.AddListener(delegate { mainController.GetComponent<MainController>().DisplayIndex(this.gameObject); });
    }

    public void Colour(int col)
    {
        switch (col)
        {
            case 0: 
                this.GetComponent<Image>().color = Color.white;
                break;
            case 1:
                this.GetComponent<Image>().color = Color.green;
                break;
            case 2:
                this.GetComponent<Image>().color = Color.red;
                break;
            case 3:
                this.GetComponent<Image>().color = Color.magenta;
                break;
            case 4:
                this.GetComponent<Image>().color = Color.cyan;
                break;
            default:
                break;
        }
    }
}
