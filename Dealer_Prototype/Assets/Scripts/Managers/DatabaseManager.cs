//using System.Collections;
//using System.Collections.Generic;
//using Constants;
//using SimpleSQL;
//using UnityEngine;

//public struct ResourceData
//{
//    [PrimaryKey] public string RegistryID { get; set; }
//    public string Path { get; set; }
//}

//public struct CharacterDefinitionData
//{
//    [PrimaryKey] public string RegistryID { get; set; }
//}

//[RequireComponent(typeof(SimpleSQLManager))]
//public class DatabaseManager : MonoBehaviour
//{
//    private static DatabaseManager _instance;

//    public static DatabaseManager Instance { get { return _instance; } }

//    private SimpleSQLManager _dbManager;

//    private void Awake()
//    {
//        if (_instance != null && _instance != this)
//        {
//            Destroy(this.gameObject);
//        }
//        else
//        {
//            _instance = this;
//        }

//        Build();
//    }

//    private void Build()
//    {
//        _dbManager = GetComponent<SimpleSQLManager>();
    
//    }

//    public List<string> GetCharacterIDs()
//    {
//        string sql = "SELECT * FROM CharacterDefinition";

//        List<CharacterDefinitionData> IDs = _dbManager.Query<CharacterDefinitionData>(sql);
//        List<string> stringIDs = new List<string>();

//        foreach(CharacterDefinitionData data in IDs)
//        {
//            stringIDs.Add(data.RegistryID);
//        }

//        return stringIDs;
//    }

//    public bool GetResourcePathFromRegistryID(RegistryID RegistryID, out string PathString)
//    {
//        string sql = "SELECT * FROM ResourcePaths WHERE RegistryID = '" + RegistryID.ToString() + "'";

//        List<ResourceData> IDs = _dbManager.Query<ResourceData>(sql);

//        if (IDs.Count > 0)
//        {

//            PathString = IDs[0].Path;
//            if(DebugManager.Instance.LogDatabase) Debug.Log(RegistryID.ToString() + " - " + PathString);
//            return true;
//        }
//        else
//        {
//            PathString = null;
//            if (DebugManager.Instance.LogDatabase) Debug.Log("Nothing found for " + RegistryID.ToString());
//            return false;
//        }
//    }
//}
