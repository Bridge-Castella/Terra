using UnityEngine;
using UnityEngine.UI;

public class HomePhotoPanel : MonoBehaviour
{
    [SerializeField] Image detailPanelImage;
    [SerializeField] GameObject homePanelBackground;
    [SerializeField] GameObject photoPanelBackground;

    public void OnClickPhoto(Image image) 
    {
        // change to clicked photo
        detailPanelImage.gameObject.SetActive(true);
        detailPanelImage.sprite = image.sprite;
        detailPanelImage.SetNativeSize();

        homePanelBackground.SetActive(false);
        photoPanelBackground.SetActive(true);
    }

    public void OnClickBackground() 
    {
        detailPanelImage.gameObject.SetActive(false);
        homePanelBackground.SetActive(true);
        photoPanelBackground.SetActive(false);
    }

    public void OnClickExitButton()
    {
        gameObject.SetActive(false);
        homePanelBackground.SetActive(false);
        UIAudio.Post(UIAudio.Instance.inGame_UI_House_Close);
    }
}
