/*
 * bot.h
 * 
 * Copyright 2015 Knoxy
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

// Define terminal colors
#define KNRM  "\x1B[0m"
#define KRED  "\x1B[31m"
#define KGRN  "\x1B[32m"
#define KYEL  "\x1B[33m"
#define KBLU  "\x1B[34m"
#define KMAG  "\x1B[35m"
#define KCYN  "\x1B[36m"
#define KWHT  "\x1B[37m"

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

// Define function: findChart
int findChart(char *sTitle)
{
	bool 		  titleFound;
	DIR           *d;
    struct dirent *dir;
    d = opendir("/home/pi/bot/charts/");
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
		printf("%i notes total.\n%sFinished loading chart.\n\n%s", noteCount, KGRN, KNRM);
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


/*	~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
 * 				TERMINAL CONTROL / RAW INPUT
 *  ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
 */ 
static struct termios   save_termios;
static int              term_saved;

int tty_raw(int fd) {       /* RAW! mode */
    struct termios  buf;

    if (tcgetattr(fd, &save_termios) < 0) /* get the original state */
        return -1;

    buf = save_termios;

    buf.c_lflag &= ~(ECHO | ICANON | IEXTEN | ISIG);
                    /* echo off, canonical mode off, extended input
                       processing off, signal chars off */

    buf.c_iflag &= ~(BRKINT | ICRNL | ISTRIP | IXON);
                    /* no SIGINT on BREAK, CR-toNL off, input parity
                       check off, don't strip the 8th bit on input,
                       ouput flow control off */

    buf.c_cflag &= ~(CSIZE | PARENB);
                    /* clear size bits, parity checking off */

    buf.c_cflag |= CS8;
                    /* set 8 bits/char */

    buf.c_oflag &= ~(OPOST);
                    /* output processing off */

    buf.c_cc[VMIN] = 1;  /* 1 byte at a time */
    buf.c_cc[VTIME] = 0; /* no timer on input */

    if (tcsetattr(fd, TCSAFLUSH, &buf) < 0)
        return -1;

    term_saved = 1;

    return 0;
}


int tty_reset(int fd) { /* set it to normal! */
    if (term_saved)
        if (tcsetattr(fd, TCSAFLUSH, &save_termios) < 0)
            return -1;

    return 0;
}




/*	~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
 * 					HOME SCREEN FUNCTIONS
 *  ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
 */ 

void showLogo() {
	system("clear");
	printf("%s", KNRM);
	printf("\n ██╗  █████╗   ██╗██████╗██╗  ████╗   ██╗");
	printf("\n ██║ ██╔████╗  ████╔═══██╚██╗██╔╚██╗ ██╔╝");
	printf("\n █████╔╝██╔██╗ ████║   ██║╚███╔╝ ╚████╔╝ ");
	printf("\n ██╔═██╗██║╚██╗████║   ██║██╔██╗  ╚██╔╝  ");
	printf("\n ██║  ████║ ╚████╚██████╔██╔╝ ██╗  ██║   ");
	printf("\n ╚═╝  ╚═╚═╝  ╚═══╝╚═════╝╚═╝  ╚═╝  ╚═╝   ");
	printf("\n RB Bot v1.3                03.05.2015 \n");
	printf("\n ------------------------------------- \n");
} 
 
int homeScreen() {
	showLogo();
	if(strlen(songTitle) == 0) {
		printf("\n %s1.)  Select Chart", KYEL);
	} else {
		printf("\n %sChart loaded: %s", KGRN, songTitle);
	}
	
	printf("\n %s2.)  Navigate", KYEL);
	printf("\n %s3.)  Start Bot", KYEL);
	printf("\n %s4.)  Exit \n", KYEL);
	int option;
	// Enter raw mode to wait for user input
	tty_raw(0);
	char userKey;
	while( (option = read(0, &userKey, 1)) == 1) {
		if( (userKey &= 255) == 0061) {
			option = 1;
			break;
		} else if( (userKey &= 255) == 0062) {
			option = 2;
			break;
		} else if( (userKey &= 255) == 0063) {
			option = 3;
			break;
		} else if( (userKey &= 255) == 0064) {
			option = 4;
			break;
		} else {
			printf("%sInvalid option!\n\r", KRED);
		}
	}
	// Exit raw mode after user input has been captured
	tty_reset(0);
	
	return option;
}

// Define function: userNavigate
void userNavigate() {
	int reading;
	char userKey;
	
	// Allow user to navigate
	showLogo();
	printf("\n NAVIGATION MODE \n");
	printf("\n %s[Backspace] \t %sExit navigation mode.", KMAG, KYEL);
	printf("\n %s[Up] (U) \t %sMove up. (upstrum)", KMAG, KYEL);
	printf("\n %s[Down] (D) \t %sMove down. (downstrum)", KMAG, KYEL);
	printf("\n %s[Enter] \t %sSelect. (green press)", KMAG, KYEL);
	printf("\n %s[B] \t %sBack. (red press)", KMAG, KYEL);
	printf("\n %s[Y] \t %sYellow press.", KMAG, KYEL);
	printf("\n%s", KNRM);
	if(tty_raw(0) < 0) {
		fprintf(stderr,"Can't go to raw mode.\n");
		exit(1);
	}
	while( (reading = read(0, &userKey, 1)) == 1) {
		// Backspace
		if( (userKey &= 255) == 0177) {
			printf("\n\r%sDone navigating!\n\n\r", KGRN);
			break;
		}
		//printf("\n\r Output: %o", userKey);
		if((userKey &=255) == 0101 || (userKey &=255) == 0165) {
			// Up Arrow / U
			gtrStrum('U');
			printf("Up\n\r");
		} else if((userKey &=255) == 0102 || (userKey &=255) == 0144) {
			// Down Arrow / D
			gtrStrum('D');
			printf("Down\n\r");
			
		} else if((userKey &=255) == 0142) {
			// B
			printf("Back\n\r");
			pressButton('R', 1);
			delay(100);
			pressButton('R', 0);
		} else if((userKey &=255) == 0015) {
			// Enter
			printf("Select\n\r");
			pressButton('G', 1);
			delay(100);
			pressButton('G', 0);
		} else if((userKey &=255) == 0171) {
			// Y
			printf("Yellow\n\r");
			pressButton('Y', 1);
			delay(100);
			pressButton('Y', 0);
		}
	}
	if(tty_reset(0) < 0) {
		fprintf(stderr, "Cannot reset terminal!\n");
		exit(-1);
	}
	
	printf("\n%sPress Enter to return to main menu...", KNRM);
	getchar();
}
