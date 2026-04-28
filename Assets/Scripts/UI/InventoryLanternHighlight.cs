using UnityEngine;

public class InventoryLanternHighlight : MonoBehaviour
{
    [SerializeField] private RectTransform lantern;
    [SerializeField] private ParticleSystem[] inventoryParticles;
    private RectTransform[] _particleTransforms;

    [SerializeField] private float lanternDistance;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _particleTransforms = new RectTransform[inventoryParticles.Length];
        for (int i = 0; i < inventoryParticles.Length; i++)
        {
            _particleTransforms[i] = inventoryParticles[i].GetComponent<RectTransform>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < _particleTransforms.Length; i++)
        {
            var distanceToLantern = Vector2.Distance(_particleTransforms[i].position, lantern.position);
            //Debug.Log("DistanceToLantern: " + distanceToLantern);
            if (distanceToLantern <= lanternDistance)
            {
                inventoryParticles[i].Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            }
            else if (!inventoryParticles[i].isPlaying)
            {
                inventoryParticles[i].Play();
            }
        }
    }
}
