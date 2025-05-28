using System;
using UnityEngine;
using System.IO;
using System.Text;

public static class WavUtility
{
    public static AudioClip ToAudioClip(byte[] wavData, int offsetSamples = 0, string name = "wav")
    {
        if (wavData == null || wavData.Length < 44)
        {
            Debug.LogError("❌ WAV 데이터가 너무 짧거나 null입니다.");
            return null;
        }

        try
        {
            // 포맷 검사
            if (System.Text.Encoding.ASCII.GetString(wavData, 0, 4) != "RIFF" ||
                System.Text.Encoding.ASCII.GetString(wavData, 8, 4) != "WAVE")
            {
                Debug.LogError("❌ WAV 포맷이 아님");
                return null;
            }

            int channels = BitConverter.ToInt16(wavData, 22);
            int sampleRate = BitConverter.ToInt32(wavData, 24);
            int bitsPerSample = BitConverter.ToInt16(wavData, 34);

            // data chunk 위치 탐색
            int subchunk2Index = -1;
            for (int i = 12; i < wavData.Length - 8;)
            {
                string chunkID = System.Text.Encoding.ASCII.GetString(wavData, i, 4);
                int chunkSize = BitConverter.ToInt32(wavData, i + 4);
                if (chunkID == "data")
                {
                    subchunk2Index = i + 8;
                    break;
                }
                i += 8 + chunkSize;
            }

            if (subchunk2Index == -1)
            {
                Debug.LogError("❌ data chunk를 찾을 수 없습니다.");
                return null;
            }

            int dataSize = wavData.Length - subchunk2Index;
            int totalSamples = dataSize / (bitsPerSample / 8);
            if (totalSamples <= 0)
            {
                Debug.LogError("❌ AudioClip 생성 실패: 샘플 수가 0 이하");
                return null;
            }

            float[] audioData = new float[totalSamples];
            int offset = subchunk2Index;
            for (int i = 0; i < totalSamples; i++)
            {
                short sample = BitConverter.ToInt16(wavData, offset);
                audioData[i] = sample / 32768f;
                offset += 2;
            }

            AudioClip clip = AudioClip.Create(name, totalSamples, channels, sampleRate, false);
            clip.SetData(audioData, offsetSamples);
            return clip;
        }
        catch (Exception ex)
        {
            Debug.LogError("❌ WAV 변환 중 예외 발생: " + ex.Message);
            return null;
        }
    }

    public static byte[] FromAudioClip(AudioClip clip, out string clipName)
    {
        clipName = clip.name;
        var samples = new float[clip.samples * clip.channels];
        clip.GetData(samples, 0);
        byte[] wav = ConvertToWav(samples, clip.channels, clip.frequency);
        return wav;
    }

    private static byte[] ConvertToWav(float[] samples, int channels, int sampleRate)
    {
        MemoryStream stream = new MemoryStream();

        // Placeholder for the header
        for (int i = 0; i < 44; i++) stream.WriteByte(0);

        Int16[] intData = new Int16[samples.Length];
        byte[] bytesData = new byte[samples.Length * 2];

        const float rescaleFactor = 32767; // to convert float to Int16

        for (int i = 0; i < samples.Length; i++)
        {
            intData[i] = (short)(samples[i] * rescaleFactor);
            byte[] byteArr = BitConverter.GetBytes(intData[i]);
            byteArr.CopyTo(bytesData, i * 2);
        }

        stream.Write(bytesData, 0, bytesData.Length);

        // Go back and write the header now that the size is known
        stream.Seek(0, SeekOrigin.Begin);

        int fileSize = (int)stream.Length - 8;
        int byteRate = sampleRate * channels * 2;

        // RIFF header
        stream.Write(Encoding.ASCII.GetBytes("RIFF"), 0, 4);
        stream.Write(BitConverter.GetBytes(fileSize), 0, 4);
        stream.Write(Encoding.ASCII.GetBytes("WAVE"), 0, 4);
        stream.Write(Encoding.ASCII.GetBytes("fmt "), 0, 4);
        stream.Write(BitConverter.GetBytes(16), 0, 4); // PCM chunk size
        stream.Write(BitConverter.GetBytes((short)1), 0, 2); // PCM format
        stream.Write(BitConverter.GetBytes((short)channels), 0, 2);
        stream.Write(BitConverter.GetBytes(sampleRate), 0, 4);
        stream.Write(BitConverter.GetBytes(byteRate), 0, 4);
        stream.Write(BitConverter.GetBytes((short)(channels * 2)), 0, 2); // block align
        stream.Write(BitConverter.GetBytes((short)16), 0, 2); // bits per sample

        // data chunk
        stream.Write(Encoding.ASCII.GetBytes("data"), 0, 4);
        stream.Write(BitConverter.GetBytes(bytesData.Length), 0, 4);

        return stream.ToArray();
    }
}
