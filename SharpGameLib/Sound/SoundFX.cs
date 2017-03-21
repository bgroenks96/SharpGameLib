/*
 * Copyright (C) 2016-2017 (See COPYRIGHT.txt for holders)
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy of
 * this software and associated documentation files (the "Software"), to deal in
 * the Software without restriction, including without limitation the rights to
 * use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of
 * the Software, and to permit persons to whom the Software is furnished to do so,
 * subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
 * FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
 * COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER
 * IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
 * CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
 * 
 */

﻿using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Audio;
using SharpGameLib.Interfaces;
using Microsoft.Xna.Framework.Media;
using System.Threading.Tasks;

namespace SharpGameLib.Sound
{
	public class SoundFX
	{
		private static SoundFX soundFXInstance;

		private readonly IDictionary<string, SoundEffect> soundEffects = new Dictionary<string, SoundEffect>();
        private readonly IDictionary<string, Song> songs = new Dictionary<string, Song>();
        private IDictionary<string, SoundEffectInstance> instances = new Dictionary<string, SoundEffectInstance>();
		private IGameContext context;
		private bool isMuted;
		private bool isDisposed;

		private SoundFX(IGameContext context)
		{
			this.context = context;
			this.isMuted = false;
			this.soundEffects = new Dictionary<String, SoundEffect>();   

			//reset singleton instance
			soundFXInstance = this;
		}

		public static SoundFX Get()
		{
			return soundFXInstance;
		}

		public static SoundFX Create(IGameContext context)
		{
			soundFXInstance?.CancelAllSoundEffects();
			soundFXInstance = soundFXInstance ?? new SoundFX(context);
			return soundFXInstance;
		}

		public void AddSoundEffect(string soundEffect, string key)
		{
			if (soundEffects.ContainsKey(key) || this.isDisposed)
			{
				return;
			}

			var effect = context.LoadContent<SoundEffect>(soundEffect);
			soundEffects.Add(key, effect);
		}

		public void StopSong()
		{
			MediaPlayer.Stop();
		}

		public void AddSong(string resourceName, string key)
		{
			if (this.isMuted || this.isDisposed)
			{
				return;
			}

            var song = context.LoadContent<Song>(resourceName);
            songs.Add(key, song);
		}

        public void PlaySong(string key)
        {
            Song song;
            songs.TryGetValue(key, out song);
            if (song == null)
            {
                return;
            }

            MediaPlayer.Play(song);
            MediaPlayer.Volume = 0.2f;
            MediaPlayer.IsRepeating = true;
        }

		public void Mute()
		{
			if (!this.isMuted)
			{
				this.isMuted = true;
				MediaPlayer.Volume = 0;
			}
			else
			{
				this.isMuted = false;
				MediaPlayer.Volume = 0.2f;
			}
		}

		public void CancelSoundEffect(string key)
		{
			if (!this.instances.ContainsKey(key))
			{
				return;
			}

			this.Dispose(this.instances[key], key);
		}

		public void CancelAllSoundEffects()
		{
			foreach (var key in this.instances.Keys.ToList())
			{
				this.CancelSoundEffect(key);
			}
		}

		public void RemoveSoundEffect(string key)
		{
			if (soundEffects.ContainsKey(key))
			{
				soundEffects.Remove(key);
			}
		}

		public void RemoveAllSoundEffects()
		{
			soundEffects.Clear();
		}

		public void PlaySoundEffect(string key, float volume = 1.0f)
		{
			if (this.isMuted || this.isDisposed)
			{
				return;
			}

			if (soundEffects.ContainsKey(key) && !this.instances.ContainsKey(key))
			{
				var soundEffect = soundEffects[key];
				var soundEffectInstance = soundEffect.CreateInstance();
				soundEffectInstance.IsLooped = false;
				soundEffectInstance.Play();
				soundEffectInstance.Volume = volume;
				instances[key] = soundEffectInstance;
				Task.Delay(soundEffect.Duration).ContinueWith(t => this.Dispose(soundEffectInstance, key));
			}
            
		}

		public void Dispose()
		{
			this.CancelAllSoundEffects();
			this.soundEffects.Clear();
			this.isDisposed = true;
			soundFXInstance = null;
		}

		private void Dispose(SoundEffectInstance instance, string key)
		{
			if (instance.IsDisposed)
			{
				return;
			}

			instance.Stop();
			instance.Dispose();
			instances.Remove(key);
		}
	}
}
