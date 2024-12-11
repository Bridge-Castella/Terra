using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DetailPanel : MonoBehaviour
{
    public Image icon;
    public TextMeshProUGUI itemNameText;
    public TextMeshProUGUI itemDescriptionText;

    private void Start()
    {
        itemNameText.text = "";
        itemDescriptionText.text = "";
    }

    private void OnDisable()
    {
        icon.gameObject.SetActive(false);
    }
}
