using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeartsHealthVisual : MonoBehaviour
{
    public static HeartHealthSystem heartHealthSystemStatic;
    public GameObject hearPrefab;

    [SerializeField] private Sprite heart0Sprite;
    [SerializeField] private Sprite heart1Sprite;
    [SerializeField] private Sprite heart2Sprite;
    [SerializeField] private Sprite heart3Sprite;
    [SerializeField] private Sprite heart4Sprite;

    private List<HeartImage> heartImageList;
    private HeartHealthSystem heartHealthSystem;

    private void Awake()
    {
        heartImageList = new List<HeartImage>();
    }

    private void Start()
    {
        //하트 개수
        HeartHealthSystem heartHealthSystem = new HeartHealthSystem(5);
        SetHeartHealthSystem(heartHealthSystem);
    }

    public void SetHeartHealthSystem(HeartHealthSystem heartHealthSystem)
    {
        this.heartHealthSystem = heartHealthSystem;
        heartHealthSystemStatic = heartHealthSystem;

        //하트시스템에 넣어준 리스트 가져옴(개수)
        List<HeartHealthSystem.Heart> heartList = heartHealthSystem.GetHeartList();
        Vector2 heartAnchoredPosition = new Vector2(0, 0);
        float rotationZ = 8f;

        //캔버스에 하트 포지션 정해줌, 조각정해줌
        for (int i = 0; i< heartList.Count; i++)
        {
            HeartHealthSystem.Heart heart = heartList[i];
            CreateHeartImage(heartAnchoredPosition, rotationZ).SetHeartFragments(heart.GetFragmentAmount());
            if (i%2 != 0)
            {
                heartAnchoredPosition += new Vector2(80, -20);
                rotationZ = 8f;
            }
            else
            {
                heartAnchoredPosition += new Vector2(80, 20);
                rotationZ = -8f;
            }
        }

        heartHealthSystem.OnDamaged += HeartHealthSystem_OnDamaged;
        heartHealthSystem.OnHealed += HeartHealthSystem_OnHealed;
        heartHealthSystem.OnDead += HeartHealthSystem_OnDead;
    }

    private void HeartHealthSystem_OnDead(object sender, System.EventArgs e)
    {
        Debug.Log("player is dead");
        //if(AudioManager.instance != null)
            //AudioManager.instance.PlaySound("life_02");
    }

    private void HeartHealthSystem_OnHealed(object sender, System.EventArgs e)
    {
        //Heart health system was healed        
        RefreshAllHearts();
    }

    private void HeartHealthSystem_OnDamaged(object sender, System.EventArgs e)
    {
        //Heart health system was damaged
        RefreshAllHearts();
    }

    private void RefreshAllHearts()
    {
        List<HeartHealthSystem.Heart> heartList = heartHealthSystem.GetHeartList();
        for (int i = 0; i < heartImageList.Count; i++)
        {
            HeartImage heartImage = heartImageList[i];
            HeartHealthSystem.Heart heart = heartList[i];
            heartImage.SetHeartFragments(heart.GetFragmentAmount());            
        }
    }

    //캔버스에 하트 보여줌
    private HeartImage CreateHeartImage(Vector2 anchoredPosition, float rotationZ)
    {
        GameObject heartGameobject = new GameObject("Heart", typeof(Image), typeof(Animation));

        heartGameobject.transform.SetParent(transform);
        heartGameobject.transform.localPosition = Vector3.zero;

        heartGameobject.GetComponent<RectTransform>().anchoredPosition = anchoredPosition;
        heartGameobject.transform.rotation *= Quaternion.Euler(0f, 0f, rotationZ);
        heartGameobject.GetComponent<RectTransform>().sizeDelta = new Vector2(80, 80);

        //Set heart sprite
        Image heartImageUI = heartGameobject.GetComponent<Image>();
        heartImageUI.sprite = heart0Sprite;

        HeartImage heartImage = new HeartImage(this, heartImageUI);
        heartImageList.Add(heartImage);

        return heartImage;
    }

    //Represents a single Heart
    public class HeartImage
    {
        private Image heartImage;
        private HeartsHealthVisual heartsHealthVisual;
        private int fragments;

        public HeartImage(HeartsHealthVisual heartsHealthVisual, Image heartImage)
        {
            this.heartsHealthVisual = heartsHealthVisual;
            this.heartImage = heartImage;
        }

        public void SetHeartFragments(int fragments)
        {
            this.fragments = fragments;
            switch (fragments)
            {
                case 0: heartImage.sprite = heartsHealthVisual.heart0Sprite;
                    Color alphaColor = new Color();
                    alphaColor.a = 0;
                    heartImage.color = alphaColor; 
                    break;
                case 1: heartImage.sprite = heartsHealthVisual.heart1Sprite; break;
                case 2: heartImage.sprite = heartsHealthVisual.heart2Sprite; break;
                case 3: heartImage.sprite = heartsHealthVisual.heart3Sprite; break;
                case 4: heartImage.sprite = heartsHealthVisual.heart4Sprite; break;
            }
            if (heartHealthSystemStatic.IsDead() || Input.GetKeyDown(KeyCode.R))
            {
                Color alphaColor = new Color(255, 255, 255);
                alphaColor.a = 1;
                heartImage.color = alphaColor;
            }
        }

        public int GetFragmentAmount()
        {
            return fragments;
        }
    }
}
