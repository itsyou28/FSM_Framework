using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIRoot : MonoBehaviour
{
    [SerializeField]
    GameObject[] arrUIPrefab;

    private IEnumerator Start()
    {
        for (int i = 0; i < arrUIPrefab.Length; i++)
        {
            GameObject obj = Instantiate(arrUIPrefab[i]);

            obj.transform.SetParent(transform, false);

            yield return true;
        }
    }
}
