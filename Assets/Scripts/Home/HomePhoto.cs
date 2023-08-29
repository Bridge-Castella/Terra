using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class HomePhoto : MonoBehaviour
{
    [SerializeField] GameObject detail;
    [SerializeField] GameObject panel;

    [SerializeField] VideoPlayer prologuePlayer;
    [SerializeField] int frameToSkip;

    [SerializeField] GameObject mainButton;
    [SerializeField] GameObject mainShadow;
    [SerializeField] Image mainImage;
    [SerializeField] Image extraImage;
    [SerializeField] Sprite prev;
    [SerializeField] Sprite after;
    [SerializeField] float appearDuration;
    [SerializeField] float suspendDuration;
    [SerializeField] Vector2 mouseHoverPos;

    [SerializeField] Image[] photos;
    [SerializeField] GameObject[] buttons;
    [SerializeField] GameObject[] shadows;

    private void Start()
    {
        // if the prologue vidoe has been played
        if (GlobalContainer.contains("HomePhoto") &&
            GlobalContainer.load<bool>("HomePhoto"))
        {
            // load full image
            mainImage.sprite = after;

            // now buttons should be interactive on start
            foreach (var button in buttons)
                button.SetActive(true);
        }

        // if the prologue video has not been played
        else
        {
            // store value just to prevent null key
            GlobalContainer.store("HomePhoto", false);

            // load partial image
            mainImage.sprite = prev;
            mainShadow.SetActive(true);
            extraImage.gameObject.SetActive(true);

            // disable buttons
            foreach (var button in buttons)
                button.SetActive(false);

            // disable other photos
            foreach (var photo in photos)
                photo.gameObject.SetActive(false);

            foreach (var shadow in shadows)
                shadow.SetActive(false);
        }
    }

    public void UpdateUI(Image photo)
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
        mainImage.transform.parent.GetComponent<RectTransform>()
            .anchoredPosition = mouseHoverPos;
    }

    public void OnExitPhoto()
    {
        mainImage.transform.parent.GetComponent<RectTransform>()
            .anchoredPosition = Vector3.zero;
    }

    public void OnClickPhoto()
    {
        GetComponentInParent<Home>().DisableButtons();

        if (GlobalContainer.load<bool>("HomePhoto"))
        {
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
        // disable buttons
        mainButton.SetActive(false);

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

        // now appear the rendered image
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

        // and activate other buttons too
        mainButton.transform.parent.gameObject.SetActive(true);
        mainButton.SetActive(true);
        foreach (var button in buttons)
            button.SetActive(true);

        foreach (var shadow in shadows)
            shadow.SetActive(true);
    }

    private IEnumerator PhotoAppearLearp()
    {
        // just grab one of the material
        // since all the photos use same shader, we can just grab anything
        var material = photos[0].material;
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
        foreach (var photo in photos)
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
