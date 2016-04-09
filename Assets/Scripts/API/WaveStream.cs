using UnityEngine;
using System.Collections;
using System.IO;

public class WaveStream {

	private int m_ChunkID;
	private int m_FileSize;
	private int m_RiffType;
	private int m_FormatID;
	private int m_FormatSize;
	private int m_FormatCode;
	private int m_Channels;
	private int m_SampleRate;
	private int m_FormatAverageBPS;
	private int m_FormatBlockAlign;
	private int m_FormatExtraSize;
	private byte[] m_FormatExtraData;
	private int m_BitDepth;
	private int m_DataID;
	private int m_DataSize;
	private byte[] m_Data;

	public int ChunkID
	{
		get
		{
			return m_ChunkID;
		}
	}

	public int FileSize
	{
		get
		{
			return m_FileSize;
		}
	}

	public int RiffType
	{
		get
		{
			return m_RiffType;
		}
	}

	public int FormatID
	{
		get
		{
			return m_FormatID;
		}
	}

	public int FormatSize
	{
		get
		{
			return m_FormatSize;
		}
	}

	public int FormatCode
	{
		get
		{
			return m_FormatCode;
		}
	}

	public int Channels
	{
		get
		{
			return m_Channels;
		}
	}

	public int SampleRate
	{
		get
		{
			return m_SampleRate;
		}
	}

	public int FormateAverageBPS
	{
		get
		{
			return m_FormatAverageBPS;
		}
	}

	public int FormatBlockAlign
	{
		get
		{
			return m_FormatBlockAlign;
		}
	}

	public int BitDepth
	{
		get
		{
			return m_BitDepth;
		}
	}

	public int FormatExtraSize
	{
		get
		{
			return m_FormatExtraSize;
		}
	}

	public byte[] FormatExtraData
	{
		get
		{
			return m_FormatExtraData;
		}
	}
	
	public int DataID
	{
		get
		{
			return m_DataID;
		}
	}

	public int DataSize
	{
		get
		{
			return m_DataSize;
		}
	}

	public byte[] Data
	{
		get
		{
			return m_Data;
		}
	}

	public WaveStream(string file)
	{
		BinaryReader data = new BinaryReader(File.Open(file, FileMode.Open));

		m_ChunkID = data.ReadInt32();
		m_FileSize = data.ReadInt32();
		m_RiffType = data.ReadInt32();
		m_FormatID = data.ReadInt32();
		m_FormatSize = data.ReadInt32();
		m_FormatCode = data.ReadInt16();
		m_Channels = data.ReadInt16();
		m_SampleRate = data.ReadInt32();
		m_FormatAverageBPS = data.ReadInt32();
		m_FormatBlockAlign = data.ReadInt16();
		m_BitDepth = data.ReadInt16();

		if (m_FormatSize == 18)
		{
			// Read any extra values
			m_FormatExtraSize = data.ReadInt16();
			m_FormatExtraData = data.ReadBytes(m_FormatExtraSize);
		}

		m_DataID = data.ReadInt32();
		m_DataSize = data.ReadInt32();

		m_Data = data.ReadBytes(m_DataSize);
		
		data.Close();
	}
}

public class WAV
{

	// convert two bytes to one float in the range -1 to 1
	static float bytesToFloat(byte firstByte, byte secondByte)
	{
		// convert two bytes to one short (little endian)
		short s = (short)((secondByte << 8) | firstByte);
		// convert to range from -1 to (just below) 1
		return s / 32768.0F;
	}

	static int bytesToInt(byte[] bytes, int offset = 0)
	{
		int value = 0;
		for (int i = 0; i < 4; i++)
		{
			value |= ((int)bytes[offset + i]) << (i * 8);
		}
		return value;
	}

	private static byte[] GetBytes(string filename)
	{
		return File.ReadAllBytes(filename);
	}
	// properties
	public float[] LeftChannel
	{
		get; internal set;
	}
	public float[] RightChannel
	{
		get; internal set;
	}
	public int ChannelCount
	{
		get; internal set;
	}
	public int SampleCount
	{
		get; internal set;
	}
	public int Frequency
	{
		get; internal set;
	}

	internal WAV()
	{
	}

	// Returns left and right double arrays. 'right' will be null if sound is mono.
	public WAV(string filename) :
		this(GetBytes(filename))
	{
	}

	public WAV(byte[] wav)
	{

		// Determine if mono or stereo
		ChannelCount = wav[22];     // Forget byte 23 as 99.999% of WAVs are 1 or 2 channels

		// Get the frequency
		Frequency = bytesToInt(wav, 24);

		// Get past all the other sub chunks to get to the data subchunk:
		int pos = 12;   // First Subchunk ID from 12 to 16

		// Keep iterating until we find the data chunk (i.e. 64 61 74 61 ...... (i.e. 100 97 116 97 in decimal))
		while (!(wav[pos] == 100 && wav[pos + 1] == 97 && wav[pos + 2] == 116 && wav[pos + 3] == 97))
		{
			pos += 4;
			int chunkSize = wav[pos] + wav[pos + 1] * 256 + wav[pos + 2] * 65536 + wav[pos + 3] * 16777216;
			pos += 4 + chunkSize;
		}
		pos += 8;

		// Pos is now positioned to start of actual sound data.
		SampleCount = (wav.Length - pos) / 2;     // 2 bytes per sample (16 bit sound mono)
		if (ChannelCount == 2)
			SampleCount /= 2;        // 4 bytes per sample (16 bit stereo)

		// Allocate memory (right will be null if only mono sound)
		LeftChannel = new float[SampleCount];
		if (ChannelCount == 2)
			RightChannel = new float[SampleCount];
		else
			RightChannel = null;

		// Write to double array/s:
		int i = 0;
		while (pos < wav.Length)
		{
			LeftChannel[i] = bytesToFloat(wav[pos], wav[pos + 1]);
			pos += 2;
			if (ChannelCount == 2)
			{
				RightChannel[i] = bytesToFloat(wav[pos], wav[pos + 1]);
				pos += 2;
			}
			i++;
		}
	}

	public override string ToString()
	{
		return string.Format("[WAV: LeftChannel={0}, RightChannel={1}, ChannelCount={2}, SampleCount={3}, Frequency={4}]", LeftChannel, RightChannel, ChannelCount, SampleCount, Frequency);
	}
}