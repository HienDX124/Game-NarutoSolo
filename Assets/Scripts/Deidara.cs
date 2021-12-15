using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//  Thử kế thừa playerKeyboard
public class Deidara : MonoBehaviour
{

    public GameObject clayCentipede;

    private bool clayCentipedeAlive = false;

    protected int SUMMON_CLAY_CENTIPEDE_STATE = 16;

    protected Animator animator;
    protected Player playerScript;

    protected RuntimeAnimatorController DeidaraShuriken;

    void Awake()
    {
        animator = GetComponent<Animator>();
        playerScript = GetComponent<Player>();
        clayCentipede = (GameObject)GameManager.instance.hashtableSumObjs["clayCentipede"];
        playerScript.privateCharacterShuriken = (RuntimeAnimatorController) GameManager.instance.hashtableShurikens["Deidara"];
    }

    void FixedUpdate() {
        playerScript.UpdateBaseAction();
        UpdateSkill();
    }

    void UpdateSkill() {
        if ((Input.GetKey(KeyCode.U) && playerScript.playerTeam == Player.TEAM_1) || (Input.GetKey(KeyCode.Keypad4) && playerScript.playerTeam == Player.TEAM_2)) {
            SummonClayCentipede();
        }
    }

    public void SummonClayCentipede() {
        // animator.SetInteger("AnimState",SUMMON_CLAY_CENTIPEDE_STATE);
        animator.Play("Deidara_summonClayCentipede");

        if (!clayCentipedeAlive) {
            Vector2 summonPosition;
            float summonPosX = gameObject.transform.localPosition.x + 1;
            float summonPosy = gameObject.transform.localPosition.y;
            float temp = clayCentipede.transform.localScale.x;
                if (temp < 0) {
                    temp *= -1;
                }
                clayCentipede.transform.localScale = new Vector3(temp, clayCentipede.transform.localScale.y,0);

            if ( gameObject.transform.localScale.x < 0) {
                summonPosX = gameObject.transform.localPosition.x - 1;

                if (temp > 0) {
                    temp *= -1;
                }
                clayCentipede.transform.localScale = new Vector3(temp, clayCentipede.transform.localScale.y,0);
            }

            summonPosition = new Vector2(summonPosX, summonPosy);
            Instantiate(clayCentipede, summonPosition, Quaternion.identity);
            clayCentipedeAlive = true;
        }
    }

}
