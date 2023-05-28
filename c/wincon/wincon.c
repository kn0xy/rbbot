/*
 * wincon.c
 *
 * Copyright 2015-2018 Knoxy (Tyler Knox)
 * Last modified 8/4/2018
 *
 */

#include "wincon.h"

bool appRunning = true;

void botStart()
{
        // Start the bot
        struct timeval tms, tmc;
        int n, nub, notesReleased = 0;
        gettimeofday(&tms, NULL);
        bot_startTime = (double) tms.tv_sec + (double) tms.tv_usec / 1000000;
        printf("\nBot started! \n");

        // Define Press/Release triggers
        double rg, rr, ry, rb, ro, rd, ru;
        rg = 0;
        rr = 0;
        ry = 0;
        rb = 0;
        ro = 0;
        rd = 0;
        ru = 0;

        // Play the chart until all notes have been pressed & released
        while(notesReleased < noteCount) {
                if(bot_notesHit < noteCount) {
					n = bot_notesHit;
     
                    if(chart_notes[n].ms >= bot_elapsedTime && chart_notes[n].hit == 0) {
                        // Press button
                        pressButton(chart_notes[n].color, 1);
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
                        if(chart_notes[n].action == 'D') {
                                pressButton('D', 1);
                                rd = bot_elapsedTime + 60;
                        } else if(chart_notes[n].action == 'U') {
                                pressButton('U', 1);
                                ru = bot_elapsedTime + 60;
                        }

                        // Generate output
                        printf("%i-%lf-%c-%c-%lf\n", bot_notesHit, bot_elapsedTime, chart_notes[n].action, chart_notes[n].color, chart_notes[n].duration);
                    }
                        
                }

                // Trigger the release of any pressed notes
                if(rg > 0) {
                        if(rg <= bot_elapsedTime) {
                                pressButton('G', 0);
                                notesReleased++;
                                rg = 0;

                        }
                }
                if(rr > 0) {
                        if(rr <= bot_elapsedTime) {
                                pressButton('R', 0);
                                notesReleased++;
                                rr = 0;

                        }
                }
                if(ry > 0) {
                        if(ry <= bot_elapsedTime) {
                                pressButton('Y', 0);
                                notesReleased++;
                                ry = 0;

                        }
                }
                if(rb > 0) {
                        if(rb <= bot_elapsedTime) {
                                pressButton('B', 0);
                                notesReleased++;
                                rb = 0;

                        }
                }
                if(ro > 0) {
                        if(ro <= bot_elapsedTime) {
                                pressButton('O', 0);
                                notesReleased++;
                                ro = 0;
                        }
                }


                // Release strums
                if(rd > 0) {
                        if(rd <= bot_elapsedTime) {
                                pressButton('D', 0);
                                rd = 0;
                        }
                }
                if(ru > 0) {
                        if(ru <= bot_elapsedTime) {
                                pressButton('U', 0);
                                ru = 0;
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
        printf("\nBot finished after %i min %i sec.\n", ttMins, ttSecs);
        appRunning = false;
}

int main(int argc, char **argv)
{
        if(argc < 2) {
			printf("\nKnoxy RB Bot - Windows Control\nLast modified 8/4/18\n");
                printf("Usage:  wincon chart_file [is_leet]\n");
                return 1;
        } else {
                bool is_leet;
                if(argc >= 3) {
                    is_leet = true;
                } else {
                    is_leet = false;
                }
                while(appRunning) {
                        char chartFilename[50];
                        strcpy(chartFilename, argv[1]);

                        if(loadChart(chartFilename, is_leet)) {
                                setupButtons();
                                if(is_leet == true) {
                                    printf("Bot ready! (Practice mode)\nPress enter to start...\n");
                                } else {
                                    printf("Bot ready!\nPress enter to start...\n");
                                }
                                getchar();
                                botStart();
                        }
                }
        }

        return 0;
}
