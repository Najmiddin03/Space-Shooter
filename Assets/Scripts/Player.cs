using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _speed = 5.5f;
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private GameObject _tripleShotPrefab;
    [SerializeField]
    private GameObject _explosionPrefab;
    [SerializeField]
    private GameObject _shieldVisualiser;
    [SerializeField]
    private GameObject[] _engine;
    [SerializeField]
    private float _fireRate = 0.15f;
    private float _canFire = -1f;
    [SerializeField]
    private int _lives = 3;
    private SpawnManager _spawnManager;
    [SerializeField]
    private int _score = 0;

    [SerializeField]
    private AudioSource _audioSource;
    [SerializeField]
    private AudioClip _laserSound;

    private bool _isTripleShotActive = false;
    private bool _isShieldActive = false;

    private UIManager _uiManager;

    void Start()
    {
        transform.position = new Vector3(0, -3, 0);
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        if( _spawnManager == null )
        {
            Debug.LogError("The spawn manager is null");
        }
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        if(_uiManager == null)
        {
            Debug.LogError("The UI Manager is null");
        }
        _audioSource = GetComponent<AudioSource>();
        if (_audioSource == null)
        {
            Debug.LogError("Laser Sound is null");
        }
    }

    void Update()
    {
        Movement();
        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
        {
            FireLaser();
        }
    }

    void Movement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        transform.Translate(new Vector3(1,0,0) * _speed * horizontalInput * Time.deltaTime);
        transform.Translate(new Vector3(0,1,0) * _speed * verticalInput * Time.deltaTime);

        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -4.8f, 0), 0);

        if ( transform.position.x >= 11.2f )
        {
            transform.position = new Vector3(-11.2f, transform.position.y, 0 );
        }
        else if ( transform.position.x <=-11.2f )
        {
            transform.position = new Vector3(11.2f, transform.position.y, 0);
        }
    }

    void FireLaser()
    {
        _canFire = Time.time + _fireRate;
        if( _isTripleShotActive == true )
        {
            Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);
        }
        else
        {
            Instantiate(_laserPrefab, transform.position + new Vector3(0, 1.03f, 0), Quaternion.identity);
        }
        _audioSource.clip = _laserSound;
        _audioSource.Play();
    }

    public void Damage()
    {
        if( _isShieldActive == true )
        {
            _isShieldActive = false;
            _shieldVisualiser.SetActive(false);
            return;
        }
        _lives--;
        if( _lives == 2 )
        {
            _engine[0].SetActive(true);
        }
        else if( _lives == 1 )
        {
            _engine[1].SetActive(true);
        }
        _uiManager.UpdateLives(_lives);
        if (_lives < 1)
        {
            _spawnManager.OnPlayerDeath();
            Destroy(this.gameObject);
            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
        }
    }

    public void TripleShotActive()
    {
        _isTripleShotActive = true;
        StartCoroutine(TripleShotPowerDownRoutine());
    }

    IEnumerator TripleShotPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _isTripleShotActive = false;
    }
    
    public void SpeedActive()
    {
        _speed = 10f;
        StartCoroutine(SpeedPowerDownRoutine());
    }

    IEnumerator SpeedPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _speed = 5.5f;
    }

    public void ShieldActive()
    {
        _isShieldActive = true;
        _shieldVisualiser.SetActive(true);
        StartCoroutine(ShieldPowerDownRoutine());
    }

    IEnumerator ShieldPowerDownRoutine()
    {
        yield return new WaitForSeconds(30f);
        _isShieldActive = false;
        _shieldVisualiser.SetActive(false);
    }

    public void Scoring(int points)
    {
        _score += points;
        _uiManager.Scoring(_score);
        if (_score >= 300)
        {
            _spawnManager.Leveling(2);
        }
        else if (_score >= 200)
        {
            _spawnManager.Leveling(1);
        }
        else if (_score >= 100)
        {
            _spawnManager.Leveling(0);
        }
    }
}
