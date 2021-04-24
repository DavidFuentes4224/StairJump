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
    private string SaveName = null;
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

    private void Awake()
    {
        if(!created)
        {
            DontDestroyOnLoad(this.gameObject);
            created = true;
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        m_saveObject = LoadData();
        if (m_saveObject == null)
        {
            InitSave();
        }
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

    public int GetHighScore()
    {
        return m_saveObject.Score;
    }

    public void UpdateCoin(int coin)
    {
        m_saveObject.Coins = coin;
    }

    public int GetCoin()
    {
        return m_saveObject.Coins;
    }

    private void OnPlayerDied(GameStateManager.DiedEventArgs e)
    {
        UpdateCoin(e.Score);
        UpdateCoin(e.Coins);
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
            Debug.Log("File not found or can't be read:");
            Debug.Log(e.Message);
            return null;
        }
    }

    public void InitSave()
    {
        SaveObject saveObject = new SaveObject()
        {
            Coins = 0,
            Score = 0,
            SelectedSprites = new SelectedSprites()
            {
                Shirt = 0,
                Beard = 0,
                Hair = 0,
                Pants = 0
            },
            Colors = new Colors
            {
                Beard = new Vector3(1f, 1f, 1f),
                Body = new Vector3(1f, 1f, 1f),
                Eyes = new Vector3(1f, 1f, 1f),
                Hair = new Vector3(1f, 1f, 1f),
                Pants = new Vector3(1f, 1f, 1f),
                Shirt = new Vector3(1f, 1f, 1f),
            }
        }; 

        m_saveObject = saveObject;
        Debug.Log("Creating a new save");
        SaveFile();
    }

    public void SaveFile()
    {
        var json = JsonUtility.ToJson(m_saveObject);

        var docPath = Application.persistentDataPath;
        using (StreamWriter outputFile = new StreamWriter(Path.Combine(docPath, $"{SaveName}.txt")))
        {
            outputFile.WriteLine(json);
            Debug.Log("Saved following JSON to File at " + docPath + "\\"+ SaveName + ".txt" + " : \n" + json);
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
