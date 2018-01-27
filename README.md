# Lifx-Rocket-League
Lifx lights changing colors upon scoring a goal in rocket league. Colors of the lights depend on which team scored.

I'm a 20 year old danish guy currently studying to become a datatechnician with specialty in programming.
Last week i started working on this project. My goal was to change the color of my Lifx lights to the color of the scoring team.
The process explained:

"I started by making a program in C# that could trigger my lights to change color, for that i used the Lifx Api. When i had that in place i opened up Rocket League and cheat engine and started looking for the pointer values for each goal.
The next step was reading from a memory address in C#, for that i used a library called Memory.dll.
Then i made a loop constantly reading the values of each goal and saving each in a value that i use to determine if the amount of goals on either team has changed.If one teams amount of goal becomes greater than it was it will trigger lifx code with matching team colors."

For those seeking to modify the program for personal use all you gotta do is insert your own lifx token under the LifxSettings class.

If there's any trouble making this work please let me know and i'll be happy to assist.
