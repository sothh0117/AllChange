using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Charcter : MonoBehaviour
{
    public GameObject character;
    Animator animator;

    public bool attacked = false;
    public Image nowHpbar;
    public float jumpPower = 350;

    bool inputJump = false;
    bool inputRight = false;
    bool inputLeft = false;
    bool isDead = false;
    Rigidbody2D rigid2D;

    BoxCollider2D col2D;

    public Status status;


    void AttackTrue()
    {
        attacked = true;
    }
    void AttackFalse()
    {
        attacked = false;
    }
    void SetAttackSpeed(float speed)
    {
        animator.SetFloat("attackSpeed", speed);
        status.atkSpeed = speed;
    }

    // Start is called before the first frame update
    void Start()
    {
        status = new Status();
        status = status.SetUnitStatus(UnitCode.swordman);

        character.transform.position = new Vector3(0, 0, 0);
        animator = GetComponent<Animator>();
        SetAttackSpeed(1.5f);

        rigid2D = GetComponent<Rigidbody2D>();
        col2D = GetComponent<BoxCollider2D>();

        StartCoroutine(CheckcharcDeath());

    }

    // Update is called once per frame
    void Update()
    {
        if (isDead) return;
        nowHpbar.fillAmount = (float)status.nowHp / (float)status.maxHp;
        /*float h = Input.GetAxis("Horizontal");
        if (h > 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
            animator.SetBool("moving", true);
            transform.Translate(Vector3.right * Time.deltaTime);
        }
        else if (h < 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
            animator.SetBool("moving", true);
            transform.Translate(Vector3.left * Time.deltaTime);
        }
        else animator.SetBool("moving", false);
        */
        if (Input.GetKey(KeyCode.RightArrow))
        {
            inputRight = true;
            transform.localScale = new Vector3(-1, 1, 1);
            animator.SetBool("moving", true);
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            inputLeft = true;
            transform.localScale = new Vector3(1, 1, 1);
            animator.SetBool("moving", true);
        }
        else animator.SetBool("moving", false);
        if (Input.GetKey(KeyCode.C) &&
            !animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            AttackTrue();
            animator.SetTrigger("attack");
            SFXManager.Instance.PlaySound(SFXManager.Instance.playerAttack);
            AttackFalse();
        }
        if (Input.GetKeyDown(KeyCode.Space) && !animator.GetBool("jumping"))
        {
            inputJump = true;
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            SceneManager.LoadScene("DBscene");
        }
        RaycastHit2D raycastHit = Physics2D.BoxCast(col2D.bounds.center, col2D.bounds.size, 0f, Vector2.down, 0.02f, LayerMask.GetMask("Ground"));
        if (raycastHit.collider != null)
            animator.SetBool("jumping", false);
        else animator.SetBool("jumping", true);
    }

    private void FixedUpdate()
    {
        if (inputRight)
        {
            inputRight = false;
            //rigid2D.AddForce(Vector2.right * moveSpeed);
            rigid2D.velocity = new Vector2(status.moveSpeed, rigid2D.velocity.y);
        }
        if (inputLeft)
        {
            inputLeft = false;
            //rigid2D.AddForce(Vector2.left * moveSpeed);
            rigid2D.velocity = new Vector2(-status.moveSpeed, rigid2D.velocity.y);
        }
        //if (rigid2D.velocity.x >= 2.5f) rigid2D.velocity = new Vector2(2.5f, rigid2D.velocity.y);
        //else if (rigid2D.velocity.x <= -2.5f) rigid2D.velocity = new Vector2(-2.5f, rigid2D.velocity.y);
        if (inputJump)
        {
            inputJump = false;
            rigid2D.AddForce(Vector2.up * jumpPower);
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player1"))
        {
            status.nowHp -= 10;
            Debug.Log(status.nowHp);
            if (status.nowHp <= 0)
            {
                //gameover Àå¸é
                animator.SetTrigger("die");
                Destroy(gameObject);
                Destroy(nowHpbar.gameObject);
            }
        }
    }

    IEnumerator CheckcharcDeath()
    {
        while (true)
        {
            if (transform.position.y < -8)
            {
                SceneManager.LoadScene("main");
            }

            if (status.nowHp <= 0)
            {
                isDead = true;
                animator.SetTrigger("die");
                yield return new WaitForSeconds(2);
                SceneManager.LoadScene("Title");
            }
            yield return new WaitForEndOfFrame();
        }
    }
}
