using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZ_Pooling;

[System.Serializable]
public class SoundManager {
    public static SoundManager i;
    [SerializeField] private Transform staticSoundContainer;

    public void Initialize() {
        i = this;
        Gun.GunFiredEvent += Gun_gunFiredEvent;
    }

    #region Event listeners
    private void Gun_gunFiredEvent(Gun gun, ProjectileLaunchParams lauchParams) {
        PlaySoundStatic(Sounds.i.gs_dyiAk, lauchParams.position);
    }
    #endregion

    /// <summary> Plays a non moving sound effect  </summary>
    public void PlaySoundStatic(SFX sfx, Vector3 position) {

        // Create SFX gameObject.
        GameObject sfxGameObject = EZ_PoolManager.Spawn(GameObjects.i.SFX_base.transform, position, Quaternion.identity).gameObject;
        sfxGameObject.transform.parent = staticSoundContainer;

        // Get random variation and play it.
        AudioSource audioSource = sfxGameObject.GetComponent<AudioSource>();
        audioSource.clip = sfx.GetRandomVariation();
        audioSource.volume = sfx.volume;
        audioSource.pitch = 1;
        audioSource.Play();
        sfxGameObject.GetComponent<Despawner>().DelayedDespawn(audioSource.clip.length);
    }

    /// <summary> Plays a moving sound effect  </summary>
    public void PlaySound(SFX sfx, Vector3 position, Transform parent ) {

        // Create SFX gameObject.
        GameObject sfxGameObject = EZ_PoolManager.Spawn(GameObjects.i.SFX_base.transform, position, Quaternion.identity).gameObject;
        sfxGameObject.transform.parent = parent;

        // Get random variation and play it.
        AudioSource audioSource = sfxGameObject.GetComponent<AudioSource>();
        audioSource.clip = sfx.GetRandomVariation();
        audioSource.volume = sfx.volume;
        audioSource.pitch = 1;
        audioSource.Play();
        sfxGameObject.GetComponent<Despawner>().DelayedDespawn(audioSource.clip.length);
    }
}
