using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Songy
{
    public partial class SongySong : MonoBehaviour
    {
        SongyPart Intro;
        SongyPart Loop;
        SongyPart Outro;
        public bool IsPlaying;
        public string FolderPath;
        float MainVolume = 1f;
        public float MinimalDistance;
        public Transform TrackingObject { get; set; }

        private void Awake()
        {
            Intro = gameObject.AddComponent<SongyPart> ();
            Loop = gameObject.AddComponent<SongyPart> ();
            Outro = gameObject.AddComponent<SongyPart> ();
            Loop.ShouldLoop = true;
        }

        private void Update()
        {
            foreach ( var clip in GetAllClipsInSongy () )
            {
                var distance = PercentageOfMinimalDistanceToClip ( clip ) / 100;
                clip.source.volume = 1f - distance;
                print ( "Volume : " + clip.source.volume );
            }
        }

        public void SetTrackingObjectOnClip( string clipName, Transform transform )
        {
            var clip = GetClipByName ( clipName );

            if ( clip == null )
            {
                throw new KeyNotFoundException ($"Clip named {clipName} does not exist in SongySong");
            }

            clip.TrackingObject = transform;
        }

        float PercentageOfMinimalDistanceToClip( SongyClip clip )
        {
            var clipPosition = clip.TrackingObject.position;
            var soundPosition = TrackingObject.position;

            var diff = Mathf.Abs ( Vector2.Distance ( clipPosition, soundPosition ) );

            return diff / MinimalDistance * 100;
        }

        public void SetClipVolume( float volume, string clipName )
        {
            var allClips = GetClipByName ( clipName );

            foreach ( var songyClip in GetAllClipsInSongy() )
            {
                songyClip.source.volume = volume;
            }
        }

        public void SetMainVolume( float volume )
        {
            MainVolume = volume;
            foreach ( var clip in GetAllClipsInSongy () )
            {
                clip.source.volume = MainVolume;
            }
        }

        /// <summary>
        /// Returns true if sound is muted
        /// </summary>
        public bool ToogleSongyClip( string clipName )
        {
            var allClips = GetClipByName ( clipName );

            foreach ( var songyClip in GetAllClipsInSongy() )
            {
                if ( songyClip.source.volume > 0 )
                {
                    songyClip.source.volume = 0f;
                    return true;
                }
                else
                {
                    songyClip.source.volume = 1f;
                    return false;
                }
            }

            throw new Exception ( "Could not find clip named : " + clipName );
        }

        public void SetSongyClipVolume( string clipName )
        {
            var allClips = GetClipByName ( clipName );

            foreach ( var songyClip in GetAllClipsInSongy() )
            {
                if ( songyClip.source.volume > 0 )
                {
                    songyClip.source.volume = 0f;
                }
                else
                {
                    songyClip.source.volume = 1f;
                }
            }
        }

        List<SongyClip> GetAllClipsInSongy()
        {
            var allClipsInSongy = new List<SongyClip> ();
            void AddIfNotNullAndNotDupes( Dictionary<string, SongyClip> clips )
            {
                if ( clips != null )
                {
                    foreach ( var clip in clips )
                    {
                        if ( !allClipsInSongy.Contains ( clip.Value ) )
                        {
                            allClipsInSongy.Add ( clip.Value );
                        }
                    }
                }
            }
            AddIfNotNullAndNotDupes ( Intro.GetClipsDic () );
            AddIfNotNullAndNotDupes ( Loop.GetClipsDic () );
            AddIfNotNullAndNotDupes ( Outro.GetClipsDic () );

            return allClipsInSongy;
        }

        private SongyClip GetClipByName( string clipName )
        {
            var allClips = GetAllClipsInSongy ();
            var returnClip =  allClips.FirstOrDefault ( c => c.ClipName == clipName);
            return returnClip;
        }

        public void SetIntro( params string[] clipNames )
        {
            AddClips ( clipNames, Intro );
        }

        public void SetLoop( params string[] clipNames )
        {
            AddClips ( clipNames, Loop );
        }

        public void SetOutro( params string[] clipNames )
        {
            AddClips ( clipNames, Outro );
        }

        void AddClips( string[] clipNames, SongyPart songyPart )
        {
            foreach ( var name in clipNames )
            {
                if ( GetClipByName ( name ) != null )
                {
                    Debug.LogWarning ( $"You are trying to add the SongyClip named {name} twice. Dont do that plz. " );
                    continue;
                }
                songyPart.AddClip ( FolderPath, name );
            }
        }

        public void PlaySongySong()
        {
            if ( Intro.HasClip () )
            {
                Intro.PlayClipNow ();
                Loop.PlayClipAfter ( Intro );
                IsPlaying = true;
            }
            else if ( Loop.HasClip () )
            {
                Loop.PlayClipNow ();
                IsPlaying = true;
            }
            else
            {
                IsPlaying = false;
            }
        }

        public bool KillSong()
        {
            if ( Loop.IsPlaying && Loop.HasClip () )
            {
                Loop.StopAndPlayNext ( Outro );
                IsPlaying = false;
                return true;
            }

            return false;
        }

    }
}

