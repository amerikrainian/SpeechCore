# SpeechCore

SpeechCore is a cross-platform C++ library that abstracts the process of communicating with various screen readers, managing low-level details and providing a clean, simple-to-use interface.

## Features

* Simple and intuitive API with straightforward usage
* Cross-platform support for various screen readers, with the option to support additional ones

## Installation

Install via NuGet Package Manager:

```bash
dotnet add package SpeechCore.CrossPlatform
```

Or via Package Manager Console in Visual Studio:

```powershell
Install-Package SpeechCore.CrossPlatform
```

## Library Notes

* Windows screen readers (NVDA, JAWS, Zhengdu, and System Access) require specific binaries, included with the package.
* Enhanced control over SAPI 5, including voice configuration and speech parameters.
* Braille output functionality is included, implemented for both NVDA and Jaws.

## Supported Screen Readers

* Windows: NVDA, JAWS, System Access, Zhengdu Screen Reader, SAPI 5
* macOS: AVSpeech
* Linux: Speech Dispatcher




## Usage

Simple usage example:

```csharp
using System;

class Program
{
    static void Main()
    {
        // Initialize the speech engine
        using var speech = new SpeechCore();
        
        
        if (speech.IsLoaded())
        {
            Console.WriteLine($"Current screen reader: {speech.CurrentDriver()}");
            Console.WriteLine("Speaking some text...");
            
            // Speak some text
            speech.Speak("This is a test for the SpeechCore library. If you're hearing this, it indicates the library is functioning properly.");
            
            // Wait for speech to complete. Useful for Sapi.
            while (speech.IsSpeaking())
            {
                System.Threading.Thread.Sleep(100);
            }
            
            Console.WriteLine("Speech completed!");
        }
        else
        {
            Console.WriteLine("No speech engines available.");
        }
        

    }
}
```


# API Documentation

## SpeechCore Class

The main class that provides access to speech synthesis and screen reader functionality.

### Constructor and Disposal

#### `SpeechCore()`
Initializes a new instance of the SpeechCore class and calls the native Speech_Init() function.

#### `Dispose()`
Releases all resources used by the SpeechCore instance. Automatically called when using `using` statements.

### Core Methods

#### `void DetectDriver()`
Detects and loads available speech drivers/screen readers on the current system.

#### `bool IsLoaded()`
Returns `true` if a speech driver is successfully loaded and ready for use.

#### `bool IsSpeaking()`
Returns `true` if the speech engine is currently speaking text.

#### `bool Speak(string text, bool interrupt = false)`
Speaks the specified text.
- **Parameters:**
  - `text`: The text to speak
  - `interrupt`: If `true`, interrupts any currently speaking text
- **Returns:** `true` if the speech was successfully initiated

#### `bool Stop()`
Stops any currently playing speech.
- **Returns:** `true` if speech was successfully stopped

#### `void Pause()`
Pauses the current speech playback.

#### `void Resume()`
Resumes paused speech playback.

### Driver Management

#### `string CurrentDriver()`
Gets the name of the currently active speech driver.
- **Returns:** Name of the current driver, or `null` if none is loaded

#### `int GetDrivers()`
Gets the total number of available speech drivers.
- **Returns:** Count of available drivers

#### `string GetDriver(int index)`
Gets the name of a specific speech driver by index.
- **Parameters:**
  - `index`: Zero-based index of the driver
- **Returns:** Name of the driver at the specified index

#### `void SetDriver(int index)`
Sets the active speech driver to the one at the specified index.
- **Parameters:**
  - `index`: Zero-based index of the driver to activate

### Speech Parameters

#### `float GetVolume()`
Gets the current speech volume level.
- **Returns:** Current volume (typically 0.0 to 1.0)

#### `void SetVolume(float volume)`
Sets the speech volume level.
- **Parameters:**
  - `volume`: Volume level (typically 0.0 to 1.0)

#### `float GetRate()`
Gets the current speech rate.
- **Returns:** Current speech rate

#### `void SetRate(float rate)`
Sets the speech rate.
- **Parameters:**
  - `rate`: Speech rate (1.0 is normal, higher values are faster)

### Voice Management

#### `string GetCurrentVoice()`
Gets the name of the currently selected voice.
- **Returns:** Name of the current voice

#### `int GetVoiceCount()`
Gets the total number of available voices.
- **Returns:** Count of available voices

#### `string GetVoice(int index)`
Gets the name of a specific voice by index.
- **Parameters:**
  - `index`: Zero-based index of the voice
- **Returns:** Name of the voice at the specified index

#### `void SetVoice(int index)`
Sets the active voice to the one at the specified index.
- **Parameters:**
  - `index`: Zero-based index of the voice to activate

### Braille Support

#### `bool Braille(string text)`
Sends text to a braille display (if supported by the current driver).
- **Parameters:**
  - `text`: Text to display in braille
- **Returns:** `true` if the text was successfully sent to braille display

### File Output

#### `void OutputToFile(string filePath, string text)`
Saves speech synthesis to an audio file (if supported by the current driver).
- **Parameters:**
  - `filePath`: Path where the audio file should be saved
  - `text`: Text to synthesize and save

### Capability Checking

#### `uint GetSpeechFlags()`
Gets the capability flags of the current speech driver.
- **Returns:** Bitwise combination of capability flags

#### `bool CheckSpeechFlags(uint flags)`
Checks if the current driver supports specific capabilities.
- **Parameters:**
  - `flags`: Capability flags to check (use SC_* constants)
- **Returns:** `true` if all specified capabilities are supported

### Advanced Configuration

#### `void PreferSapi(bool preferSapi)`
Sets preference for SAPI (Speech API) on Windows systems.
- **Parameters:**
  - `preferSapi`: `true` to prefer SAPI over other drivers

## Constants

### Capability Flags

- `SC_SPEECH_FLOW_CONTROL` - Supports speech flow control (pause/resume)
- `SC_SPEECH_PARAMETER_CONTROL` - Supports speech parameter control (volume/rate)
- `SC_VOICE_CONFIG` - Supports voice configuration
- `SC_FILE_OUTPUT` - Supports file output
- `SC_HAS_SPEECH` - Has speech synthesis capability
- `SC_HAS_BRAILLE` - Has braille output capability
- `SC_HAS_SPEECH_STATE` - Can report speech state

## Error Handling

The library uses return values to indicate success or failure:
- Methods returning `bool` indicate success (`true`) or failure (`false`)
- Methods returning strings may return `null` if the operation fails
- Always check `IsLoaded()` before attempting speech operations

## Thread Safety

The SpeechCore class is not thread-safe. If you need to use it from multiple threads, implement appropriate synchronization mechanisms.

## Platform Requirements

- **Windows**: Windows 7 or later (both x86 and x64 supported)
- **macOS**: macOS 10.12 or later (Intel and Apple Silicon)
- **Linux**: Most modern distributions (x64 and ARM64)

## Native Dependencies

This package includes native libraries and their dependencies for all supported platforms:

### Windows (x86 & x64):
- `SpeechCore.dll` - Main speech library
- Visual C++ Runtime libraries (if required)
- Additional Windows-specific dependencies

### macOS:
- `libSpeechCore.dylib` - macOS dynamic library

### Linux:
- `libSpeechCore.so` - Linux shared library

The native libraries and all their dependencies are automatically copied to your output directory and included in published applications.

## License

[MIT License](LICENSE)