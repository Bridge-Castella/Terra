using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseItemInterface : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject prefab_obj;
    private PlayerMove playerFace;
    private bool itemUsableArea = false;

    void Start()
    {
        playerFace = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMove>();
        //prefab_obj = Resources.Load("../Prefabs/Item/Potion_Vine.prefab") as GameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (!itemUsableArea)
        {
            return;
        }

        //e버튼 누르면 keymapping 예슬님 문의 
        if (Input.GetButtonDown("TalktoNpc"))
        {
            GameObject obj = MonoBehaviour.Instantiate(prefab_obj);

            if (playerFace.facingRight)
            {
                obj.GetComponent<Rigidbody2D>().AddForce(Vector2.right);
                obj.transform.position = transform.position + new Vector3(3, 1, 0);
            }
            else
            {
                obj.GetComponent<Rigidbody2D>().AddForce(Vector2.left);
                obj.transform.position = transform.position + new Vector3(-3, 0, 0);
            }

           



            //bool a = GameObject.Find("terra").GetComponent<PlayerMove>().facingRight;
            //if (a)
            //{
            //    obj.GetComponent<Rigidbody2D>().AddForce(Vector2.right *10000);
            //}
            //else
            //    obj.GetComponent<Rigidbody2D>().AddForce(Vector2.left);
            //RemovePotion(obj);
            //Invoke("RemovePotion", 2); // 2초뒤 LaunchProjectile함수 호출
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        // 사다리인 경우에만 동작
        if (collider.CompareTag("Ladder"))
        {
            itemUsableArea = true;
            return;
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        // 사다리인 경우에만 동작
        if (collider.CompareTag("Ladder"))
        {
            itemUsableArea = false;
            return;
        }
    }

    void RemovePotion(GameObject obj)
    {
        Destroy(obj,2.0f);
    }
}
