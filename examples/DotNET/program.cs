using System;

class SpeechCoreExample
{
    // Variables and function defines
    static string phrase = "Some text";

    static string menuStr = @"
1: Speak a phrase
2: Speak phrase with low volume
3: Speak a phrase with fast rate
4: List available voices
5: Change voice
6: Test speech flags
7: Exit
";

    static void SetRate(SpeechCore speech, float rate)
    {
        speech.SetRate(rate);
        Console.WriteLine($"Speaking rate set to {speech.GetRate()}");
        speech.Speak(phrase, false);
    }

    static void SetVolume(SpeechCore speech, float volume)
    {
        speech.SetVolume(volume);
        Console.WriteLine($"Speaking volume set to {speech.GetVolume()}");
        speech.Speak(phrase, false);
    }

    static void GetVoices(SpeechCore speech)
    {
        int voiceCount = speech.GetVoiceCount();
        Console.WriteLine($"Number of voices: {voiceCount}");
        for (int i = 0; i < voiceCount; i++)
        {
            Console.WriteLine($"Voice: {speech.GetVoice(i)}");
        }
    }

    static void SetVoice(SpeechCore speech, int index)
    {
        Console.WriteLine($"Setting voice to {speech.GetVoice(index)}");
        speech.SetVoice(index);
    }

    static void TestFlags(SpeechCore speech)
    {
        bool brailleFlag = speech.CheckSpeechFlags(SpeechCore.SC_HAS_BRAILLE);
        bool speechFlag = speech.CheckSpeechFlags(SpeechCore.SC_HAS_SPEECH);
        Console.WriteLine($"Braille: {brailleFlag}\nSpeech: {speechFlag}");
    }

    static void Main(string[] args)
    {
        using (var speech = new SpeechCore())
        {
            while (true)
            {
                Console.WriteLine("SpeechCore example\nSelect an option");
                Console.WriteLine(menuStr);
                Console.Write("Type an option: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        speech.Speak(phrase, false);
                        break;
                    case "2":
                        SetVolume(speech, 43);
                        break;
                    case "3":
                        SetRate(speech, 89);
                        break;
                    case "4":
                        GetVoices(speech);
                        break;
                    case "5":
                        SetVoice(speech, 1);
                        break;
                    case "6":
                        TestFlags(speech);
                        break;
                    case "7":
                        Console.WriteLine("Exiting");
                        return;
                    default:
                        Console.WriteLine("Unknown option");
                        break;
                }
            }
        }
    }
}