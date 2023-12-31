using System;
using UnityEngine;
using UnityEngine.UI;

public class Bullet : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    private Rigidbody2D _rigid;
    private int _damage;
    private bool _targetPlayer;
    
    public float speed;
    
    public void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _rigid = GetComponent<Rigidbody2D>();
        _rigid.velocity = transform.right * speed;
    }

    public void Init(bool isTargetPlayer, int damage, Sprite sprite = null)
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _rigid = GetComponent<Rigidbody2D>();
        
        if(sprite != null)
            _spriteRenderer.sprite = sprite;
        _damage = damage;
        _targetPlayer = isTargetPlayer;   
        
        _rigid.velocity = transform.right * speed;
    
    }
    
    public void OnTriggerEnter2D(Collider2D col)
    {
        if ((_targetPlayer && col.CompareTag("Player")) || 
            (!_targetPlayer && col.CompareTag("Enemy")))
        {
            col.GetComponent<State>().GetDamage(_damage);
            Destroy(gameObject);
        }
        else if (col.CompareTag("ENV") || col.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
    }
}