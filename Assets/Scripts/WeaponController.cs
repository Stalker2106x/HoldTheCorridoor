using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponState
{
  Idle,
  Cooldown,
  Reloading
}

public enum FireMode
{
  SemiAuto,
  Auto,
  Burst
}

public class WeaponController : MonoBehaviour
{
  [SerializeField]
  GameObject bulletPrefab;
  [SerializeField]
  AudioClip gunshotSound;
  [SerializeField]
  AudioClip reloadSound;
  [SerializeField]
  AudioClip dryfireSound;

  [SerializeField]
  int damage;

  [SerializeField]
  int bulletsPerShot;

  [SerializeField]
  int maxAmmo;

  [SerializeField]
  int accuracy;

  [SerializeField]
  float cooldownDelay;

  [SerializeField]
  float reloadDelay;

  [SerializeField]
  public FireMode fireMode;

  public bool unlocked;
  int _ammo;
  WeaponState _state;
  AudioSource _audioEmitter;

  public int damageUpgrade;
  public int magazineUpgrade;

  float _reloadTimer;
  float _cooldownTimer;

  // Start is called before the first frame update
  void Start()
  {
    _audioEmitter = GetComponent<AudioSource>();
  }

  public void Init()
  {
    magazineUpgrade = 0;
    damageUpgrade = 0;
    SetState(WeaponState.Idle);
    _ammo = maxAmmo;
    UpdateUI();
  }

  public void UpdateUI()
  {
    FindObjectOfType<UIController>().SetAmmoIndicator(_ammo, maxAmmo + (magazineUpgrade * 2));
  }

  void SetState(WeaponState state)
  {
    _state = state;
    switch (state)
    {
      case WeaponState.Idle:
        break;
      case WeaponState.Cooldown:
        _cooldownTimer = cooldownDelay;
        break;
      case WeaponState.Reloading:
        _reloadTimer = reloadDelay;
        break;
    }
  }

  public bool Fire()
  {
    if (_state != WeaponState.Idle) return (false);
    if (_ammo == 0)
    {
      if (_ammo == 0)
      {
        SetState(WeaponState.Cooldown);
        _audioEmitter.PlayOneShot(dryfireSound);
      }
      return (false);
    }
    _audioEmitter.PlayOneShot(gunshotSound);
    var target = new Vector3(transform.position.x - 0.1f, 2, transform.position.z) + transform.forward;
    for (int i = 0; i < bulletsPerShot; i++)
    {
      var bullet = Instantiate(bulletPrefab, target - new Vector3(0, 0, 0.2f * i), transform.rotation);
      bullet.transform.Rotate(0f, Random.Range(0, (100 - accuracy)/2) * (Random.Range(0, 2) == 0 ? 1 : -1), 0f);
      bullet.GetComponent<BulletController>().damage = (int)(damage * (1 + (damageUpgrade * 0.25f)));
    }
    SetState(WeaponState.Cooldown);
    _ammo--;
    UpdateUI();
    return (true);
  }

  public bool Reload()
  {
    if (_ammo == maxAmmo + (magazineUpgrade * 2) || _state == WeaponState.Reloading) return (false);
    _audioEmitter.PlayOneShot(reloadSound);
    SetState(WeaponState.Reloading);
    return (true);
  }

  public void EndReload()
  {
    _state = WeaponState.Idle;
    _ammo = maxAmmo + (magazineUpgrade * 2);
    UpdateUI();
  }

  // Update is called once per frame
  void Update()
  {
    if (_state == WeaponState.Reloading)
    {
      _reloadTimer -= Time.deltaTime;
      if (_reloadTimer <= 0)
      {
        EndReload();
      }
    }
    if (_state == WeaponState.Cooldown)
    {
      _cooldownTimer -= Time.deltaTime;
      if (_cooldownTimer <= 0)
      {
        SetState(WeaponState.Idle);
      }
    }
  }
}
