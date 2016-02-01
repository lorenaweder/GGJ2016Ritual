using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SoundManager : MonoBehaviour {

	public bool gameAudio = false;

	public AudioSource effectsSource;
	public AudioSource musicSource;

	protected float defaultEffectsVolume = 1f;
	protected float defaultMusicVolume = 1f;

	public AudioClip[] gameSfx;
	public AudioClip[] bkgMusic;


	public static SoundManager instance = null;


	void Awake()
	{
		if(instance != null)
		{
			if(this.gameAudio)
			{
				Game.soundManager = instance;
				instance.PlayLoopingTheme(1);	
			}
			else
				instance.PlayLoopingTheme(0);	
			Destroy(this.gameObject);
		}
		else
		{
			instance = this;
			DontDestroyOnLoad(this);

			if(this.gameAudio)
			{
				Game.soundManager = instance;
				PlayLoopingTheme(1);
			}
			else
				PlayLoopingTheme(0);
		}
	}

	public void PlaySoundEffect(int index, float volume = 1f)
	{
		if(index >= gameSfx.Length)
			return;
		effectsSource.PlayOneShot(gameSfx[index], 1);
	}
	
	public void PlayLoopingTheme(int index)
	{
		musicSource.volume = .2f;
		musicSource.clip = bkgMusic[index];
		musicSource.loop = true;
		musicSource.Play();
	}

	public void PlayElementSound(Elements element) 
	{
		switch (element) 
		{
			case Elements.FIRE:
				PlaySoundEffect (0);
				break;
			case Elements.EARTH:
				PlaySoundEffect (1);
				break;
			case Elements.WATER:
				PlaySoundEffect (2);
				break;
			case Elements.AIR:
				PlaySoundEffect (3);
				break;
		}
	}

	public void PlayPlayerHit() 
	{
		PlaySoundEffect ((int)Random.Range (4, 6));
	}


	public virtual void Reset()
	{
		ResetSource(musicSource);					
		ResetSource(effectsSource);					
	}

	protected void ResetSource(AudioSource src)
	{
		src.loop = false;
		src.Stop();
		src.clip = null;
	}

	void OnDestroy()
	{
		Reset();
	}

}