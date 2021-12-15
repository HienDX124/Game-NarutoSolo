using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour {

    public GameObject enemy;

    public float speed = 8;
    public float maxVelocity = 4;
    public float jumpForce = 56;
    public float BaseDamage = 15;

    public float MaxHitPoint = 100;
    public float currentHP;

    private float timeBtwAttack;
    public float startTimeBtwAttack;
    public Transform attackPos;
    public LayerMask whatIsEnemy;
    public float attackRange;

    [HideInInspector] public RuntimeAnimatorController privateCharacterShuriken;
    float SRK_timeBtwThrow;

    [HideInInspector] public int playerTeam;
    [HideInInspector] public const int TEAM_1 = 1;
    [HideInInspector] public const int TEAM_2 = 2;

    protected int IDLE_STATE = 0;
    protected int RUN_STATE = 1;
    protected int JUMP_STATE = 2;
    protected int ATTACK_NORMAL_STATE = 3;

    [HideInInspector] public int direction = 0;
    [HideInInspector] public const int LEFT_DIR = 0;
    [HideInInspector] public const int RIGHT_DIR = 1;

    protected bool isJumping = false;
    protected bool activeSkill = false;

    [HideInInspector] public Animator animator;
    [HideInInspector] public Rigidbody2D myBody;

    protected virtual void Awake()
    {   
        
        //  Initial basic value
        myBody = this.GetComponent<Rigidbody2D>();
        animator = this.GetComponent<Animator>();
        animator.SetInteger("AnimState",0);
        currentHP = MaxHitPoint;

        //  Attack Normal
        startTimeBtwAttack = 1.5f;
        if (gameObject.name == "Player1") {
            attackPos = GameObject.Find("AttackPos1").transform;
            whatIsEnemy = LayerMask.GetMask("Player" + 2);
        }
        else {
            attackPos = GameObject.Find("AttackPos2").transform;
            whatIsEnemy = LayerMask.GetMask("Player" + 1);
        } 
        attackRange = 0.25f;
        
        //  Shuriken
        SRK_timeBtwThrow = timeBtwAttack * 2;
    }

    protected virtual void FixedUpdate()
    {
        UpdateBaseAction();
    }

    public void UpdateBaseAction() {
        Run();
        Jump();
        AttackNormal();
        PlayerDeath();
        UpdateDirection();
        ThrowShuriken();
    }

    public void SetAnimatorController(RuntimeAnimatorController runtimeCtrl) {
        animator.runtimeAnimatorController = runtimeCtrl;
    }

    public void SetPlayerTeam(int teamSet) {
        playerTeam = teamSet;
    }

    public void Run() {
        float forceX = 0f;
        float velX = Mathf.Abs(myBody.velocity.x);   

        void MoveLeft(){
            direction = LEFT_DIR;
            if (velX < maxVelocity) {
                animator.SetInteger("AnimState",RUN_STATE);
                // animator.Play("Run");
                forceX = -speed;
                Vector3 temp = transform.localScale;
                if (temp.x > 0) {
                    temp.x *= -1;
                    transform.localScale = temp;
                }
            }
        }

        void MoveRight(){
            direction = RIGHT_DIR;
            if (velX < maxVelocity) {
                animator.SetInteger("AnimState",RUN_STATE);
                // animator.Play("Run");
                forceX = speed;
                Vector3 temp = transform.localScale;
                temp.x = Mathf.Abs(temp.x);
                transform.localScale = temp;
            }
        }

        if ((Input.GetKey(KeyCode.D) && playerTeam == TEAM_1) ||(Input.GetKey(KeyCode.RightArrow) && playerTeam == TEAM_2)) { 
            MoveRight();
        }

        else if ( (Input.GetKey(KeyCode.A) && playerTeam == TEAM_1) || (Input.GetKey(KeyCode.LeftArrow) && playerTeam == TEAM_2)) {   
            MoveLeft();
        } 

        else {
            animator.SetInteger("AnimState",IDLE_STATE);
        }
        
        myBody.AddForce(new Vector2(forceX, 0));
    }

    public void Jump() {
        if ((Input.GetKey(KeyCode.Keypad2) && playerTeam == TEAM_2) ||  (Input.GetKey(KeyCode.K) && playerTeam == TEAM_1)) {
            if(isJumping) {
                animator.SetInteger("AnimState",JUMP_STATE);
                return;
            }

            Vector2 tempJumpForce = Vector2.up * jumpForce;
            myBody.AddForce(tempJumpForce);
            isJumping = true;
        }
    }
    
    private void OnCollisionEnter2D(Collision2D col) {
        if (col.gameObject.CompareTag("Ground") || col.gameObject.CompareTag("Player") || col.gameObject.CompareTag("Summoned_object")) {
            isJumping = false;
        }
    }

    public void AttackNormal() {
        if (timeBtwAttack <= 0) {
            if ((Input.GetKey(KeyCode.Keypad1) && playerTeam == TEAM_2) || (Input.GetKey(KeyCode.J) && playerTeam == TEAM_1)) { 
                // animator.SetInteger("AnimState",ATTACK_NORMAL_STATE);
                animator.Play("Hit2_0");
                Debug.Log(this.name + " attacked!");
                Collider2D[] eneiesToDamage = Physics2D.OverlapCircleAll(attackPos.position, attackRange, whatIsEnemy);
                for (int i = 0; i < eneiesToDamage.Length; i++) {
                    eneiesToDamage[i].GetComponent<Player>().TakeDamage(BaseDamage);
                }
            timeBtwAttack = startTimeBtwAttack;
            }
        }
        else {
            timeBtwAttack -= Time.deltaTime;
        }
    }

    public void TakeDamage(float dmg) {
        this.currentHP -= dmg;
        Debug.Log(this.name + " took " + dmg + " dmg");
        animator.Play("Hurted");
        Vector3 ShowTextPos = new Vector3(this.transform.position.x, this.transform.position.y + this.transform.localScale.y/2, this.transform.position.z);

        GameManager.instance.ShowText("-" + dmg.ToString(), 40, Color.yellow, ShowTextPos,  Vector3.up * 35, 1f);
    }

    void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;    
        Gizmos.DrawWireSphere(attackPos.position, attackRange);
    }

    public void PlayerDeath() {
        if (currentHP <= 0) {

            //  add Death Animation
            float durationDeathAnim = 0;
            Destroy(gameObject,durationDeathAnim);
        }
    }

    public void UpdateDirection() {
        if (animator.GetInteger("AnimState") == IDLE_STATE) {
            if (gameObject.transform.position.x > enemy.transform.position.x) {
                Vector3 temp = gameObject.transform.localScale;
                if (temp.x > 0) {
                    temp.x *= -1;
                }
                gameObject.transform.localScale = new Vector3(temp.x, temp.y, temp.z);
                direction = LEFT_DIR;
            } else {
                Vector3 temp = gameObject.transform.localScale;
                temp.x = Mathf.Abs(temp.x);
                gameObject.transform.localScale = new Vector3(temp.x, temp.y, temp.z);
                direction = RIGHT_DIR;
            }
        }
    }

    public void ThrowShuriken() {
        

        if (SRK_timeBtwThrow <= 0) {
            //  Play animation throw weapon

            //  Instancetiate weapon object
                //  Add animator of weapon
            if ((Input.GetKey(KeyCode.Alpha7) && playerTeam == TEAM_1) || (Input.GetKey(KeyCode.Keypad7) && playerTeam == TEAM_2) ) {

                GameObject playerShuriken = GameManager.instance.playerShuriken;
                
                Animator SRK_Animator = playerShuriken.GetComponent<Animator>();
                Rigidbody2D SRK_Body = playerShuriken.GetComponent<Rigidbody2D>();
                Vector3 SRK_position = playerShuriken.transform.position;
                
                Shuriken instanceShuriken = playerShuriken.GetComponent<Shuriken>();
                instanceShuriken.moveSpeed = 5;
                instanceShuriken.shurikenDirection = direction;
                instanceShuriken.ShurikenDamage = BaseDamage * 1.5f;

                SRK_Animator.runtimeAnimatorController = privateCharacterShuriken;

                Instantiate(playerShuriken, attackPos.position, Quaternion.identity);
                
                SRK_timeBtwThrow = startTimeBtwAttack;
            }
        }
        else {
            SRK_timeBtwThrow -= Time.deltaTime;
        }
    }    



}
