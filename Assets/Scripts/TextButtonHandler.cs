using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TextButtonHandler : MonoBehaviour {

    public void ButtonEnter() {
        GetComponent<Text>().color = Color.red;
    }

    public void ButtonExit() {
        GetComponent<Text>().color = Color.black;
    }
}
