using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSFXPlayer : MonoBehaviour {

    static CameraSFXPlayer currentInstance;

    public static void PlayClip(string clip)
    {
        if (currentInstance == null)
        {
            currentInstance = GameObject.FindObjectOfType<CameraSFXPlayer>();
        }

        if (currentInstance != null && currentInstance.dictSources.ContainsKey(clip))
        {
            currentInstance.dictSources[clip].Play();
        }
    }

    [System.Serializable]
    public struct StringSources
    {
        public string StrSrc;
        public AudioSource AudioSrc;
    }

    public StringSources[] sources;
    private Dictionary<string, AudioSource> dictSources = new Dictionary<string, AudioSource>();

	// Use this for initialization
	void Start () {
		foreach(StringSources src in sources)
        {
            dictSources.Add(src.StrSrc, src.AudioSrc);
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
