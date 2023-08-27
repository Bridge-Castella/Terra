using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayAttack : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject rangeCenter;
    public GameObject Projectile;
    public Vector3 range;
    public int ProjectileCnt=1;
    public float DelayTime =2.0f;
    public float HeightOffset=5.0f;

    void Start()
    {
        Invoke("DelayProjectile",DelayTime);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void DelayProjectile()
    {
        for (int i = 0; i < ProjectileCnt; ++i)
        {
            GameObject temp = Instantiate(Projectile, new Vector3(rangeCenter.transform.position.x - range.x/2 + (range.x*i / ProjectileCnt), rangeCenter.transform.position.y + HeightOffset,
            rangeCenter.transform.position.z), Quaternion.identity);
            
            ButterProjectile a = temp.GetComponent<ButterProjectile>();
            a.Shoot(rangeCenter);
        }


        Destroy(gameObject, 0.3f);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(rangeCenter.transform.position, range);
    }
}
