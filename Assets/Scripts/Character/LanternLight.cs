using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;

[Serializable]
public class FlickerLayer
{
    [Tooltip("Speed of this noise layer's oscillation.")]
    [SerializeField, Range(0.01f, 10f)] public float speed = 3f;

    [Tooltip("How much this layer contributes to the overall flicker (0 = none, 1 = full).")]
    [SerializeField, Range(0f, 1f)] public float strength = 1f;

    [Tooltip("Perlin noise Y-axis offset - change to get a different noise pattern.")]
    [SerializeField] public float noiseOffset = 0f;
}

[Serializable]
public class LanternFlickerSettings
{
    [Tooltip("Enable or disable the flicker effect.")]
    [SerializeField] public bool enabled = true;

    [Tooltip("Overall flicker intensity. Scales the combined noise result. 0 = no flicker, 1 = full flicker.")]
    [SerializeField, Range(0f, 1f)] public float flickerIntensity = 0.15f;

    [Tooltip("Individual noise layers that are blended together to produce the flicker.")]
    [SerializeField] public List<FlickerLayer> flickerLayers = new List<FlickerLayer>
    {
        new FlickerLayer { speed = 3f, strength = 1f, noiseOffset = 0f },
        new FlickerLayer { speed = 0.7f, strength = 0.4f, noiseOffset = 10f },
    };

    [Tooltip("How much the light radius flickers (0 = radius stays constant).")]
    [SerializeField, Range(0f, 1f)] public float radiusFlickerStrength = 0.05f;

    [Header("Color Tint")]
    [Tooltip("How much the light color temperature shifts during flicker (0 = no color shift).")]
    [SerializeField, Range(0f, 1f)] public float colorFlickerStrength = 0.08f;

    [Tooltip("The warmer tint applied at the bottom of a flicker dip.")]
    [SerializeField] public Color warmTint = new Color(1f, 0f, 0f);

    [Header("Sudden Dim Settings")]
    [Tooltip("Chance per second of a sudden brief dim.")]
    [SerializeField, Range(0f, 5f)] public float suddenDimChance = 0.3f;

    [Tooltip("How deep a sudden dim can be (multiplied against intensity, lower = deeper dim).")]
    [SerializeField, Range(0f, 1f)] public float suddenDimDepth = 0.4f;

    [Tooltip("Duration range (seconds) for a sudden dim event.")]
    [SerializeField, MinMaxRange(0.01f, 0.5f)] public RangedFloat suddenDimDuration = new RangedFloat { minValue = 0.04f, maxValue = 0.15f };
}

[Serializable]
public class LanternLightSettings
{
    [Header("Lantern Settings")]
    
    [Tooltip("The maximum distance the directional lantern light reaches.")]
    [SerializeField] public float directionalLightRange = 2f; 
    
    [Tooltip("The radius of the spherical light surrounding the lantern.")]
    [SerializeField] public float ambientLightRadius = 1f;
    
    [Tooltip("The brightness of the lantern light.")]
    [SerializeField] public float lightIntensity = .5f; 
    
    [Tooltip("The color of the lantern light.")]
    [SerializeField] public Color lightColor = new Color(1f, .84f, .09f);
    
    [Tooltip("The inner and outer angles of the directional light.")]
    [SerializeField, MinMaxRange(0f,360f)] public RangedFloat directionalLightAngle = new RangedFloat { minValue = 15f, maxValue = 100f }; 
}

public struct LanternLightSnapshot
{
    public float DirectionalIntensity;
    public float PointIntensity;
    public float DirectionalOuterRadius;
    public float DirectionalInnerRadius;
    public float PointOuterRadius;
    public float PointInnerRadius;
    public float DirectionalOuterAngle;
    public float DirectionalInnerAngle;
    public Color Color;

    public static LanternLightSnapshot FromLights(Light2D directional, Light2D point)
    {
        return new LanternLightSnapshot
        {
            DirectionalIntensity = directional.intensity,
            PointIntensity = point.intensity,
            DirectionalOuterRadius = directional.pointLightOuterRadius,
            DirectionalInnerRadius = directional.pointLightInnerRadius,
            PointOuterRadius = point.pointLightOuterRadius,
            PointInnerRadius = point.pointLightInnerRadius,
            DirectionalOuterAngle = directional.pointLightOuterAngle,
            DirectionalInnerAngle = directional.pointLightInnerAngle,
            Color = directional.color,
        };
    }

    public static LanternLightSnapshot FromSettings(LanternLightSettings settings, DirectionalLightConfig dir, PointLightConfig pt)
    {
        return new LanternLightSnapshot
        {
            DirectionalIntensity = settings.lightIntensity * dir.intensityRatio,
            PointIntensity = settings.lightIntensity * pt.intensityRatio,
            DirectionalOuterRadius = settings.directionalLightRange * dir.outerRadiusRatio,
            DirectionalInnerRadius = settings.directionalLightRange * dir.innerRadiusRatio,
            PointOuterRadius = settings.ambientLightRadius * pt.outerRadiusRatio,
            PointInnerRadius = settings.ambientLightRadius * pt.innerRadiusRatio,
            DirectionalOuterAngle = settings.directionalLightAngle.maxValue,
            DirectionalInnerAngle = settings.directionalLightAngle.minValue,
            Color = settings.lightColor,
        };
    }

    public static LanternLightSnapshot Lerp(LanternLightSnapshot a, LanternLightSnapshot b, float t)
    {
        return new LanternLightSnapshot
        {
            DirectionalIntensity = Mathf.Lerp(a.DirectionalIntensity, b.DirectionalIntensity, t),
            PointIntensity = Mathf.Lerp(a.PointIntensity, b.PointIntensity, t),
            DirectionalOuterRadius = Mathf.Lerp(a.DirectionalOuterRadius, b.DirectionalOuterRadius, t),
            DirectionalInnerRadius = Mathf.Lerp(a.DirectionalInnerRadius, b.DirectionalInnerRadius, t),
            PointOuterRadius = Mathf.Lerp(a.PointOuterRadius, b.PointOuterRadius, t),
            PointInnerRadius = Mathf.Lerp(a.PointInnerRadius, b.PointInnerRadius, t),
            DirectionalOuterAngle = Mathf.Lerp(a.DirectionalOuterAngle, b.DirectionalOuterAngle, t),
            DirectionalInnerAngle = Mathf.Lerp(a.DirectionalInnerAngle, b.DirectionalInnerAngle, t),
            Color = Color.Lerp(a.Color, b.Color, t),
        };
    }

    public void ApplyTo(Light2D directional, Light2D point)
    {
        directional.intensity = DirectionalIntensity;
        point.intensity = PointIntensity;
        directional.pointLightOuterRadius = DirectionalOuterRadius;
        directional.pointLightInnerRadius = DirectionalInnerRadius;
        point.pointLightOuterRadius = PointOuterRadius;
        point.pointLightInnerRadius = PointInnerRadius;
        directional.pointLightOuterAngle = DirectionalOuterAngle;
        directional.pointLightInnerAngle = DirectionalInnerAngle;
        directional.color = Color;
        point.color = Color;
    }
}

[Serializable]
public class DirectionalLightConfig
{
    [Tooltip("The rate at which the directional light intensity decreases over distance.")]
    [SerializeField] public float falloff = .5f;
    [Tooltip("The ratio of the outer radius of the directional light to the lantern range.")]
    [SerializeField] public float outerRadiusRatio = 1f;
    [Tooltip("The ratio of the inner radius of the directional light to the lantern range.")]
    [SerializeField] public float innerRadiusRatio = .5f;
    [Tooltip("The ratio of the directional light intensity to the lantern intensity.")]
    [SerializeField] public float intensityRatio = 1f;
}

[Serializable]
public class PointLightConfig
{
    [Tooltip("The rate at which the point light intensity decreases over distance.")]
    [SerializeField] public float falloff = .75f;
    [Tooltip("The ratio of the outer radius of the point light to the lantern area.")]
    [SerializeField] public float outerRadiusRatio = 1f;
    [Tooltip("The ratio of the inner radius of the point light to the lantern area.")]
    [SerializeField] public float innerRadiusRatio = 0f;
    [Tooltip("The ratio of the point light intensity to the lantern intensity.")]
    [SerializeField] public float intensityRatio = 1f;
}

struct SuddenDimState
{
    public bool Active;
    public float Timer;
    public float Duration;
    public float Depth;

    public void Reset()
    {
        Active = false;
        Timer = 0f;
    }
}


public class LanternLight : MonoBehaviour
{
    [SerializeField] int initialLanternSetting = 0;
    
    [Tooltip("Duration in seconds for lerping between lantern settings at runtime.")]
    [SerializeField] private float transitionDuration = 0.5f;
    
    [SerializeField] private List<LanternLightSettings> lanternSettings = new List<LanternLightSettings>() { new LanternLightSettings() };

    [Header("Flicker Settings")]
    [Tooltip("Settings controlling the realistic lantern flicker effect.")]
    [SerializeField] private LanternFlickerSettings flickerSettings = new LanternFlickerSettings();

    [Header("Directional Light Settings")]
    [SerializeField] private DirectionalLightConfig directionalConfig = new DirectionalLightConfig();

    [Header("Point Light Settings")]
    [SerializeField] private PointLightConfig pointConfig = new PointLightConfig();

    private Light2D _directionalLight;
    private Light2D _pointLight;
    private int _currentLanternSettingIndex;

    public int CurrentLanternSettingIndex => _currentLanternSettingIndex;
    public int LanternSettingsCount => lanternSettings.Count;

    private float _flickerTimeOffset;
    private SuddenDimState _suddenDim;
    private LanternLightSnapshot _baseSnapshot;
    private Coroutine _transitionCoroutine;

    // ReSharper disable Unity.PerformanceAnalysis
    private void FindLights()
    {
        if (_directionalLight == null)
        {
            
            Transform directionLightTransform = transform.Find("LanternDirectionalLight");
            if (directionLightTransform != null)
            {
                _directionalLight = directionLightTransform.GetComponent<Light2D>();
            }
            else
            {
                Debug.LogWarning("No directional light transform found.");
            }
        }
        if (_pointLight == null)
        {
            Transform pointLightTransform = transform.Find("LanternPointLight");
            if (pointLightTransform != null)
            {
                _pointLight = pointLightTransform.GetComponent<Light2D>();
            }
            else
            {
                Debug.LogWarning("No point light transform found.");
            }
        }
    }

    private void Reset()
    {
        if (transform.Find("LanternDirectionalLight") == null)
        {
            GameObject directionalLightObject = new GameObject("LanternDirectionalLight");
            directionalLightObject.transform.SetParent(transform, false);
            _directionalLight = directionalLightObject.AddComponent<Light2D>();
            _directionalLight.lightType = Light2D.LightType.Point;
        }

        if (transform.Find("LanternPointLight") == null)
        {
            GameObject pointLightObject = new GameObject("LanternPointLight");
            pointLightObject.transform.SetParent(transform, false);
            _pointLight = pointLightObject.AddComponent<Light2D>();
            _pointLight.lightType = Light2D.LightType.Point;
        }
        SetLightingSettingsByIndex(0);
    }

    private void OnValidate()
    {
        FindLights();
        if (_directionalLight == null || _pointLight == null) return;
        SetLightingSettingsByIndex(initialLanternSetting);
    }

    private void OnEnable()
    {
        FindLights();
        _flickerTimeOffset = UnityEngine.Random.Range(0f, 1000f);
        _suddenDim.Reset();
        SetLightingSettingsByIndex(initialLanternSetting);
    }

    private void Update()
    {
        if (!_directionalLight || !_pointLight)
        {
            return;
        }
        if (flickerSettings != null && flickerSettings.enabled)
        {
            ApplyFlicker();
        }
    }
    
    public void SetLightingSettingsByIndex(int index)
    {
        if (lanternSettings.Count == 0)
        {
            DebugEditor.LogWarning($"[LanternLight] No lantern settings found on '{gameObject.name}'.");
            return;
        }

        _currentLanternSettingIndex = Mathf.Clamp(index, 0, lanternSettings.Count - 1);
        LanternLightSettings settings = lanternSettings[_currentLanternSettingIndex];

        _directionalLight.color = settings.lightColor;
        _pointLight.color = settings.lightColor;

        _directionalLight.intensity = settings.lightIntensity * directionalConfig.intensityRatio;
        _pointLight.intensity = settings.lightIntensity * pointConfig.intensityRatio;

        _directionalLight.falloffIntensity = directionalConfig.falloff;
        _pointLight.falloffIntensity = pointConfig.falloff;

        _directionalLight.pointLightOuterAngle = settings.directionalLightAngle.maxValue;
        _directionalLight.pointLightInnerAngle = settings.directionalLightAngle.minValue;

        _directionalLight.pointLightOuterRadius = settings.directionalLightRange * directionalConfig.outerRadiusRatio;
        _directionalLight.pointLightInnerRadius = settings.directionalLightRange * directionalConfig.innerRadiusRatio;
        _pointLight.pointLightInnerRadius = settings.ambientLightRadius * pointConfig.innerRadiusRatio;
        _pointLight.pointLightOuterRadius = settings.ambientLightRadius * pointConfig.outerRadiusRatio;

        _baseSnapshot = LanternLightSnapshot.FromLights(_directionalLight, _pointLight);
    }
    
    public void TransitionToSetting(int index)
    {
        if (lanternSettings.Count == 0) return;
        index = Mathf.Clamp(index, 0, lanternSettings.Count - 1);

        if (_transitionCoroutine != null)
            StopCoroutine(_transitionCoroutine);

        _transitionCoroutine = StartCoroutine(TransitionCoroutine(index));
    }

    private IEnumerator TransitionCoroutine(int targetIndex)
    {
        var from = LanternLightSnapshot.FromLights(_directionalLight, _pointLight);
        var to = LanternLightSnapshot.FromSettings(lanternSettings[targetIndex], directionalConfig, pointConfig);

        float elapsed = 0f;
        float duration = Mathf.Max(transitionDuration, 0.001f);

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.SmoothStep(0f, 1f, Mathf.Clamp01(elapsed / duration));

            var lerped = LanternLightSnapshot.Lerp(from, to, t);
            lerped.ApplyTo(_directionalLight, _pointLight);
            _baseSnapshot = lerped;

            yield return null;
        }

        SetLightingSettingsByIndex(targetIndex);
        _transitionCoroutine = null;
    }

    private void ApplyFlicker()
    {
        // Blend all flicker layers via weighted average 
        float totalWeight = 0f;
        float weightedSum = 0f;

        if (flickerSettings.flickerLayers != null && flickerSettings.flickerLayers.Count > 0)
        {
            foreach (var layer in flickerSettings.flickerLayers)
            {
                if (layer.strength <= 0f) continue;
                float t = Time.time + _flickerTimeOffset;
                float noise = Mathf.PerlinNoise(t * layer.speed, layer.noiseOffset);
                weightedSum += noise * layer.strength;
                totalWeight += layer.strength;
            }
        }

        // Normalize to 0-1 range
        float combined = totalWeight > 0f ? weightedSum / totalWeight : 0.5f;

        // Map from 0-1 Perlin range to a flicker multiplier centered around 1
        float flickerMultiplier = 1f - (0.5f - combined) * 2f * flickerSettings.flickerIntensity;

        // Sudden dim
        HandleSuddenDim( ref flickerMultiplier);

        flickerMultiplier = Mathf.Clamp(flickerMultiplier, 0.05f, 1.5f);

        // Apply intensity 
        _directionalLight.intensity = _baseSnapshot.DirectionalIntensity * flickerMultiplier;
        _pointLight.intensity = _baseSnapshot.PointIntensity * flickerMultiplier;

        // Apply radius flicker 
        if (flickerSettings.radiusFlickerStrength > 0f)
        {
            float radiusMult = 1f - (1f - flickerMultiplier) * flickerSettings.radiusFlickerStrength;
            _directionalLight.pointLightOuterRadius = _baseSnapshot.DirectionalOuterRadius * radiusMult;
            _directionalLight.pointLightInnerRadius = _baseSnapshot.DirectionalInnerRadius * radiusMult;
            _pointLight.pointLightOuterRadius = _baseSnapshot.PointOuterRadius * radiusMult;
            _pointLight.pointLightInnerRadius = _baseSnapshot.PointInnerRadius * radiusMult;
        }

        // Apply color shift 
        if (flickerSettings.colorFlickerStrength > 0f)
        {
            float colorLerp = (1f - flickerMultiplier) * flickerSettings.colorFlickerStrength;
            Color flickerColor = Color.Lerp(_baseSnapshot.Color, flickerSettings.warmTint, Mathf.Clamp01(colorLerp));
            _directionalLight.color = flickerColor;
            _pointLight.color = flickerColor;
        }
    }

    private void HandleSuddenDim(ref float flickerMultiplier)
    {
        if (_suddenDim.Active)
        {
            _suddenDim.Timer -= Time.deltaTime;
            if (_suddenDim.Timer <= 0f)
            {
                _suddenDim.Active = false;
            }
            else
            {
                float progress = 1f - (_suddenDim.Timer / _suddenDim.Duration);
                float envelope = Mathf.Sin(progress * Mathf.PI);
                flickerMultiplier *= Mathf.Lerp(1f, _suddenDim.Depth, envelope);
            }
        }
        else if (flickerSettings.suddenDimChance > 0f)
        {
            // Roll for a sudden dim this frame
            if (UnityEngine.Random.value < flickerSettings.suddenDimChance * Time.deltaTime)
            {
                _suddenDim.Active = true;
                _suddenDim.Duration = UnityEngine.Random.Range(
                    flickerSettings.suddenDimDuration.minValue,
                    flickerSettings.suddenDimDuration.maxValue);
                _suddenDim.Timer = _suddenDim.Duration;
                _suddenDim.Depth = flickerSettings.suddenDimDepth;
            }
        }
    }
}
