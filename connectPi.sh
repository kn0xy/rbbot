#!/bin/sh

sudo stty -F /dev/ttyUSB0 raw
sudo pppd /dev/ttyUSB0 115200 10.0.5.1:10.0.5.2 proxyarp local noauth debug nodetach dump nocrtscts passive persist maxfail 0 holdoff 1
