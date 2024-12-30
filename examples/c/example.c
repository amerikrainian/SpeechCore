#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <wchar.h>
#include "SpeechCore.h"

#define PHRASE L"Some text"

void set_rate(float rate) {
    Speech_Set_Rate(rate);
    printf("Speaking rate set to %f\n", Speech_Get_Rate());
    Speech_Output(PHRASE, false);
}

void set_volume(float volume) {
    Speech_Set_Volume(volume);
    printf("Speaking volume set to %f\n", Speech_Get_Volume());
    Speech_Output(PHRASE, false);
}

void get_voices() {
    int voice_count = Speech_Get_Voices();
    printf("Number of voices: %d\n", voice_count);
    for (int i = 0; i < voice_count; i++) {
        printf("Voice: %ls\n", Speech_Get_Voice(i));
    }
}

void set_voice(int index) {
    printf("Setting voice to %ls\n", Speech_Get_Voice(index));
    Speech_Set_Voice(index);
}

void test_flags() {
    uint32_t flags = Speech_Get_Flags();
    printf("Braille: %s\nSpeech: %s\n",
           (flags & SC_HAS_BRAILLE) ? "true" : "false",
           (flags & SC_HAS_SPEECH) ? "true" : "false");
}

int main() {
    Speech_Init();

    char choice[10];
    while (1) {
        printf("SpeechCore example\nSelect an option\n");
        printf("1: Speak a phrase\n");
        printf("2: Speak phrase with low volume\n");
        printf("3: Speak a phrase with fast rate\n");
        printf("4: List available voices\n");
        printf("5: Change voice\n");
        printf("6: Test speech flags\n");
        printf("7: Exit\n");
        printf("Type an option: ");

        fgets(choice, sizeof(choice), stdin);
        choice[strcspn(choice, "\n")] = 0; // Remove newline

        if (strcmp(choice, "1") == 0) {
            Speech_Output(PHRASE, false);
        } else if (strcmp(choice, "2") == 0) {
            set_volume(43.0f);
        } else if (strcmp(choice, "3") == 0) {
            set_rate(89.0f);
        } else if (strcmp(choice, "4") == 0) {
            get_voices();
        } else if (strcmp(choice, "5") == 0) {
            set_voice(1);
        } else if (strcmp(choice, "6") == 0) {
            test_flags();
        } else if (strcmp(choice, "7") == 0) {
            printf("Exiting\n");
            break;
        } else {
            printf("Unknown option\n");
        }
    }

    Speech_Free();
    return 0;
}