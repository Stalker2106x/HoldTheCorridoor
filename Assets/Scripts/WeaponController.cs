using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum WeaponState
{
  Idle,
  Cooldown,
  Reloading
}

public enum FireMode
{
  SemiAuto,
  Auto
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
  int maxAmmo = 10;

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
    _cooldownTimer = 0.1f;
  }

  public void Init()
  {
    _audioEmitter = GetComponent<AudioSource>();
    magazineUpgrade = 0;
    damageUpgrade = 0;
    _state = WeaponState.Idle;
    _ammo = maxAmmo;
    UpdateUI();
  }

  public void UpdateUI()
  {
    FindObjectOfType<UIController>().SetAmmoIndicator(_ammo, maxAmmo + (magazineUpgrade * 2));
  }

  public bool Fire()
  {
    if (_state != WeaponState.Idle) return (false);
    if (_ammo == 0)
    {
      if (_ammo == 0) _audioEmitter.PlayOneShot(dryfireSound);
      return (false);
    }
    _audioEmitter.PlayOneShot(gunshotSound);
    var target = new Vector3(transform.position.x, 2, transform.position.z) + transform.forward;
    var bullet = Instantiate(bulletPrefab, target, transform.rotation);
    bullet.GetComponent<BulletController>().damage = (int)(damage * (1 + (damageUpgrade * 0.25f)));
    _ammo--;
    _state = WeaponState.Cooldown;
    UpdateUI();
    return (true);
  }

  public bool Reload()
  {
    if (_ammo == maxAmmo + (magazineUpgrade * 2) || _state == WeaponState.Reloading) return (false);
    _audioEmitter.PlayOneShot(reloadSound);
    _state = WeaponState.Reloading;
    _reloadTimer = 1.2f;
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
        _state = WeaponState.Idle;
        _cooldownTimer = 0.1f;
      }
    }
  }
}
