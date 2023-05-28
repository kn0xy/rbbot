/*
 * bot.h
 * 
 * Copyright 2015 Knoxy
*/

#include <stdio.h>
#include <dirent.h>
#include <string.h>
#include <stdlib.h>
#include <assert.h>
#include <unistd.h>

#include <signal.h>
#include <sys/time.h>

typedef int bool;
enum { false, true };

struct Note
{
	float ms;
	float duration;
	char color;
	char action;
	int hit;
};

struct Note chart_notes[20000];

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

int strpos(char *haystack, char *needle)
{
   char *p = strstr(haystack, needle);
   if (p)
      return p - haystack;
   return -1;
}


int findChart(char *sTitle)
{
	bool 		  titleFound;
	DIR           *d;
    struct dirent *dir;
    d = opendir("./charts");
    titleFound = false;
    // Finish the name of the chart
	char *titleAppend = "_guitar_expert.bot";
	char *chartTitle = strcat(sTitle, titleAppend);
    if (d)
    {
      while ((dir = readdir(d)) != NULL)
      {
		  char str1[100];
		  char str2[100];
		  int len;
		  int ret;
		  strcpy(str1, dir->d_name);
		  strcpy(str2, chartTitle);

		  len = strlen(str1);
		  ret = strncmp(str1, str2, len);
		  
		  if(ret == 0) {
			  titleFound = true;
		  }
      }
      closedir(d);
    }
    
    return(titleFound);
}


int noteCount = 0;
double bot_finalMs;
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

int loadChart(char *cTitle)
{
	bool loaded;

	// Determine absolute chart path
	char chartPath[100];
	strcpy(chartPath, "./charts/");
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
		printf("%i notes total.\nFinished loading chart.\n\n", noteCount);
		loaded = true;
    }
	
	return(loaded);
}


double bot_startTime;
double bot_elapsedTime;
int bot_notesHit = 0;
