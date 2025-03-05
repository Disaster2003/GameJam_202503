using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        // 左に移動
        if (Input.GetKey(KeyCode.A))
        {
            transform.position = new Vector3(transform.position.x - speed, transform.position.y, transform.position.z);
        }
        // 右に移動
        if (Input.GetKey(KeyCode.D))
        {
            transform.position = new Vector3(transform.position.x + speed, transform.position.y, transform.position.z);
        }

        //地面と接触している時
        if(ground.GronudCheck())
        {
            groundTime++;   //地面との接触時間の加算
            //ジャンプ
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Rigidbody2D rbody2D = GetComponent<Rigidbody2D>();  //rigidbody2dを取得
                if (groundTime > comboJumpTime) jumpCount = 0;  //連続ジャンプではない場合は0
                float totalJumpMultiplier = (float)Math.Pow(jumpMultiplier, jumpCount); //ジャンプ力の倍率をジャンプの回数によって変える
                rbody2D.AddForce(Vector2.up * jumpPower * totalJumpMultiplier); //ジャンプ

                jumpCount++;    //ジャンプ回数を増やす
                jumpCount %= 3; //3で割ったあまりを取ることで３回以上の連続ジャンプが発生しないようにする
            }
        }
        else
        {
            groundTime = 0; //地面と離れたら初期化
        }
    }


    private float speed = 0.003f; //移動速度
    private float jumpPower = 300f;    //ジャンプ力
    private static float jumpMultiplier = 1.2f;    //連続ジャンプ時のジャンプ力の倍率
    private int jumpCount = 0;  //連続ジャンプの回数
    private int groundTime = 0;   //地面と接触している時間
    private static int comboJumpTime = 600;   //連続ジャンプの猶予時間
    public GroundCheck ground;  //地面との当たり判定のクラスを設定
}
