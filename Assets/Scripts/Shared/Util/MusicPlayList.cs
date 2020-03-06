using UnityEngine;

public class MusicPlayList : MonoBehaviour
{
    public AudioClip[] songs;

    public AudioSource player;

    float time = 0;

    int counter = 0;

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;

        if (this.time > 0.5f)
        {
            this.time = 0;

            if (!this.player.isPlaying)
            {
                this.counter++;
                this.counter %= this.songs.Length;

                this.player.clip = this.songs[this.counter];
                this.player.Play();
            }
        }
    }
}
