using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICharacterView : MonoBehaviour {

    public GameObject[] characters;
    private Dictionary<int, GameObject> CharacterToPrefab=null;

    private int currentCharacter = 0;

    public int CurrectCharacter
    {
        get
        {
            return currentCharacter;
        }
        set
        {
            currentCharacter = value;
            this.UpdateCharacter();
        }
    }

	// Use this for initialization
	void Start () {
        CharacterToPrefab = new Dictionary<int, GameObject>();

        foreach (var pair in characters)
        {
            int Tid = 0;
            if(int.TryParse(pair.name,out Tid))
            {
                CharacterToPrefab.Add(Tid, pair);
            }
        }
        CurrectCharacter = -1;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void UpdateCharacter()
    {
        if (CharacterToPrefab == null)
        {
            foreach (var pair in characters)
            {
                pair.SetActive(false);
            }
            return;
        }
        foreach (var pair in CharacterToPrefab)
        {
            if (pair.Key == currentCharacter)
            {
                pair.Value.SetActive(true);
            }
            else pair.Value.SetActive(false);
        }
    }
}
