using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using FiniteStateMachine;

public class UICameraPos : MonoBehaviour
{
    public static Dictionary<STATE_ID, Transform> dicPos;

    [SerializeField]
    STATE_ID eBelongState;

    private void Awake()
    {
        if (dicPos == null)
            dicPos = new Dictionary<STATE_ID, Transform>();

        if (!dicPos.ContainsKey(eBelongState))
            dicPos.Add(eBelongState, transform);
    }
}
