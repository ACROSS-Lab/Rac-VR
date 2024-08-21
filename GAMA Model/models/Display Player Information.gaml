/**
* Name: DisplayPlayerInformation
* Based on the internal skeleton template. 
* Author: patricktaillandier
* Tags: 
*/

model DisplayPlayerInformation

global {
	image_file mini_map_image_file <- image_file("../includes/mini_map.png");
	//color of the different players
	float time_exploration <- 120 #s; 
}

experiment main type: gui {
	output {
		display map type: 3d axes: false{
			image mini_map_image_file refresh: false;
		}	
	}
}



//Species that will make the link between GAMA and Unity. It has to inherit from the built-in species asbtract_unity_linker
species unity_linker parent: abstract_unity_linker {
	//name of the species used to represent a Unity player
	string player_species <- string(unity_player);

	//in this model, no information will be automatically sent to the Player at every step, so we set do_info_world to false
	bool do_send_world <- false;
	
	action update_score(string id, int score) {
		ask (unity_player first_with (each.name = id)) {
			int diff <- score - current_score;
			current_score <- score;
			team_score <- team_score + diff;
		}
	}
	
	action active_player(string id) {
		
		ask (unity_player first_with (each.name = id)) {
			if not to_display_agent {
				do player_phase;
			}
		}
			 
	}
	
	action desactive_player(string id) {
		ask (unity_player first_with (each.name = id)) {
			to_display_agent <- false;
		}
			 
	}
	

}


species unity_player parent: abstract_unity_player {
	//size of the player in GAMA
	float player_size <- 1.0;

	//color of the player in GAMA
	rgb color ;
	
	//vision cone distance in GAMA
	float cone_distance <- 10.0 * player_size;
	
	//vision cone amplitude in GAMA
	float cone_amplitude <- 90.0;

	//rotation to apply from the heading of Unity to GAMA
	float player_rotation <- 90.0;
	
	//display the player
	bool to_display_agent <- false;
	
	
	float remaining_time ;
	int current_score;
	int team_score;
	
	float previous_time;
	
	action player_phase {
		current_score <- 0;
		previous_time <- gama.machine_time;
		remaining_time <- time_exploration;
		to_display_agent <- true;
		ask unity_linker {
			do send_message players: unity_player as list mes: ["start_exploration":: true];
		}
	}
	
	reflex manage_time when: remaining_time > 0 {
		remaining_time <- remaining_time - (gama.machine_time - previous_time)/1000.0;
		previous_time <- gama.machine_time ;
	}
	
	//default aspect to display the player as a circle with its cone of vision
	aspect default {
		if to_display_agent {
			draw sphere(player_size/2.0) at: location + {0, 0, 1.0} color: color ;
			draw player_perception_cone() color: rgb(color, 0.5);
		}
	}
	
	aspect score {
		draw square(5) color: color at: {10, 10 + int(self) * 10 };
		string info <- "Team score:" + team_score;
		if (to_display_agent) {
			info <- info +  " Current player score: " + current_score + " remaining time: "  + round(remaining_time) + "s";
		}
		draw info at: {20, 10 + int(self) * 10 } font: font(15) color: #black ;
	}
}

//default Unity (VR) experiment that inherit from the SimpleMessage experiment
//The unity type allows to create at the initialization one unity_linker agent
experiment vr_xp parent:main autorun: true type: unity {
	//minimal time between two simulation step
	float minimum_cycle_duration <- 0.05;

	//name of the species used for the unity_linker
	string unity_linker_species <- string(unity_linker);
	
	//allow to hide the "map" display and to only display the displayVR display 
	list<string> displays_to_hide <- ["map"];
	
	

	//action called by the middleware when a player connects to the simulation
	action create_player(string id) {
		ask unity_linker {
			do create_player(id);
			ready_to_move_player << last(unity_player);
		}
		ask last(unity_player) {
			color <- rgb((id split_with(" - "))[0]);
			
		}
	}

	//action called by the middleware when a plyer is remove from the simulation
	action remove_player(string id_input) {
		if (not empty(unity_player)) {
			ask first(unity_player where (each.name = id_input)) {
				do die;
			}
		}
	}
	
		 
	output { 
		//In addition to the layers in the map display, display the unity_player and let the possibility to the user to move players by clicking on it.
		display displayVR parent: map  {
			species unity_player;
			
		}
		display Scores {
			species unity_player aspect: score;
		}
	} 
}