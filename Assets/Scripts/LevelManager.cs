using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelManager : MonoBehaviour {

    // This dictionary 
    public static Level[] levels = new Level[1];

    void Start() {
        levels[0] = new Level("LevelOne", true);
    }

}
