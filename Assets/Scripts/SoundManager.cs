using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class SoundManager
{
    static List<AudioSource> m_playingClips = new List<AudioSource>();

    public static void Initialize()
    {
    }

    public static void SetSfxEnabled(bool enabled)
    {
    }

    public static void SetBgmEnabled(bool enabled)
    {
    }

    public static AudioSource PlaySfx(string soundName)
    {
        return PlayClip(soundName, false, "sfx");
    }

    public static AudioSource PlaySfx(string soundName, bool loop)
    {
        return PlayClip(soundName, loop, "sfx");
    }

    public static AudioSource PlayBgm(string soundName)
    {
        return PlayClip(soundName, true, "bgm");
    }

    public static AudioSource PlayBgm(string soundName, bool loop)
    {
        return PlayClip(soundName, loop, "bgm");
    }

    public static AudioSource PlayClip(string soundName)
    {
        return PlayClip(soundName, false, "");
    }

    public static AudioSource PlayClip(string soundName, bool loop)
    {
        return PlayClip(soundName, loop, "");
    }

    public static AudioSource PlayClip(string soundName, bool loop, string soundType)
    {
        GameObject soundGO = App.Create(soundName);
        if (soundGO != null)
        {
            AudioSource asComp = soundGO.GetComponent<AudioSource>();
            if (asComp != null)
            {
                m_playingClips.Add(asComp);
                soundGO.transform.localPosition = Vector3.zero;
                // asComp.volume = 1.0f;
                asComp.dopplerLevel = 0.0f;
                asComp.spread = 0.0f;
                asComp.loop = loop;
                asComp.Play();
                return asComp;
            }
        }

        return null;
    }

    public static void StopClip(AudioSource asComp)
    {
        if (asComp != null)
        {
            asComp.Stop();
            m_playingClips.Remove(asComp);
            GameObject.Destroy(asComp.gameObject);
        }
    }

    public static void Update()
    {
        List<AudioSource> clipsToBeCleaned = new List<AudioSource>();

        for (int i = 0; i < m_playingClips.Count; ++i)
        {
            AudioSource asComp = m_playingClips[i];

            if (asComp == null || !asComp.isPlaying)
            {
                clipsToBeCleaned.Add(asComp);
            }
        }

        for (int j = 0; j < clipsToBeCleaned.Count; ++j)
        {
            AudioSource asComp = clipsToBeCleaned[j];

            if (asComp != null)
            {
                m_playingClips.Remove(asComp);
                GameObject.Destroy(asComp.gameObject);
                clipsToBeCleaned.Remove(asComp);
            }
        }
    }
}