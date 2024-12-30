import java.util.Scanner;

public class SpeechCoreExample {
    // Variables and function defines
    private static final String phrase = "Some text";

    private static final String menuStr = """
            1: Speak a phrase
            2: Speak phrase with low volume
            3: Speak a phrase with fast rate
            4: List available voices
            5: Change voice
            6: Test speech flags
            7: Exit
            """;

    private static void setRate(SpeechCore speech, float rate) {
        speech.setRate(rate);
        System.out.printf("Speaking rate set to %f%n", speech.getRate());
        speech.speak(phrase, false);
    }

    private static void setVolume(SpeechCore speech, float volume) {
        speech.setVolume(volume);
        System.out.printf("Speaking volume set to %f%n", speech.getVolume());
        speech.speak(phrase, false);
    }

    private static void getVoices(SpeechCore speech) {
        int voiceCount = speech.getVoiceCount();
        System.out.printf("Number of voices: %d%n", voiceCount);
        for (int i = 0; i < voiceCount; i++) {
            System.out.printf("Voice: %s%n", speech.getVoice(i));
        }
    }

    private static void setVoice(SpeechCore speech, int index) {
        System.out.printf("Setting voice to %s%n", speech.getVoice(index));
        speech.setVoice(index);
    }

    private static void testFlags(SpeechCore speech) {
        boolean brailleFlag = speech.checkSpeechFlags(SpeechCore.SC_HAS_BRAILLE);
        boolean speechFlag = speech.checkSpeechFlags(SpeechCore.SC_HAS_SPEECH);
        System.out.printf("Braille: %b%nSpeech: %b%n", brailleFlag, speechFlag);
    }

    public static void main(String[] args) {
        try (SpeechCore speech = new SpeechCore(); Scanner scanner = new Scanner(System.in)) {
            while (true) {
                System.out.println("SpeechCore example\nSelect an option");
                System.out.println(menuStr);
                System.out.print("Type an option: ");
                String choice = scanner.nextLine();

                switch (choice) {
                    case "1":
                        speech.speak(phrase, false);
                        break;
                    case "2":
                        setVolume(speech, 43);
                        break;
                    case "3":
                        setRate(speech, 89);
                        break;
                    case "4":
                        getVoices(speech);
                        break;
                    case "5":
                        setVoice(speech, 1);
                        break;
                    case "6":
                        testFlags(speech);
                        break;
                    case "7":
                        System.out.println("Exiting");
                        return;
                    default:
                        System.out.println("Unknown option");
                        break;
                }
            }
        }
    }
}