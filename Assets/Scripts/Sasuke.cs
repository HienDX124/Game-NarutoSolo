using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sasuke : MonoBehaviour
{

    // public Animator animator;
    // public Rigidbody2D myBody;

    public GameObject Kirin;

    protected int SUMMON_KIRIN_STATE = 16;
    protected int KIRIN_ATTACK_STATE = 17;
    
    protected Animator KirinAnimator;
    protected bool firstTimeCompile = true;
    bool KirinIsAppearing = false;

    protected Animator animator;
    protected Player playerScript;

    void Awake()
    {
        animator = GetComponent<Animator>();
        playerScript = GetComponent<Player>();
        playerScript.privateCharacterShuriken = (RuntimeAnimatorController) GameManager.instance.hashtableShurikens["Sasuke"];
    }

    protected void FixedUpdate() {
        playerScript.UpdateBaseAction();
        UpdateSkill();

    }

    public void UpdateSkill() {
        SummonKirin();
    }

    public void SummonKirin() {

        if ((Input.GetKey(KeyCode.U) && playerScript.playerTeam == Player.TEAM_1) || (Input.GetKey(KeyCode.Keypad4) && playerScript.playerTeam == Player.TEAM_2)) {
            if (!KirinIsAppearing) {
                animator.Play("Sasuke_call_kirin");

                Vector3 summonPosition = new Vector3(transform.localPosition.x,3.5f,0);
                Instantiate(Kirin, summonPosition, Quaternion.identity);
                KirinIsAppearing = true;
            }
        }
    }
}
