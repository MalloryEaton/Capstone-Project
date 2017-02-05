using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageController : MonoBehaviour {
    
    private Animator anim;
    public RuneController target;
    Quaternion originalTransform;

    void Start ()
    {
        anim = GetComponent<Animator>();
	}
	
	void Update ()
    {
        
    }

    public void PlayAttack1Animation(RuneController rune)
    {
        originalTransform = transform.rotation;
        transform.LookAt(rune.transform);
        anim.Play("Attack1");
        //transform.rotation = originalTransform;
    }
}
