using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class CharacterBase : MonoBehaviour
{
    protected virtual void Awake()
    {
        SetUp();
    }

    private void Update()
    {
        Updated();
    }

    private void FixedUpdate()
    {
        FixedUpdated();
    }

    public virtual void SetUp() { }
    public virtual void Updated() { }
    public virtual void FixedUpdated() { }
}
