using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapSet : MonoBehaviour
{
    [SerializeField]
    private Button _lookAroundBtn;
    [SerializeField]
    private CanvasGroup _buttonCG;
    [SerializeField]
    private Button _closeBtn;
    [SerializeField]
    private Button _map1Btn;
    [SerializeField]
    private Button _map2Btn;
    [SerializeField]
    private Button _map3Btn;

    private void Start()
    {
        _lookAroundBtn.onClick.AddListener(() =>
        {
            _buttonCG.gameObject.SetActive(true);
            _buttonCG.DOFade(1f, 0.5f);
        });

        _closeBtn.onClick.AddListener(() =>
        {
            _buttonCG.DOFade(0f, 0.5f).OnComplete(() =>
            {
                _buttonCG.gameObject.SetActive(false);
            });
        });

        _map1Btn.onClick.AddListener(() =>
        {
            GlobalContainer.store("StartPos", new Vector3(-67.2f, -17.8f));
            MapManager.LoadMap(MapManager.MapIndex.Map1);
        });

        _map2Btn.onClick.AddListener(() =>
        {
            GlobalContainer.store("StartPos", new Vector3(155.9f, 35.1f));
            MapManager.LoadMap(MapManager.MapIndex.Map2);
        });

        _map3Btn.onClick.AddListener(() =>
        {
            GlobalContainer.store("StartPos", new Vector3(142.9f, 242.01f));
            MapManager.LoadMap(MapManager.MapIndex.Map3);
        });
    }
}
