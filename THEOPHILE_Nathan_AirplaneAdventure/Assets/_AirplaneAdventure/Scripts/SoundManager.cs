using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private Airplane _Airplane;
    [SerializeField] private AudioClip _CollectibleExplosion, _Engine, _EngineBoost, _Music, _ShellExplosion, _ShotFiring, _TankExplosion;
    private List<AudioSource> _AudioSources = new List<AudioSource>();

    void Start()
    {
        PlaySound(_Music, true);
        PlaySound(_Engine, true);
        _Airplane.OnPlaneCrashed += GameOver;
        _Airplane.OnCollectibleHit += CollectibleHit;
    }

    private void PlaySound(AudioClip audioClip, bool loop = false)
    {
        AudioSource lAudioSource = gameObject.AddComponent<AudioSource>();
        _AudioSources.Add(lAudioSource);
        lAudioSource.loop = loop;
        lAudioSource.clip = audioClip;
        lAudioSource.Play();
    }

    private void GameOver() {
        _AudioSources[0].Stop();
        _AudioSources[1].Stop();
        PlaySound(_TankExplosion); }

    private void CollectibleHit() => PlaySound(_CollectibleExplosion);
}