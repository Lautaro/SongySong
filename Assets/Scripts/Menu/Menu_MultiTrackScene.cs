using Songy;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Menu_MultiTrackScene : MonoBehaviour
{
    public SongySong songySong;

    public void ToggleSongyClip( string songyClipName )
    {
        var wasMuted = songySong.ToogleSongyClip ( songyClipName );
        SetVolumeSlider ( wasMuted ? 0f : 1f, songyClipName );
    }

    void SetVolumeSlider( float value, string sliderName )
    {
        var slider = GetComponentsInChildren<Slider> ().ToList ().
            First ( s => s.name.ToLower().Contains ( sliderName.Replace(" ", "").ToLower() ) );

        slider.value = value;
    }

    public void SongBtn()
    {
        if ( songySong.IsPlaying )
        {
            songySong.KillSong ();
        }
        else
        {
            songySong.PlaySongySong ();
        }
    }

    public void SetSongyClipVolume( float volume, string songyClipName )
    {
        songySong.SetClipVolume ( volume, songyClipName );
    }

    public void SetBasicBeatVolume( float volume )
    {
        songySong.SetClipVolume ( volume, "BasicBeat" );
    }

    public void SetBassVolume( float volume )
    {
        songySong.SetClipVolume ( volume, "Bass" );
    }

    public void SetChordsVolume( float volume )
    {
        songySong.SetClipVolume ( volume, "Chords" );
    }

    public void SetPadsVolume( float volume )
    {
        songySong.SetClipVolume ( volume, "Pads" );
    }

    public void SetArrpegioVolume( float volume )
    {
        songySong.SetClipVolume ( volume, "Arrpegio" );
    }

    public void SetExtraBeatVolume( float volume )
    {
        songySong.SetClipVolume ( volume, "ExtraBeat" );
    }
}
