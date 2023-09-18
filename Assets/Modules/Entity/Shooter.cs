using System.Collections;
using UnityEngine;

public class Shooter : AttackerBase
{
    public GameObject bulletPrefab;
    public Sprite bulletSprite;

    public bool isPlayer;
    public int damage;

    private bool _onShoot;
    public Transform _target;
    
    private DetectEntity _detectEntity;

    public void Start()
    {
        _onShoot = false;
        StartCoroutine(ImShoot());
        StopAttack();
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
            if (_target == null) continue;
            
            var dir = transform.position - _target.transform.position;
            var rot = Mathf.Atan2(-dir.y, -dir.x) * Mathf.Rad2Deg;
            
            var obj = Instantiate(bulletPrefab, transform.position, Quaternion.Euler(0, 0, rot));
            
            var bullet = obj.GetComponent<Bullet>();
            bullet.Init(isTargetPlayer: !isPlayer, damage, bulletSprite);
            
            yield return new WaitForSeconds(attackInterval);
        }
    }
}