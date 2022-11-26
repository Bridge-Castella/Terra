using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum BirdState
{
    FollowPlayer,
    Around,
    Stop,
};
public class BirdMove : MonoBehaviour
{
    // Start is called before the first frame update

    private BirdState state;
    private Transform playerTransform;
    public GameObject player;
    public float followSpeed;
    public float meetDistance;

    void Start()
    {
        followSpeed = 4.0f;
        meetDistance = 1.5f;
        player = GameObject.FindGameObjectWithTag("Player");
        playerTransform = player.GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case BirdState.FollowPlayer:
                {
                    FollowPlayer();
                    break;
                }
            case BirdState.Around:
                {
                    ArroundPlayer();
                    break;
                }
            case BirdState.Stop:
                {

                    break;
                }

                break;

        }  
    }

    void FollowPlayer()
    {
        Vector3 newPos = new Vector3(playerTransform.position.x, playerTransform.position.y);
        transform.position = Vector3.Slerp(transform.position, newPos, followSpeed * Time.deltaTime);
        if (CheckMeetPlayer())
        {
            //state = BirdState.Around;
        }
        //도착 하면 Stop 상태 
    }

    void ArroundPlayer()
    {
        // transform.RotateAround(Planet.transform.position, Vector3.down, speed * Time.deltaTime);
        transform.RotateAround(playerTransform.position, new Vector3(0, 0, 0.0001f), 500.0f * Time.deltaTime);
        //Vector3 newPos = new Vector3(playerTransform.position.x, playerTransform.position.y);
        //transform.position = Vector3.Slerp(transform.position, newPos, followSpeed * Time.deltaTime);
    }

    bool CheckMeetPlayer()
    {
        float distanceOther = Vector3.Distance(transform.position, playerTransform.position);
        if (distanceOther <= meetDistance)
            {
            print("Distance to other: " + distanceOther);
            return true;
                
            }
        print("Distance to other: " + distanceOther);
        return false;
    }

    
    


}
