# Rock Band Bot

A bot to automatically play Rock Band 2 guitar (or bass) on expert difficulty. Originally created in 2015 and last updated in 2018.    
I haven't touched this project in several years so I decided to put it here in case someone else may find it helpful or interesting.

<br>

## Hardware
* [Rasbperry Pi (Model A+)](https://www.raspberrypi.com/products/raspberry-pi-1-model-a-plus/)
* [USB to TTL Serial Cable](https://www.adafruit.com/product/954?gclid=Cj0KCQjw98ujBhCgARIsAD7QeAgtxVoputFIMj-5HBrepFHJincd81hUkQvDKLHzjLAIXdg_X87k_5EaAuShEALw_wcB)
* [Xplorer Guitar controller (Xbox 360)](https://guitarhero.fandom.com/wiki/X-plorer_Controller)
* [Optocouplers](https://www.mouser.com/ProductDetail/Everlight/EL817?qs=vs%252BWWTB4QKYUQzCxB0orLQ%3D%3D)
* Jumper wires
* Breadboard

Since there is no ethernet port on the Raspberry Pi Model A+, I used a USB-to-TTL serial cable to communicate with the Pi via UART.  

<br>

## Software
* The [Chart Creator](./chartCreator/chartCreator) program (Windows only) was written in VB.NET
  * Scans image files located in `chartCreator/charts` and processes them into .bot files
<br>

* The actual bot itself was written in C, and compiled on the Pi
* Originally I ran the bot directly on the Pi using [screen](https://linux.die.net/man/1/screen), but later changed to [stty](https://linux.die.net/man/1/stty)+[pppd](https://linux.die.net/man/8/pppd) for improved stability and cross-platform compatibility
  * For more info check out [connectPi.sh](connectPi.sh)
<br>

* In 2018 I created the [Bot Controller](./botController) program in VB.NET to control the bot using a Windows 10 laptop
  * The driver for the USB-to-TTL serial cable is located in the `Profilic_Win8_x64_x86` folder and is needed to expose the COM port on Windows systems so that the software can communicate with the Raspberry Pi.
    
<br><br><br>

#### _More info coming soon!_
