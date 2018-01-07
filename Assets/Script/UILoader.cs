using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FiniteStateMachine;

public class UILoader : MonoBehaviour
{
    [SerializeField]
    Slider progressBar;
    [SerializeField]
    Text progressText;

    void Start()
    {
        State tstate = FSM_Layer.Inst.GetState(FSM_LAYER_ID.UserStory, FSM_ID.USMain, STATE_ID.USMain_Loading);
        tstate.EventStart += OnStart_USLoading;
    }

    private void OnDestroy()
    {
        State tstate = FSM_Layer.Inst.GetState(FSM_LAYER_ID.UserStory, FSM_ID.USMain, STATE_ID.USMain_Loading);
        tstate.EventStart -= OnStart_USLoading;
    }

    private void OnStart_USLoading(TRANS_ID transID, STATE_ID stateID, STATE_ID preStateID)
    {
        StartCoroutine(LoadUI());
    }

    IEnumerator LoadUI()
    {
        ResourceRequest request = Resources.LoadAsync("UIPrefab/UIRoot");

        while(!request.isDone)
        {
            progressBar.value = request.progress;
            progressText.text = Mathf.FloorToInt(request.progress * 100).ToString();

            yield return true;
        }

        progressBar.value = 1;
        progressText.text = "100";

        Instantiate(request.asset);

        UIBinder.Inst.CompleteRegist();
    }
}
