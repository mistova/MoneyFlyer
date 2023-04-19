using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    Camera cam;
    public GameObject target;
    public float speed = 1;
    public Vector3 posOffset;
    [HideInInspector] public Vector3 defPosOffset;

    public Vector3 rotOffset;
    Quaternion realRotOffset;
    public bool canFollow = true;

    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponent<Camera>();
        canFollow = true;
        defPosOffset = posOffset;

        //Vector3 goPos = GameManager.instance.player.transform.position + posOffset;
        //transform.position = goPos;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (InputManager.IsMoving()) 
            FollowPlayer(target,default, false,false);

    }




    void FollowPlayer(GameObject player, float spd = 0, bool useLerp = false,bool constX = false)
    {
        float realSpeed = 0;
        if (spd == 0) realSpeed = speed;
        else realSpeed = spd;


        Vector3 goPos = player.transform.position + posOffset;
        if (constX)
            goPos.x = 0;

        if (useLerp)
            transform.position = Vector3.Lerp(transform.position, goPos, Time.deltaTime * realSpeed);
        else
            transform.position = Vector3.MoveTowards(transform.position, goPos, Time.deltaTime * realSpeed);

        //Transform target = player.transform;
        //// compute position
        //transform.position = target.TransformPoint(goPos);


        //// compute rotation
        //transform.LookAt(player.transform);

    }


    public void ChangePosOffset(Vector3 posOf,string que = "equal")
    {
        if(que == "plus")
            posOffset += posOf;
        else if (que == "minus")
            posOffset -= posOf;
        else if (que == "equal")
            posOffset = posOf;
    }


    public void ChangeRotation(Vector3 targetRot)
    {
        StartCoroutine(IEChangeRotation(targetRot));
    }


    IEnumerator IEChangeRotation(Vector3 targetRot)
    {
        float dis = Quaternion.Angle(transform.rotation, Quaternion.Euler(targetRot));
        var step = 2 * Time.deltaTime;

        //target.transform.position -= new Vector3(0, 2, 0);

        while (dis > 0)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler((Vector3)targetRot), step);

            dis = Quaternion.Angle(transform.rotation, Quaternion.Euler(targetRot));
            
            yield return null;
        }
    }

}
