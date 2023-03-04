using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SavingPoint : MonoBehaviour
{
    public Image inputTimeImage;
    public Image inputTimeFillImage;

    [SerializeField] private float range;
    [SerializeField] private float inputTime;

    private Transform player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        if(PlayerInSight())
        {
            if (Input.GetKey(KeyCode.UpArrow))
            {
                inputTimeImage.gameObject.SetActive(true);
                inputTimeFillImage.fillAmount += Time.deltaTime;
                inputTime -= Time.deltaTime;
                
            }

            if (inputTime <= 0f)
            {
                inputTimeImage.gameObject.SetActive(false);
                inputTimeFillImage.fillAmount = 0f;
                inputTime = 1f;
                PositionSaved();
            }

            if (Input.GetKeyUp(KeyCode.UpArrow))
            {
                inputTimeImage.gameObject.SetActive(false);
                inputTimeFillImage.fillAmount = 0f;
                inputTime = 1f;
            }
                
        }
    }

    private void PositionSaved()
    {
        ControlManager.instance.startPoint = transform;
    }

    private bool PlayerInSight()
    {
        if (Vector2.Distance(transform.position, player.position) <= range)
        {
            return true;
        }

        return false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
