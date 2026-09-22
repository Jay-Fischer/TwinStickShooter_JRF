using System.Collections;
using UnityEngine;

public class PlayerShootScript : MonoBehaviour
{
    TS_Inputs _inputs;
    PlayerController _ctrl;

    [Header("SpawnSetup")]
    public Transform bulletSpawnPoint;

    [Header("Bullet Types")]
    public Rigidbody largeBullet;
    public Rigidbody baseBullet;
    public float ShotForce = 700.0f;
    public int shotID = 1;
    public int spreadCount = 3;
    public float totalSpreadAngle = 30;
    public float shotPowerUpTime;
    public float variablePowerUpTime;
    public int shootMultiplier = 1;
    public float shotForceMultiplier = 1;

    [Header("Set Shoot Time")]
    public float shootSpeed = 0.2f;
    public bool canShoot;

    private void Awake()
    {
        _ctrl = GetComponent<PlayerController>();
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
        shotPowerUpTime -= Time.deltaTime;
        variablePowerUpTime -= Time.deltaTime;
        if (shotPowerUpTime <= 0)
        {
            shotID = 1;
        }
        if (variablePowerUpTime <= 0)
        {
            shootMultiplier = 1;
            shotForceMultiplier = 1;
        }
    }
    IEnumerator PlayerShoot()
    {
        if(_ctrl.isGamePad) RumbleManager.Instance.RumblePulse(0.2f, 0.2f, 0.15f);
        canShoot = false;
        Rigidbody _shot;
        switch (shotID)
        {
            case 1:
                _shot = Instantiate(baseBullet, bulletSpawnPoint.position, bulletSpawnPoint.rotation) as Rigidbody;
                _shot.AddForce(bulletSpawnPoint.forward * (ShotForce * shotForceMultiplier));
                yield return new WaitForSeconds(shootSpeed / shootMultiplier);
                
                break;

            case 2:
                _shot = Instantiate(largeBullet, bulletSpawnPoint.position, bulletSpawnPoint.rotation) as Rigidbody;
                _shot.AddForce(bulletSpawnPoint.forward * (ShotForce * shotForceMultiplier) * 0.5f);
                yield return new WaitForSeconds((shootSpeed * 2) / shootMultiplier);

                break;

            case 3:
                float angleStep = totalSpreadAngle / (spreadCount - 1);
                float startAngle = -totalSpreadAngle / 2f;

                for (int i = 0; i < spreadCount; i++)
                {
                    float _angle = startAngle + angleStep * i;
                    Quaternion _rotation = Quaternion.AngleAxis(_angle, Vector3.up);

                    Vector3 _direction = _rotation * transform.forward;

                    _shot = Instantiate(baseBullet, bulletSpawnPoint.position,
                        Quaternion.LookRotation(_direction)) as Rigidbody;
                    _shot.AddForce(_direction * (ShotForce* shotForceMultiplier));
                }
                yield return new WaitForSeconds(shootSpeed / shootMultiplier);

                break;

            
        }
        canShoot = true;

    }

    public void UpgradeSpreadShot()
    {
        if (spreadCount < 64)
        {
            spreadCount++;
        }
        if (totalSpreadAngle < 180)
        {
            totalSpreadAngle += 5;
        }
    }

    public void BigShotPowerup(){
        if (shotID == 2){
            shotPowerUpTime += 15.0f;
        }
        else
        {
            shotID = 2;
            shotPowerUpTime = 15.0f;
        }
    }
    public void SpreadShotPowerup()
    {
        if (shotID == 3)
        {
            shotPowerUpTime += 15.0f;
        }
        else
        {
            shotID = 3;
            shotPowerUpTime = 15.0f;
        }
    }

    public void ShootSpeedPowerUp()
    {
        if (shootMultiplier > 1)
        {
            variablePowerUpTime += 15.0f;
        }
        else
        {
            shootMultiplier = 2;
            shotForceMultiplier = 1;
            variablePowerUpTime = 15.0f;
        }
    }
    public void ShootForcePowerUp()
    {
        if (shotForceMultiplier > 1)
        {
            variablePowerUpTime += 15.0f;
        }
        else
        {
            shootMultiplier = 1;
            shotForceMultiplier = 1.5f;
            variablePowerUpTime = 15.0f;
        }
    }
}
