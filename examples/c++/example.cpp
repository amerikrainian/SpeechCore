#include <iostream>
#include <string>
#include "SpeechCore.h"

const std::wstring PHRASE = L"Some text";

class SpeechCoreWrapper {
public:
    SpeechCoreWrapper() {
        Speech_Init();
    }

    ~SpeechCoreWrapper() {
        Speech_Free();
    }

    void setRate(float rate) {
        Speech_Set_Rate(rate);
        std::wcout << L"Speaking rate set to " << Speech_Get_Rate() << std::endl;
        Speech_Output(PHRASE.c_str(), false);
    }

    void setVolume(float volume) {
        Speech_Set_Volume(volume);
        std::wcout << L"Speaking volume set to " << Speech_Get_Volume() << std::endl;
        Speech_Output(PHRASE.c_str(), false);
    }

    void getVoices() {
        int voiceCount = Speech_Get_Voices();
        std::wcout << L"Number of voices: " << voiceCount << std::endl;
        for (int i = 0; i < voiceCount; i++) {
            std::wcout << L"Voice: " << Speech_Get_Voice(i) << std::endl;
        }
    }

    void setVoice(int index) {
        std::wcout << L"Setting voice to " << Speech_Get_Voice(index) << std::endl;
        Speech_Set_Voice(index);
    }

    void testFlags() {
        uint32_t flags = Speech_Get_Flags();
        std::wcout << L"Braille: " << ((flags & SC_HAS_BRAILLE) ? L"true" : L"false") << std::endl;
        std::wcout << L"Speech: " << ((flags & SC_HAS_SPEECH) ? L"true" : L"false") << std::endl;
    }

    void speak(const std::wstring& text, bool interrupt = false) {
        Speech_Output(text.c_str(), interrupt);
    }
};

int main() {
    SpeechCoreWrapper speech;

    std::wcout.imbue(std::locale(""));

    std::wstring choice;
    while (true) {
        std::wcout << L"SpeechCore example\nSelect an option\n";
        std::wcout << L"1: Speak a phrase\n";
        std::wcout << L"2: Speak phrase with low volume\n";
        std::wcout << L"3: Speak a phrase with fast rate\n";
        std::wcout << L"4: List available voices\n";
        std::wcout << L"5: Change voice\n";
        std::wcout << L"6: Test speech flags\n";
        std::wcout << L"7: Exit\n";
        std::wcout << L"Type an option: ";

        std::getline(std::wcin, choice);

        if (choice == L"1") {
            speech.speak(PHRASE);
        } else if (choice == L"2") {
            speech.setVolume(43.0f);
        } else if (choice == L"3") {
            speech.setRate(89.0f);
        } else if (choice == L"4") {
            speech.getVoices();
        } else if (choice == L"5") {
            speech.setVoice(1);
        } else if (choice == L"6") {
            speech.testFlags();
        } else if (choice == L"7") {
            std::wcout << L"Exiting\n";
            break;
        } else {
            std::wcout << L"Unknown option\n";
        }
    }

    return 0;
}