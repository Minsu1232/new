using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Arrow", menuName = "Arrow")]
public class ArrowDamageScriptable : ScriptableObject
{
    public int initialDamage;
    public int damagePerSecond;
    public float duration;
    public string effectType;
    public ParticleSystem particleEffect;
    public ParticleSystem particleEffectSecond;
    public int stamina;
    public int neutralizeValue;
    public int destructionValue;
    public Sprite arrowStateIcon;
    public AudioClip arrowSound;
    public AudioClip hitSound;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
