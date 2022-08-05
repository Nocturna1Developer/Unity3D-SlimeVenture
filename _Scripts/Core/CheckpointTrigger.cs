using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CheckpointTrigger: MonoBehaviour
{
    private GameMaster _gameMaster;

    private void Start()
    {
        _gameMaster = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<GameMaster>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            _gameMaster.lastCheckpointPosition = transform.position;
            _gameMaster.lastCheckpointPositionOfCamera = transform.position;
        }
    }
}
