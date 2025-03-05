using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //地面と接触しているかどうか
    public bool GronudCheck()
    {
        //判定内に障害物が入っているならtrue
        if (isGroundEnter || isGroundStay)
        {
            isGround = true;
        }
        else if (isGroundExit)
        {
            isGround = false;   //入っていないならfalse
        }

        //全ての判定をリセット
        isGroundEnter = false;
        isGroundStay = false;
        isGroundExit = false;
        return isGround;
    }

    //判定内に障害物が入った時
    private void OnTriggerEnter2D(Collider2D collision)
    {
        isGroundEnter = true;
    }

    //判定内に障害物が入っている時
    private void OnTriggerStay2D(Collider2D collision)
    {
      　isGroundStay = true;
    }

    //判定内の障害物が出た時
    private void OnTriggerExit2D(Collider2D collision)
    {
        isGroundExit = true;
    }

    private bool isGround = false;  //地面に触れているかどうか
    private bool isGroundEnter = false, isGroundStay = false, isGroundExit = false; //コリジョンのそれぞれの判定
}
