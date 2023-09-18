using System.Collections;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Sprite bulletSprite;
    public float shootInterval;

    public bool isPlayer;
    public int damage;

    private bool _onShoot;
    public Transform _target;

    public void Start()
    {
        _onShoot = true;
        StartCoroutine(ImShoot());
    }
    
    public void StartShootMode(Transform target)
    {
        _target = target;
        _onShoot = true;
    }
    
    public void StopShootMode()
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
            bullet.Init(isPlayer, damage, bulletSprite);
            
            yield return new WaitForSeconds(shootInterval);
        }
    }
}