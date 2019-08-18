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
        class SongyPart : MonoBehaviour
        {
            public string SongyPartName { get; set; }
            Dictionary<string, SongyClip> clips;
            public bool ShouldLoop { get; set; }
            bool isPlaying;
            public bool IsPlaying { get { return isPlaying; } }
            double paddingTime = 0.01;
            bool isScheduledToStart;
            internal double ScheduledToStartAt { get; private set; }
            internal double EndsPlayingDspTime { get; set; }

            public  bool HasClip()
            {
                return clips != null && clips.Count () > 1;
            }
            public Dictionary<string, SongyClip> GetClipsDic()
            {
                return clips;
            }
       
            public  AudioSource GetSongyClipSource(string songyclipName)
            {
                return clips.FirstOrDefault ( kvp => kvp.Key == songyclipName ).Value.source;
            }

            public SongyPart AddClip(string folderPath, string clipName )
            {
                if ( clips == null )
                {
                    clips = new Dictionary<string, SongyClip> ();
                }

                if ( clips.ContainsKey(clipName) )
                {
                    throw new Exception ("SongyPart already has a clip called " + clipName);
                }

                var path = $"{folderPath}\\{clipName}"; 
               var  clip = Resources.Load<AudioClip> ( path );

                if ( clip == null )
                {
                    throw new Exception ( $"AudioClip does not exist at given path: {path}" );
                }

                var songyClip = new SongyClip (gameObject.AddComponent<AudioSource>(), clip, clipName);
                clips.Add ( clipName, songyClip );
                
                return this;
            }

            SongyClip FirstClip()
            {
                return clips.ElementAt ( 0 ).Value;
            }

            public void PlayClipNow()
            {
                SchedulePlay ( CurrentDspTime () + paddingTime );
            }

             void SchedulePlay( double dspTime )
            {
                var firstClip = FirstClip ();
                var firstSource = firstClip.source;

                ScheduledToStartAt = dspTime; ;
                isScheduledToStart = true;
                EndsPlayingDspTime = ScheduledToStartAt + firstClip.Duration;

                foreach ( var source  in clips.Values.Select(c => c.source) )
                {
                    source.loop = ShouldLoop;
                    source.PlayScheduled ( dspTime );
                }
            }

           public void PlayClipAfter( SongyPart songPart )
            {
                var dspTime = songPart.EndsPlayingDspTime;
                SchedulePlay ( dspTime );
            }

            public void Update()
            {
                if ( CurrentDspTime () >= EndsPlayingDspTime && IsPlaying == true && !ShouldLoop )
                {
                    isPlaying = false;
                    isScheduledToStart = false;
                    print ( SongyPartName + " STOPPED PLAYING  at " + CurrentDspTime () );
                    print ( "TimeSamples when stopped  = " + FirstClip ().source.timeSamples );

                }

                if ( CurrentDspTime () >= ScheduledToStartAt && isScheduledToStart == true && IsPlaying == false )
                {
                    print ( SongyPartName + " STARTS PLAYING  at " + CurrentDspTime () );
                    isPlaying = true;
                }
            }

            public void StopAndPlayNext( SongyPart nextSongPart )
            {
                Stop ();
                nextSongPart.SchedulePlay ( EndsPlayingDspTime );
            }

            public void Stop()
            {
                EndsPlayingDspTime = CurrentDspTime () + paddingTime;

                foreach ( var source in clips.Values.Select ( c => c.source ) )
                {
                    source.SetScheduledEndTime ( EndsPlayingDspTime );
                }
            }

            private double CurrentDspTime()
            {
                return AudioSettings.dspTime;
            }
        }
    }
}
