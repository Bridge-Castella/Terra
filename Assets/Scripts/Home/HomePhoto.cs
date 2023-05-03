using UnityEngine;
using UnityEngine.UI;

public class HomePhoto : MonoBehaviour
{
    [SerializeField] GameObject detail;

    public void UpdateUI(Image photo)
    {
        var rect = detail.GetComponent<RectTransform>();
        var image = detail.GetComponent<Image>();

        Vector2 size = new Vector2();
        size.x = photo.rectTransform.rect.width * 2.0f;
        size.y = photo.rectTransform.rect.height * 2.0f;
        rect.sizeDelta = size;

        image.sprite = photo.sprite;
    }
}
