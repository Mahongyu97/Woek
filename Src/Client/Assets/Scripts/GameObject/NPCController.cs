using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Managers;
public class NPCController : MonoBehaviour
{
    public int ID;
    public Animator animator;
    public GameObject Choose;
    private bool isInteractive = false;
    Vector3 orgiForward;
    // Start is called before the first frame update
    void Start()
    {
        if (animator == null) animator = GetComponentInChildren<Animator>();
        if (Choose == null) Choose = transform.Find("Choose").gameObject;
        Choose.SetActive(false);
        orgiForward = transform.forward;
    }

    private void OnMouseEnter()
    {
        Choose.SetActive(true);
    }
    private void OnMouseExit()
    {
        Choose.SetActive(false);
    }
    private void OnMouseDown()
    {
        if (isInteractive == false)
        {
            isInteractive = true;
            StopAllCoroutines();
            StartCoroutine(DoEvent());
        }
    }

    
    IEnumerator DoEvent()
    {
        yield return FaceToPlayer();
        if (NPCManager.Instance.TryNpcResponse(ID))
        {
            animator.SetTrigger("Talk");
        }
        yield return new WaitForSeconds(3);
        isInteractive = false;
        yield return ReturnFaceToPlayer();
    }
    IEnumerator FaceToPlayer()
    {
        Vector3 playerDir = (Models.User.Instance.CurrentCharacterObject.transform.position-transform.position).normalized;
        while (Mathf.Abs(Vector3.Angle(transform.forward,playerDir))>5)
        {
            transform.forward = Vector3.Lerp(transform.forward, playerDir, Time.deltaTime * 5);
            yield return null;
        }
    }
    IEnumerator ReturnFaceToPlayer()
    {
        while (Mathf.Abs(Vector3.Angle(transform.forward, orgiForward)) > 1)
        {
            transform.forward = Vector3.Lerp(transform.forward, orgiForward, Time.deltaTime * 7);
            yield return null;
        }
    }
}
