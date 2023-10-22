using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeMonster : MonoBehaviour
{
    [SerializeField] private float attackCoolDown;
    [SerializeField] private float range;
    [SerializeField] private LayerMask playerLayer;
    private float cooldownTimer = Mathf.Infinity;

    private Animator anim;

    private MonsterPatrol monsterPatrol;

    public GameObject projectile;
    public GameObject shootPos;
    public GameObject rangeCenter;

    private Transform player;

    private void Awake()
    {
        anim = GetComponent<Animator>();

        player = GameObject.FindGameObjectWithTag("Player").transform;

        monsterPatrol = GetComponentInParent<MonsterPatrol>();
    }

    private void Update()
    {
        cooldownTimer += Time.deltaTime;

        if(PlayerInSight())
        {
            if (cooldownTimer >= attackCoolDown)
            {
                cooldownTimer = 0;
                anim.SetTrigger("meleeAttack");
                Instantiate(projectile, shootPos.transform.position, Quaternion.identity);
                InGameAudio.Post(InGameAudio.Instance.inGame_Monster_mush);
            }
        }

        if(monsterPatrol != null)
        {
            monsterPatrol.enabled = !PlayerInSight();
        }
    }

    private bool PlayerInSight()
    {
        if(Vector2.Distance(rangeCenter.transform.position, player.position) <= range)
        {
            return true;
        }

        return false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(rangeCenter.transform.position, range);
    }
}
