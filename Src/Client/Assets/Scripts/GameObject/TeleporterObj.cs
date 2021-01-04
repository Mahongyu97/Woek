using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common.Data;
public class TeleporterObj : MonoBehaviour
{
    public int ID;
    private Mesh mesh;
    // Start is called before the first frame update
    void Start()
    {
        mesh = this.gameObject.GetComponent<MeshFilter>().sharedMesh;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (mesh != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawMesh(mesh, transform.position /*+ Vector3.up * transform.localScale.y * 0.5f*/, transform.rotation, transform.lossyScale);
        }
        UnityEditor.Handles.color = Color.red;
        UnityEditor.Handles.ArrowHandleCap(0, transform.position, transform.rotation, 1f, EventType.Repaint);
    }
#endif

    private void OnTriggerEnter(Collider other)
    {
        PlayerInputController pc = other.GetComponent<PlayerInputController>();
        if(pc!=null&& pc.enabled)
        {
            Debug.Log($"OnTriggerEnter Teleporter{ID}");
            TeleporterDefine define = null;
            if(Managers.DataManager.Instance.Teleporters.TryGetValue(ID,out define))
            {
                if (define.LinkTo > 0 && Managers.DataManager.Instance.Teleporters.ContainsKey(define.LinkTo))
                {
                    Services.MapService.Instance.SendMapTeleporter(ID);
                }
            }
        }
    }
}
