using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using System;

public class CharacterMovement : MonoBehaviour
{
    // Needs a player input component on the player to work

    [Tooltip("Player Movement Speed")]
    [SerializeField]
    private float _speed = 2f;

    [Tooltip("Player Movement Speed Lerp Factor")]
    [SerializeField]
    private float _lerpSpeed = .75f;

    [SerializeField]
    private SpriteRenderer _spriteRenderer;

    [Header("Player Sprites")]

    [SerializeField]
    private DirectionInfo upDirectionInfo;

    [SerializeField]
    private DirectionInfo upRightDirectionInfo;

    [SerializeField]
    private DirectionInfo rightDirectionInfo;

    [SerializeField]
    private DirectionInfo downRightDirectionInfo;

    [SerializeField]
    private DirectionInfo downDirectionInfo;

    [SerializeField]
    private DirectionInfo downLeftDirectionInfo;

    [SerializeField]
    private DirectionInfo leftDirectionInfo;

    [SerializeField]
    private DirectionInfo upLeftDirectionInfo;

    [Header("Player Sounds")]

    [SerializeField]
    private float stepInterval = 0.3f;
    private float _stepTimer = 0f;

    private AudioManager audioManager;

    private Transform _lanternLightTransform;
    private Transform _lanternTransform;
    private Vector2 _movementInput;
    private Transform _transform;
    private Rigidbody2D _rb;

    public event EventHandler<EventArgs> InteractPressed;
    public void OnInteractPressed(EventArgs e)
    {
        if (InteractPressed != null)
        {
            InteractPressed(this, e);
        }
    }

    [Serializable]
    struct DirectionInfo
    {
        public float lookRotation;
        public Sprite rotSprite;
        public Vector2 relativePosition;

        public DirectionInfo(float rot, Sprite spr, Vector2 pos)
        {
            lookRotation = rot;
            rotSprite = spr;
            relativePosition = pos;
        }

    };

    Dictionary<Vector2, DirectionInfo> looks;


    private void Awake()
    {
        _transform = GetComponent<Transform>();
        _rb = GetComponent<Rigidbody2D>();
        _lanternTransform = transform.Find("Lantern");
        _lanternLightTransform = _lanternTransform.Find("LanternDirectionalLight");


        looks = new Dictionary<Vector2, DirectionInfo>()
        {
            // left
            { new Vector2(-1, 0), leftDirectionInfo },

            // right
            { new Vector2(1, 0), rightDirectionInfo },

            // up
            { new Vector2(0, 1), upDirectionInfo },

            // down
            { new Vector2(0, -1), downDirectionInfo },

            // up left
            { new Vector2(-.71f, .71f), upLeftDirectionInfo },

            // up right
            { new Vector2(.71f, .71f), upRightDirectionInfo },

            // down left
            { new Vector2(-.71f, -.71f), downLeftDirectionInfo },
            
            // down right
            { new Vector2(.71f, -.71f), downRightDirectionInfo },
        };
    }


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
                _lanternLightTransform.rotation = Quaternion.Euler(0, 0, look.Value.lookRotation);
                _lanternTransform.localPosition = look.Value.relativePosition;
                _spriteRenderer.sprite = look.Value.rotSprite;
            }

        }

    }

    private void OnMove(InputValue inputValue)
    {
        _movementInput = inputValue.Get<Vector2>();
    }

    private void OnInteract(InputValue inputValue)
    {
        if (inputValue.isPressed) OnInteractPressed(null);
    }

    // TODO: Implement footsteps
    // private void HandleFootstepSound()
    // {
    //     if (Time.timeScale > 0 && isGrounded && characterVelocity.magnitude > 0 && _stepTimer <= 0)
    //     {
    //         audioManager.PlayOneShot(AudioID.SFX.Player.Movement.footsteps, new string[] {"surfaceType"}, new string[] {"Sand"}, null);
    //         _stepTimer = stepInterval;
    //     }
    //     _stepTimer -= Time.deltaTime;
    // }
}

