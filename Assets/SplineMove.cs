
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor.U2D;
#endif

using UnityEngine.U2D;
//{
    public class SplineMove : MonoBehaviour
    {
   
    bool bPointMove =false;
    int currentTargetIndex = 1;
    private GameObject playerTerra =null;
    Vector3[] controlPoints;
    int Direction = 1;
    float vertical;

    
    void Start()
    {
        SpriteShapeController spriteShapeController = gameObject.GetComponent<SpriteShapeController>();
        // Get the Spline component of the Sprite Shape Controller
        Spline spline = spriteShapeController.spline;

            // Create an array to store the control points
       controlPoints = new Vector3[spriteShapeController.spline.GetPointCount()];
        
        // Get the control points of the spline and store them in the array
        for (int i = 0; i < controlPoints.Length; i++)
        {
            controlPoints[i] = spline.GetPosition(i) +gameObject.transform.position;
        }
    }
    void FixedUpdate()
    {
        if( bPointMove && null != playerTerra)
        {

            vertical = Input.GetAxis("Vertical");
            if (Mathf.Abs(vertical) > 0f)
            {
                float inputDirection = (vertical);
                if(inputDirection >= 0)
                {
                    //currentTargetIndex -=1;
                    if(Direction == -1)
                    {
                        if(currentTargetIndex < controlPoints.Length -1)
                            currentTargetIndex += 1;
                        else
                            currentTargetIndex  = controlPoints.Length -1;
                    
                    Direction = 1;
                    }
                    if (null != playerTerra)
                        playerTerra.transform.position = Vector3.Lerp(playerTerra.transform.position, controlPoints[currentTargetIndex], Time.deltaTime);
                }
                else
                {
                    if (Direction == 1)
                    {
                        if(currentTargetIndex >=1)
                            currentTargetIndex -= 1;
                        else
                            currentTargetIndex = 0;
                    
                    Direction = -1;
                    }
                    if (null != playerTerra)
                        playerTerra.transform.position = Vector3.Lerp(playerTerra.transform.position, controlPoints[currentTargetIndex], Time.deltaTime);
                }


            }
            else
            {
                
            }

            //Debug.Log("currentIndext "+currentTargetIndex);
            
            CheckArrivePoint();
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        
        if (other.tag == "Player")
        {
            MoveToControlPoint(other.gameObject); 
            //Debug.Log("TriggerOn");
            //Terra 무중력 On
        }
    }

    private void MoveToControlPoint(GameObject player)
    {
        playerTerra = player;
        //currentTargetIndex = 1;

        PlayerMove playerMoveScript = playerTerra.GetComponent<PlayerMove>();
        playerMoveScript.fallGravityMultiflier=0.0f;
        bPointMove = true;
        playerMoveScript.isLaddering = true;
    }
    
    private void CheckArrivePoint()
    {
        if(0<= currentTargetIndex && currentTargetIndex <= controlPoints.Length)
        {
            float dist = Vector3.Distance(playerTerra.transform.position,controlPoints[currentTargetIndex]);   
            if(dist <= 1.5f)
            {         
                currentTargetIndex = currentTargetIndex + (Direction * 1);
                if(controlPoints.Length <= currentTargetIndex)
                {
                    currentTargetIndex = 0;
                    bPointMove =false;
                    Debug.Log("Arrive");
                    
                    //Terra 무중력 Off

                    PlayerMove playerMoveScript = playerTerra.GetComponent<PlayerMove>();
                    playerMoveScript.fallGravityMultiflier=10.0f;
                    playerMoveScript.isLaddering =false;

                    return;
                }
                //Debug.Log("Clear" + (currentTargetIndex - 1) + controlPoints[currentTargetIndex]);
            }
        }
    }
    }
//}
