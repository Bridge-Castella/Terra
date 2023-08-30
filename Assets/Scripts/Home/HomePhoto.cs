using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class HomePhoto : MonoBehaviour
{
    [SerializeField] Home homeController;
    [SerializeField] GameObject detail;
    [SerializeField] GameObject panel;

    [SerializeField] VideoPlayer prologuePlayer;
    [SerializeField] int frameToSkip;

    [SerializeField] RectTransform photoGroup;
    [SerializeField] float appearDuration;
    [SerializeField] float suspendDuration;
    [SerializeField] Vector2 mouseHoverPos;

    [SerializeField] Image[] activatablePhotos;
    [SerializeField] GameObject[] activatableButtons;
    [SerializeField] GameObject[] activatableShadows;

    private void Start()
    {
        UpdateUI();
    }

    public void UpdateUI()
    {
        if (!GlobalContainer.contains("HomePhoto"))
            GlobalContainer.store("HomePhoto", false);

        ActivatePhotos(GlobalContainer.load<bool>("HomePhoto"));
    }

    private void ActivatePhotos(bool active)
    {
        foreach (var button in activatableButtons)
            button.SetActive(active);

        foreach (var photo in activatablePhotos)
            photo.gameObject.SetActive(active);

        foreach (var shadow in activatableShadows)
            shadow.SetActive(active);
    }

    public void UpdateDetailPanel(Image photo)
    {
        var rect = detail.GetComponent<RectTransform>();
        var image = detail.GetComponent<Image>();

        // size up image
        Vector2 size = new Vector2
        {
            x = photo.rectTransform.rect.width * 2.0f,
            y = photo.rectTransform.rect.height * 2.0f
        };
        rect.sizeDelta = size;

        // apply image
        image.sprite = photo.sprite;
    }

    public void OnEnterPhoto()
    {
        photoGroup.anchoredPosition = mouseHoverPos;
    }

    public void OnExitPhoto()
    {
        photoGroup.anchoredPosition = Vector3.zero;
    }

    public void OnClickPhoto()
    {
        GetComponentInParent<Home>().DisableButtons();

        if (GlobalContainer.load<bool>("HomePhoto"))
        {
            homeController.DisableButtons();
            homeController.ActivatePanelBackground(true);

            // show the panel
            panel.SetActive(true);
        }
        else
        {
            // play prologue video
            prologuePlayer.gameObject.SetActive(true);
            prologuePlayer.Play();
            prologuePlayer.frame = 0;
            StartCoroutine(PlayPrologue());
        }
    }

    private IEnumerator PlayPrologue()
    {
        // disable all the buttons
        homeController.DisableButtons();

        // this is to make video player prepare for playing
        prologuePlayer.Pause();

        // fade in just to hide frame drop
        SceneFader.instance.FadeIn();
        while (SceneFader.instance.Active)
            yield return null;

        // wait until the player is ready to play
        // but if the clip is null (in case of forgot to download it) we should not check it
        if (prologuePlayer.clip != null)
        {
            while (!prologuePlayer.isPrepared)
                yield return null;
        }

        // now appear the rendering image which used in video player
        var drawImage = prologuePlayer.GetComponent<RawImage>();
        var color = drawImage.color;
        color.a = 1.0f;
        drawImage.color = color;

        // now we can play the video
        SceneFader.instance.FadeOut();
        while (SceneFader.instance.Active)
            yield return null;

        // finally play the video
        prologuePlayer.Play();
        while (prologuePlayer.isPlaying)
            yield return null;

        // notify that the video has been played
        GlobalContainer.store("HomePhoto", true);
        prologuePlayer.gameObject.SetActive(false);

        // now we can show other photos
        yield return PhotoAppearLearp();

        // and activate other buttons
        foreach (var button in activatableButtons)
            button.SetActive(true);

        // reset shader for shadow effect
        foreach (var photo in activatablePhotos)
            photo.material = null;

        // enable shaders
        foreach (var shadow in activatableShadows)
            shadow.SetActive(true);

        homeController.EnableButtons();
    }

    private IEnumerator PhotoAppearLearp()
    {
        // just grab one of the material
        // since all the photos use same shader, we can just grab anything
        var material = activatablePhotos[0].material;
        var timeElasped = 0f;

        // just to make sure that the photo starts from the beginning
        material.SetFloat("Value", 0f);
        yield return null;

        while (timeElasped < suspendDuration)
        {
            timeElasped += Time.deltaTime;
            yield return null;
        }

        // now photo can appear
        foreach (var photo in activatablePhotos)
            photo.gameObject.SetActive(true);

        timeElasped = 0f;
        while (timeElasped < appearDuration)
        {
            material.SetFloat("Value", timeElasped / appearDuration);
            timeElasped += Time.deltaTime;
            yield return null;
        }
        material.SetFloat("Value", 1f);
    }
}
