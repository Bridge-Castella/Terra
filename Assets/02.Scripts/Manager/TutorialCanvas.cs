using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialCanvas : MonoBehaviour
{
    [SerializeField]
    private CanvasGroup fadeCanvasGroup;

    private void Start()
    {
        if (LoginManager.IsGameLoaded)
        {
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(true);

            Sequence sequence = DOTween.Sequence();
            sequence.Append(fadeCanvasGroup.DOFade(1f, 1f))
                .Append(fadeCanvasGroup.DOFade(0f, 1f));
        }
    }
}
