# SpeechCore API Documentation

## Overview

SpeechCore is a cross-platform Python wrapper for screen reader and text-to-speech functionality. It provides a unified interface for speech synthesis, voice control, and braille output across different operating systems. The library supports multiple speech drivers and includes Windows-specific SAPI (Speech API) functionality.

## Installation

```bash
pip install speechcore
```

## Classes

### SpeechCore

Main class for speech synthesis and screen reader functionality. Only one instance may exist at a time.

**Context Manager Support**: Can be used with `with` statement for automatic initialization and cleanup.

```python
with SpeechCore() as speech:
    speech.output("Hello, world!")
```

### Sapi

Windows-only class providing direct access to Microsoft's Speech API (SAPI). Only available on Windows platforms.

**Context Manager Support**: Can be used with `with` statement for automatic initialization and cleanup.

```python
# Windows only
with Sapi() as sapi:
    sapi.speak("Hello from SAPI!")
```

## SpeechCore Methods

### Initialization and Management

#### `SpeechCore.init()`
Initialize the SpeechCore library.

- **Returns**: None
- **Raises**: `InitializationError` if initialization fails

#### `SpeechCore.free()`
Free resources and shutdown the library.

- **Returns**: None

#### `SpeechCore.is_loaded() -> bool`
Check if SpeechCore is currently loaded and initialized.

- **Returns**: `bool` - True if loaded, False otherwise

### Driver Management

#### `detect_driver() -> None`
Automatically detect and set the best available speech driver.

- **Returns**: None

#### `get_driver(index: int) -> str`
Get the name of a speech driver by index.

- **Parameters**: 
  - `index` (int): Driver index
- **Returns**: `str` - Driver name

#### `current_driver() -> str`
Get the name of the currently active speech driver.

- **Returns**: `str` - Current driver name

#### `set_driver(index: int) -> None`
Set the active speech driver by index.

- **Parameters**: 
  - `index` (int): Driver index to activate
- **Returns**: None

#### `get_drivers() -> int`
Get the total number of available speech drivers.

- **Returns**: `int` - Number of available drivers

### Voice Management

#### `get_voice(index: int) -> str`
Get the name of a voice by index.

- **Parameters**: 
  - `index` (int): Voice index
- **Returns**: `str` - Voice name

#### `get_current_voice() -> str`
Get the name of the currently active voice.

- **Returns**: `str` - Current voice name

#### `set_voice(index: int) -> None`
Set the active voice by index.

- **Parameters**: 
  - `index` (int): Voice index to activate
- **Returns**: None

#### `get_voices() -> int`
Get the total number of available voices.

- **Returns**: `int` - Number of available voices

### Audio Control

#### `set_volume(offset: float) -> None`
Set the speech volume level.

- **Parameters**: 
  - `offset` (float): Volume level (typically 0.0 to 1.0)
- **Returns**: None

#### `get_volume() -> float`
Get the current speech volume level.

- **Returns**: `float` - Current volume level

#### `set_rate(offset: float) -> None`
Set the speech rate (speed).

- **Parameters**: 
  - `offset` (float): Speech rate value
- **Returns**: None

#### `get_rate() -> float`
Get the current speech rate.

- **Returns**: `float` - Current speech rate

### Speech Output

#### `output(text: str, interrupt: bool = False) -> bool`
Speak the given text.

- **Parameters**: 
  - `text` (str): Text to speak
  - `interrupt` (bool): Whether to interrupt current speech (default: False)
- **Returns**: `bool` - True if successful

#### `output_braille(text: str) -> bool`
Output text to braille display.

- **Parameters**: 
  - `text` (str): Text to output to braille
- **Returns**: `bool` - True if successful

#### `output_file(filename: str, text: str) -> None`
Save speech output to an audio file.

- **Parameters**: 
  - `filename` (str): Output file path
  - `text` (str): Text to convert to audio
- **Returns**: None

### Playback Control

#### `resume() -> None`
Resume paused speech.

- **Returns**: None

#### `pause() -> None`
Pause current speech.

- **Returns**: None

#### `stop() -> None`
Stop current speech immediately.

- **Returns**: None

#### `is_speaking() -> bool`
Check if speech is currently active.

- **Returns**: `bool` - True if speaking, False otherwise

### System Information

#### `get_speech_flags() -> int`
Get current speech system capability flags.

- **Returns**: `int` - Bitfield of capability flags

#### `check_speech_flags(flags: int) -> bool`
Check if specific speech flags are set.

- **Parameters**: 
  - `flags` (int): Flags to check
- **Returns**: `bool` - True if flags are set

## Sapi Methods (Windows Only)

### Initialization

#### `Sapi.init()`
Initialize the SAPI interface.

- **Returns**: None
- **Raises**: `InitializationError` if initialization fails
- **Raises**: `NotImplementedError` on non-Windows platforms

#### `Sapi.release()`
Release SAPI resources.

- **Returns**: None

#### `Sapi.sapi_loaded() -> bool`
Check if SAPI is currently loaded.

- **Returns**: `bool` - True if loaded, False otherwise

### Voice Control

#### `voice_set_rate(offset: float) -> None`
Set SAPI voice speech rate.

- **Parameters**: 
  - `offset` (float): Speech rate value
- **Returns**: None

#### `voice_get_rate() -> float`
Get current SAPI voice speech rate.

- **Returns**: `float` - Current speech rate

#### `voice_set_volume(offset: float) -> None`
Set SAPI voice volume level.

- **Parameters**: 
  - `offset` (float): Volume level
- **Returns**: None

#### `voice_get_volume() -> float`
Get current SAPI voice volume level.

- **Returns**: `float` - Current volume level

#### `get_voice(index: int) -> str`
Get SAPI voice name by index.

- **Parameters**: 
  - `index` (int): Voice index
- **Returns**: `str` - Voice name

#### `get_current_voice() -> str`
Get current SAPI voice name.

- **Returns**: `str` - Current voice name

#### `set_voice_by_index(index: int) -> None`
Set SAPI voice by index.

- **Parameters**: 
  - `index` (int): Voice index
- **Returns**: None

#### `set_voice(voice_name: str) -> None`
Set SAPI voice by name.

- **Parameters**: 
  - `voice_name` (str): Name of voice to set
- **Returns**: None

#### `get_voices() -> int`
Get number of available SAPI voices.

- **Returns**: `int` - Number of voices

### SAPI Speech Output

#### `speak(text: str, interrupt: bool = False, xml: bool = False) -> None`
Speak text using SAPI.

- **Parameters**: 
  - `text` (str): Text to speak
  - `interrupt` (bool): Whether to interrupt current speech (default: False)
  - `xml` (bool): Whether text contains SSML markup (default: False)
- **Returns**: None

#### `output_file(filename: str, text: str, xml: bool = False) -> None`
Save SAPI speech to audio file.

- **Parameters**: 
  - `filename` (str): Output file path
  - `text` (str): Text to convert
  - `xml` (bool): Whether text contains SSML markup (default: False)
- **Returns**: None

### SAPI Playback Control

#### `resume() -> None`
Resume paused SAPI speech.

- **Returns**: None

#### `pause() -> None`
Pause current SAPI speech.

- **Returns**: None

#### `stop() -> None`
Stop current SAPI speech.

- **Returns**: None

## Constants

Speech capability flags available from `__speech_common`:

- `SC_SPEECH_FLOW_CONTROL` - Speech flow control support
- `SC_SPEECH_PARAMETER_CONTROL` - Speech parameter control support  
- `SC_VOICE_CONFIG` - Voice configuration support
- `SC_FILE_OUTPUT` - File output support
- `SC_HAS_SPEECH` - Speech synthesis support
- `SC_HAS_BRAILLE` - Braille output support
- `SC_HAS_SPEECH_STATE` - Speech state monitoring support

## Exceptions

### `InitializationError`
Raised when SpeechCore or SAPI initialization fails.

### `NotLoadedError`
Raised when attempting to use methods before proper initialization.

## Example Usage

```python
from speechcore import SpeechCore

# Basic usage with context manager
with SpeechCore() as speech:
    speech.output("Hello, this is SpeechCore!")
    
    # Check available voices
    voice_count = speech.get_voices()
    for i in range(voice_count):
        print(f"Voice {i}: {speech.get_voice(i)}")
    
    # Adjust speech parameters
    speech.set_rate(0.5)  # Slower speech
    speech.set_volume(0.8)  # Lower volume
    
    speech.output("This is slower and quieter speech.")

# Windows-specific SAPI usage
if sys.platform == "win32":
    from speechcore import Sapi
    
    with Sapi() as sapi:
        sapi.speak("Hello from Windows SAPI!")
```