using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using UnityEngine.SceneManagement;

//현재 테스트 코드에서 씬 로드를 하면 기존의 테스트 씬이 언로드 되서 정상적인 테스트가 이뤄지지 않는다. 
public class PlayModeTest
{
	[UnityTest]
	public IEnumerator PopupTest()
    {
        AsyncOperation async = SceneManager.LoadSceneAsync("WorkingScene");

        while (!async.isDone)
            yield return null;

        GameObject.Instantiate(Resources.Load("UIPrefab/Overlay Canvas"));

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


    [UnityTest]
    public IEnumerator MessageTest()
    {
        AsyncOperation async = SceneManager.LoadSceneAsync("WorkingScene");

        while (!async.isDone)
            yield return null;

        GameObject.Instantiate(Resources.Load("UIPrefab/Overlay Canvas"));

        yield return null;

        float elapseTime = 0;
        float preTime = Time.realtimeSinceStartup;

        while (elapseTime < 10)
        {
            elapseTime += Time.realtimeSinceStartup-preTime;
            preTime = Time.realtimeSinceStartup;

            EMC_MAIN.Inst.NoticeEventOccurrence(EMC_CODE.DISP_MSG, elapseTime.ToString());

            yield return new WaitForSeconds(0.8f);
        }
    }
}
