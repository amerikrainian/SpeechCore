using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Collections.Generic;

public class SpeechCore : IDisposable
{
    private bool disposed = false;
    private const string DllName = "SpeechCore";

    public const uint SC_SPEECH_FLOW_CONTROL = 1 << 0;
    public const uint SC_SPEECH_PARAMETER_CONTROL = 1 << 1;
    public const uint SC_VOICE_CONFIG = 1 << 2;
    public const uint SC_FILE_OUTPUT = 1 << 3;
    public const uint SC_HAS_SPEECH = 1 << 4;
    public const uint SC_HAS_BRAILLE = 1 << 5;
    public const uint SC_HAS_SPEECH_STATE = 1 << 6;

    private static readonly bool IsWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);

    [DllImport(DllName)]
    private static extern void Speech_Init();

    [DllImport(DllName)]
    private static extern void Speech_Free();

    [DllImport(DllName)]
    private static extern void Speech_Detect_Driver();

    [DllImport(DllName)]
    private static extern IntPtr Speech_Current_Driver();

    [DllImport(DllName)]
    private static extern IntPtr Speech_Get_Driver(int index);

    [DllImport(DllName)]
    private static extern void Speech_Set_Driver(int index);

    [DllImport(DllName)]
    private static extern int Speech_Get_Drivers();

    [DllImport(DllName)]
    private static extern uint Speech_Get_Flags();

    [DllImport(DllName)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool Speech_Is_Loaded();

    [DllImport(DllName)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool Speech_Is_Speaking();

    [DllImport(DllName, EntryPoint = "Speech_Output")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool Speech_Output_Windows([MarshalAs(UnmanagedType.LPWStr)] string text, [MarshalAs(UnmanagedType.Bool)] bool interrupt);

    [DllImport(DllName, EntryPoint = "Speech_Output")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool Speech_Output_Unix(IntPtr textPtr, [MarshalAs(UnmanagedType.Bool)] bool interrupt);

    [DllImport(DllName, EntryPoint = "Speech_Braille")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool Speech_Braille_Windows([MarshalAs(UnmanagedType.LPWStr)] string text);

    [DllImport(DllName, EntryPoint = "Speech_Braille")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool Speech_Braille_Unix(IntPtr textPtr);

    [DllImport(DllName)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool Speech_Stop();

    [DllImport(DllName)]
    private static extern float Speech_Get_Volume();

    [DllImport(DllName)]
    private static extern void Speech_Set_Volume(float offset);

    [DllImport(DllName)]
    private static extern float Speech_Get_Rate();

    [DllImport(DllName)]
    private static extern void Speech_Set_Rate(float offset);

    [DllImport(DllName)]
    private static extern IntPtr Speech_Get_Current_Voice();

    [DllImport(DllName)]
    private static extern IntPtr Speech_Get_Voice(int index);

    [DllImport(DllName)]
    private static extern void Speech_Set_Voice(int index);

    [DllImport(DllName)]
    private static extern int Speech_Get_Voices();

    [DllImport(DllName, EntryPoint = "Speech_Output_File")]
    private static extern void Speech_Output_File_Windows([MarshalAs(UnmanagedType.LPStr)] string filePath, [MarshalAs(UnmanagedType.LPWStr)] string text);

    [DllImport(DllName, EntryPoint = "Speech_Output_File")]
    private static extern void Speech_Output_File_Unix([MarshalAs(UnmanagedType.LPStr)] string filePath, IntPtr textPtr);

    [DllImport(DllName)]
    private static extern void Speech_Resume();

    [DllImport(DllName)]
    private static extern void Speech_Pause();

    [DllImport(DllName)]
    private static extern void Speech_Prefer_Sapi([MarshalAs(UnmanagedType.Bool)] bool prefer_sapi);

    public SpeechCore()
    {
        Speech_Init();
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposed)
        {
            Speech_Free();
            disposed = true;
        }
    }

    ~SpeechCore()
    {
        Dispose(false);
    }

    private IntPtr StringToWChar(string text)
    {
        if (string.IsNullOrEmpty(text))
            return IntPtr.Zero;

        byte[] bytes;
        if (IsWindows)
        {

            bytes = Encoding.Unicode.GetBytes(text + "\0");
        }
        else
        {
            bytes = Encoding.UTF32.GetBytes(text + "\0");
        }

        IntPtr ptr = Marshal.AllocHGlobal(bytes.Length);
        Marshal.Copy(bytes, 0, ptr, bytes.Length);
        return ptr;
    }

    
    private string WCharToString(IntPtr ptr)
    {
        if (ptr == IntPtr.Zero)
            return null;

        if (IsWindows)
        {
            return Marshal.PtrToStringUni(ptr);
        }
        else
        {
            var bytes = new List<byte>();
            int offset = 0;

            while (true)
            {
                byte[] charBytes = new byte[4];
                Marshal.Copy(ptr + offset, charBytes, 0, 4);

                if (charBytes[0] == 0 && charBytes[1] == 0 && charBytes[2] == 0 && charBytes[3] == 0)
                    break;

                bytes.AddRange(charBytes);
                offset += 4;
            }

            if (bytes.Count == 0)
                return string.Empty;

            return Encoding.UTF32.GetString(bytes.ToArray());
        }
    }

    public void DetectDriver() => Speech_Detect_Driver();

    public string CurrentDriver() => WCharToString(Speech_Current_Driver());

    public string GetDriver(int index) => WCharToString(Speech_Get_Driver(index));

    public void SetDriver(int index) => Speech_Set_Driver(index);

    public int GetDrivers() => Speech_Get_Drivers();

    public uint GetSpeechFlags() => Speech_Get_Flags();

    public bool IsLoaded() => Speech_Is_Loaded();

    public bool IsSpeaking() => Speech_Is_Speaking();

    public bool Speak(string text, bool interrupt = false)
    {
        if (string.IsNullOrEmpty(text))
            return false;

        if (IsWindows)
        {
            return Speech_Output_Windows(text, interrupt);
        }
        else
        {
            IntPtr textPtr = StringToWChar(text);
            try
            {
                return Speech_Output_Unix(textPtr, interrupt);
            }
            finally
            {
                if (textPtr != IntPtr.Zero)
                    Marshal.FreeHGlobal(textPtr);
            }
        }
    }

    public bool Braille(string text)
    {
        if (string.IsNullOrEmpty(text))
            return false;

        if (IsWindows)
        {
            return Speech_Braille_Windows(text);
        }
        else
        {
            IntPtr textPtr = StringToWChar(text);
            try
            {
                return Speech_Braille_Unix(textPtr);
            }
            finally
            {
                if (textPtr != IntPtr.Zero)
                    Marshal.FreeHGlobal(textPtr);
            }
        }
    }

    public bool Stop() => Speech_Stop();

    public float GetVolume() => Speech_Get_Volume();

    public void SetVolume(float volume) => Speech_Set_Volume(volume);

    public float GetRate() => Speech_Get_Rate();

    public void SetRate(float rate) => Speech_Set_Rate(rate);

    public string GetCurrentVoice() => WCharToString(Speech_Get_Current_Voice());

    public string GetVoice(int index) => WCharToString(Speech_Get_Voice(index));

    public void SetVoice(int index) => Speech_Set_Voice(index);

    public int GetVoiceCount() => Speech_Get_Voices();

    public void OutputToFile(string filePath, string text)
    {
        if (IsWindows)
        {
            Speech_Output_File_Windows(filePath, text);
        }
        else
        {
            IntPtr textPtr = StringToWChar(text);
            try
            {
                Speech_Output_File_Unix(filePath, textPtr);
            }
            finally
            {
                if (textPtr != IntPtr.Zero)
                    Marshal.FreeHGlobal(textPtr);
            }
        }
    }

    public void Resume() => Speech_Resume();

    public void Pause() => Speech_Pause();

    public bool CheckSpeechFlags(uint flags) => (GetSpeechFlags() & flags) != 0;

    public void PreferSapi(bool preferSapi) => Speech_Prefer_Sapi(preferSapi);
}