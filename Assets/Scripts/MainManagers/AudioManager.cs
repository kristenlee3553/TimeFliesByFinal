using UnityEngine;

/// <summary>
/// Manager that controls the audio of the game.
/// <br></br>
/// Author: Kristen Lee
/// </summary>
public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioClip[] clips;
    [SerializeField] private AudioClip[] bgMusic;

    [SerializeField] private AudioSource audioPlayer;
    [SerializeField] private AudioSource bgMusicPlayer;

    private int currentMusicIndex;

    public static AudioManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(transform.gameObject);
    }

    public void PlayHorseTrot()
    {
        audioPlayer.PlayOneShot(clips[6]);
    }

    public void PlayLevelCompleteSound()
    {
        audioPlayer.PlayOneShot(clips[5]);
    }

    public void PlayLeverSound()
    {
        audioPlayer.PlayOneShot(clips[4]);
    }

    public void PlayOrbSound()
    {
        audioPlayer.PlayOneShot(clips[0]);
    }

    public void PlayJumpSound()
    {
        audioPlayer.PlayOneShot(clips[3]);
    }
    
    public void PlayChangeTimeSound()
    {
        audioPlayer.PlayOneShot(clips[1]);
    }

    public void PlayGameOverSound()
    {
        audioPlayer.PlayOneShot(clips[2]);
    }

    public void PlayBgMusic(int index)
    {
        currentMusicIndex = index;
        bgMusicPlayer.Stop();
        bgMusicPlayer.clip = bgMusic[index];
        bgMusicPlayer.Play();
    }

    public int GetCurrentMusicIndex()
    {
        return currentMusicIndex;
    }

    /// <summary>
    /// Plays currently stored background music
    /// </summary>
    public void PlayBgMusic()
    {
        bgMusicPlayer.Play();
    }

    public void StopBgMusic()
    {
        bgMusicPlayer.Stop();
    }
}
