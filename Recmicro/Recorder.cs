using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recmicro
{
    public enum StatysRecorder
    {
        Start,
        Stop,
    }
    public class Recorder
    {
        public WaveIn waveSource = null;
        public WaveFileWriter waveFile = null;

        public string NameAudioFile = "myfile.wav";
        public string DirectoryRecorder = "Recorder";

        public StatysRecorder statysRecorder = StatysRecorder.Stop;

        public Recorder()
        {
            if (!Directory.Exists(DirectoryRecorder))
            {
                Directory.CreateDirectory(DirectoryRecorder);
            }
        }
        
        public void Start()
        {
            if (statysRecorder == StatysRecorder.Start)
                return;

            waveSource = new WaveIn();
            waveSource.WaveFormat = new WaveFormat(44100, 1);
            waveSource.DataAvailable += new EventHandler<WaveInEventArgs>(waveSource_DataAvailable);
            waveSource.RecordingStopped += new EventHandler<StoppedEventArgs>(waveSource_RecordingStopped);

         
            waveFile = new WaveFileWriter(Path.Combine(DirectoryRecorder, NameAudioFile), waveSource.WaveFormat);
            waveSource.StartRecording();
            statysRecorder = StatysRecorder.Start;
        }
        public void Stop()
        {
            if (statysRecorder == StatysRecorder.Stop)
                return;


            waveSource.StopRecording();
            
        }
        void waveSource_DataAvailable(object sender, WaveInEventArgs e)
        {
            if (waveFile != null)
            {
                
                waveFile.Write(e.Buffer, 0, e.BytesRecorded);
                waveFile.Flush();
            }
        }

        void waveSource_RecordingStopped(object sender, StoppedEventArgs e)
        {
            if (waveSource != null)
            {
                waveSource.Dispose();
                waveSource = null;
            }
            if (waveFile != null)
            {
                waveFile.Dispose();
                waveFile = null;
            }
            statysRecorder = StatysRecorder.Stop;
        }
    }
}
