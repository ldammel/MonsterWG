using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSpawner : MonoBehaviour
{
    public GameObject[] prefabs;

    private PlayerInputManager _inputManager;

    private void Start()
    {
        _inputManager = GetComponent<PlayerInputManager>();

        for (int i = 0; i < Gamepad.all.Count; i++)
        {
            Gamepad gamepad = Gamepad.all[i];
            Debug.LogFormat("Found Gamepad: {0}", gamepad.ToString());
            JoinPlayer(_inputManager.playerCount, gamepad);
        }

        bool keyboardAdded = false;

        if (_inputManager.playerCount == 0)
        {
            JoinPlayer(0, Keyboard.current);
            keyboardAdded = true;
        }
        if(_inputManager.playerCount == 1)
        {
            if (keyboardAdded)
            {
                Instantiate(prefabs[1]);
            }
            else
            {
                PlayerInput player = JoinPlayer(1, Keyboard.current);
                player.SwitchCurrentActionMap("Player");
            }
        }
    }

    private PlayerInput JoinPlayer(int index, InputDevice device)
    {
        if (_inputManager.playerCount >= _inputManager.maxPlayerCount)
            return null;

        _inputManager.playerPrefab = prefabs[_inputManager.playerCount];

        PlayerInput input = _inputManager.JoinPlayer(index, -1, null, device);
        if (device != null)
        {
            Debug.LogFormat("Spawned player with ID {0} for device {1}", index, device.ToString());
        }
        else
        {
            Debug.LogFormat("Spawned player with ID {0}", index);
        }

        FindObjectOfType<Cinemachine.CinemachineTargetGroup>().AddMember(input.transform, 1, 4);

        return input;
    }
}
