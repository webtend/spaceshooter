using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3.5f;
    private float _speedMultiplier = 2;
    [SerializeField]
    private GameObject _lazerPrefab;
    [SerializeField]
    private GameObject _tripleshotPrefab;
    [SerializeField]
    private GameObject _ShieldVisualizer;
    [SerializeField]
    private float _fireRate = 0.15f;
    private float _canFire = -1f;
    [SerializeField]
    private int _lives = 3;
    private SpawnManager _spawnManager;
    [SerializeField]
    private GameObject _rightenginePrefab, _leftenginePrefab;

    private bool _isTripleShotActive = false;
    private bool _isSpeedBoostActive = false;
    private bool _isShieldActive = false;

    [SerializeField]
        private int _Score;
    private UIManager _uiManager;
    [SerializeField]
    private AudioClip _laserAudioClip;
    private AudioSource _audioSource;
    //variable for the clip
    // Start is called before the first frame update
    void Start()
    {
        //처음 시작포인트를 중간으로
        transform.position = new Vector3(0, 0, 0);
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();

        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _audioSource = GetComponent<AudioSource>();

        if (_spawnManager == null)
        {
            Debug.LogError("The Spawn Manager is Null.");
        }

        if (_uiManager == null)
        {
            Debug.LogError("The UI Manager is Null.");
        }

        if (_audioSource == null)
        {
            Debug.LogError("AudioSource on the player is Null.");
        }
        else
        {
            _audioSource.clip = _laserAudioClip;
        }
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();
        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
        {
            FireLazer();
        }
    }

    void CalculateMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // 좌우 움직임
        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);

            transform.Translate(direction * _speed * Time.deltaTime);


        //클램핑 위아래 리밋
        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -3.8f, 0), 0);

        if (transform.position.x >= 11)
        {
            transform.position = new Vector3(-11, transform.position.y, 0);
        }
        else if (transform.position.x <= -11)
        {
            transform.position = new Vector3(11, transform.position.y, 0);
        }
    }

    void FireLazer()
    {
        //스페이스바
            _canFire = Time.time + _fireRate;
            var offset = new Vector3(0, 1.05f, 0);
            

            if (_isTripleShotActive == true)
        {
            Instantiate(_tripleshotPrefab, transform.position, Quaternion.identity);
        }
        else
        {
            Instantiate(_lazerPrefab, transform.position + offset, Quaternion.identity);
        }

        _audioSource.Play();
    }
    public void Damage()
    {

        if (_isShieldActive == true)
        {
            _isShieldActive = false;
            _ShieldVisualizer.SetActive(false);
            return;
        }
            
        _lives -= 1;

        if(_lives == 2)
        {
            _leftenginePrefab.SetActive(true);
        }
        else if(_lives == 1)
        {
            _rightenginePrefab.SetActive(true);
        }


        _uiManager.UpdateLives(_lives);

        if(_lives < 1)
        {
            _spawnManager.OnPlayerDeath();
            Destroy(gameObject);
        }
    }
    public void TripleShotActive()
    {
        _isTripleShotActive = true;
        StartCoroutine(TripleShotPowerDownRoutine());
    }
    IEnumerator TripleShotPowerDownRoutine()
    {
        while (_isTripleShotActive == true)
        {

            yield return new WaitForSeconds(5.0f);
            _isTripleShotActive = false;
        }

    }

    public void SpeedBoostActive()
    {
        _isSpeedBoostActive = true;
        _speed *= _speedMultiplier;
        StartCoroutine(SpeedBoostPowerDownRoutine());
    }
    IEnumerator SpeedBoostPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _isSpeedBoostActive = false;
        _speed /= _speedMultiplier;
    }

    public void ShieldActive()
    {
        _isShieldActive = true;
        _ShieldVisualizer.gameObject.SetActive(true);
    }

    public void AddScore(int points)
    {
        _Score += points;
        _uiManager.UpdateScore(_Score);
    }

}
