**#CO-OP Snake 2D Game**
**Playable Game Link** :- https://abhishek-kadam.itch.io/co-op-snake-2d
**LinkedIn Post about game** :- https://www.linkedin.com/posts/abhishek-kadam98_unity-buildinpublic-gamedev-activity-7148016332150673408-BkCB?utm_source=share&utm_medium=member_desktop

**About the game** :-
A 2D Snake game with Co-Op functionalities. Players have option of playing single player as well as Co-Op to accommodate for two players. Game has two types of food  items to eat(One food will increase the score and other food will decrease the score), snake to grow, special power-ups abilities such as (speed boost, score boost and shield), screen wrapping (To enter from one end and exit from opposite end), catchy sounds and sound effects, simple UI with play, pause, mode selection panel,  lobby(Main Menu), quit buttons and score functionalities with game over and game win conditions.

**How to play** :- 
1. Single Player :- WASD and arrow keys.
2. Co-Op mode :-   WASD for one player and arrow keys for second player.

**Game Features and Implementations** :- 
1. The game has basic core snake game functionalities. Such as movement in all 4 directions, Implemented screen wrapping in all four directions, snake will die after biting himself, snake will grow after eating food.
2. Power-Ups :- Snake has 3 types of power-ups. 
	a. Shield → When snake collects a shield then snake will not die when the shield is active. 
	b. Score Boost → Snake will gain 2x Score Points for each mass-gainer food.
	c. Speed Boost → Snake will increase the speed after collecting this power-up. Speed will go up like 2x, 4x and further before cooldown is reached.
	d. Cooldown for each power up is implemented and the cooldown is time flexible. 
	e. Power ups will spawn @ random place @ random interval of time. 
3. Foods :- There will be two types of food in this game.
	a. Mass Gainer → Which will increase the length of snake.
	b. Mass Burner → Which will decrease the length of snake.
	c. Every implementation flexible, which means by how many units you want to increase/decrease the length of snake through code.
	d. Foods will spawn at random places at random interval of time and will get destroyed after some time if not eaten. 
4. Co-Op Functionality :- Two-snakes in the game where one snake moves using WASD and other moves using arrow keys. If Snake A bites snake B then snake B will die & vice versa. 
5. Scoring :- 
	a. Mass Gainer Food will increase the score.
	b. Mass Burner food will decrease the score.
6. UI Implementation :- 
	a. Implemented basic UI like death, Win, Score, game over, Lobby UI for the game
b. Implemented Pause/Resume, Restart and Quit Buttons. 
7. Sound :- Sounds such as bgd music, sfx for button clicks, snake death, food eating, power ups added using singleton design pattern.
