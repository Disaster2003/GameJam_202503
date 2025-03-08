using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UI;
using System;
using static UnityEngine.UI.Image;
using System.Drawing;
using static UnityEngine.RuleTile.TilingRuleOutput;
using NUnit.Framework.Constraints;
using UnityEngine.Audio;
using Unity.VisualScripting;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        rbody2D = GetComponent<Rigidbody2D>();
        // �R���|�[�l���g�̎擾
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();

        // �^�C�}�[�̏�����
        timerAnimation = 0f;

        NullCheck();

        // �摜�̏�����
        spriteRenderer.sprite = spritesWalk[0];

        //�W�����v�J�E���g������
        JumpCountImage[0].enabled = true;
        for (int i = 1; i < JumpCountImage.Length; i++)
        {
            JumpCountImage[i].enabled = false;  //�S�Ĕ�\���ɂ���
        }

        //�T�E���h�̏�����
        audioSource.clip = moveSE;

        //�v���C���[�R���C�_�[�̏�����
        playerCollider2D = GetComponent<Collider2D>();

        //�ړ����x�̏�����
        moveSpeed = defaultMoveSpeed;

        //����������
        input = Input.GetAxisRaw("Horizontal");
    }

    // Update is called once per frame
    void Update()
    {
        //�A���W�����v�̎��ԓ��Ȃ�
        if(groundTime < jumpComboTime)
        {
            float gageRatio = groundTime / jumpComboTime;
            JumpGageImage.fillAmount = 1.0f - gageRatio;
        }
        //�A���W�����v�̎��ԊO�����ڂ̃W�����v�Ȃ�
        if(groundTime > jumpComboTime || jumpCount == 0)
        {
            JumpGageImage.enabled = false;   //�Q�[�W���\��
        }

        //�W�����v�L�[���������Ƃ�(�n�ʂɂ��Ă���ꍇ)
        if (Input.GetKeyDown(KeyCode.Space) && isGround)
        {
            isJump = true;
            return;
        }

        //�����v���C���[���~�܂��Ă��Ȃ����
        if (input != 0)
        {
            if (RayWidth < 0) RayWidth *= -1;   //���C�̕������ɖ߂�
            RayWidth *= input;  //�����̌����̔��肪�s���悤�ɂ���
        }

       
        //�n�ʂɐڂ��Ă���Ƃ�
        if (isColliding)
        {
            if (!isGround && state == State.FALL)
            {
                state = State.LAND; //�����n�ʂɐڂ���1��ڂȂ�A�j���[�V�����𒅒n�ɂ���
                audioSource.PlayOneShot(landSE); //���nSE�Đ�
            }
            //�~�܂��Ă���Ƃ�
            if (input == 0)
            {
                if (state != State.LAND)  //���n�A�j���[�V�����ł͂Ȃ��A�O�Ɠ����ʒu�Ȃ�
                {
                    state = State.STOP; //�A�j���[�V������ҋ@�ɂ���
                }
            }
            //�����Ă���Ƃ�
            else
            {
                state = State.WALK; //�A�j���[�V����������ɂ���
                audioSource.clip = moveSE;  //SE������ɂ���
            }
            isGround = true; //�㉺�ɓ����Ă��Ȃ��ꍇ
            if(isGround && rbody2D.velocity.y <= 0) moveSpeed = defaultMoveSpeed;   //�ړ����x��ʏ�ɖ߂�
            groundTime += Time.deltaTime;   //�n�ʂƂ̐ڐG���Ԃ̉��Z
            if (groundTime < jumpComboTime && jumpCount != 0) JumpGageImage.enabled = true;   //�Q�[�W��\��
        }
        //�n�ʂɐڂ��Ă��Ȃ��Ƃ�
        else
        {
            isGround = false;
            //�㏸
            if (rbody2D.velocity.y > 0)
            {
                state = State.JUMP; //�A�j���[�V�������W�����v�ɂ���
                groundTime = 0; //�n�ʂƗ��ꂽ�珉����
                JumpGageImage.enabled = false;   //�Q�[�W���\��
            }
            //���~
            else if (rbody2D.velocity.y < 0)
            {
                state = State.FALL; //�A�j���[�V�����𗎉��ɂ���
                groundTime = 0; //�n�ʂƗ��ꂽ�珉����
                JumpGageImage.enabled = false;   //�Q�[�W���\��
            }
        }

        //�n�ʂɂ��Ă��āA3��W�����v���Ă�����
        if (isGround && rbody2D.velocity.y <= 0 && (jumpCount == 0 || groundTime > jumpComboTime))
        {
            for(int i = 0; i < JumpCountImage.Length; i++)
            {
                JumpCountImage[i].enabled = false;
            }
        }

        //�A�j���[�V�����ESE����
        switch (state)
        {
            case State.STOP:
                Animation(spritesStop);
                if(timerAudio >= intervalAudio) audioSource.Stop(); //�����~�߂�
                break;
            case State.WALK:
                Animation(spritesWalk);
                PlaySE(moveSE); //����SE�Đ�
                break;
            case State.RUN:
                Animation(spritesRun);
                break;
            case State.JUMP:
                Animation(spritesJump);
                break;
            case State.FALL:
                Animation(spritesFall);
                audioSource.Stop(); //�����~�߂�
                break;
            case State.LAND:
                Animation(spritesLanded);
                break;
        }

        if (timerAudio < intervalAudio)
        {
            // �C���^�[�o����
            timerAudio += Time.deltaTime;
        }
        if (groundTime > jumpComboTime) jumpCount = 0;  //�A���W�����v�ł͂Ȃ��ꍇ��0
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.layer != LayerMask.NameToLayer(GroundLayer)) return;   //��Q���̃��C���[�ȊO�Ȃ�I��
        ContactPoint2D maxPoint = collision.contacts[0];    //��ԍ����Փ˓_
        foreach (ContactPoint2D contact in collision.contacts)
        {
            //��ԏ������Փ˓_���擾
            if (maxPoint.point.y < contact.point.y)
            {
                maxPoint = contact;
            }
        }

        float angle = Vector2.Angle(maxPoint.normal, Vector2.up); // ������Ƃ̊p�x
        //��������Ȃ��Ȃ�
        if (angle != 90.0f && angle != 270.0f)
        {
            if (maxPoint.point.y <= transform.position.y)
            {
                isColliding = true;
                return;
            }

        }
        //�Ԃ������R���W�������v���C���[�����Ⴂ�ʒu�ɂ�����
        if (collision.collider.bounds.max.y <= playerCollider2D.bounds.min.y)
        {
            isColliding = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer != LayerMask.NameToLayer(GroundLayer)) return;   //��Q���̃��C���[�ȊO�Ȃ�I��
        ContactPoint2D maxPoint = collision.contacts[0];    //��ԍ����Փ˓_
        foreach (ContactPoint2D contact in collision.contacts)
        {
            //��ԏ������Փ˓_���擾
            if (maxPoint.point.y < contact.point.y)
            {
                maxPoint = contact;
            }
        }

        float angle = Vector2.Angle(maxPoint.normal, Vector2.up); // ������Ƃ̊p�x
        //��������Ȃ��Ȃ�
        if (angle != 90.0f && angle != 270.0f)
        {
            if (maxPoint.point.y <= transform.position.y)
            {
                isColliding = true;
                return;
            }

        }
        //�Ԃ������R���W�������v���C���[�����Ⴂ�ʒu�ɂ�����
        if (collision.collider.bounds.max.y <= playerCollider2D.bounds.min.y)
        {
            isColliding = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.layer != LayerMask.NameToLayer(GroundLayer)) return;   //��Q���̃��C���[�ȊO�Ȃ�I��
        isColliding = false;
    }

    void FixedUpdate()
    {
        //�W�����v
        if (isJump && isGround)
        {
            rbody2D.velocity = new Vector2(rbody2D.velocity.x, 0);  //y���̗͂�0�ɂ���
            float totalJumpMultiplier = (float)Math.Pow(jumpMultiplier, jumpCount); //�W�����v�͂̔{�����W�����v�̉񐔂ɂ���ĕς���
            rbody2D.AddForce(Vector2.up * jumpPower * totalJumpMultiplier); //�W�����v
            if(jumpCount == 2) moveSpeed = jumpMoveSpeed;  //�ړ����x���W�����v���̑��x�ɕύX

            AudioSource tempAudioSource = gameObject.AddComponent<AudioSource>();   // �ꎞ�I�� AudioSource ���쐬
            tempAudioSource.pitch = defaultPitch + jumpCount * pitchMultiplier;  // �s�b�`��ύX
            tempAudioSource.PlayOneShot(jumpSE);    // �����Đ�
            Destroy(tempAudioSource, jumpSE.length);  // ���̒�������ɍ폜


            //�W�����v�J�E���g�̕\��
            //1��
            if (jumpCount >= 0) JumpCountImage[0].enabled = true;
            else JumpCountImage[0].enabled = false;
            //2��
            if (jumpCount >= 1) JumpCountImage[1].enabled = true;
            else JumpCountImage[1].enabled = false;
            //3��
            if (jumpCount >= 2) JumpCountImage[2].enabled = true;
            else JumpCountImage[2].enabled = false; 

            jumpCount++;    //�W�����v�񐔂𑝂₷
            jumpCount %= maxJumpCombo; //�ő�A���W�����v�̉񐔂Ŋ��������܂����邱�ƂŘA���W�����v���ő�ȏ�ɔ������Ȃ��悤�ɂ���
            isJump = false;
        }
        // ���Ɉړ�
        if (Input.GetKey(KeyCode.A))
        {
            transform.position -= new Vector3(moveSpeed, 0, 0);
            transform.eulerAngles = new Vector3(0, 180, 0);
            //�v���C���[�ȊO�̉�]�𒼂�

            timerAudio = 0f;
        }
        // �E�Ɉړ�
        if (Input.GetKey(KeyCode.D))
        {
            transform.position += new Vector3(moveSpeed, 0, 0);
            transform.eulerAngles = new Vector3(0, 0, 0);
            timerAudio = 0f;
        }
        input = Input.GetAxisRaw("Horizontal"); //�������擾
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
                    if (state == State.LAND)
                    {
                        state = State.STOP;   //�������n�̃A�j���[�V�������I�������ҋ@�ɕύX
                        return; //�摜���߂�Ȃ��悤�ɂ���
                    }
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

    private void PlaySE(AudioClip clip)
    {
        if (audioSource.clip == clip && audioSource.isPlaying)
        {
            return;   //�������ōĐ����Ȃ�I��
        }
        audioSource.Stop(); //�����~�߂�
        audioSource.clip = clip;    //����ύX
        audioSource.Play(); //�����Đ�
    }


    //�v���C���[�̍s��
    private enum State
    {
        STOP = 0,   //�ҋ@
        WALK = 1,   //����
        RUN = 2,    //����
        JUMP = 3,   //�W�����v
        FALL = 4,   //����
        LAND = 5, //���n
    }


    private bool isColliding = false;   //�R���W�����ɂԂ�������
    Collider2D playerCollider2D = null; //�v���C���[�̃R���C�_�[
    Rigidbody2D rbody2D;  //rigidbody2d���擾
    private float input;    //�v���C���[�̕���
    private State state = State.STOP;   //�v���C���[�̌��݂̓��� 
    private float moveSpeed = 0;
    [Header("�ړ����x")]
    public float defaultMoveSpeed = 0.08f; //�ړ����x
    public float jumpMoveSpeed = 0.15f;  //�W�����v���̈ړ����x
    [Header("�W�����v")]
    public float jumpPower = 350f;    //�W�����v��
    public float jumpMultiplier = 1.2f;    //�A���W�����v���̃W�����v�͂̔{��
    public float jumpComboTime = 1;   //�A���W�����v�̗P�\����
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
    public UnityEngine.UI.Image JumpGageImage = null;
    [SerializeField] public UnityEngine.UI.Image[] JumpCountImage;
    private SpriteRenderer spriteRenderer = null;
    [Header("�A�j���[�V�����摜")]
    [SerializeField] private Sprite[] spritesStop;
    [SerializeField] private Sprite[] spritesWalk;
    [SerializeField] private Sprite[] spritesRun;
    [SerializeField] private Sprite[] spritesJump;
    [SerializeField] private Sprite[] spritesFall;
    [SerializeField] private Sprite[] spritesLanded;
    private float timerAnimation = 0f;
    [SerializeField, Header("�A�j���[�V�����Ԋu")]
    private float INTERVAL_ANIMATION = 0f;

    private AudioSource audioSource;
    private float intervalAudio = 0.5f;    //SE���K���Đ�����鎞��
    private float timerAudio = 0f;
    [Header("SE")]
    public AudioClip moveSE;
    public AudioClip jumpSE;
    public AudioClip landSE;
    public float defaultPitch = 1.0f; //�W�����v�̏����s�b�`
    public float pitchMultiplier = 0f;  //�s�b�`�̏㏸��
}
