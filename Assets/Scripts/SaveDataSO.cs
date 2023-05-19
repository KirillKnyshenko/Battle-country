using UnityEngine;
using System.IO;

[CreateAssetMenu()]
public class SaveDataSO : ScriptableObject
{
    [SerializeField] private int _level = 1;
    public int level => _level;
    public int loop = 1;

    public bool soundVolume = true;
    public bool musicVolume = true;

    public void Save()
    {
        string data;
        if (!File.Exists(BuildPath("Save.JSON"))){
            data = JsonUtility.ToJson(ScriptableObject.CreateInstance<SaveDataSO>());
            JsonUtility.FromJsonOverwrite(data, this);
        }

        data = JsonUtility.ToJson(this);
        File.WriteAllText(BuildPath("Save.JSON"), data);
    }

    public void Load() {
        if (File.Exists(BuildPath("Save.JSON")))
        {
            JsonUtility.FromJsonOverwrite(File.ReadAllText(BuildPath("Save.JSON")), this); 
        }  
        else
        {
            Save();
        }
    }

    public void NextLevel() {
        _level++;
    }

    public void SetLevel(int level) {
        _level = level;
    }

    public void SetAudioData(bool musicVolume, bool soundVolume) {
        this.musicVolume = musicVolume;
        this.soundVolume = soundVolume;
    }

    public void Delete() {
        File.Delete(BuildPath("Save.JSON"));
    }

    private string BuildPath(string key) {
        return Path.Combine(Application.persistentDataPath, key);
    }
}
