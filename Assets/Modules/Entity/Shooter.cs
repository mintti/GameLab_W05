using System.Collections;
using UnityEngine;

public class Shooter : Attacker
{
    public GameObject bulletPrefab;
    public Sprite bulletSprite;
    public float shootInterval;

    public bool isPlayer;
    public int damage;

    private bool _onShoot;
    public Transform _target;
    
    private DetectEntity _detectEntity;

    public void Start()
    {
        _onShoot = false;
        _detectEntity = GetComponentInChildren<DetectEntity>();
        _detectEntity.Init(this);
        StartCoroutine(ImShoot());
    }
    
    public override void StartAttack(Transform target)
    {
        _target = target;
        _onShoot = true;
    }
    
    public override void StopAttack()
    {
        _onShoot = false;
    }
    
    IEnumerator ImShoot()
    {
        while (true)
        {
            yield return new WaitUntil(() => _onShoot);
            
            var dir = transform.position - _target.transform.position;
            var rot = Mathf.Atan2(-dir.y, -dir.x) * Mathf.Rad2Deg;
            
            var obj = Instantiate(bulletPrefab, transform.position, Quaternion.Euler(0, 0, rot));
            
            var bullet = obj.GetComponent<Bullet>();
            bullet.Init(isTargetPlayer: !isPlayer, damage, bulletSprite);
            
            yield return new WaitForSeconds(shootInterval);
        }
    }
}