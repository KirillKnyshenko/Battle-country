using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class LevelListSO : ScriptableObject
{
    public SaveDataSO saveDataSO;
    public List<string> levels;

    public int GetNextLevel() {
        saveDataSO.level += 1;

        if (saveDataSO.level >= levels.Count)
        {
            saveDataSO.level = 0;
            saveDataSO.loop++;
        }

        return saveDataSO.level;
    }

    public string GetCurrentLevelName() {
        return levels[saveDataSO.level];
    }
}

