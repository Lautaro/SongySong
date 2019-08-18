using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Songy
{
    public class MusicPlayer : MonoBehaviour
    {
        // Start is called before the first frame update
        public SongySong songy;
        void Awake()
        {
            songy = GetComponent<SongySong>();
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                if ( !songy.IsPlaying  )
                {
                    songy.PlaySongySong ();
                }
                else
                {
                    songy.KillSong ();
                }
            }
        }
    }
}




