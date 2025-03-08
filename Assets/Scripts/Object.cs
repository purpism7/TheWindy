using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        
        spriteRenderer.sortingOrder = -(int)(transform.position.y * 100f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
