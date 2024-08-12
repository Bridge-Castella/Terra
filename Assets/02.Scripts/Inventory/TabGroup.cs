using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TabGroup : MonoBehaviour
{
    public List<TabUIButton> tabButtons;
    public Sprite tabIdle;
    public Sprite tabHover;
    public Sprite tabActive;
    public TabUIButton seletedTab;
    public List<GameObject> objectsToSwap;
    public TextMeshProUGUI slotTitleText;

    public void OnTabEnter(TabUIButton button)
    {
        ResetTabs();
        if(seletedTab == null || button != seletedTab)
        {
            button.backGround.sprite = tabHover;
            button.icon.sprite = button.selectedIcon;
        }
    }

    public void OnTabExit(TabUIButton button)
    {
        ResetTabs();
    }

    public void OnTabSelected(TabUIButton button)
    {
        seletedTab = button;
        ResetTabs();
        button.backGround.sprite = tabActive;
        button.icon.sprite = button.selectedIcon;
        slotTitleText.text = button.slotTitle;
        int index = button.transform.GetSiblingIndex();
        for(int i = 0; i<objectsToSwap.Count; i++)
        {
            if(i==index)
            {
                objectsToSwap[i].SetActive(true);
                
            }
            else
            {
                objectsToSwap[i].SetActive(false);
            }
        }
    }

    public void ResetTabs()
    {
        foreach(TabUIButton button in tabButtons)
        {
            if(seletedTab != null && button == seletedTab)
            {
                continue;
            }
            button.backGround.sprite = tabIdle;
            button.icon.sprite = button.idleIcon;
        }
    }

    private void Start()
    {
        OnTabSelected(tabButtons[0]);
    }
}
