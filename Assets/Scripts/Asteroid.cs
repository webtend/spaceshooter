﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField]
    private float _rotatespeed = 20.0f;
    [SerializeField]
    private GameObject _explosionPrefab;
    private SpawnManager _spawnManager;

    private void Start()
    {
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
    }
 
    // Start is called before the first frame update


    // Update is called once per frame
    void Update()
    {
        
        transform.Rotate(Vector3.forward * _rotatespeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.gameObject.tag == "Lazer")
        {
            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            
            Destroy(other.gameObject);
           _spawnManager.startSpawning();
            Destroy(this.gameObject, 0.25f);
        }

    }

}
