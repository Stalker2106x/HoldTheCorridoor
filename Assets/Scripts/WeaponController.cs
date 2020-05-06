using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum WeaponState
{
  Idle,
  Reloading
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

  int _damage;
  int _ammo;
  int _maxAmmo = 10;
  WeaponState _state;
  AudioSource _audioEmitter;

  public int damageUpgrade;
  public int magazineUpgrade;

  float _reloadTimer;

  // Start is called before the first frame update
  void Start()
  {
  }

  public void Init()
  {
    _audioEmitter = GetComponent<AudioSource>();
    magazineUpgrade = 0;
    damageUpgrade = 0;
    _state = WeaponState.Idle;
    _damage = 20;
    _ammo = _maxAmmo;
    _maxAmmo = 10;
    UpdateUI();
  }

  public void UpdateUI()
  {
    FindObjectOfType<UIController>().SetAmmoIndicator(_ammo, _maxAmmo + (magazineUpgrade * 2));
  }

  public bool Fire()
  {
    if (_ammo == 0 || _state == WeaponState.Reloading)
    {
      if (_ammo == 0) _audioEmitter.PlayOneShot(dryfireSound);
      return (false);
    }
    _audioEmitter.PlayOneShot(gunshotSound);
    var target = new Vector3(transform.position.x, 2, transform.position.z) + transform.forward;
    var bullet = Instantiate(bulletPrefab, target, transform.rotation);
    bullet.GetComponent<BulletController>().damage = (int)(_damage * (1 + (damageUpgrade * 0.1f)));
    _ammo--;
    UpdateUI();
    return (true);
  }

  public bool Reload()
  {
    if (_ammo == _maxAmmo || _state == WeaponState.Reloading) return (false);
    _audioEmitter.PlayOneShot(reloadSound);
    _state = WeaponState.Reloading;
    _reloadTimer = 1.2f;
    return (true);
  }

  public void EndReload()
  {
    _state = WeaponState.Idle;
    _ammo = _maxAmmo + (magazineUpgrade * 2);
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
  }
}
