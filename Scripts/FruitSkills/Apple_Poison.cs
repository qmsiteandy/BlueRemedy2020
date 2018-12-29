using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Apple_Poison : MonoBehaviour {

    //public GameObject poisonSmoke;
    public float explosionRadius = 2.5f;
    public LayerMask enemyLayer;
    public float FX_deleteDelay = 0.75f;

    private ParticleSystem poisonParticle;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rigid;

    void Awake()
    {
        poisonParticle = GetComponent<ParticleSystem>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigid = GetComponent<Rigidbody2D>();
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        Collider2D[] enemys = Physics2D.OverlapCircleAll(transform.position, explosionRadius, enemyLayer);

        for(int x = 0; x < enemys.Length; x++)
        {
            Debug.Log("enemy" + (x + 1) + "："+enemys[x].name);
            //enemys[x].GetComponent<>
        }

        rigid.velocity = Vector2.zero;
        rigid.gravityScale = 0.05f;
        spriteRenderer.enabled = false;

        poisonParticle.Play();
        Destroy(gameObject, FX_deleteDelay);
    }
}
