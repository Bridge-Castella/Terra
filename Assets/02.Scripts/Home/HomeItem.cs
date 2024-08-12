using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HomeItem : MonoBehaviour
{
    [SerializeField] Image icon;
    [SerializeField] TextMeshProUGUI amount;

    private int idx = -1;
    private string description;
    private HomeDetailPanel detail;
    
    public void UpdateUI(Sprite image, int amount,
        string description, int idx, HomeDetailPanel detail)
    {
        this.icon.sprite = image;
        this.amount.text = amount.ToString();

        this.idx = idx;
        this.description = description;
        this.detail = detail;

        this.icon.gameObject.SetActive(true);
        this.amount.gameObject.SetActive(true);
    }

    public void UpdateDetailPanel()
    {
        if (idx == -1)
            return;

        detail.gameObject.SetActive(true);
        detail.UpdateUI(description, idx);
    }
}
