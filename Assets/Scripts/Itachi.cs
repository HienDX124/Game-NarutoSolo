using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Itachi : MonoBehaviour{

    protected Animator animator;
    protected Player playerScript;

    void Awake()
    {
        animator = GetComponent<Animator>();
        playerScript = GetComponent<Player>();
    }

    void FixedUpdate() {
        playerScript.UpdateBaseAction();
        UpdateSkill();
    }

    void UpdateSkill() {

    }

}
