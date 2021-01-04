using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour
{

    public BoxCollider MapBoundingBox;
    // Start is called before the first frame update
    void Start()
    {
        Managers.MinimapManager.Instance.UpdateMiniMap(MapBoundingBox);
    }
}
