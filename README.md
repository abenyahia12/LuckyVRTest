# LuckyVRTest
To play the game , build it , than open the game.
first screen Type your name and click on PLAY 
you will be redirected to a waiting room , when 2 players are in the waiting room the game starts , first player to start the room is MASTER the second is slave
when in the game , Increment number of stacks to bet using + sign and decrease using - sign
choose a color then CLICK BET
the UI becomes not interactable until other player makes his/her bet
then result is processed in both screens and UI becomes interactable again.
-----------------------------------------------------------------------------

the main logic , I made everything work locally first then made two RPC calls to inform about bets and to process the bets.

My Launcher.cs takes care of the authentication and joining the loading room
MyGameManager.cs processes RPC calls and reveals the betting Color to both clients
MyLocalGameManager.cs is the middleman between gamemanager and Display Scripts( UI MANAGER and Chip Instantiation)
UIManager.cs takes care of displaying the main player Information on screen along with setting the camera to the correct position at start)

Chip Instantiation takes care of generating Chips/stacks/Colors in the beginning .
ChipInstantiation tooke care of generating chips
had a randomseed initialized so that both players have the same randomization for their colors
I made object pooling work by Deactivating removed stacks when loosing and activating the deactivated stacks then generating more stacks and colors if needed.

UtilityClass used this to make some structs and enums I deemed usefull for my tasks
MyPlayer.cs used it to encapsulate the player's variable and show them on editor too so I can debug easily
-------------------------------------------------------------------------------------------------------------------------------------------------------
Then I added the Photon layer last so I think I could've done better in how I Could implement it but also I had to relearn Photon because I was using Beamable and Gamesparks for the past 3 years, but it was very easy to pick up again

PS : I wanted to work on this test on the weekend but I Got sick , so I started this test on Sunday Night and during weekdays I was working fulltime and doing the test during the night .

Thanks
