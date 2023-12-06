using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadderOpacity : MonoBehaviour
{
    public float value =10.0f;
    public float duration =300000.0f;
    public bool bIsLadderUpdate = false;
    public bool bFirstVisible =true;
    public GameObject particleSystemAlert;
    // Start is called before the first frame update

    private UnityEngine.U2D.SpriteShapeRenderer r;
    private Color c;
    private float remainTime;
    private float time = 0.0f;
    
    void Start()
    {
        if (GetComponentInParent<UnityEngine.U2D.SpriteShapeRenderer>())
        {
            r = GetComponentInParent<UnityEngine.U2D.SpriteShapeRenderer>();
            c = new Color();
            c = Color.white;
            c.a = 0;
            r.color = c;

            // particleSystemAlert = transform.Find("PortalEffect").gameObject;

            //StartCoroutine(LerpFunction(speed));
        }

    }

    // Update is called once per frame
    void Update()
    {
        if(bIsLadderUpdate)
        {
            if (time < duration)
            {
                c.a = Mathf.Lerp(0, 255, time / duration);
                r.color = c;
                time += Time.deltaTime;
                if(time/duration >= 1)
                {
                    bIsLadderUpdate =false;
                    
                }
            }
        }
    }

    public void ShowLadderOpacity()
    {
        if(!bIsLadderUpdate)
        {   bIsLadderUpdate = true;
            particleSystemAlert.active = false;
            //
            UnityEngine.Debug.Log("player is dead");
            //particleSystemAlert.SetActive(false);
        }
    }
}
