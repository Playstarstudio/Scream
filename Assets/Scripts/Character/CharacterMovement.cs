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

    [SerializeField]
    private SpriteRenderer _spriteRenderer;

    [Header("Player Sprites")]

    [SerializeField]
    private Sprite upSprite;

    [SerializeField]
    private Sprite upRightSprite;

    [SerializeField]
    private Sprite rightSprite;

    [SerializeField]
    private Sprite downRightSprite;

    [SerializeField]
    private Sprite downSprite;

    [SerializeField]
    private Sprite downLeftSprite;

    [SerializeField]
    private Sprite leftSprite;

    [SerializeField]
    private Sprite upLeftSprite;



    private Vector2 _movementInput;
    private Transform _transform;


    Dictionary<Vector2, DirectionInfo> looks;


    private void Awake()
    {
        _transform = GetComponent<Transform>();


        looks = new Dictionary<Vector2, DirectionInfo>()
        {
            // left
            { new Vector2(-1, 0), new DirectionInfo(90, leftSprite) },

            // right
            { new Vector2(1, 0), new DirectionInfo(-90, rightSprite) },

            // up
            { new Vector2(0, 1), new DirectionInfo(0, upSprite) },

            // down
            { new Vector2(0, -1), new DirectionInfo(180, downSprite) },

            // up left
            { new Vector2(-.71f, .71f), new DirectionInfo(45, upLeftSprite) },

            // up right
            { new Vector2(.71f, .71f), new DirectionInfo(-45, upRightSprite) },

            // down left
            { new Vector2(-.71f, -.71f), new DirectionInfo(135, downLeftSprite) },
            
            // down right
            { new Vector2(.71f, -.71f), new DirectionInfo(-135, downRightSprite) },
        };
    }

    struct DirectionInfo
    {
        public float lookRotation;
        public Sprite rotSprite;

        public DirectionInfo(float rot, Sprite spr)
        {
            lookRotation = rot;
            rotSprite = spr;
        }
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
                _lanternTransform.rotation = Quaternion.Euler(0, 0, look.Value.lookRotation);
                _spriteRenderer.sprite = look.Value.rotSprite;
            }

        }

    }

    private void OnMove(InputValue inputValue)
    {
        _movementInput = inputValue.Get<Vector2>();
    }
}

