using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    #region 싱글톤
    // 싱글톤 접근용 프로퍼티
    public static SoundManager instance
    {
        get
        {
            if (m_instance == null) // 싱글톤 변수에 오브젝트가 할당되지 않았다면
            {
                // 씬에서 게임매니저 오브젝트를 찾아서 할당
                m_instance = FindObjectOfType<SoundManager>();
            }

            return m_instance;
        }
    }

    private static SoundManager m_instance;
    #endregion

    public AudioClip[] audioClips;
    AudioSource audioSource;
    public Option option;
    
    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        option.LoadOptionData();
    }

    public void PlaySound(string name)
    {
        for (int i = 0; i < audioClips.Length; i++)
        {
            if (audioClips[i].name == name)
            {
                audioSource.PlayOneShot(audioClips[i]);
            }
        }
    }
}
