using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CreditCanvas : MonoBehaviour
{
    [SerializeField]
    private Image _background;
    [SerializeField]
    private CanvasGroup _creditPanel;
    [SerializeField]
    private float _bgFadeRate;

    private CanvasGroup _fadeout;

    public void FadeCredit(CanvasGroup fadeout)
    {
        _fadeout = fadeout;
        gameObject.SetActive(true);
        fadeout.DOFade(0f, 0.5f);
        _background.DOFade(_bgFadeRate, 0.5f);
        _creditPanel.DOFade(1f, 0.5f);

        if (MapManager.state.map != MapManager.MapIndex.Login)
        {
            ControlManager.instance.player.GetComponent<PlayerMove>().enabled = false;
        }
    }

    private void Start()
    {
        _background.GetComponent<Button>().onClick.AddListener(() =>
        {
            _fadeout.DOFade(1f, 0.5f);
            if (MapManager.state.map != MapManager.MapIndex.Login)
            {
                _creditPanel.DOFade(0f, 0.5f).OnComplete(() =>
                {
                    GNBCanvas.instance.OptionPanel.GetComponent<Option>().LoadLoginScene();
                });
            }
            else
            {
                _background.DOFade(0f, 0.5f);
                _creditPanel.DOFade(0f, 0.5f).OnComplete(() =>
                {
                    gameObject.SetActive(false);
                });
            }            
        });
    }
}
