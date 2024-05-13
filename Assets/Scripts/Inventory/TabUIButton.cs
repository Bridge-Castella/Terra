using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Image))]
public class TabUIButton : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
{
    public TabGroup tabGroup;
    public string slotTitle;
    public Image backGround;
    public Image icon;
    public Sprite selectedIcon;
    public Sprite idleIcon;

    public void OnPointerClick(PointerEventData eventData)
    {
        UIAudio.Post(UIAudio.Instance.inGame_UI_Inventory_Click);
        tabGroup.OnTabSelected(this);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        tabGroup.OnTabEnter(this);
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        tabGroup.OnTabExit(this);
    }

    // Start is called before the first frame update
    void Start()
    {
        backGround = GetComponent<Image>();
        icon = transform.GetChild(0).GetComponentInChildren<Image>();
    }
}
