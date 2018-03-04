using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlayModeTest
{
	[UnityTest]
	public IEnumerator PopupTest()
    {
        AsyncOperation async = SceneManager.LoadSceneAsync("WorkingScene");

        while (!async.isDone)
            yield return null;

        GameObject.Instantiate(Resources.Load("UIPrefab/Popup Canvas"));

		yield return null;

        bool popupShow = true;
        System.Action<bool> callback = (result) => 
        {
            UDL.LogWarning("Callback");
            popupShow = false;
        };
        EMC_MAIN.Inst.NoticeEventOccurrence(EMC_CODE.POPUP, "Title", "Content Message", 0, callback);

        while (popupShow)
            yield return null;
	}
}
