using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureController : MonoBehaviour
{
  [SerializeField]
  AudioClip growlSound;
  [SerializeField]
  AudioClip attackSound;

  Animator _animator;
  AudioSource _audioEmitter;
  float _speed;
  int _health;
  int _damage;

  float _attackTimer;
  float _soundTimer;
  // Start is called before the first frame update
  void Start()
  {
    _soundTimer = 3f;
    _attackTimer = 0f;
    _animator = GetComponent<Animator>();
    _audioEmitter = GetComponent<AudioSource>();
  }

  public void Init(int health, int damage, float speed)
  {
    _health = health;
    _damage = damage;
    _speed = speed;
  }

  private void OnCollisionEnter(Collision collision)
  {
    BulletController bullet;

    if ((bullet = collision.gameObject.GetComponent<BulletController>()) != null)
    {
      transform.Translate(Vector3.back * 0.5f);
      _health -= bullet.damage;
      if (_health <= 0)
      {
        FindObjectOfType<PlayerController>().AddResource(20);
        Destroy(gameObject);
      }
    }
  }

  // Update is called once per frame
  void Update()
  {
    var mainPlayer = FindObjectOfType<PlayerController>();
    transform.LookAt(mainPlayer.transform);
    _attackTimer -= Time.deltaTime;
    if (Vector3.Distance(transform.position, mainPlayer.transform.position) > 2.5f) //Walk to player
    {
      _animator.SetBool("Walking", true);
      transform.position = Vector3.MoveTowards(transform.position, mainPlayer.transform.position, _speed * Time.deltaTime);
    }
    else //Contact
    {
      if (_attackTimer <= 0)
      {
        _attackTimer = 2f;
        _animator.SetTrigger("Attack");
        _audioEmitter.PlayOneShot(attackSound);
        FindObjectOfType<PlayerController>().Hurt(_damage);
      }
      else
      {
        _animator.SetBool("Walking", false);
      }
    }
    _soundTimer -= Time.deltaTime;
    if (_soundTimer <= 0)
    {
      _soundTimer = 3f;
      if (Random.Range(0, FindObjectsOfType<CreatureController>().Length * 2) == 0) _audioEmitter.PlayOneShot(growlSound);
    }
  }
}
