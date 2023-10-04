using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

using static HomePhotoButton;

public class HomePhoto : MonoBehaviour
{
    [System.Serializable]
    public struct Photo
    {
        public bool activatable;
        public Image image;
        public Shadow shadow;
        public GameObject button;
        public VideoPlayer prologueVideo;
        public PhotoType photo;
    }

    [SerializeField] Home homeController;
    [SerializeField] GameObject detail;
    [SerializeField] GameObject panel;

    [SerializeField] int frameToSkip;

    [SerializeField] RectTransform photoGroup;
    [SerializeField] float appearDuration;
    [SerializeField] float suspendDuration;
    [SerializeField] Vector2 mouseHoverPos;

    [SerializeField] bool DEBUG_DONT_PLAY_PROLOGUE_VIDEO;
    [SerializeField] Photo[] photos;

    private int hoverRefCount = 0;

    private void Start()
    {
        UpdateUI();

        for (int i = 0; i < photos.Length; i++)
        {
            var current = photos[i];
            if ((int)current.photo != i)
            {
                var temp = photos[(int)current.photo];
                photos[(int)current.photo] = current;
                photos[i] = temp;
            }
        }
    }

    public void UpdateUI()
    {
        if (!GlobalContainer.contains("HomePhoto") ||
            GlobalContainer.load<bool[]>("HomePhoto") == null)
        {
            var status = new bool[photos.Length];
            for (int i = 0; i < photos.Length; i++)
                status[i] = photos[i].activatable;

            GlobalContainer.store("HomePhoto", status);
        }

        ActivatePhotos();
    }

    private void ActivatePhotos()
    {
        var active = AllPhotoShouldAppear();

        foreach (var photo in photos)
        {
            if (!photo.activatable)
                continue;

            photo.image.gameObject.SetActive(active);
            photo.shadow.gameObject.SetActive(active);
        }
    }

    private void StoreShownHistory(PhotoType photoType)
    {
        var status = GlobalContainer.load<bool[]>("HomePhoto");
        status[(int)photoType] = true;
        GlobalContainer.store("HomePhoto", status);
    }

    private bool AllPhotoShouldAppear()
    {
        bool active = true;
        foreach (var state in GlobalContainer.load<bool[]>("HomePhoto"))
        {
            if (!state)
            {
                active = false;
                break;
            }
        }
        return active;
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

    public void OnEnterPhoto(PhotoType photo)
    {
        if (!AllPhotoShouldAppear())
        {
            photos[(int)photo].image.GetComponent<RectTransform>()
                .anchoredPosition = mouseHoverPos;
            return;
        }

        hoverRefCount++;
        photoGroup.anchoredPosition = mouseHoverPos;
    }

    public void OnExitPhoto(PhotoType photo)
    {
        if (!AllPhotoShouldAppear())
        {
            photos[(int)photo].image.GetComponent<RectTransform>()
                .anchoredPosition = Vector3.zero;
            return;
        }

        hoverRefCount--;
        if (hoverRefCount <= 0)
        {
            hoverRefCount = 0;
            photoGroup.anchoredPosition = Vector3.zero;
        }
    }

    public void OnClickPhoto(PhotoType photoType)
    {
        hoverRefCount = 0;
        GetComponentInParent<Home>().DisableButtons();

        if (AllPhotoShouldAppear())
        {
            homeController.DisableButtons();
            homeController.ActivatePanelBackground(true);

            // show the panel
            panel.SetActive(true);

            // early return
            return;
        }

        var videoPlayer = photos[(int)photoType].prologueVideo;
        if (videoPlayer == null)
        {
            Debug.LogError("ERROR: Unable to find prologue video from such photo. " +
                "This will effect nothing.");
            return;
        }

#if UNITY_EDITOR
        if (DEBUG_DONT_PLAY_PROLOGUE_VIDEO)
        {
            StoreShownHistory(photoType);
            if (!AllPhotoShouldAppear())
            {
                homeController.EnableButtons();
                return;
            }

            // now we can show other photos
            StartCoroutine(PhotoAppearLearp());

            homeController.EnableButtons();
            return;
        }
#endif

        videoPlayer.gameObject.SetActive(true);
        videoPlayer.Play();
        videoPlayer.frame = 0;
        StartCoroutine(PlayPrologue(videoPlayer, photoType));
    }

    private IEnumerator PlayPrologue(VideoPlayer prologuePlayer, PhotoType photoType)
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
        else
        {
            Debug.LogError("ERROR: Could not find prologue video file. " +
                "Please download it from google drive.\n" +
                "Since some files exceeds the size limit of github recommendation, " +
                "it is better to be managed manually.");
            Debug.LogError("Files should be placed in Assets/Art/Prologue/, " +
                "and must be linked to each video player.\n" +
                "Video Player components are located in the following path (in hierachy)\n" +
                "Family photo: 02.Map_0/GNBCanvas/Content/HomePanel/PrologueFamily\n" +
                "Town photo: 02.Map_0/GNBCanvas/Content/HomePanel/PrologueTown");
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

        StoreShownHistory(photoType);
        prologuePlayer.gameObject.SetActive(false);

        if (!AllPhotoShouldAppear())
        {
            homeController.EnableButtons();
            yield break;
        }

        // now we can show other photos
        yield return PhotoAppearLearp();

        homeController.EnableButtons();
    }

    private IEnumerator PhotoAppearLearp()
    {
        // just grab one of the material
        // since all the photos use same shader, we can just grab anything
        var material = photos[0].image.material;
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
            photo.image.gameObject.SetActive(true);

        timeElasped = 0f;
        while (timeElasped < appearDuration)
        {
            material.SetFloat("Value", timeElasped / appearDuration);
            timeElasped += Time.deltaTime;
            yield return null;
        }
        material.SetFloat("Value", 1f);

        // reset shader for shadow effect
        foreach (var photo in photos)
        {
            photo.image.material = null;
            photo.shadow.gameObject.SetActive(true);
            photo.button.SetActive(true);
        }
    }
}
