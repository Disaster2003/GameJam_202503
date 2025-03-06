using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System;
using static UnityEngine.UI.Image;
using System.Drawing;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        rbody2D = GetComponent<Rigidbody2D>();
        // �R���|�[�l���g�̎擾
        spriteRenderer = GetComponent<SpriteRenderer>();

        // �^�C�}�[�̏�����
        timerAnimation = 0f;

        NullCheck();

        // �摜�̏�����
        spriteRenderer.sprite = spritesWalk[0];
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isJump = true;
            return;
        }

        Vector2 StartPositionL = transform.position; //�Փ˔���̊J�n�ʒu(�v���C���[�̒��S)
        Vector2 StartPositionR = transform.position + new Vector3(0.3f, 0, 0); //�Փ˔���̊J�n�ʒu(�v���C���[�̒��S)
        int LayerObject = LayerMask.GetMask(GroundLayer);   //�Փ˔�������郌�C���[
        UnityEngine.Color color = UnityEngine.Color.red;
        Debug.DrawRay(StartPositionL, Vector2.down, color, RayDistance);
        Debug.DrawRay(StartPositionR, Vector2.down, color, RayDistance);
        RaycastHit2D HitObjectL = Physics2D.Raycast(StartPositionL, Vector2.down, RayDistance, LayerObject);    //�������ɂ��Ēn�ʂƐڂ��Ă��邩�ǂ���
        RaycastHit2D HitObjectR = Physics2D.Raycast(StartPositionR, Vector2.down, RayDistance, LayerObject);    //�������ɂ��Ēn�ʂƐڂ��Ă��邩�ǂ���

        if (rbody2D.velocity.y <= 0)
        {
            if (HitObjectL || HitObjectR)
            {
                isGround = true;
                groundTime += Time.deltaTime;   //�n�ʂƂ̐ڐG���Ԃ̉��Z
            }
        }
        else
        {
            isGround = false;
            groundTime = 0; //�n�ʂƗ��ꂽ�珉����
        }

        Animation(spritesWalk);
    }

    /// <summary>
    /// null�`�F�b�N
    /// </summary>
    private void NullCheck()
    {
        if (spriteRenderer == null)
        {
            Debug.LogError("SpriteRenderer�R���|�[�l���g���擾�ł��܂���");
        }
        if (INTERVAL_ANIMATION == 0f)
        {
            Debug.LogError("�A�j���[�V�����Ԋu�����ݒ�ł�");
        }
    }

    /// <summary>
    /// �A�j���[�V����
    /// </summary>
    /// <param name="_sprites">�A�j���[�V�����p�摜</param>
    private void Animation(Sprite[] _sprites)
    {
        if (timerAnimation < INTERVAL_ANIMATION)
        {
            // �C���^�[�o����
            timerAnimation += Time.deltaTime;
            return;
        }

        // �A�j���[�V����
        timerAnimation = 0;
        for (int i = 0; i < _sprites.Length; i++)
        {
            if (spriteRenderer.sprite == _sprites[i])
            {
                if (i == _sprites.Length - 1)
                {
                    // �ŏ��̉摜�ɖ߂�
                    spriteRenderer.sprite = _sprites[0];
                    return;
                }
                else
                {
                    // ���̉摜��
                    spriteRenderer.sprite = _sprites[i + 1];
                    return;
                }
            }
            else if (i == _sprites.Length - 1)
            {
                // �摜��ύX����
                spriteRenderer.sprite = _sprites[0];
                return;
            }
        }
    }
    
    void FixedUpdate()
    {
        //�W�����v
        if (isJump && isGround)
        {
            if (groundTime > jumpComboTime) jumpCount = 0;  //�A���W�����v�ł͂Ȃ��ꍇ��0
            float totalJumpMultiplier = (float)Math.Pow(jumpMultiplier, jumpCount); //�W�����v�͂̔{�����W�����v�̉񐔂ɂ���ĕς���
            rbody2D.AddForce(Vector2.up * jumpPower * totalJumpMultiplier); //�W�����v

            jumpCount++;    //�W�����v�񐔂𑝂₷
            jumpCount %= maxJumpCombo; //�ő�A���W�����v�̉񐔂Ŋ��������܂����邱�ƂŘA���W�����v���ő�ȏ�ɔ������Ȃ��悤�ɂ���
            isJump = false;
        }
        // ���Ɉړ�
        if (Input.GetKey(KeyCode.A))
        {
            transform.position -= new Vector3(moveSpeed, 0, 0);
        }
        // �E�Ɉړ�
        if (Input.GetKey(KeyCode.D))
        {
            transform.position += new Vector3(moveSpeed, 0, 0);
        }
    }

    Rigidbody2D rbody2D;  //rigidbody2d���擾
    public float moveSpeed = 0.1f; //�ړ����x
    public float jumpPower = 300f;    //�W�����v��
    private int jumpCount = 0;  //�A���W�����v�̉�
    private float groundTime = 0;   //�n�ʂƐڐG���Ă��鎞��
    private bool isGround  = false; //�n�ʂƐڐG���Ă��邩�ǂ���
    private bool isJump = false;    //�W�����v�L�[����
    private static int maxJumpCombo = 3;    //�A���W�����v�̉�
    public float jumpMultiplier = 1.2f;    //�A���W�����v���̃W�����v�͂̔{��
    public float jumpComboTime = 600;   //�A���W�����v�̗P�\����
    public string GroundLayer = "Block"; //�n�ʂ̔�������郌�C���[
    public float RayDistance = 0.5f;    //���C�̒���

    private SpriteRenderer spriteRenderer = null;
    [Header("�A�j���[�V�����摜")]
    [SerializeField] private Sprite[] spritesWalk;
    [SerializeField] private Sprite[] spritesRun;
    [SerializeField] private Sprite[] spritesJump;
    private float timerAnimation = 0f;
    [SerializeField, Header("�A�j���[�V�����Ԋu")]
    private float INTERVAL_ANIMATION = 0f;
}
