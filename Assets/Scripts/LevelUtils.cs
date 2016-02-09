using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LevelUtils : MonoBehaviour {

    public string levelName;

    private PlayerSwapper ps;
    private Text colorText;

    void Awake() {
        colorText = GameObject.Find("ControlledText").GetComponent<Text>();
        ps = base.GetComponent<PlayerSwapper>();
    }

    void Update() {

        if (Input.GetButtonDown("ResetLevel")) {
            Application.LoadLevel(levelName);
        }

        else {
            for (int i = 0; i < ps.characters.Count; i++) {
                if (ps.characters[i].GetComponent<PlayerController>().enabled) {
                    ChangeColorText(ps.characters[i].GetComponent<Character>().color);
                }
            }
        }

    }

    public void ChangeColorText(string newColor) {
        switch (newColor) {

            // Primary Colors
            case "red":
                colorText.text = "Red";
                colorText.color = Color.red;
                break;
            case "yellow":
                colorText.text = "Yellow";
                colorText.color = Color.yellow;
                break;
            case "blue":
                colorText.text = "Blue";
                colorText.color = Color.blue;
                break;

            // Secondary Colors
            case "orange": // Red + Yellow
                colorText.text = "Orange";
                colorText.color = new Color(1, 140f / 255f, 0);
                break;
            case "green": // Blue + Yellow
                colorText.text = "Green";
                colorText.color = Color.green;
                break;
            case "purple": // Red + Blue
                colorText.text = "Purple";
                colorText.color = new Color(153f / 255f, 50f / 255f, 204f / 255f);
                break;

            // The "I went too far" color
            case "brown":
                colorText.text = "Brown";
                colorText.color = new Color(139f / 255f, 69f / 255f, 19f / 255f);
                break;

            // End of Spectrum Colors
            case "black":
                colorText.text = "Black";
                colorText.color = Color.black;
                break;
            case "white":
                colorText.text = "White";
                colorText.color = Color.white;
                break;
            case "grey": // Black + White
                colorText.text = "Grey";
                colorText.color = Color.grey;
                break;

            // Mixed with white, primaries
            case "cyan": // Blue + White
                colorText.text = "Cyan";
                colorText.color = new Color(0, 1, 1);
                break;
            case "pink": // Red + White
                colorText.text = "Pink";
                colorText.color = new Color(1, 105f / 255f, 180f / 255f);
                break;
            case "lightyellow": // Yellow + White
                colorText.text = "Light Yellow";
                colorText.color = new Color(1, 1, 102f / 255f);
                break;

            // Mixed with white, secondaries
            case "lightpurple": // Purple + White
                colorText.text = "Light Purple";
                colorText.color = new Color(218f / 255f, 112f / 255f, 214f / 255f);
                break;
            case "lightgreen": // Green + White
                colorText.text = "Light Green";
                colorText.color = new Color(150f / 255f, 1, 150f / 255f);
                break;
            case "lightorange": // Orange + White
                colorText.text = "Light Orange";
                colorText.color = new Color(1, 180f / 255f, 0);
                break;

            // If, somehow, no color is selected, it's always good to have a fail-safe
            default:
                colorText.text = "None";
                colorText.color = Color.white;
                break;
        }
    }
}
