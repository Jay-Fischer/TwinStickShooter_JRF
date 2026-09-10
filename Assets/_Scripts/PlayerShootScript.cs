using System.Collections;
using UnityEngine;

public class PlayerShootScript : MonoBehaviour
{
    TS_Inputs _inputs;

    [Header("SpawnSetup")]
    public Transform bulletSpawnPoint;

    [Header("Bullet Types")]
    public Rigidbody baseBullet;
    public float ShotForce = 700.0f;

    [Header("Set Shoot Time")]
    public float shootSpeed = 0.2f;
    public bool canShoot;

    private void Awake()
    {
        canShoot = true;
        _inputs = new TS_Inputs();
    }

    private void OnEnable()
    {
        _inputs.Enable();
    }

    private void OnDisable()
    {
        _inputs.Disable();
    }

    private void Update()
    {
        if (_inputs.Player.Shoot.IsPressed() && canShoot)
        {
            StartCoroutine(PlayerShoot());
        }
    }
    IEnumerator PlayerShoot()
    {
        canShoot = false;
        Rigidbody _shot;
        _shot = Instantiate(baseBullet, bulletSpawnPoint.position, bulletSpawnPoint.rotation) as Rigidbody;
        _shot.AddForce(bulletSpawnPoint.forward * ShotForce);
        yield return new WaitForSeconds(shootSpeed);
        canShoot = true;
    }
}
