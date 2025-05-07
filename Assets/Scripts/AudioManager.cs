using System;
using System.Collections.Generic;
using UnityEngine;
using static Unity.VisualScripting.Member;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [SerializeField] private int _sfxPool = 5;
    [SerializeField] private int _musicPool = 2;
    [SerializeField] private float _maxCamDistance = 100;

    [SerializeField] private List<SoundData> sounds = new List<SoundData>();

    private SoundSources[] _sfxSource;
    private SoundSources[] _musicSource;

    public void PlaySFXAt(string sfxName, int priority, Vector3 pos)
    {
        AudioClip clip = FindSound(sfxName);
        if (clip == null)
            return;

        float sqrDistance = (CameraSystem.Instance.transform.position - pos).sqrMagnitude;
        if (sqrDistance > _maxCamDistance * _maxCamDistance) // Drop sfx
            return;

        SoundSources sfx = FindBestSFXSource(sqrDistance, priority);
        if (sfx == null) // no availabe sfx source
            return;

        sfx.name = name;
        sfx.priority = priority;
        sfx.source.transform.position = pos;
        PlaySound(sfx.source, clip);
    }

    public void PlaySFX(string sfxName, int priority)
    {
        Vector3 pos = CameraSystem.Instance.transform.position;
        PlaySFXAt(sfxName, priority, pos);
    }

    public void PlayMusic(string musicName, int priority, bool solo)
    {
        AudioClip clip = FindSound(musicName);
        if (clip == null)
            return;

        if (solo)
        {
            foreach (SoundSources music in _musicSource)
            {
                music.source.Stop();
            }
            _musicSource[0].name = name;
            _musicSource[0].priority = priority;
            PlaySound(_musicSource[0].source, clip);
        }
        else
        {
            SoundSources music = FindBestMusicSource(priority);
            music.name = name;
            music.source.priority = priority;
            PlaySound(music.source, clip);
        }
    }

    public void SetAudioVolume(float vol)
    {
        foreach (SoundSources music in _musicSource)
        {
            music.source.volume = vol;
        }
    }

    public void SetSFXVolume(float vol)
    {
        foreach (SoundSources sfx in _sfxSource)
        {
            sfx.source.volume = vol;
        }
    }


    private void Awake()
    {
        Debug.Assert(Instance == null);
        Instance = this;

        Init();
    }

    private void Init()
    {
        _sfxSource = new SoundSources[_sfxPool];
        _musicSource = new SoundSources[_musicPool];

        for (int i = 0; i < _sfxPool; i++)
        {
            _sfxSource[i] = new SoundSources($"sfx_{i}", transform);
            _sfxSource[i].source.spatialBlend = 1;
            _sfxSource[i].source.minDistance = 1;
            _sfxSource[i].source.maxDistance = 100;
            _sfxSource[i].source.rolloffMode = AudioRolloffMode.Linear;
        }

        for (int i = 0; i < _musicPool; i++)
        {
            _musicSource[i] = new SoundSources($"music_{i}", transform);
            _musicSource[i].source.loop = true;
            _musicSource[i].source.volume = .7f;
        }
    }

    private void OnDestroy()
    {
        Instance = null;
    }

    private AudioClip FindSound(string name)
    {
        foreach (SoundData s in sounds)
        {
            if (s.name == name) return s.clip;
        }
        return null;
    }

    private SoundSources FindBestSFXSource(float sqrDistanceToCam, int priority)
    {
        int index = -1;
        float distance = sqrDistanceToCam;

        int priorityIndex = -1;
        int lowestPriority = priority;

        for (int i = 0; i < _sfxPool; i++)
        {
            if (!_sfxSource[i].source.isPlaying)
                return _sfxSource[i];

            float d = (_sfxSource[i].source.transform.position - CameraSystem.Instance.transform.position).sqrMagnitude;
            if (d > distance)
            {
                distance = d;
                index = i;
            }

            if (_sfxSource[i].priority < lowestPriority)
            {
                priorityIndex = i;
            }
        }

        if (index >= 0)
            return _sfxSource[index];
        else if (priorityIndex >= 0)
            return _sfxSource[priorityIndex];

        return null;
    }

    private SoundSources FindBestMusicSource(int priority)
    {
        int index = -1;
        int p = priority;

        for (int i = 0; i < _musicPool; i++)
        {
            if (!_musicSource[i].source.isPlaying)
                return _musicSource[i];

            if (_musicSource[i].priority < p)
            {
                p = _musicSource[i].priority;
                index = i;
            }
        }

        if (index >= 0)
            return _musicSource[index];

        return null;
    }

    private void PlaySound(AudioSource source, AudioClip clip)
    {
        source.Stop();
        source.clip = clip;
        source.Play();
    }




    [Serializable]
    public class SoundData
    {
        public string name;
        public AudioClip clip;
    }

    private class SoundSources
    {
        public string name;
        public int priority;
        public AudioSource source;

        public SoundSources(string name, Transform parent)
        {
            source = new GameObject(name, typeof(AudioSource)).GetComponent<AudioSource>();
            source.transform.parent = parent;
        }
    }
}
