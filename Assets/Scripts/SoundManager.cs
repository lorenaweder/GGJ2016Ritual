using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class LoopingMusicTheme {
	public AudioClip introClip;
	public AudioClip centerClip;
	public AudioClip finalClip;
}

public class SoundManager : MonoBehaviour {

	public float minPitch;
	public float maxPitch;

	public AudioSource effectsSource;
	public AudioSource musicSource;

	public Dictionary<string, AudioSource> loopEffectSources = new Dictionary<string, AudioSource>();
	protected AudioSource loopEffectSource;

	protected float defaultEffectsVolume = 1f;
	protected float defaultMusicVolume = 1f;

	public AudioClip[] gameSfx;
	public AudioClip[] bkgMusic;
	public AudioClip buttonEffect;

	private bool musicEnabled;
	private bool effectsEnabled;


	public static SoundManager instance = null;
	void Awake()
	{
		if(instance != null)
		{
			instance.PlayCenterMusic(1);
			Destroy(instance.gameObject);
		}
		instance = this;
		PlayCenterMusic(0);
		DontDestroyOnLoad(this);
	}

	void Start(){
		Music(true);
		Effects(true);
	}

	// TODO singleton de sonido???
	// TODO cambiar lo de volumen y demas por lo nuevo de snapshots de audio

	public void Effects(bool s){
		effectsEnabled = s;
		effectsSource.mute = !s;
		foreach(AudioSource src in loopEffectSources.Values){
			src.mute = !s;
		}
	}
	
	public void Music(bool s){
		musicEnabled = s;
		musicSource.mute = !s;
	}
	
	protected void SetEffectsVolume(float v){
		effectsSource.volume = v;
		foreach(AudioSource src in loopEffectSources.Values){
			src.volume = v;
		}
	}
	
	protected void SetMusicVolume(float v){
		musicSource.volume = v;
	}

	public void PauseGameEffects(){
		foreach(AudioSource src in loopEffectSources.Values){
			if(src.clip != null)
				src.Pause();
		}
	}

	public void ResumeGameEffects(){
		foreach(AudioSource src in loopEffectSources.Values){
			if(src.clip != null)
				src.Play();
		}
	}

	public void PlaySoundEffect(int index, float volume = 1f){
		if(index >= gameSfx.Length)
			return;
		effectsSource.PlayOneShot(gameSfx[index], 1);
	}

	public void PlaySoundEffect(AudioClip effectClip, float volume = 1f){
		effectsSource.pitch = 1f;
		volume = (effectsEnabled) ? volume : 0;
		effectsSource.PlayOneShot(effectClip, volume);
	}
	
	public void PlayLoopEffectOnChannel(AudioClip effectClip, string channel = "", float volume = 1f){
		if(loopEffectSources.ContainsKey(channel)){
			// Piso un canal existente
			loopEffectSource = loopEffectSources[channel];
			if(loopEffectSource.isPlaying && loopEffectSource.clip == effectClip){
				// o no hago nada si ya esta reproduciendo el clip
				return;
			}
		}else{
			// Creo un nuevo canal
			loopEffectSource = gameObject.AddComponent<AudioSource>();
			loopEffectSources.Add(channel, loopEffectSource);
		}
		
		loopEffectSource.clip = effectClip;
		volume = (effectsEnabled) ? volume : 0;
		loopEffectSource.volume = volume;
		loopEffectSource.loop = true;
		loopEffectSource.Play();
	}
	
	public void StopLoopEffectsOnChannel(string channel = ""){
		if(loopEffectSources.ContainsKey(channel)){
			loopEffectSource = loopEffectSources[channel];
			loopEffectSource.Stop();
			loopEffectSource.clip = null;
		}
	}
	
	public void StopAllLoopEffects(){
		foreach(AudioSource src in loopEffectSources.Values){
			ResetSource(src);
		}
	}
	
	public void PlayCenterMusic(int index){
		//StopCoroutine("PlayIntroMusic");
		musicSource.volume = .2f;
		musicSource.clip = bkgMusic[index];
		musicSource.loop = true;
		musicSource.Play();
	}
	
	public void playButtonEffect(){
		PlaySoundEffect(buttonEffect);
	}
	


	public virtual void Reset(){
		StopAllCoroutines();						// Evita que se reproduzca mainTheme.centerClip
		ResetSource(musicSource);					// Resetea la musica
		ResetSource(effectsSource);					// los efectos OneShot
		StopAllLoopEffects();						// los efectos loopeables
		ClearEffectChannels();						// y elimina los AudioSource innecesarios
		Music(true);								// vuelvo el canal de musica a on/off
		Effects(true);								// vuelvo los canales de efectos a on/off
	}
	
	protected void ClearEffectChannels(){
		foreach(AudioSource src in loopEffectSources.Values){
			src.clip = null; // innecesario?
			Destroy(src);
		}
		loopEffectSources.Clear();
	}
	
	protected void ResetSource(AudioSource src){
		src.loop = false;
		src.Stop();
		src.clip = null;
	}

	public void PlayElementSound(Elements element) {
		switch (element) {
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

	public void PlayPlayerHit() {
		PlaySoundEffect ((int)Random.Range (4, 6));
	}

}
