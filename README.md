# LumiaControl
Creating a dll to integrate in Streamer.bot to control my lights via Lumia API

The contents of Program.cs shall later be ported to streamer bot. The remaining classes will be compiled into a dll

In order to use it with your Lumiastream, key and IP in Progam.cs need to be adapted.

You can add your lights to one of two groups, LEFT and RIGHT. The idea is that a user can use a command like this:


green red to blue purple

And it will turn all LEFT lights to green, RIGHT lights to red, and then transition to the next set of colors
