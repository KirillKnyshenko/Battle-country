using UnityEngine;
using System.IO;

[CreateAssetMenu()]
public class SaveDataSO : ScriptableObject
{
    public int level = 1;
    public int loop = 1;

    public void Save() {
        var data = JsonUtility.ToJson(this);
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

    public void Delete() {
        File.Delete(BuildPath("Save.JSON"));
    }

    private string BuildPath(string key) {
        return Path.Combine(Application.persistentDataPath, key);
    }
}
