using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AnimationHelper : SerializedMonoBehaviour
{
    [OdinSerialize]
    private Dictionary<string, Animator> Animators = new Dictionary<string, Animator>();

    public Animator animator { get; private set; }


    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
    }

    public void SetBool(string name, bool value)
    {
        if (Animators.ContainsKey(name))
        {
            foreach(var anim in Animators)
            {
                anim.Value.gameObject.SetActive(false);
            }

            Animators[name].gameObject.SetActive(true);
            Animators[name].SetBool(name, value);
        }
        else
        {
            animator.SetBool(name, value);
        }
    }

    public void SetInt(string name, int value)
    {
        if (Animators.ContainsKey(name))
        {
            foreach (var anim in Animators)
            {
                anim.Value.gameObject.SetActive(false);
            }

            Animators[name].gameObject.SetActive(true);
            Animators[name].SetInteger(name, value);
        }
        else
        {
            animator.SetInteger(name, value);
        }
    }
}
