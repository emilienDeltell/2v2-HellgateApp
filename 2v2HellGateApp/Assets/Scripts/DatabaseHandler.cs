using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using Newtonsoft.Json;
using System.Linq;

public class DatabaseHandler : MonoBehaviour
{
    GameManager gm;
    //Collection of vehicles
    [SerializeField]
    public List<HgStats2v2> _hgStats2v2 = new List<HgStats2v2> ();
    //Root directory where databases exist outside of resources.
    private string _persistentDataPath = string.Empty;
    //Extension of the database files
    private const string FILE_EXTENSION = @".json";
    //Database file name
    private const string DATABASE_NAME = @"hgstats2v2";

    public List<HgStats2v2> HGStats2v2 { get => _hgStats2v2; set => _hgStats2v2 = value; }

    private void Awake() {
        SetDataPath ();
    }

    private void Start() {
        gm = FindObjectOfType<GameManager> ();
        Debug.Log (Application.dataPath);
        OnClick_LoadDatabase ();
        if (HGStats2v2[0].player1 == string.Empty) { // If the players name is empty
            gm.TextChange (gm.ProfileMessageText, gm.CreationString); 
            gm.popUpObject.SetActive (true);
        } else {
            gm.TextChange (gm.ProfileMessageText, gm.ProfileInputString);
            StartCoroutine (LoadFromJsonDatabaseCoroutine (0.1f));
        }
        //OnClick_LoadDatabase ();
    }
    IEnumerator LoadFromJsonDatabaseCoroutine(float loadingTime) {
        yield return new WaitForSeconds (loadingTime);
        gm.LoadFromJsonDatabase ();

    }


    //Loads the HG database from disk.
    public void OnClick_LoadDatabase() {
        HGStats2v2 = ReturnDatabase<HgStats2v2> (DATABASE_NAME);
        print ("Loaded database");
    }

    //Saves the HG's list to disk
    public void OnClick_SaveDatabase() {
        SaveToJson (HGStats2v2, DATABASE_NAME);
        print ("Save database");
    }

    //Set the persistent data path field. 
    private void SetDataPath() {
        string path = Application.persistentDataPath;
        if (path.Length < 1) {
            return;
        }

        //check if the path has the separator
        if (char.Parse (path.Substring (path.Length - 1, 1)) != Path.DirectorySeparatorChar || char.Parse (path.Substring (path.Length - 1, 1)) != Path.AltDirectorySeparatorChar) {
            path += Path.AltDirectorySeparatorChar;
        }

        //check if the path exists
        string directory = Path.GetDirectoryName (path);
        if (!Directory.Exists (directory)) {
            DirectoryInfo info = Directory.CreateDirectory (directory);
            //If creation failed
            if (!info.Exists) {
                Debug.Log ("SetDataPath -> Directory creation failed");
                return;
            }
        }

        _persistentDataPath = path;
        print (_persistentDataPath);
    }


    //Removes the default file extension from path
    //when you load from the resources you dont include the file extension 
    private string RemoveFileExtension(string path) {
        if (path.Length >= FILE_EXTENSION.Length) {
            //If file extension exists, remove it. example: mydirectory/file.json
            if (path.ToLower ().Substring (path.Length - FILE_EXTENSION.Length, FILE_EXTENSION.Length) == FILE_EXTENSION.ToLower ())
                return path.Substring (0, path.Length - FILE_EXTENSION.Length);
            //File extension doesn't exist
            else
                return path;
        }
        //Path isn't long enought to contain file extension.
        else {
            return path;
        }
    }


    //when you save and you load from disk you do include the file extension
    //Adds the default file extension to path.
    private string AddFileExtension(string path) {
        if (path.Length >= FILE_EXTENSION.Length) {
            //If file extension exists, leave it.
            if (path.ToLower ().Substring (path.Length - FILE_EXTENSION.Length, FILE_EXTENSION.Length) == FILE_EXTENSION.ToLower ())
                return path;
            //File extension doesn't exist
            else
                return (path + FILE_EXTENSION);
        }
        //Path isn't long enought to contain file extension.
        else 
        {
            return (path + FILE_EXTENSION);
        }
    }

    //Formats a file path to the current platform
    private string FormatPlatformPath(string path) {
        if (path == string.Empty) {
            Debug.LogError ("FormatPlatformPath -> path is empty");
            return string.Empty;
        }
        //directory\file.json
        //directory/file.json
        path = path.Replace (@"\", @"/");

        string convertedPath = string.Empty;

        string[] directories = path.Split (char.Parse (@"/"));

        for (int i = 0; i < directories.Length; i++) {
            //If only one entry then set path as first entr
            if (directories.Length == 1) {
                convertedPath = directories[i];
                break;
            }
            //More than one directory is specified.
            else {
                convertedPath = Path.Combine (convertedPath, directories[i]);
            }
        }
        return convertedPath;
    }


    //Write the text to a file path
    private void WriteToFile(string text, string path, bool formatPath = true) {
        if (path == string.Empty) {
            Debug.LogError ("WriteToFile -> path is empty");
            return;
        }
        //If to format to the platform
        if (formatPath) {
            path = FormatPlatformPath (path);

        }

        try {
            string directory = Path.GetDirectoryName (path);
            //If the directory doesn't exist try to create it.
            if (!Directory.Exists (directory)) {
                Directory.CreateDirectory (directory);
            }

            using (FileStream fs = new FileStream (path, FileMode.Create)) {
                using (StreamWriter writer = new StreamWriter (fs)) 
                    writer.Write (text);
                
            }
        } catch (Exception ex) {
            Debug.LogError ("WriteToFile -> Save hit an exception of" + ex.Message);
        }

    }


    //Returns string result of a text file from Resources
    private string ReturnFileResource(string path) {
        //Remove default file extension and format the path to the platform
        path = RemoveFileExtension (path);
        path = FormatPlatformPath (path);

        if (path == string.Empty) {
            Debug.LogError ("ReturnFileResource -> path is empty");
            return string.Empty;
        }

        //Try to load text from file path.
        TextAsset textAsset = Resources.Load (path) as TextAsset;

        if (textAsset != null)
            return textAsset.text;
        else {
            Debug.LogError ("ReturnFileResource -> there was nothing here");
            return string.Empty;
        }
            
    }


    //Returns string result of a text file from the persistent data path.
    private string ReturnFileText(string path) {
        //Add default file extension and format the path to the platform
        path = AddFileExtension (path);
        path = FormatPlatformPath (path);

        if (path == string.Empty) {
            Debug.LogError ("ReturnFileText -> path is empty");
            return string.Empty;
        }

        if (!File.Exists(path)) {
            Debug.LogError ("ReturnFileText -> path does not exist");
            return string.Empty;
        }

        try 
        {
            return File.ReadAllText (path);
        } 
        catch (Exception ex)
        {
            Debug.LogError ("ReturnFileText -> exception occurred during retrieval, exception of:" + ex.Message);
            return string.Empty;
        }
    }


    //Returns a database at the File path.
    private List<T> ReturnDatabase<T>(string path) {
        string persistentPath = _persistentDataPath + AddFileExtension (path);
        string result;

        //If persistent path contains file.
        if (File.Exists (persistentPath))
            result = ReturnFileText (persistentPath);
        //Doesn't exist in persistent path, try to load from resources.
        else 
            result = ReturnFileResource (path);

        if (result.Length != 0) {
            return JsonConvert.DeserializeObject<List<T>> (result).ToList ();
        } else {
            Debug.LogWarning ("ReturnDatabase -> result text is empty");
            return new List<T> ();
        }
    }

    //Serializes an object and writes the text to the file at path.
    private void SaveToJson(object saveObject, string path) {
        if (saveObject == null) { 
            Debug.LogWarning ("SaveToJson -> object does not exist");
            return;
        }
        if (path.Length == 0) { 
            Debug.LogWarning ("SaveToJson -> path string does not exist");
            return;
        }
        if (_persistentDataPath == string.Empty) { 
            Debug.LogWarning ("SaveToJson -> persistentDataPath does not exist");
            return;
        }
        //Add extension on file path if it does not already exist.
        path = AddFileExtension (path);

        string result = JsonConvert.SerializeObject (saveObject, Formatting.Indented);
        //Remove directory separate character if it exists on the first character.
        if (char.Parse (path.Substring (0, 1)) == Path.DirectorySeparatorChar || char.Parse (path.Substring (0, 1)) == Path.AltDirectorySeparatorChar) {
            path = path.Substring (1);
        }

        WriteToFile (result, _persistentDataPath + path);
    }

}
