using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;
using System.Collections.Generic;

namespace Dog_Sliding
{
    public class Audio
    {
        public Dictionary<string, SoundEffectInstance> _soundInstances;
        public Dictionary<string, Song> _songs;
        private float _SFXVolume;
        private bool isMuted;
        private float previousMusicVolume;
        private float previousSFXVolume;


        public Audio()
        {
            _soundInstances = new Dictionary<string, SoundEffectInstance>();
            _songs = new Dictionary<string, Song>();
            _SFXVolume = 0.3f;
            isMuted = false;
        }

        public void LoadSounds(ContentManager Content)
        {
            // Song Loading
            _songs["Bgm"] = Content.Load<Song>("happy-sandbox");

            // Sound Effect Loading
            SoundEffect _sound1 = Content.Load<SoundEffect>("heart-beat");
            SoundEffect _sound2 = Content.Load<SoundEffect>("menu-click");
            SoundEffect _sound3 = Content.Load<SoundEffect>("success-clear");
            SoundEffect _sound4 = Content.Load<SoundEffect>("lost-game-over");
            SoundEffect _sound5 = Content.Load<SoundEffect>("perritoguautierno");
            SoundEffect _sound6 = Content.Load<SoundEffect>("freezing-hza");
            SoundEffect _sound7 = Content.Load<SoundEffect>("howtoplay_click");

            _soundInstances.Add("heart-beat", _sound1.CreateInstance());
            _soundInstances.Add("menu-click", _sound2.CreateInstance());
            _soundInstances.Add("success-clear", _sound3.CreateInstance());
            _soundInstances.Add("lost-game-over", _sound4.CreateInstance());
            _soundInstances.Add("perritoguautierno", _sound5.CreateInstance());
            _soundInstances.Add("freezing-hza", _sound6.CreateInstance());
            _soundInstances.Add("howtoplay_click", _sound7.CreateInstance());

            foreach (var _sinst in _soundInstances.Values)
            {
                // Setup SFX Volume for each Instances
                _sinst.Volume = _SFXVolume;
            }
        }

        public void PlayEffect(string name)
        {
            if (_soundInstances.ContainsKey(name))
            {
                _soundInstances[name].Play();
            }
        }

        public void PlayMusic(string name, float volume)
        {
            if (_songs.ContainsKey(name))
            {
                MediaPlayer.Stop();
                MediaPlayer.Volume = MathHelper.Clamp(volume, 0f, 1f);
                MediaPlayer.Play(_songs[name]);
                MediaPlayer.IsRepeating = true;
            }
        }

        public void SetVolume(float volume)
        {
            MediaPlayer.Volume = MathHelper.Clamp(volume, 0f, 1f);

            _SFXVolume = MathHelper.Clamp(volume, 0f, 1f);

            foreach (var _sinst in _soundInstances.Values)
            {
                _sinst.Volume = _SFXVolume;
            }
        }

        public void IncreaseVolume(float increment = 0.1f)
        {
            if (!isMuted)
            {
                float newVolume = MathHelper.Clamp(MediaPlayer.Volume + increment, 0f, 1f);
                SetVolume(newVolume);

                if (isMuted)
                {
                    UnmuteAll();
                }
            }
        }

        public void DecreaseVolume(float decrement = 0.1f)
        {
            if (!isMuted)
            {
                float newVolume = MathHelper.Clamp(MediaPlayer.Volume - decrement, 0f, 1f);
                SetVolume(newVolume);

                if (newVolume <= 0f)
                {
                    MuteAll();
                }
            }
        }

        public void MuteAll()
        {
            if (!isMuted)
            {
                previousMusicVolume = MediaPlayer.Volume;
                previousSFXVolume = _SFXVolume;

                MediaPlayer.Volume = 0f;

                foreach (var _sinst in _soundInstances.Values)
                {
                    _sinst.Volume = 0f;
                }

                isMuted = true;
            }
        }

        public void UnmuteAll()
        {
            if (isMuted)
            {
                MediaPlayer.Volume = previousMusicVolume;
                foreach (var _sinst in _soundInstances.Values)
                {
                    _sinst.Volume = previousSFXVolume;
                }

                isMuted = false;
            }
        }

        public bool IsMuted()
        {
            return isMuted;
        }

        public float GetCurrentVolume()
        {
            return MediaPlayer.Volume;
        }
    }
}