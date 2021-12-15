using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour{
    private ArrayList arrayList = new ArrayList();

    private int teamSet = 1;

    private GameManager m_GC;
    private CharacterController characterController;

    //  TEMP: BEGIN

    public static GameManager instance;

    protected Player scriptForPlayer1;
    protected Player scriptForPlayer2;


    public GameObject Player1;
    public GameObject Player2;

    public RuntimeAnimatorController[] AnimatorCtrls;
    public Hashtable hashtableAnimators = new Hashtable();

    public RuntimeAnimatorController[] ShurikensAnimators;
    public Hashtable hashtableShurikens = new Hashtable();
    
    public GameObject[] SummonedObjects;
    public Hashtable hashtableSumObjs = new Hashtable();

    public FloatingTextManager floatingTextManager;
    public GameObject playerShuriken;
    //  TEMP: END

    void Awake(){
        instance = this;

        hashtableAnimators.Add("Deidara",AnimatorCtrls[0]);
        hashtableAnimators.Add("Sasuke",AnimatorCtrls[1]);
        hashtableAnimators.Add("Naruto",AnimatorCtrls[2]);
        hashtableAnimators.Add("Itachi",AnimatorCtrls[3]);
     
        hashtableShurikens.Add("Deidara",ShurikensAnimators[0]);
        hashtableShurikens.Add("Sasuke",ShurikensAnimators[1]);

        hashtableSumObjs.Add("clayCentipede", SummonedObjects[0]);
        hashtableSumObjs.Add("Kirin", SummonedObjects[1]);

        scriptForPlayer1 = Player1.GetComponent<Player>();
        scriptForPlayer2 = Player2.GetComponent<Player>();

    }

    public void ShowText(string msg, int fontSize, Color color, Vector3 position, Vector3 motion, float duration) {
        floatingTextManager.Show(msg, fontSize, color, position, motion, duration );
    }

    public void PlayerChooseCharacter(string CharacterName) {
        if (teamSet < 3) {
            if (teamSet == 1) {

                //  Add script
                AddScript(CharacterName, Player1);
                Activator.CreateInstance(Type.GetType(CharacterName));
                
                //  Load Animator
                scriptForPlayer1.SetAnimatorController((RuntimeAnimatorController) hashtableAnimators[CharacterName]);
                scriptForPlayer1.animator.SetInteger("AnimState",0);

                //  Set team
                var type = Type.GetType("Naruto");
                scriptForPlayer1.SetPlayerTeam(teamSet);

                teamSet ++;
            }
            else {

                //  Add script
                AddScript(CharacterName, Player2);

                //  Load Animator
                scriptForPlayer2.SetAnimatorController((RuntimeAnimatorController) hashtableAnimators[CharacterName]);
                scriptForPlayer2.animator.SetInteger("AnimState",0);

                //  Set team
                scriptForPlayer2.SetPlayerTeam(teamSet);
                teamSet ++;

                //  Hide character choose button
                GameObject[] ChooseCharacterBtns = GameObject.FindGameObjectsWithTag("ChooseCharacter");
                foreach (var charBtn in ChooseCharacterBtns)
                {   
                    charBtn.SetActive(false);
                }
            }
        }
        else {
            return;
        }
    }

    public void AddScript(string CharacterScriptName, GameObject go) {

        //  Add component
        var type = Type.GetType(CharacterScriptName);        
        go.AddComponent(type);

    }
}