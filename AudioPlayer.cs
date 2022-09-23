using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Threading;
using NAudio.Wave;

namespace WPFCarta
{
    public class AudioPlayer
    {
        static WaveOut waveOut = null;
        static volatile bool backgroundPlaying = false;

        public static bool IsPlaying
        {
            get
            {
                if (waveOut != null)
                    return waveOut.PlaybackState == PlaybackState.Playing;

                return false;
            }
        }

        public static bool IsBackgroundPlaying
        {
            get
            {
                return backgroundPlaying;
            }
        }

        static Mp3FileReader reader = null;
        static MemoryStream ms = null; 

        public static void Cleanup()
        {
            // dispose old stuff
            if (waveOut != null)
            {
                waveOut.Stop();
                waveOut.Dispose();
                waveOut = null;
            }

            if (reader != null)
            {
                reader.Dispose();
                reader = null;
            }

            if (ms != null)
            {
                ms.Dispose();
                ms = null;
            }
        }

        public static void PlayResource(byte[] stuff)
        {
            Cleanup();

            // create new stuff
            ms = new MemoryStream(stuff);
            reader = new Mp3FileReader(ms);
            WaveStream cs = WaveFormatConversionStream.CreatePcmStream(reader);
            BlockAlignReductionStream bs = new BlockAlignReductionStream(cs);
            waveOut = new WaveOut(WaveCallbackInfo.FunctionCallback());

            waveOut.Init(bs);
            waveOut.Volume = 100.0f / ((float)Convert.ToDouble(ConfigWindow.Volume));
            waveOut.Play();
        }

        public static void PlayResourceInBackground(byte[] stuff1, byte[] stuff2)
        {
            new Thread(new ThreadStart(() => 
            {
                backgroundPlaying = true;

                Cleanup();

                // create new stuff
                ms = new MemoryStream(stuff1);
                reader = new Mp3FileReader(ms);
                WaveStream cs = WaveFormatConversionStream.CreatePcmStream(reader);
                BlockAlignReductionStream bs = new BlockAlignReductionStream(cs);
                waveOut = new WaveOut(WaveCallbackInfo.FunctionCallback());

                waveOut.Init(bs);
                waveOut.Volume = ((float)Convert.ToDouble(ConfigWindow.Volume)) / 100.0f;
                waveOut.Play();

                while(waveOut != null && waveOut.PlaybackState == PlaybackState.Playing)
                {
                    Thread.Sleep(100);
                }

                if (stuff2 != null)
                {
                    Cleanup();

                    ms = new MemoryStream(stuff2);
                    reader = new Mp3FileReader(ms);
                    cs = WaveFormatConversionStream.CreatePcmStream(reader);
                    bs = new BlockAlignReductionStream(cs);
                    waveOut = new WaveOut(WaveCallbackInfo.FunctionCallback());

                    waveOut.Init(bs);
                    waveOut.Play();

                    while (waveOut != null && waveOut.PlaybackState == PlaybackState.Playing)
                    {
                        Thread.Sleep(100);
                    }
                }

                backgroundPlaying = false;
            }
            )).Start();

        }
    }
}
