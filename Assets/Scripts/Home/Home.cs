using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Home : MonoBehaviour
{
    [SerializeField] GameObject[] buttons;

    public void EnableButtons()
    {
        foreach (var button in buttons)
            button.SetActive(true);
    }

    public void DisableButtons()
    {
        foreach (var button in buttons)
            button.SetActive(false);
    }
}
