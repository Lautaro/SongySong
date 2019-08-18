using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Songy
{
    public partial class SongySong 
    {
        /// <summary>
        /// Clas Nested in SongySong class
        /// </summary>
        class SongyClip
        {
            public AudioSource source { get; private set; }
            public double Duration { get; private set; }
            public Transform TrackingObject { get; set; }
            public string ClipName { get; private set; }


            public SongyClip(AudioSource _source, AudioClip clip, string clipName )
            {
                source = _source;
                source.clip = clip;
                Duration = ( double )clip.samples / clip.frequency;
                ClipName = clipName;
   
            }
        }
    }
}
