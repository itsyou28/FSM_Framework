using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FiniteStateMachine;

public class UICameraControl : MonoBehaviour
{

    private void Start()
    {
        FSM_Layer.Inst.RegisterEventChangeLayerState(FSM_LAYER_ID.UserStory, OnChangeUserStory);
    }

    void OnChangeUserStory(TRANS_ID transID, STATE_ID stateid, STATE_ID preStateID)
    {
        Transform t;
        if (UICameraPos.dicPos.TryGetValue(stateid, out t))
            StartCameraMove(t);
    }

    public void StartCameraMove(Transform _target)
    {
        StopCoroutine("CameraMove");
        StartCoroutine("CameraMove", _target);
    }
    
    //현재위치, 목표위치 Vector3.lerp, Quaternion.Lerp
    IEnumerator CameraMove(Transform target)
    {
        float animTime = 1;
        float accumeTime = 0;
        float reviseTime = 0;
        Vector3 startPos = transform.position ;
        Vector3 EndPos = target.position;
        Quaternion startRot = transform.rotation;
        Quaternion endRot = target.rotation;

        while (accumeTime <= animTime)
        {
            accumeTime += Time.unscaledDeltaTime;
            reviseTime = accumeTime / animTime;

            transform.position = Vector3.Slerp(startPos, EndPos, reviseTime);
            transform.rotation = Quaternion.Slerp(startRot, endRot, reviseTime);

            yield return true;
        }
    }
}
