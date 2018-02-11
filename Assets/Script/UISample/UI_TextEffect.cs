using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UI_TextEffect : MonoBehaviour
{
    [SerializeField]
    Text text;

    public IEnumerator FlashShow(int number)
    {
        text.text = number.ToString();

        yield return new WaitForSeconds(0.3f);

        text.text = "";
    }
    
    public IEnumerator ScaleAlphaShow(int number)
    {
        text.text = number.ToString();
        text.color = Color.black;

        float aniTime = 0.3f;
        float elapseTime = 0;
        float reverseTime = 1 / aniTime;
        Vector3 vScale = Vector3.zero;

        while(elapseTime <= aniTime)
        {
            elapseTime += Time.deltaTime;

            vScale = Vector3.Lerp(Vector3.zero, Vector3.one, elapseTime * reverseTime);

            transform.localScale = vScale;
            
            yield return true;
        }

        aniTime = 0.2f;
        elapseTime = 0;
        reverseTime = 1 / aniTime;
        Vector3 vt = Vector3.one * 3;
        Color alpha = text.color;

        while(elapseTime <= aniTime)
        {
            elapseTime += Time.deltaTime;
            vScale = Vector3.Lerp(Vector3.one, vt, elapseTime * reverseTime);
            alpha.a = Mathf.Lerp(1, 0, elapseTime * reverseTime);
            text.color = alpha;
            transform.localScale = vScale;

            yield return true;
        }

        text.text = "";
        transform.localScale = Vector3.zero;
    }

}
