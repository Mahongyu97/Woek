using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICharInfo : MonoBehaviour {


    public SkillBridge.Message.NCharacterInfo info;

    public Image tex;
    public Text charName;
    // Use this for initialization
    void Start () {
        Set();
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    public void Set()
    {
        if (info != null)
        {
            this.charName.text = this.info.Name;
        }
    }
}
