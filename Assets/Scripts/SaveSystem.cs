using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public enum diff{ easy, medium,hard,insane};
public static class SaveSystem
{
    public static void Save(ScoreBoard board, int difficulty)
    {

#if UNITY_ANDROID && !UNITY_EDITOR
        string path = Application.persistentDataPath + "/" + (diff)difficulty + ".score";
#else
        string path = Application.streamingAssetsPath + "/" + (diff)difficulty + ".score";
#endif
        BinaryFormatter fortmatter = new BinaryFormatter();
        FileStream stream = new FileStream(path, FileMode.Create);

        fortmatter.Serialize(stream, board);
        stream.Close();
    }
    public static ScoreBoard Load(int difficulty)
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        string path = Application.persistentDataPath + "/" + (diff)difficulty + ".score";
#else
        string path = Application.streamingAssetsPath  + "/" + (diff)difficulty + ".score";
#endif
        if (File.Exists(path))
        {
            BinaryFormatter fortmatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            ScoreBoard board = (ScoreBoard)fortmatter.Deserialize(stream);
            stream.Close();
            return board;
        }
        else
        {
            ScoreBoard board = new ScoreBoard();
            Save(board, difficulty);
            return board;
        }
    }
    public static void EraseGame()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        string path = Application.persistentDataPath + "/lastGame.save";
#else
        string path = Application.streamingAssetsPath + "/lastGame.save";
#endif
        if (File.Exists(path))
        {
            File.Delete(path);
        }
    }
    public static void SaveGame(int difficulty, int score, List<Ball> board)
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        string path = Application.persistentDataPath + "/lastGame.save";
#else
        string path = Application.streamingAssetsPath + "/lastGame.save";
#endif
        BinaryFormatter fortmatter = new BinaryFormatter();
        FileStream stream = new FileStream(path, FileMode.Create);
        GameData gameData = new GameData(difficulty, score, board);
        fortmatter.Serialize(stream, gameData);
        stream.Close();
    }
    public static GameData LoadGame()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        string path = Application.persistentDataPath + "/lastGame.save";
#else
        string path = Application.streamingAssetsPath + "/lastGame.save";
#endif
        if (File.Exists(path))
        {
            BinaryFormatter fortmatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            GameData game = (GameData)fortmatter.Deserialize(stream);
            stream.Close();
            return game;
        }
        return null;
    }
    
}
