using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace AudioVisualizer
{
    //An audio data object, for pre-analyzing tracks instead of during run-time
    //Use AudioRecorder to fill in the data
    //Use the AudioSampler.audioFiles list to use the data
    public class AudioData 
    {
        public float clipLength; // the length of the audio clip in seconds
        public int bufferSize; // the size of each sample array
        public bool abs; //did this data use abs or not
        public List<Beat> beats; // times when each beat was recorded
        // all of the audiofrequencydata
        public List<Array> subBassSamples;
        public List<Array> bassSamples;
        public List<Array> lowMidSamples;
        public List<Array> midSamples;
        public List<Array> upperMidSamples;
        public List<Array> highSamples;
        public List<Array> veryHighSamples;
        public List<Array> decibalSamples;


        //constructor
        public AudioData(AudioRecorder recorder, float clipLength)
        {
            beats = new List<Beat>();
            subBassSamples = new List<Array>();
            bassSamples = new List<Array>();
            lowMidSamples = new List<Array>();
            midSamples = new List<Array>();
            upperMidSamples = new List<Array>();
            highSamples = new List<Array>();
            veryHighSamples = new List<Array>();
            decibalSamples = new List<Array>();
            this.clipLength = clipLength;
            bufferSize = recorder.sampleBufferSize;
            abs = recorder.abs;
        }

        //record a Beat into our List
        public void RecordBeat(float time, float volume)
        {
            beats.Add(new Beat(time,volume, true));
        }

        //record samples of a specified FrequencyRange
        public void RecordSamples(FrequencyRange freqRange,float[] data)
        {
            List<Array> samples = GetSampleArray(freqRange);
            samples.Add(new Array(data, true));
        }

        //cast all formatted data (strings) into their appropriate data types
        public void Decompress()
        {
            foreach(Beat b in beats)
            {
                b.Decompress();
            }
            DecompressArrayList(subBassSamples);
            DecompressArrayList(bassSamples);
            DecompressArrayList(lowMidSamples);
            DecompressArrayList(midSamples);
            DecompressArrayList(upperMidSamples);
            DecompressArrayList(highSamples);
            DecompressArrayList(veryHighSamples);
            DecompressArrayList(decibalSamples);

        }

        void DecompressArrayList(List<Array> list)
        {
            foreach (Array a in list)
            {
                a.Decompress();
            }
        }

        //grab samples out of the data container, belonging to the given freqRange.
        //time - is the desired track time we're grabbing samples from.
        public float[] GetSamples(FrequencyRange freqRange,float time)
        {

            if (time > clipLength)
            {
                //return empty samples if the song has ended
                return new float[bufferSize];
            }

            List<Array> samples = GetSampleArray(freqRange);

            float percentThroughSong = time / clipLength;
            int sampleIndex = (int)((samples.Count - 1) * percentThroughSong);
            if( percentThroughSong < 0)
            {
                sampleIndex = 0;
            }


            if(samples.Count == 0)
            {
                Debug.LogError("Error: samples in the FrequencyRange: " + freqRange + " were not recorded for this file");
                return null;
            }
            return samples[sampleIndex].data;

        }

        //grab the correct sample storage list based on FrequencyRange
        public List<Array> GetSampleArray(FrequencyRange freqRange)
        {
            switch (freqRange)
            {
                case FrequencyRange.SubBase:
                    return subBassSamples;
                case FrequencyRange.Bass:
                    return bassSamples;
                case FrequencyRange.LowMidrange:
                    return lowMidSamples;
                case FrequencyRange.Midrange:
                    return midSamples;
                case FrequencyRange.UpperMidrange:
                    return upperMidSamples;
                case FrequencyRange.High:
                    return highSamples;
                case FrequencyRange.VeryHigh:
                    return veryHighSamples;
                default:
                case FrequencyRange.Decibal:
                    return decibalSamples;
            }
        }

        //used for debugging.
        string ArrayToString(float[] data)
        {
            string s = "[";
            foreach (float f in data)
            {
                s += f + ",";
            }
            s += "]";
            return s;
        }
    }

    //a custom class, to view 2D arrays in the inspector
    [System.Serializable]
    public class Array
    {
        public float[] data;
        // formatted into a string for smaller file size during storage
        public string[] formattedData; 

        public Array(float[] input, bool compress = false)
        {
            //store the formatted data
            if (compress)
            {
                formattedData = new string[input.Length];
                for (int i = 0; i < input.Length; i++)
                {
                    formattedData[i] = input[i].ToString("F3");
                }
            }
            else
            {
                data = input;
            }
        }

        //cast all string data into float data for usage
        public void Decompress()
        {
            data = new float[formattedData.Length];
            for (int i = 0; i < formattedData.Length; i++)
            {
                data[i] = float.Parse(formattedData[i]); 
            }

            //clear out the 'formattedData" from memory
            formattedData = null;
        }
    }



    //info related to a detected beat, the volume of the music paired with the time the beat was detected
    [System.Serializable]
    public class Beat
    {
        public float time;
        public float volume;

        // formatted into a string for smaller file size during storage
        public string formattedTime;
        public string formattedVolume;

        public Beat(float beatTime, float beatVolume, bool compress = false)
        {
            if (compress)
            {
                formattedTime = beatTime.ToString("F3");
                formattedVolume = beatVolume.ToString("F3");
            }
            else
            {
                time = beatTime;
                volume = beatVolume;
            }
        }

        public void Decompress()
        {
            time = float.Parse(formattedTime);
            volume = float.Parse(formattedVolume);
        }
    }

}
