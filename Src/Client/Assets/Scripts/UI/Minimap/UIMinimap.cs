using Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Managers;

public class UIMinimap : MonoBehaviour {

    public Collider minimapBoundingBox;
    public Image minimap;
    public Image arrow;
    public Text mapName;

    private Transform playerTransform;
	// Use this for initialization
	void Start () {
        MinimapManager.Instance.UIMinimap = this;
    }

    public void UpdateMap()
    {
        this.mapName.text = User.Instance.CurrentMapData.Name;
        this.minimap.overrideSprite = MinimapManager.Instance.LoadCurrentMinimap();
        this.minimap.SetNativeSize();
        this.minimap.transform.localPosition = Vector3.zero;
        
        this.playerTransform = null;
    }

    // Update is called once per frame
    void Update() {
        if (playerTransform == null)
        {
            this.playerTransform = MinimapManager.Instance.Player;
            if (playerTransform == null)
                return;
        }
        if (minimapBoundingBox == null)
        {
            this.minimapBoundingBox = MinimapManager.Instance.MapBoundBox;
            if (minimapBoundingBox == null)
                return;
        }
            
        float realWidth = minimapBoundingBox.bounds.size.x;
        float realHeight = minimapBoundingBox.bounds.size.z;

        float relaX = playerTransform.position.x - minimapBoundingBox.bounds.min.x;
        float relaY = playerTransform.position.z - minimapBoundingBox.bounds.min.z;

        float pivotX = Mathf.Clamp01(relaX / realWidth);
        float pivotY = Mathf.Clamp01(relaY / realHeight);

        this.minimap.rectTransform.pivot = new Vector2(pivotX, pivotY);
        this.minimap.rectTransform.localPosition = Vector2.zero;
        this.arrow.transform.eulerAngles = new Vector3(0, 0, -playerTransform.eulerAngles.y);
	}
}
