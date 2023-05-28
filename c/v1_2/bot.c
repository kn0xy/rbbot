/*
 * bot.c
 * 
 * Copyright 2015 Knoxy
 * 
 */

#include "bot.h"

void botStart()
{
	// Start the bot
	struct timeval tms, tmc;
	int n, nub, notesReleased = 0;
	gettimeofday(&tms, NULL);
	bot_startTime = (double) tms.tv_sec + (double) tms.tv_usec / 1000000;
	printf("\nBot started! \n");
	
	// Define Press/Release triggers
	double rg, rr, ry, rb, ro;
	rg = 0;
	rr = 0;
	ry = 0;
	rb = 0;
	ro = 0;

	// Play the chart until all notes have been pressed & released
	while(notesReleased < noteCount) {
		if(bot_notesHit < noteCount) {
			if(bot_notesHit < noteCount - 3) {
				nub = bot_notesHit + 3;
			} else if(bot_notesHit < noteCount - 2) {
				nub = bot_notesHit + 2;
			} else {
				nub = bot_notesHit + 1;
			}
			for(n=bot_notesHit; n < nub; n++) {
				if(chart_notes[n].ms <= bot_elapsedTime && chart_notes[n].hit == 0) {
					// Press button
					//pressButton(chart_notes[n].color, 1);
					chart_notes[n].hit = 1;
					bot_notesHit++;
					if(chart_notes[n].color == 'G') {
						rg = bot_elapsedTime + chart_notes[n].duration;
					} else if(chart_notes[n].color == 'R') {
						rr = bot_elapsedTime + chart_notes[n].duration;
					} else if(chart_notes[n].color == 'Y') {
						ry = bot_elapsedTime + chart_notes[n].duration;
					} else if(chart_notes[n].color == 'B') {
						rb = bot_elapsedTime + chart_notes[n].duration;
					} else if(chart_notes[n].color == 'O') {
						ro = bot_elapsedTime + chart_notes[n].duration;
					}
					// Handle Strums
					if(chart_notes[n].action != 'X') {
						//gtrStrum(chart_notes[n].action);
					}
					
					// Generate output
					printf("%i \t %lf \t ", bot_notesHit, bot_elapsedTime);
					char noteAction[12];
					if(chart_notes[n].action == 'D') {
						strcpy(noteAction, "[downstrum]");
						printf("%s \t ", noteAction);
					} else if(chart_notes[n].action == 'U') {
						strcpy(noteAction, "[upstrum]");
						printf("%s \t ", noteAction);
					} else {
						printf("\t \t ");
					}
					printf("Press %c for %.02f ms. \n", chart_notes[n].color, chart_notes[n].duration);
				}
			}
		}
		
		// Trigger the release of any pressed notes
		if(rg > 0) {
			if(rg <= bot_elapsedTime) {
				//pressButton('G', 0);
				notesReleased++;
				printf("\t %lf \t \t \t ( Released G ) \n", bot_elapsedTime);
				rg = 0;
				
			}
		}
		if(rr > 0) {
			if(rr <= bot_elapsedTime) {
				//pressButton('R', 0);
				notesReleased++;
				printf("\t %lf \t \t \t ( Released R ) \n", bot_elapsedTime);
				rr = 0;
				
			}
		}
		if(ry > 0) {
			if(ry <= bot_elapsedTime) {
				//pressButton('Y', 0);
				notesReleased++;
				printf("\t %lf \t \t \t ( Released Y ) \n", bot_elapsedTime);
				ry = 0;
				
			}
		}
		if(rb > 0) {
			if(rb <= bot_elapsedTime) {
				//pressButton('B', 0);
				notesReleased++;
				printf("\t %lf \t \t \t ( Released B ) \n", bot_elapsedTime);
				rb = 0;
				
			}
		}
		if(ro > 0) {
			if(ro <= bot_elapsedTime) {
				//pressButton('O', 0);
				notesReleased++;
				printf("\t %lf \t \t \t ( Released O ) \n", bot_elapsedTime);
				ro = 0;
			}
		}

		// Update elapsed time
		gettimeofday(&tmc, NULL);
		double rt = (double) tmc.tv_sec + (double) tmc.tv_usec / 1000000;
		bot_elapsedTime = (rt - bot_startTime) * 1000;
		
		// Force break out of loop when the last note has been released
		if(bot_finalMs + 1 <= bot_elapsedTime) notesReleased = noteCount;
	}
	
	// Show total time elapsed
	int secsTaken = bot_elapsedTime / 1000;
	int ttMins = secsTaken / 60;
	int ttSecs = secsTaken % 60;
	printf("\nBot finished after %i min %i sec.\n\n", ttMins, ttSecs);
	printf("%sRestart? (y/n) ", KYEL);
	getchar();
	
}

int main(int argc, char **argv)
{
	bool appRunning = true;
	int userOption;
	
	setupButtons();
	
	while(appRunning) {
		userOption = homeScreen();
		if(userOption == 1) {
			if(strlen(songTitle) == 0) {
				showLogo();
				// Get the title of the chart to load
				printf("%sEnter chart title: ", KYEL);
				if(fgets(songTitle, 50, stdin) != NULL) {
					bool titleFound;
					bool chartLoaded;
					songTitle[strlen(songTitle)-1] = '\0';
					// Search for the title in the ./charts folder
					titleFound = findChart(songTitle);
					if(titleFound) {
						// Load chart file
						printf("%sLoading chart %s...\n", KNRM, songTitle);
						chartLoaded = loadChart(songTitle);
						if(chartLoaded) {
							printf("\n%sPress Enter to return to main menu...", KNRM);
							getchar();
						}
					} else {
						// chart file could not be found
						memset(&songTitle[0], 0, 50);
						printf("%sChart not found!\n\n", KRED);
						printf("%sPress Enter to return to main menu...", KNRM);
						getchar();
					}
				} else {
					// user did not enter a song title
					memset(&songTitle[0], 0, 50);
					printf("%sNo song entered!\n\n", KRED);
					printf("%sPress Enter to return to menu...", KNRM);
						getchar();
				}
			}
		}
		
		if(userOption == 2) {
			userNavigate();
		}
		
		if(userOption == 3) {
			printf("\n Press Enter to start... \n");
			getchar();
			botStart();
		}
		
		if(userOption == 4) {			
			appRunning = false;
		}
		
	}
	printf("\n%s", KNRM);
	return(0);
}





