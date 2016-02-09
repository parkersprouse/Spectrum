using UnityEngine;
using System.Collections;

public class DoorManager : MonoBehaviour {

    public string color;

    private PlayerSwapper playerSwapper;

    void Start() {
        playerSwapper = GameObject.Find("_Manager").GetComponent<PlayerSwapper>();
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == "Character") {
            if (other.gameObject.GetComponent<Character>().color == color) {
                other.gameObject.GetComponent<Character>().isFinishedLevel = true;
                playerSwapper.SwapCharacter();
                other.gameObject.SetActive(false);
            }
        }
    }

}
