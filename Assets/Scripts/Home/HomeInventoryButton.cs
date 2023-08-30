using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeInventoryButton : HomeButtonBase
{
    [SerializeField] HomeInventory controller;

    public override void OnMouseEnter()
    {
        controller.OnHoverInventory();
    }

    public override void OnMouseExit()
    {
        controller.OnHoverEndInventory();
    }

    public override void OnMouseClick()
    {
        controller.OnClickInventory();
    }
}
