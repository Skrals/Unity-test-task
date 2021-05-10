using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelSwitch : MonoBehaviour
{
    [SerializeField] GameObject settingsPanel;
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.H))
        {
            if(!settingsPanel.activeSelf)
            {
                settingsPanel.SetActive(true);
            }
            else
            {
                settingsPanel.SetActive(false);
            }
        }
    }
}
