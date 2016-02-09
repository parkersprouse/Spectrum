using UnityEngine;
using System.Collections;

public class Level {

    private string levelName;
    public string LevelName {
        get {
            return levelName;
        }
        set {
            levelName = value;
        }
    }

    private bool unlocked = false;
    public bool Unlocked {
        get {
            return unlocked;
        }
        set {
            unlocked = value;
        }
    }

    public Level(string name, bool unlocked) {
        this.levelName = name;
        this.unlocked = unlocked;
    }

}
