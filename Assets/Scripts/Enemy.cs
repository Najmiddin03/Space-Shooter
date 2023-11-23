using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4f;

    [SerializeField]
    private AudioSource _explosion;

    private Player _player;
    private Animator _enemyExplode;

    void Start()
    {
        _explosion = GetComponent<AudioSource>();
        _enemyExplode = GetComponent<Animator>();
        if( _enemyExplode == null )
        {
            Debug.LogError("Enemy exlplode is null");
        }
        _player = GameObject.Find("Player").GetComponent<Player>();
        if (_player == null)
        {
            Debug.LogError("Player is null");
        }
        
    }

    void Update()
    {
        transform.Translate(new Vector3(0, -1, 0) * _speed * Time.deltaTime);
        if(transform.position.y <=-6.8f)
        {
            float randomX = Random.Range(-8f, 8f);
            transform.position = new Vector3(randomX, 7, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //laser collision
        if(other.tag == "Laser")
        {
            Destroy(other.gameObject);
            _enemyExplode.SetTrigger("OnEnemyDeath");
            _speed = 0;
            Destroy(GetComponent<Collider2D>());
            _explosion.Play();
            Destroy(this.gameObject, 2.38f);
            _player.Scoring(10);
        }

        //player collision
        if(other.tag == "Player")
        {
            _player.Damage();
            _enemyExplode.SetTrigger("OnEnemyDeath");
            _speed = 0;
            Destroy(GetComponent<Collider2D>());
            _explosion.Play();
            Destroy(this.gameObject, 2.38f);
        }

    }
}
