using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Kender.uGUI;

public class OptionsMenuHandler : MonoBehaviour {

    private ComboBox resolutions;
    private Toggle fullscreen;

	void Start () {
        // Set up our fullscreen toggle button
        if (GameObject.Find("FullscreenToggle") != null) {
            fullscreen = GameObject.Find("FullscreenToggle").GetComponent<Toggle>();
            if (Screen.fullScreen) fullscreen.isOn = true;
            else fullscreen.isOn = false;
        }

        // Set up our resolution dropdown box
        if (GameObject.Find("ResolutionList") != null) {
            resolutions = GameObject.Find("ResolutionList").GetComponent<ComboBox>();
            ComboBoxItem[] items = new ComboBoxItem[Screen.resolutions.Length];
            int counter = 0;

            Resolution[] res = Screen.resolutions;
            foreach (Resolution r in res) {
                items[counter] = new ComboBoxItem(r);
                counter++;
            }

            resolutions.Items = items;
            resolutions.ItemsToDisplay = 5;

            Resolution tmpRes = Screen.currentResolution;
            foreach (Resolution r in Screen.resolutions) {
                if (Screen.width == r.width && Screen.height == r.height) {
                    tmpRes = r;
                    break;
                }
            }
            resolutions.SelectedIndex = System.Array.IndexOf(Screen.resolutions, tmpRes);
        }
	}

    public void SaveSettings() {
        var r = resolutions.Items[resolutions.SelectedIndex].Res;
        Screen.SetResolution(r.width, r.height, fullscreen.isOn);
    }
	
}
