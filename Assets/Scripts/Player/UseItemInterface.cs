using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseItemInterface : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject prefab_obj;

    void Start()
    {
      //prefab_obj = Resources.Load("../Prefabs/Item/Potion_Vine.prefab") as GameObject;
    }

    // Update is called once per frame
    void Update()
    {
        //e��ư ������ keymapping ������ ���� 
        if (Input.GetButtonDown("TalktoNpc"))
        {
            GameObject obj = MonoBehaviour.Instantiate(prefab_obj);
            obj.transform.position = gameObject.transform.position + new Vector3(0,3,0);
            RemovePotion(obj);
            //Invoke("RemovePotion", 2); // 2�ʵ� LaunchProjectile�Լ� ȣ��
        }
    }

    void RemovePotion(GameObject obj)
    {
        Destroy(obj,2.0f);
    }
}
