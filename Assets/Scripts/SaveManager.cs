using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using static AvatarRenderer;

public class SaveManager : MonoBehaviour
{
    [SerializeField]
    private string SaveName;
    [SerializeField]
    private static bool created = false;
    [SerializeField]
    private SaveObject m_saveObject = null;

    private static SaveManager instance;
    public static SaveManager Instance
    {
        get
        {
            return instance;
        }
    }

    //LINK GAMESTATEMANAGER TO UPDATE SAVEOBJECT COIN + HIGH SCORE
    //LINK AVATAR RENDER TO READ + WRITE TO SAVE OBJECT

    private void Awake()
    {
        if(!created)
        {
            DontDestroyOnLoad(this.gameObject);
            created = true;
            instance = this;
        }
        m_saveObject = LoadData();
    }

    void Start()
    {
        GameStateManager.PlayerDied += OnPlayerDied;
        //SaveTest();
    }

    public void UpdateScore(int score)
    {
        if (score > m_saveObject.Score)
            m_saveObject.Score = score;
    }

    public void UpdateCoin(int coin)
    {
        m_saveObject.Coins = coin;
    }

    private void OnPlayerDied(GameStateManager.DiedEventArgs e)
    {
        UpdateCoin(e.Score);
        SaveFile();
    }

    public SaveObject LoadData()
    {
        try
        {
            var saveObject = new SaveObject();

            using (var sr = new StreamReader($"{Application.persistentDataPath}/{SaveName}.txt"))
            {
                var data = sr.ReadToEnd();
                saveObject = JsonUtility.FromJson<SaveObject>(data);
            }
            return saveObject;
        }
        catch (IOException e)
        {
            Debug.Log("The file could not be read:");
            Debug.Log(e.Message);
            return null;
        }
    }

    public void SaveTest()
    {
        SaveObject saveObject = new SaveObject()
        {
            Coins = 153,
            Score = 42,
            SelectedSprites = new SelectedSprites()
            {
                Shirt = 1,
                Beard = 1,
                Hair = 1,
                Pants = 1
            },
            Colors = new Colors
            {
                Beard = new Vector3(0.5f, 1, 0.4f),
                Body = new Vector3(0.5f, 1, 0.4f),
                Eyes = new Vector3(0.5f, 1, 0.4f),
                Hair = new Vector3(0.5f, 1, 0.4f),
                Pants = new Vector3(0.5f, 1, 0.4f),
                Shirt = new Vector3(0.5f, 1, 0.4f),
            }
        };

        Debug.Log("Coins: " + saveObject.Coins);
        Debug.Log("Score: " + saveObject.Score);
        Debug.Log("Colors: " + saveObject.Colors);
        Debug.Log("Sprites: " + saveObject.SelectedSprites);

        var json = JsonUtility.ToJson(saveObject);
        Debug.Log("JSON: " + json);

        string docPath = Application.persistentDataPath;

        // Write the string array to a new file named "WriteLines.txt".
        using (StreamWriter outputFile = new StreamWriter(Path.Combine(docPath, $"{SaveName}.txt")))
        {
            outputFile.WriteLine(json);
        }
    }

    

    public Colors GetColors()
    {
        return m_saveObject.Colors;
    }

    public SelectedSprites GetSelectedSprites()
    {
        return m_saveObject.SelectedSprites;
    }

    public void SaveFile()
    {
        var json = JsonUtility.ToJson(m_saveObject);

        var docPath = Application.persistentDataPath;
        using (StreamWriter outputFile = new StreamWriter(Path.Combine(docPath, $"{SaveName}.txt")))
        {
            outputFile.WriteLine(json);
            Debug.Log("Saved following JSON to File at "+ docPath + SaveName + ".txt" + " : \n" + json);
        }
    }

    public void UpdateSprites(int m_selectedHair, int m_selectedBeard)
    {
        m_saveObject.SelectedSprites.Hair = m_selectedHair;
        m_saveObject.SelectedSprites.Beard= m_selectedBeard;
    }

    public void UpdateColors(Dictionary<SELECTEDPART, SpriteRenderer> m_SpriteColorByPartName)
    {
        var colors = new Colors
        {
            Beard = Utils.ColorToVector(m_SpriteColorByPartName[SELECTEDPART.BEARD].color),
            Hair = Utils.ColorToVector(m_SpriteColorByPartName[SELECTEDPART.HAIR].color),
            Body = Utils.ColorToVector(m_SpriteColorByPartName[SELECTEDPART.BODY].color),
            Eyes = Utils.ColorToVector(m_SpriteColorByPartName[SELECTEDPART.EYES].color),
            Pants = Utils.ColorToVector(m_SpriteColorByPartName[SELECTEDPART.PANTS].color),
            Shirt = Utils.ColorToVector(m_SpriteColorByPartName[SELECTEDPART.SHIRT].color),
        };
        m_saveObject.Colors = colors;
    }
}
