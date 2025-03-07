using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UI;
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

        //�ړ����x�̏�����
        moveSpeed = defaultMoveSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        //�A���W�����v�̎��ԓ��Ȃ�
        if(groundTime < jumpComboTime)
        {
            float gageRatio = groundTime / jumpComboTime;
            //JumpGageSpriteRender.transform.localScale = new Vector3(1.0f - gageRatio, 0.1f, 1.0f);  // �Q�[�W��L�k (X���̑傫����ύX)
            JumpGageImage.fillAmount = 1.0f - gageRatio;
        }
        else if(groundTime > jumpComboTime)
        {
           //JumpGageSpriteRender.enabled = false;   //�Q�[�W���\��
            JumpGageImage.enabled = false;   //�Q�[�W���\��
        }

        //�W�����v�L�[���������Ƃ�
        if (Input.GetKeyDown(KeyCode.Space) && isGround)
        {
            isJump = true;
            return;
        }

        float input = Input.GetAxisRaw("Horizontal");   //�v���C���[�̕�����ǂݎ��
        //�����v���C���[���~�܂��Ă��Ȃ����
        if (input != 0)
        {
            if (RayWidth < 0) RayWidth *= -1;   //���C�̕������ɖ߂�
            RayWidth *= input;  //�����̌����̔��肪�s���悤�ɂ���
        }

            Vector2 StartPositionL = transform.position; //�Փ˔���̊J�n�ʒu(�v���C���[�̒��S)
        Vector2 StartPositionR = transform.position + new Vector3(RayWidth, 0, 0); //�Փ˔���̊J�n�ʒu
        int LayerObject = LayerMask.GetMask(GroundLayer);   //�Փ˔�������郌�C���[
        UnityEngine.Color color = UnityEngine.Color.red;
        Debug.DrawRay(StartPositionL, Vector2.down, color, RayDistance);
        Debug.DrawRay(StartPositionR, Vector2.down, color, RayDistance);
        RaycastHit2D HitObjectL = Physics2D.Raycast(StartPositionL, Vector2.down, RayDistance, LayerObject);    //�������ɂ��Ēn�ʂƐڂ��Ă��邩�ǂ���
        RaycastHit2D HitObjectR = Physics2D.Raycast(StartPositionR, Vector2.down, RayDistance, LayerObject);    //�������ɂ��Ēn�ʂƐڂ��Ă��邩�ǂ���

        //�n�ʂɐڂ��Ă���Ƃ�
        if (HitObjectL || HitObjectR)
        {
            moveSpeed = defaultMoveSpeed;   //�ړ����x��ʏ�ɖ߂�
            if (input == 0)
            {
                state = State.STOP; //�A�j���[�V������ҋ@�ɂ���
            }
            else
            {
                state = State.WALK; //�A�j���[�V����������ɂ���
            }

            //�㉺�ɓ����Ă��Ȃ��Ƃ�
            if (rbody2D.velocity.y == 0)
            {
                isGround = true;
                groundTime += Time.deltaTime;   //�n�ʂƂ̐ڐG���Ԃ̉��Z
            }

            //if(groundTime < jumpComboTime) JumpGageSpriteRender.enabled = true;    //�Q�[�W��\��
            if (groundTime < jumpComboTime) JumpGageImage.enabled = true;   //�Q�[�W��\��
        }
        //�n�ʂɐڂ��Ă��Ȃ��Ƃ�
        else
        {
            if (rbody2D.velocity.y >= 0)
            {
                state = State.JUMP; //�A�j���[�V�������W�����v�ɂ���
            }
            else if(rbody2D.velocity.y <= 0)
            {
                state = State.FALL; //�A�j���[�V�����𗎉��ɂ���
            }

            isGround = false;
            groundTime = 0; //�n�ʂƗ��ꂽ�珉����
            //JumpGageSpriteRender.enabled = false;   //�Q�[�W���\��
            JumpGageImage.enabled = false;   //�Q�[�W���\��
        }

        // �i�s�����֌�����ς���
        if (input > 0)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
        }
        else if (input < 0)
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
        }

        //�A�j���[�V��������
        switch (state)
        {
            case State.STOP:
                Animation(spritesStop);
                break;
            case State.WALK:
                Animation(spritesWalk);
                break;
            case State.RUN:
                Animation(spritesRun);
                break;
            case State.JUMP:
                Animation(spritesJump);
                break;
            case State.FALL:
                Animation(spritesFall);
                break;

        }
        
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
            if(jumpCount == 0) moveSpeed = jumpMoveSpeed;  //�ړ����x���W�����v���̑��x�ɕύX
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

    //�v���C���[�̍s��
    enum State
    {
        STOP = 0,   //�ҋ@
        WALK = 1,   //����
        RUN = 2,    //����
        JUMP = 3,   //�W�����v
        FALL = 4,   //����
    }


    Rigidbody2D rbody2D;  //rigidbody2d���擾
    private State state = State.STOP;   //�v���C���[�̌��݂̓���   
    private float moveSpeed = 0;
    [Header("�ړ����x")]
    public float defaultMoveSpeed = 0.1f; //�ړ����x
    public float jumpMoveSpeed = 0.3f;  //�W�����v���̈ړ����x
    [Header("�W�����v")]
    public float jumpPower = 300f;    //�W�����v��
    public float jumpMultiplier = 1.2f;    //�A���W�����v���̃W�����v�͂̔{��
    public float jumpComboTime = 600;   //�A���W�����v�̗P�\����
    private int jumpCount = 0;  //�A���W�����v�̉�
    private static int maxJumpCombo = 3;    //�A���W�����v�̉�
    private bool isJump = false;    //�W�����v�L�[����
    [Header("�n�ʂ̔���")]
    public string GroundLayer = "Block"; //�n�ʂ̔�������郌�C���[
    public float RayDistance = 0.5f;    //���C�̒���
    public float RayWidth = 0.3f;   //���C�̕�
    private float groundTime = 0;   //�n�ʂƐڐG���Ă��鎞��
    private bool isGround  = false; //�n�ʂƐڐG���Ă��邩�ǂ���

    [Header("�W�����v�Q�[�W")]
    //public SpriteRenderer JumpGageSpriteRender = null;
    public UnityEngine.UI.Image JumpGageImage = null;
    private SpriteRenderer spriteRenderer = null;
    [Header("�A�j���[�V�����摜")]
    [SerializeField] private Sprite[] spritesStop;
    [SerializeField] private Sprite[] spritesWalk;
    [SerializeField] private Sprite[] spritesRun;
    [SerializeField] private Sprite[] spritesJump;
    [SerializeField] private Sprite[] spritesFall;
    private float timerAnimation = 0f;
    [SerializeField, Header("�A�j���[�V�����Ԋu")]
    private float INTERVAL_ANIMATION = 0f;
}
