using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState
{
  Active,
  Inactive,
  Dead
}

public class PlayerController : MonoBehaviour
{
  [SerializeField]
  AudioClip errorSound;
  [SerializeField]
  AudioClip upgradeSound;
  [SerializeField]
  AudioClip[] walkSounds;

  public int speedUpgrade;

  int _health;
  int _resources;
  PlayerState _state;
  WeaponController _weapon;

  AudioSource _audioEmitter;
  Animator _animator;
  float _speed;

  float _stepTimer;
  // Start is called before the first frame update
  void Start()
  {
    _stepTimer = 0.5f;
    _animator = GetComponent<Animator>();
    _audioEmitter = transform.Find("SoundEmitter").GetComponent<AudioSource>();
  }

  public void Init()
  {
    Equip(FindObjectOfType<ArsenalController>().weapons[0]);
    _state = PlayerState.Active;
    _health = 100;
    _resources = 75000;
    _speed = 5;
    transform.position = new Vector3(31.14f, 1f, 12.61f);
    _weapon.Init();
    //Update UI
    FindObjectOfType<UIController>().SetHealthIndicator(_health);
    FindObjectOfType<UIController>().SetResourcesIndicator(_resources);
  }

  public void Equip(GameObject weapon)
  {
    //Unequip previous
    if (_weapon)
    {
      var oldWeapon = _weapon.gameObject;
      oldWeapon.SetActive(false);
      oldWeapon.transform.parent = FindObjectOfType<WorkbenchController>().transform;
    }
    //Equip new
    weapon.SetActive(true);
    weapon.transform.SetParent(gameObject.transform, false);
    _weapon = transform.Find("Weapon").GetComponent<WeaponController>();

  }

  public void SetState(PlayerState state)
  {
    if (_state == PlayerState.Active && state != PlayerState.Active) _animator.SetBool("Walking", false);
    _state = state;
  }

  public void AddResource(int value)
  {
    _resources += value;
    FindObjectOfType<UIController>().SetResourcesIndicator(_resources);
  }

  void Fire()
  {
    if (!_weapon.Fire()) return;
    _animator.SetTrigger("Attack");
  }

  void Reload()
  {
    if (!_weapon.Reload()) return;
  }

  public bool Pay(int amount)
  {
    if (_resources < amount)
    {
      _audioEmitter.PlayOneShot(errorSound);
      return (false);
    }
    _resources -= amount;
    _audioEmitter.PlayOneShot(upgradeSound);
    FindObjectOfType<UIController>().SetResourcesIndicator(_resources);
    return (true);
  }

  public void Die()
  {
    _state = PlayerState.Dead;
    FindObjectOfType<UIController>().SetGameOverUI(true);
  }

  public void Hurt(int value)
  {
    _health -= value;
    if (_health < 0) _health = 0;
    if (_health == 0) Die();
    FindObjectOfType<UIController>().SetHealthIndicator(_health);
  }

  void Interact()
  {
    if (Vector3.Distance(transform.position, FindObjectOfType<SwitchController>().transform.position) < 4)
    {
      FindObjectOfType<GameController>().SetWave();
    }
    if (Vector3.Distance(transform.position, GameObject.Find("Workbench").transform.position) < 4)
    {
      SetState(PlayerState.Inactive);
      FindObjectOfType<UIController>().SetWorkbenchUI(true);
    }
  }

  private void OnCollisionEnter(Collision collision)
  {
  }

  // Update is called once per frame
  void Update()
  {
    if (_state != PlayerState.Active) return;
    Vector3 move = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
    RaycastHit hit;
    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

    if (Physics.Raycast(ray, out hit))
    {
      var target = new Vector3(hit.point.x, transform.position.y, hit.point.z);
      transform.LookAt(target);
    }
    if (Input.GetMouseButton(0) && _weapon.fireMode == FireMode.Auto || Input.GetMouseButtonDown(0) && _weapon.fireMode == FireMode.SemiAuto) Fire();
    if (Input.GetKeyDown(KeyCode.R)) Reload();
    if (Input.GetKeyDown(KeyCode.E)) Interact();
    transform.Translate(move * _speed * Time.deltaTime, Space.World);
    _stepTimer -= Time.deltaTime;
    if (move.magnitude != 0 && _stepTimer <= 0)
    {
      _stepTimer = 0.5f;
      _audioEmitter.PlayOneShot(walkSounds[Random.Range(0, walkSounds.Length)]);
    }
    _animator.SetBool("Walking", move.magnitude != 0);
  }
}
