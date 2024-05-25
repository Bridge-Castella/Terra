using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CreditCanvas : MonoBehaviour
{
    [SerializeField]
    private CanvasGroup _demoPanel;
    [SerializeField]
    private CanvasGroup _creditPanel;

    private void Start()
    {
        _demoPanel.DOFade(1f, 1.5f).OnComplete(() =>
        {
            _demoPanel.GetComponent<Button>().interactable = true;
        });
        _demoPanel.GetComponent<Button>().onClick.AddListener(OnClickDemoPanel);
        _creditPanel.GetComponent<Button>().onClick.AddListener(() =>
        {
            SceneManager.LoadScene("01.Login");
        });
    }

    private void OnClickDemoPanel()
    {
        _creditPanel.gameObject.SetActive(true);
        _demoPanel.DOFade(0f, 0.5f);
        _creditPanel.DOFade(1f, 0.5f).OnComplete(() =>
        {
            _creditPanel.GetComponent<Button>().interactable = true;
        });
    }
}
