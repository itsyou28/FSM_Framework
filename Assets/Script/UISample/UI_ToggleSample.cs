using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;
using System.Collections;

public class UI_ToggleSample : MonoBehaviour
{
    [SerializeField]
    ToggleGroup group;
    [SerializeField]
    GameObject[] rowPanel;

    Bindable<string> curToggle;

    private void Awake()
    {
        curToggle = BindableRepo.Inst.GetBindedData(S_Bind_Idx.ToggleMode);
        Initialize();
    }

    void Initialize()
    {
        int maxToggle = 10;
        int half = Mathf.CeilToInt(maxToggle * 0.5f);

        GameObject toggleOrigin = Resources.Load("UIPrefab/ToggleBtn") as GameObject;
        
        for (int i = 0; i < maxToggle; i++)
        {
            GameObject obj = Instantiate(toggleOrigin);

            if (i < half)
                obj.transform.SetParent(rowPanel[0].transform, false);
            else
                obj.transform.SetParent(rowPanel[1].transform, false);

            Toggle toggle = obj.GetComponent<Toggle>();
            Text text = obj.GetComponentInChildren<Text>();
            
            string modeText = "Mode " + i;
            text.text = modeText;

            toggle.group = group;

            toggle.onValueChanged.AddListener((bValue) =>
            {
                if (bValue)
                    curToggle.Value = modeText;
            });

            if (i == 0)
                toggle.isOn = true;
        }
    }
}
