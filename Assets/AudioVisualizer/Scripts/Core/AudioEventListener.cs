using UnityEngine;
using UnityEngine.Events;
using System.Collections;

/// <summary>
/// Listen for beats in the given frequencyRange.
/// Can either automatically detect beats or be be fine tuned for a song by hand.
/// </summary>
namespace AudioVisualizer
{
    
    //Unity OnChange float event
    [System.Serializable]
    public class OnFrequencyPublicEvent : UnityEvent<float> {
    };

    //custom class for onChange event, and min/max value
    [System.Serializable]
    public class OnPublicFrequencyChanged
    {
        public OnFrequencyPublicEvent onChange; // hook in a public float variable here
        //it will be changed between these min/max values with the audio frequency.
        public float minValue = 0;
        public float maxValue = 1;
    }

    /// <summary>
    /// Listen for audio events.
    /// </summary>
    public class AudioEventListener : MonoBehaviour
    {
        [Tooltip("Index into the AudioSampler audioSources list")]
        public int audioIndex = 0; // index into audioSampler audioSources or audioFIles list. Determines which audio source we want to sample
        [Tooltip("OnBeat events will trigger x seconds before the beat (Requires SilentAudio or AudioFiles)")]
        public float preBeatOffset = 0; // OnBeat events will trigger "preBeatOffset" seconds before the beat occurs.
        [Tooltip("The frequency range you want to listen to")]
        public FrequencyRange frequencyRange = FrequencyRange.Decibal; // what frequency will we listen to? 
        [Tooltip("How many samples are taken every frame?")]
        public int sampleBufferSize = 40;
        [Tooltip("If currentAudio> averageAudio*beatThreshold we have a beat")]
        public float beatThreshold = 1.3f; // audio threshold, if current is > avg*threshold, we have a beat
        [Tooltip("Automatically adjust the beatThreshold")]
        public bool automaticThreshold = true; // automatically detects and moves the beatThreshold
        [Tooltip("Consecutive beats below this time limit will not be registered. \n A good beatLimiter is about 1/2 bpm")]
        public float beatLimiter = 0; // consecutive beats below this time limit will not be regsitered
        [Tooltip("Automatically adjust the beatLimiter")]
        public bool automaticLimiter = false;
        [Tooltip("Show debug info?")]
        public bool debug = false; // print debug statements?
        [Tooltip("Calls public events when a beat is detected")]
        public UnityEvent OnBeat; // call these method when a beat hits

        [Tooltip("Adjusts public float values between min and max")]
        public OnPublicFrequencyChanged onFrequencyChanged; //custom class for onChange<float> event between a min and max value.
        //delegate, so we can listen for beat events. When an event is detected, we'll call the public OnBeat() methods.
        public delegate void BeatEvent(Beat beat);
        public static BeatEvent OnBeatRecognized;


        // public DebugChart avgChart;

        private bool canDetect = true; // flag indicating if we can detect another beat or not
        private float lastFreq = 0; // the frequency of the last detected beat
        private float lastVariance = 0;// the variance of the last frame
        private float[] sampleBuffer; //buffers the audio samples taken
        private int index = 0; // our index into the sample buffer.
        float avgEnergy; // compute the current average
        float variance; // compare everything in the sample buffer to the current average
        float varyPercent; //how much variance are we seeing?
        float frequency; // the current frequency!
        
        float avgBeatTime = 0; //avg time between beats.
        float lastBeatTime = 0; //the time the last beat was recorded
        float totalBeatTime = 0; // sum of every beat time, used to track avgBeatTime
        int numBeats = 0;
        void Awake()
        {
            sampleBuffer = new float[sampleBufferSize]; //buffers the audio samples taken
            //initialize our array
            for (int i = 0; i < sampleBuffer.Length; i++)
            {
                sampleBuffer[i] = 0;
            }

        }

        private void OnEnable()
        {
            AudioSampler.AudioUpdate += AudioUpdate;
        }

        private void OnDisable()
        {
            AudioSampler.AudioUpdate -= AudioUpdate;
        }

        // Update is called once per frame
        void AudioUpdate(float audioTimer, float deltaTime)
        {
            if (automaticThreshold)
            {
                AutomaticDetection(audioTimer); // automatic input
            }
            else
            {
                CustomDetection(audioTimer); // custom input
            }

            HandleFrequencyEvents();

        }

        //set float values equal to the normalized frequency of the audio
        //normalized frequency ranges 0 to 1
        //output value = minValue + (maxValue-minValue)*normalizedFrequency
        void HandleFrequencyEvents()
        {
            float scaledFrequency = GetScaledFrequency(onFrequencyChanged.minValue, onFrequencyChanged.maxValue);
            //get a value between min-max, using the frequency.
            if (onFrequencyChanged != null)
            {

                onFrequencyChanged.onChange.Invoke(scaledFrequency);
            }
        }



        void CustomDetection(float audioTime)
        {

            if (index >= sampleBuffer.Length)
            {
                index = 0;
            }
            frequency = AudioSampler.instance.GetFrequencyVol(audioIndex, frequencyRange,false, AudioSampler.instance.silentAudio.useSilentAudio); // get the root means squared value of the audio right now
            sampleBuffer[index] = frequency; // replace the oldest sample in our runningAvg array with the new sample
            index++;
            avgEnergy = GetAvgEnergy(); // compute the current average


            //if instantEnergy is > beatThreshold*avgEnergy
            if (frequency > beatThreshold * avgEnergy)
            {

                float deltaTime = audioTime - lastBeatTime;
                if (deltaTime < beatLimiter)
                {
                    if (debug)
                    {
                        Debug.Log("BEAT LIMITED, reduce beatLimiter to allow more beats through.");
                    }
                    //do not register a beat if the time since the last beat is < beatLimiter
                    return;
                }

                lastBeatTime = audioTime;
                totalBeatTime += deltaTime;
                numBeats++;
                avgBeatTime = totalBeatTime / numBeats;
                if (automaticLimiter)
                {
                    beatLimiter = avgBeatTime * .5f; // don't allow consecutive beats < half of the avgBeatTime apart
                }

                float volume = AudioSampler.instance.GetRMS(audioIndex, false, AudioSampler.instance.silentAudio.useSilentAudio);

                //Debug.Log("Live Beat: " + audioTime);
                OnBeat.Invoke(); // call the public OnBeat methods
                if (OnBeatRecognized != null) // if we have a listener for this event
                {
                    OnBeatRecognized.Invoke(new Beat(audioTime, volume)); // send the event
                    if (debug)
                    {
                        Debug.Log("Beat Detected");
                    }
                }


            }

            if (debug)
            {
                Debug.Log("FreqVolume: " + frequency + " beatThreshold: " + beatThreshold * avgEnergy);
            }

           
        }

       

        void AutomaticDetection(float audioTime)
        {
            if (index >= sampleBuffer.Length)
            {
                //canDetect = true;
                index = 0;
            }

            frequency = AudioSampler.instance.GetFrequencyVol(audioIndex, frequencyRange,false, AudioSampler.instance.silentAudio.useSilentAudio); // get the root means squared value of the audio right now
            sampleBuffer[index] = frequency; // replace the oldest sample in our runningAvg array with the new sample
            index++;
            //don't do anything until we have filled our buffer up completely
            if (sampleBuffer[sampleBuffer.Length - 1] == 0)
            {
                return;
            }

            avgEnergy = GetAvgEnergy(); // compute the current average
            variance = GetAvgVariance(avgEnergy); // compare everything in the sample buffer to the current average
            varyPercent = 1 - ((avgEnergy - variance) / avgEnergy); //how much variance are we seeing?
            beatThreshold = 1 + varyPercent; //beatThreshold in range 1-2


            //if we can't detect beats
            if (!canDetect)
            {
                //check to see if we should be able to detect them again.
                //we can start looking for new beats once freq drops
                if (frequency < (2 - beatThreshold) * avgEnergy)
                {
                    canDetect = true;
                }
            }

            //if instantEnergy is > beatThreshold*avgEnergy
            if (frequency > beatThreshold * avgEnergy && canDetect)
            {
                //Debug.Log("\n Beat Detected, (" + freq + " > " + beatThreshold*avgEnergy + " ) \n");
                canDetect = false; //reset the flag. we can't detect another beat, until freq drops below 'lastFreq'
                lastFreq = frequency;  //record the frequency of the last beat
                lastVariance = varyPercent; // record the variancePercent of the last beat
                
                float deltaTime = audioTime - lastBeatTime;
                if (deltaTime < beatLimiter)
                {
                    if(debug)
                    {
                        Debug.Log("BEAT LIMITED, reduce beatLimiter to allow more beats through.");
                    }
                    //do not register a beat if the time since the last beat is < beatLimiter
                    return;
                }

                lastBeatTime = audioTime;
                totalBeatTime += deltaTime;
                numBeats++;
                avgBeatTime = totalBeatTime / numBeats;
                if (automaticLimiter)
                {
                    beatLimiter = avgBeatTime * .5f; // don't allow consecutive beats < half of the avgBeatTime apart
                }

                OnBeat.Invoke(); // call the public OnBeat methods
                //Debug.Log("Live Beat: " + audioTime);
                //Debug.Log("frequency: " + frequency + " > " + beatThreshold + "*" + avgEnergy);
                float rms = AudioSampler.instance.GetRMS(audioIndex, false, AudioSampler.instance.silentAudio.useSilentAudio);

                if (OnBeatRecognized != null) // if we have a listener for this event
                {
                    OnBeatRecognized.Invoke(new Beat(audioTime, rms)); // send the event
                    if (debug)
                    {
                        Debug.Log("Beat Detected at " + audioTime);
                    }
                }

            }

            if (debug)
            {
                Debug.Log("Freq: " + frequency + " beatThreshold: " + beatThreshold * avgEnergy);
            }
            
        }

        //Get the max frequency in the sample buffer, divide the current frequency by it.
        //returns a 0-1 value.
        public static float GetNormalizedFrequency(float frequency,float[] values)
        {
            //find the highest sample
            float max = -Mathf.Infinity;
            foreach(float sampleFreq in values)
            {
                max = Mathf.Max(max, sampleFreq);
            }

            //return currentSample/maxSample
            return (frequency / max);
        }

        //average all the samples in the sample buffer.
        float GetAvgEnergy()
        {
            float sum = 0;
            for (int i = 0; i < sampleBuffer.Length; i++)
            {
                sum += sampleBuffer[i];
            }

            float avg = sum / sampleBuffer.Length;
            return avg;
        }

        //get the variance of samples in the sample buffer.
        float GetAvgVariance(float avg)
        {
            float sum = 0;
            for (int i = 0; i < sampleBuffer.Length; i++)
            {
                float variance = (sampleBuffer[i] - avg);  //compare each sample in our buffer, to the avg. 
                //Debug.Log("Variance: " + i + " = " + sampleBuffer[i] + "-" + avg);
                sum += Mathf.Abs(variance); //sum up all variances, to get the avgVariance
            }

            float avgVariance = sum / sampleBuffer.Length;
            return avgVariance;
        }

        //get a frequency between a max and min
        public float GetScaledFrequency(float min, float max)
        {
            float delta = max - min; //delta = max-min
            float scaledValue = min + delta * GetNormalizedFrequency(frequency, sampleBuffer); //min + delta*frequency

            return scaledValue;
        }
    }
}
