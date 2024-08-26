/**
* Name: DisplayPlayerInformation
* Based on the internal skeleton template. 
* Author: patricktaillandier
* Tags: 
*/
model DisplayPlayerInformation

import "Display Player Information.gaml"
 

global {
	
	
	reflex when: cycle = 0 {
		 loop c over: village_colors {
			create unity_player with: [name:: string(c) + " - village"+cycle] returns:p;
		}
	}


	reflex {

		ask unity_player {
			do wander amplitude: 30.0;
			bool cond <- flip(0.8);
			if (!finished) {
				if (not active) {
					current_score <- 0;
					active <- true;
				}
				if cond {remaining_time <- remaining_time - 1;}
				if remaining_time <= 0 {
					active <- false;
					finished <- true;
					create unity_player with: [name:: string(color) + " - village"+cycle];
					my_team.generation <- my_team.generation + 1;
				} else {
				if cond and flip(0.1) {
				current_score <- current_score + 1;
				my_team.score <- my_team.score + 1;
			}
				}

			}

		}
	} 
}




