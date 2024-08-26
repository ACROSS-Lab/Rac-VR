/**
* Name: DisplayPlayerInformation
* Based on the internal skeleton template. 
* Author: patricktaillandier
* Tags: 
*/
model DisplayPlayerInformation

global {
	bool fake;
	int num_players <- 2;

	//color of the different players
	float time_exploration <- 120 #s;
	map<rgb, team> teams <- [];
	map<rgb,rgb > colors <- [#green::rgb(60, 181, 0), #yellow::rgb(208, 190, 83), #red::rgb(212, 106, 52), #blue::rgb(52, 101, 212)];

	init {
		loop c over: colors.keys {
			create team with: [color::c] returns: t;
			teams[c] <- first(t);
		}
		if (fake) {
 		loop c over: colors.keys {
			create unity_player with: [name:: string(c) + " - village"+cycle];
		}}
	}

	reflex when: fake {

		ask unity_player {
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

species team {
	list<unity_player> players <- [];
	int score <- 0;
	rgb color;
}

//Species that will make the link between GAMA and Unity. It has to inherit from the built-in species asbtract_unity_linker
species unity_linker parent: abstract_unity_linker {
//name of the species used to represent a Unity player
	string player_species <- string(unity_player);

	//in this model, no information will be automatically sent to the Player at every step, so we set do_info_world to false
	bool do_send_world <- false;
	int min_num_players <- num_players;
	int max_num_players <- num_players;

	action update_score (string id, int score) {
		ask (unity_player first_with (each.name = id)) {
			int diff <- score - current_score;
			current_score <- score;
			my_team.score <- my_team.score + diff;
		}

	}

	action update_player_timer (string id, int time_remaining) {
		ask (unity_player first_with (each.name = id)) {
			if (not active) {
				current_score <- 0;
				active <- true;
			}

			remaining_time <- time_remaining;
		}

	}

	action desactive_player (string id) {
		ask (unity_player first_with (each.name = id)) {
			active <- false;
			finished <- true;
		}

	}

}

species unity_player parent: abstract_unity_player skills:[moving]{
//size of the player in GAMA
	float player_size <- 1.0;

	//color of the player in GAMA
	rgb color;

	//vision cone distance in GAMA
	float cone_distance <- 10.0 * player_size;

	//vision cone amplitude in GAMA
	float cone_amplitude <- 90.0;

	//rotation to apply from the heading of Unity to GAMA
	float player_rotation <- 90.0;

	//is the player active ? 
	bool active <- false;

	//has the player finished ? 
	bool finished <- false;
	team my_team;
	int remaining_time <- 240;
	int current_score;

	init {
		color <- rgb((name split_with (" - "))[0]);
		my_team <- teams[color];
		my_team.players << self;
	}

	//default aspect to display the player as a circle with its cone of vision
	aspect default {
		if active {
			draw sphere(player_size / 2.0) at: location + {0, 0, 1.0} color: color;
			draw player_perception_cone() color: rgb(color, 0.5);
		}
	}
	
	reflex when: fake {
		do wander amplitude: 30.0;
	}
}


experiment "Experiment with fake players" {
	
	image_file mini_map_image_file <- image_file("../includes/mini_map_no_text.png");
	map<rgb,rgb > text_colors <- [#green::#white, #yellow::#black, #red::#white, #blue::#white];
	
	font text <- font("Arial", 24, #bold);
	font title <- font("Arial", 18, #bold);
	int x_origin <- 50;
	int x_interval <- 60;
	int y_interval <- 40;
	int box_size <- 30;
	
	//minimal time between two simulation step
	float minimum_cycle_duration <- 0.05;
	
	action _init_ {
		create simulation with: [fake::true];
	}
	
	output {
		layout #stack controls: false consoles: false toolbars: false navigator: false editors: false tray: false tabs: false;

		display displayVR type: 3d background: #black axes: false {
			image mini_map_image_file refresh: false;
			//camera 'default' location: {50, 117, 111} target: {50, 50, 0.0};
			overlay  position: {0 #px, 0 #px} size: {0 #px, 0 #px} background: #black border: #black rounded: false {
				float y <- 2 * y_interval #px;
				draw rectangle((10 * x_interval) #px , 10* box_size #px)at: {x_origin + (4 * x_interval) #px, y} color: rgb(0,0,0,0.5);
				draw "Team score" at: {x_origin + (1 * x_interval) #px, y_interval #px} anchor: #top_center color: #white font: title;
				draw "Player" at: {x_origin + (4 * x_interval) #px, y_interval #px} anchor: #top_center color: #white font: title;
				draw "Score" at: {x_origin + (6 * x_interval) #px, y_interval #px} anchor: #top_center color: #white font: title;
				draw "Time left" at: {x_origin + (8 * x_interval) #px, y_interval #px} anchor: #top_center color: #white font: title;
	
				map<rgb,team> temp <- [];
				loop t over:  (teams.values sort_by each.score) {
					temp[t.color] <- t;
				}

				loop p over: reverse(temp.pairs) {
					draw rectangle((10 * x_interval) #px, box_size #px) at: {x_origin + (4 * x_interval) #px, y} color: (p.key);
					draw string(p.value.score) at: {x_origin + (1 * x_interval) #px, y} anchor: #center color: text_colors[p.key] font: text;
					draw "#" + string(length(p.value.players)) at: {x_origin + (4 * x_interval) #px, y} anchor: #center color: text_colors[p.key] font: text;
					draw string(last(p.value.players).current_score) at: {x_origin + (6 * x_interval) #px, y} anchor: #center color: text_colors[p.key] font: text;
					draw string(last(p.value.players).remaining_time) + " sec" at: {x_origin + (8 * x_interval) #px, y} anchor: #center color: text_colors[p.key] font: text;
					y <- y + y_interval #px;
				}

			}
			species unity_player;
		}
	}
	
	
}

//default Unity (VR) experiment that inherit from the SimpleMessage experiment
//The unity type allows to create at the initialization one unity_linker agent
experiment vr_xp  autorun: false type: unity parent: "Experiment with fake players"{
	
	action _init_ {
		create simulation with: [fake::false];
	}



	//name of the species used for the unity_linker
	string unity_linker_species <- string(unity_linker);


	//action called by the middleware when a player connects to the simulation
	action create_player (string id) {
		ask unity_linker {
			do create_player(id);
			ready_to_move_player << last(unity_player);
		}
	}

	//action called by the middleware when a plyer is remove from the simulation
	action remove_player (string id_input) {
		if (not empty(unity_player)) {
			ask first(unity_player where (each.name = id_input)) {
				do die;
			}
		}
	}



}
