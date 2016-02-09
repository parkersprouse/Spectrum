using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerSwapper : MonoBehaviour {

    public List<GameObject> characters;
    
    private CameraFollow mainCamera;

    void Start() {
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraFollow>();

        for (int i = 0; i < characters.Count; i++) {
            characters[i].GetComponent<PlayerController>().enabled = false;
        }

        characters[0].GetComponent<PlayerController>().enabled = true;
        GameObject.Find(characters[0].GetComponent<Character>().color).transform.Translate(0, 0, 1);
        mainCamera.target = characters[0].GetComponent<PlayerController>();
    }

    void Update() {
        if (Input.GetButtonDown("Swap")) {
            SwapCharacter();
        }
    }

    public void SwapCharacter() {
        for (int i = 0; i < characters.Count; i++) {
            if (characters[i].GetComponent<PlayerController>().enabled) {
                GameObject.Find(characters[i].GetComponent<Character>().color).transform.Translate(0, 0, -1);
                if (i + 1 < characters.Count) {
                    characters[i + 1].GetComponent<PlayerController>().enabled = true;
                    GameObject.Find(characters[i + 1].GetComponent<Character>().color).transform.Translate(0, 0, 1);
                    mainCamera.ChangeTarget(characters[i + 1].GetComponent<PlayerController>());
                }
                else {
                    characters[0].GetComponent<PlayerController>().enabled = true;
                    GameObject.Find(characters[0].GetComponent<Character>().color).transform.Translate(0, 0, 1);
                    mainCamera.ChangeTarget(characters[0].GetComponent<PlayerController>());
                }
                characters[i].GetComponent<PlayerController>().enabled = false;
                characters[i].GetComponent<Character>().velocity = new Vector3(0, characters[i].GetComponent<Character>().velocity.y, characters[i].GetComponent<Character>().velocity.z);
                break;
            }
        }

        for (int i = 0; i < characters.Count; i++) {
            if (characters[i].GetComponent<Character>().isFinishedLevel) {
                characters.RemoveAt(i);
            }
        }
    }
}
