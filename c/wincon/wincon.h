/*
 * wincon.h
 *
 * Copyright 2015-2018 Knoxy (Tyler Knox)
 * Last modified 7/21/2018
*/


#include <stdio.h>
#include <termios.h>
#include <dirent.h>
#include <string.h>
#include <stdlib.h>
#include <assert.h>
#include <unistd.h>
#include <signal.h>
#include <sys/time.h>
#include <wiringPi.h>


// Define boolean type
typedef int bool;
enum { false, true };

// Define note structure
struct Note
{
        float ms;
        float duration;
        char color;
        char action;
        int hit;
};

// Define global variables
struct Note chart_notes[20000];
int noteCount = 0;
double bot_finalMs;
double bot_startTime;
double bot_elapsedTime;
int bot_notesHit = 0;
char songTitle[50];


// Define function: str_split
char** str_split(char* a_str, const char a_delim)
{
    char** result    = 0;
    size_t count     = 0;
    char* tmp        = a_str;
    char* last_comma = 0;
    char delim[2];
    delim[0] = a_delim;
    delim[1] = 0;

    /* Count how many elements will be extracted. */
    while (*tmp)
    {
        if (a_delim == *tmp)
        {
            count++;
            last_comma = tmp;
        }
        tmp++;
    }

    /* Add space for trailing token. */
    count += last_comma < (a_str + strlen(a_str) - 1);

    /* Add space for terminating null string so caller
       knows where the list of returned strings ends. */
    count++;

    result = malloc(sizeof(char*) * count);

    if (result)
    {
        size_t idx  = 0;
        char* token = strtok(a_str, delim);

        while (token)
        {
            assert(idx < count);
            *(result + idx++) = strdup(token);
            token = strtok(0, delim);
        }
        assert(idx == count - 1);
        *(result + idx) = 0;
    }

    return result;
}

// Define function: strpos
int strpos(char *haystack, char *needle)
{
   char *p = strstr(haystack, needle);
   if (p)
      return p - haystack;
   return -1;
}


// Define function: parseNotes
int parseNotes(char *cp)
{
        // Load the chart file
        FILE *chartFile = fopen(cp, "r");
        char line[255];

        // Skip over chart header
        fgets(line, sizeof(line), chartFile);
        fgets(line, sizeof(line), chartFile);
        fgets(line, sizeof(line), chartFile);


        // Import the notes
        int nc;
        for(nc=0; nc < noteCount; nc++) {
                char tim[70];
                fgets(tim, sizeof(line), chartFile);
                char **lineParts = str_split(tim, '-');
                if(lineParts) {
                        // Break down note data
                        sscanf(*(lineParts+0), "%f", &chart_notes[nc].ms);
                        sscanf(*(lineParts + 1), "%c", &chart_notes[nc].color);
                        sscanf(*(lineParts + 2), "%c", &chart_notes[nc].action);
                        sscanf(*(lineParts + 3), "%f", &chart_notes[nc].duration);
                        chart_notes[nc].hit = 0;

                        // Free memory
                        free(*(lineParts+0));
                        free(*(lineParts+1));
                        free(*(lineParts+2));
                        free(*(lineParts+3));
                        free(lineParts);
                }
    }
    fclose(chartFile);

    // Set end of chart
    bot_finalMs = chart_notes[noteCount-1].ms + chart_notes[noteCount-1].duration;

        return(nc);
}

// Define function: loadChart
int loadChart(char *cTitle, bool cLeet)
{
        bool loaded;

        // Determine absolute chart path
        char chartPath[100];
        if(cLeet == true) {
            strcpy(chartPath, "/home/pi/bot/charts_1337/");
        } else {
            strcpy(chartPath, "/home/pi/bot/charts/");
        }
        strcat(chartPath, cTitle);

        // Load the chart file
        FILE *chartFile = fopen(chartPath, "r");
        int lineCount = 0;
        char line[256];

        // Determine note count
        while (fgets(line, sizeof(line), chartFile)) {
                if(lineCount >= 3) {
                        int isNote = -1;
                        if ((isNote = strpos( line, "-")) != -1) {
                                noteCount++;
                        }
                }
                lineCount++;
    }
    fclose(chartFile);

    // Parse the notes
    int notesParsed = parseNotes(chartPath);
    if(notesParsed == noteCount) {
                loaded = true;
    }

        return(loaded);
}


// Define function: pressButton
void pressButton(char btn, int pressRelease) {
        int pin;

        if(btn == 'G') {
                pin = 4;
        }
        if(btn == 'R') {
                pin = 5;
        }
        if(btn == 'Y') {
                pin = 26;
        }
        if(btn == 'B') {
                pin = 27;
        }
        if(btn == 'O') {
                pin = 25;
        }
        if(btn == 'D') {
                pin = 0;
        }
        if(btn == 'U') {
                pin = 14;
        }

        if(pressRelease > 0) {
                digitalWrite(pin, HIGH);
        } else {
                digitalWrite(pin, LOW);
        }

}

// Define function: gtrStrum
void gtrStrum(char dir) {
        if(dir == 'D') {
                digitalWrite(0, HIGH);
                delay(100);
                digitalWrite(0, LOW);
        } else {
                digitalWrite(14, HIGH);
                delay(100);
                digitalWrite(14, LOW);
        }
}


// Define function: setupButtons
void setupButtons() {
        wiringPiSetup();
        pinMode(0, OUTPUT);
        pinMode(4, OUTPUT);
        pinMode(5, OUTPUT);
        pinMode(14, OUTPUT);
        pinMode(25, OUTPUT);
        pinMode(26, OUTPUT);
        pinMode(27, OUTPUT);
        digitalWrite(0, LOW);
        digitalWrite(4, LOW);
        digitalWrite(5, LOW);
        digitalWrite(14, LOW);
        digitalWrite(25, LOW);
        digitalWrite(26, LOW);
        digitalWrite(27, LOW);
}
