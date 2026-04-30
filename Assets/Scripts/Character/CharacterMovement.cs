using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using System;
using UnityEngine.SceneManagement;

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
    private float stepInterval = 5f;
    private float _stepTimer = 0f;

    private new AudioManager audio;

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
    
    // used when teleporting to a new scene to set the player facing the correct direction
    public void SetFacingDirection(Vector2 direction)
    {
        ApplyDirection(direction);
    }

    private void ApplyDirection(Vector2 direction)
    {
        if (looks == null || direction.sqrMagnitude < 0.01f) return;

        Vector2 normalized = direction.normalized;
        float bestDot = float.MinValue;
        Vector2 bestKey = Vector2.down;

        foreach (var key in looks.Keys)
        {
            float dot = Vector2.Dot(normalized, key.normalized);
            if (dot > bestDot)
            {
                bestDot = dot;
                bestKey = key;
            }
        }

        HandleFootstepSound(direction);
        
        var info = looks[bestKey];
        _lanternLightTransform.rotation = Quaternion.Euler(0, 0, info.lookRotation);
        _lanternTransform.localPosition = info.relativePosition;
        _spriteRenderer.sprite = info.rotSprite;
    }

    private void Awake()
    {
        _transform = GetComponent<Transform>();
        _rb = GetComponent<Rigidbody2D>();
        _rb.gravityScale = 0f;
        _lanternTransform = transform.Find("Lantern");
        _lanternLightTransform = _lanternTransform.Find("LanternDirectionalLight");
        audio = AudioManager.Instance;

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
        ApplyDirection(_movementInput);
    }

    private void FixedUpdate()
    {
        Vector2 targetPosition = _rb.position + _movementInput * (_speed * Time.fixedDeltaTime);
        _rb.MovePosition(targetPosition);
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
    private void HandleFootstepSound(Vector2 direction)
    {
        // Debug.Log(_stepTimer);
        
        if (Time.timeScale > 0 && direction.magnitude > 0 && _stepTimer < 0)
        {
            string roomType = AudioID.SceneToRoomMap[SceneManager.GetActiveScene().name];
            
            audio.PlayOneShot(AudioID.SFX.Player.Movement.footsteps, 
                new string[] {"roomType"}, 
                new string[] {roomType}, 
                GameObject.Find("Character"));
                
            _stepTimer = stepInterval;
        }
        // Debug.Log($"{_stepTimer} - {Time.deltaTime} = {_stepTimer - Time.deltaTime}");
        _stepTimer -= Time.deltaTime;
    }
}
