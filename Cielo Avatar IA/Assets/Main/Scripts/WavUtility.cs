using System;
using System.IO;
using UnityEngine;

public static class WavUtility
{
    public static AudioClip ToAudioClip(byte[] wavFile, int offsetSamples = 0, string name = "wav")
    {
        // Create a stream and binary reader based on the given bytes
        MemoryStream stream = new MemoryStream(wavFile);
        BinaryReader reader = new BinaryReader(stream);

        // Skip the riff header
        reader.ReadBytes(12);

        // Read format chunk
        int fmtSize = reader.ReadInt32();
        int fmtCode = reader.ReadInt16();
        int channels = reader.ReadInt16();
        int sampleRate = reader.ReadInt32();
        int byteRate = reader.ReadInt32();
        int blockAlign = reader.ReadInt16();
        int bitDepth = reader.ReadInt16();

        // Skip the rest of the format chunk if present
        if (fmtSize > 16)
        {
            reader.ReadBytes(fmtSize - 16);
        }

        // Read data chunk
        int dataSize = 0;
        while (reader.BaseStream.Position != reader.BaseStream.Length)
        {
            string chunkID = new string(reader.ReadChars(4));
            int chunkSize = reader.ReadInt32();

            if (chunkID.ToLower() == "data")
            {
                dataSize = chunkSize;
                break;
            }

            reader.ReadBytes(chunkSize);
        }

        // Read the data chunk
        byte[] data = reader.ReadBytes(dataSize);

        // Convert the byte data to floats
        float[] audioData = new float[dataSize / 2];
        for (int i = 0; i < audioData.Length; i++)
        {
            audioData[i] = BitConverter.ToInt16(data, i * 2) / 32768.0f;
        }

        // Create the audio clip
        AudioClip audioClip = AudioClip.Create(name, audioData.Length, channels, sampleRate, false);
        audioClip.SetData(audioData, 0);

        return audioClip;
    }
}
