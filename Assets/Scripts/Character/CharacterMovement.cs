using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class CharacterMovement : MonoBehaviour
{
    // Needs a player input component on the player to work

    [Tooltip("Player Movement Speed")]
    [SerializeField]
    private float _speed = 2f;

    [Tooltip("Player Movement Speed Lerp Factor")]
    [SerializeField]
    private float _lerpSpeed = .75f;

    [Tooltip("Gets rotated when the player moves, should be the directional light")]
    [SerializeField]
    private Transform _lanternTransform;

    private Vector2 _movementInput;
    private Transform _transform;


    private void Awake()
    {
        _transform = GetComponent<Transform>();
    }

    private Dictionary<Vector2, float> looks = new Dictionary<Vector2, float>()
        {
            // left
            { new Vector2(-1, 0), 90 },

            // right
            { new Vector2(1, 0), -90 },

            // up
            { new Vector2(0, 1), 0 },

            // down
            { new Vector2(0, -1), 180 },

            // up left
            { new Vector2(-.71f, .71f), 45 },

            // up right
            { new Vector2(.71f, .71f), -45 },

            // down left
            { new Vector2(-.71f, -.71f), 135 },
            
            // down right
            { new Vector2(.71f, -.71f), -135 },
        };
    private void Update()
    {
        Vector3 input3D = (Vector3)_movementInput;
        _transform.position = Vector3.Lerp(_transform.position, _transform.position + input3D * _speed, _lerpSpeed * Time.deltaTime);

        // left = -1, 0, rot should be z = 90
        // right = 1, 0, rot should be z = -90
        // up = 0, 1, rot should be 0
        // down = 0, -1, rot should be 180
        // diags are .71 for each

        float movX = Mathf.Round(_movementInput.x * 100.0f) / 100.0f;
        float movY = Mathf.Round(_movementInput.y * 100.0f) / 100.0f;
        foreach (var look in looks)
        {
            float lookX = look.Key.x;
            float lookY = look.Key.y;

            if (Mathf.Approximately(lookX, movX) && Mathf.Approximately(lookY, movY))
            {
                _lanternTransform.rotation = Quaternion.Euler(0, 0, look.Value);
            }


        }

    }

    private void OnMove(InputValue inputValue)
    {
        _movementInput = inputValue.Get<Vector2>();
    }
}

