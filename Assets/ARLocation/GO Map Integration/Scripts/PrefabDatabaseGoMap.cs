using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ARLocation {
    [CreateAssetMenu(fileName = "PrefabDbGoMap", menuName = "AR+GPS/PrefabDatabaseGoMap")]
    public class PrefabDatabaseGoMap : ScriptableObject
    {
	[System.Serializable]
	public class PrefabDatabaseEntry
	{
	    /// <summary>
	    ///   The `MeshId` associated with the prefab. Should match a `MeshId` from the data created
	    ///   the Web Map Editor (https://editor.unity-ar-gps-location.com).
	    /// </summary>
	    public string MeshId;

	    /// <summary>
	    ///   The prefab you want to associate with the `MeshId`.
	    /// </summary>
	    public GameObject Prefab;

	    public GameObject MapPinPrefab;
	}

	public List<PrefabDatabaseEntry> Entries;

	public PrefabDatabase ToPrefabDb()
	{
	    var db = new PrefabDatabase();

	    db.Entries = new List<PrefabDatabase.PrefabDatabaseEntry>();

	    foreach(var entry in Entries)
	    {
		db.Entries.Add(
			       new PrefabDatabase.PrefabDatabaseEntry()
			       {
				   MeshId = entry.MeshId,
				       Prefab = entry.Prefab,
			       }
		);
	    }
	    
	    return db;
	}

	public GameObject GetEntryById(string Id)
	{
	    GameObject result = null;

	    foreach(var entry in Entries)
	    {
		if (entry.MeshId == Id)
		{
		    result = entry.Prefab;
		    break;
		}
	    }

	    return result;
	}

	public PrefabDatabaseEntry GetDbEntryById(string Id)
	{
	    PrefabDatabaseEntry result = null;

	    foreach(var entry in Entries)
	    {
		if (entry.MeshId == Id)
		{
		    result = entry;
		    break;
		}
	    }

	    return result;
	}
    }
}
