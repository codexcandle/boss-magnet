# boss-magnet
2-week "jam demo" game built with Unity, &
inspired by the famous title:

[Katamari Damacy](https://en.wikipedia.org/wiki/Katamari_Damacy)

For the "Boss Magnet" project, 
I put together a simple adventure game 
starring a spherical (ai robot?) magnet 
that's looking to oblige it's "boss" 
by retrieving an assigned count of colored 
boxes.  The count & color are randomly 
assigned.  

However, 2 "alien greys" roam the level 
attempting to obstruct your mission.  Each 
time they touch the player, HP is deducted, 
and all carried blocks are lost.  While 
testing, a "cute" scenario occurred that 
led to the player getting "kicked" up and 
out of the existing play area.  Subsequently,
they would just tumble in space.  This 
seemed fun, so a "lost in space" end 
message was added, just before triggering 
game over.  The same can be said for the 
"magnet boost" power-up as it was the by-product
of a happy accident in the code.
 
Regarding the game architecture, the project 
consists of 3 scenes - start, 
play, & end.  Though most of the scripts 
can be found referenced in the "play" scene, 
a "GameState" model object is instatiated 
(& kept alive throughout) via the first scene's 
call to it's "GameStart.cs" script.  This 
triggers the play scene to load, and the 
gameplay is handed over to the "LevelLogic" 
class.  LevelLogic includes references to 
subsequent view controller classes.  The most 
important of these controllers is the 
"PlayerController" which controls player 
movement & interaction.  (From an architecture 
standpoint, both of these mentioned classes 
are probably too monolithic in their current 
state for my liking, and additional decoupling 
could also be achieved via use of an event manager.)

With all of this being said, hopefully, 
however, you'll find the supplied source code 
clean & easy-to-follow. :)
