/**
* Name: RAC_Display_Scores
* Based on the internal skeleton template. 
* Author: Patrick Taillandier, Alexis Drogoul
* Tags: 
*/
model RAC_Display_Scores

global {
	int num_players <- 4;
	map<rgb, team> player_teams <- [];
	list<rgb> village_colors <- [#green, #yellow, #red, #blue];

	init {
		loop c over: village_colors {
			create team with: [color::c] returns: t;
			player_teams[c] <- first(t);
		}
	}
}

species team {
	list<unity_player> players <- [];
	int score <- 0;
	rgb color;
	int generation <- 1;
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
		unity_player player <- unity_player first_with (each.name = id);
		if (player != nil) {
			ask player {
				int diff <- score - current_score;
				current_score <- score;
				my_team.score <- my_team.score + diff;
			}
		}
	}

	action update_player_timer (string id, int time_remaining) {
		unity_player player <- unity_player first_with (each.name = id);
		if (player != nil) {
			ask player {
				if (not active) {
					current_score <- 0;
					active <- true;
				}
				remaining_time <- time_remaining;
			}
		}
	}

	action desactive_player (string id) {
		unity_player player <- unity_player first_with (each.name = id);
		if (player != nil) {
			ask player {
				active <- false;
				finished <- true;
				my_team.generation <- my_team.generation + 1;
			}
		}
	}
}

species unity_player parent: abstract_unity_player skills: [moving] {
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
		my_team <- player_teams[color];
		my_team.players << self;
	}

	//default aspect to display the player as a circle with its cone of vision
	aspect default {
		if active {
			draw sphere(player_size / 2.0) at: location + {0, 0, 1.0} color: color;
			draw player_perception_cone() color: rgb(color, 0.5);
		}
	}
}

//default Unity (VR) experiment that inherit from the SimpleMessage experiment
//The unity type allows to create at the initialization one unity_linker agent
experiment vr_xp autorun: false type: unity {

	//name of the species used for the unity_linker
	string unity_linker_species <- string(unity_linker);

	//action called by the middleware when a player connects to the simulation
	action create_player (string id) {
		ask unity_linker {
			do create_player(id);
			if (!(ready_to_move_player contains(last(unity_player)))) {
			ready_to_move_player << last(unity_player);}
		}
	}

	//action called by the middleware when a player is remove from the simulation
	action remove_player (string id_input) {
		if (not empty(unity_player)) {
			ask first(unity_player where (each.name = id_input)) {
				do die;
			}
		}
	}

	image_file mini_map_image_file <- image_file("../includes/mini_map_no_text.png");
	map<rgb, rgb> text_colors <- [#green::#white, #yellow::#black, #red::#white, #blue::#white];
	font text <- font("Arial", 24, #bold);
	font title <- font("Arial", 18, #bold);
	int x_origin <- 50;
	int x_interval <- 60;
	int y_interval <- 40;
	int box_size <- 30;

	//minimal time between two simulation step
	float minimum_cycle_duration <- 0.05;
	output {
		layout #stack controls: false consoles: false toolbars: false navigator: false editors: false tray: false tabs: false;
		display displayVR type: 3d background: #black axes: false {
			image mini_map_image_file refresh: false;
			camera 'default' location: {50, 117, 111} target: {50, 50, 0.0};
			overlay position: {0 #px, 0 #px} size: {0 #px, 0 #px} background: #black border: #black rounded: false {
				float y <- 2 * y_interval #px;
				draw rectangle((10 * x_interval) #px, 10 * box_size #px) at: {x_origin + (4 * x_interval) #px, y} color: rgb(0, 0, 0, 0.5);
				draw "Team score" at: {x_origin + (1 * x_interval) #px, y_interval #px} anchor: #top_center color: #white font: title;
				draw "Player" at: {x_origin + (4 * x_interval) #px, y_interval #px} anchor: #top_center color: #white font: title;
				draw "Score" at: {x_origin + (6 * x_interval) #px, y_interval #px} anchor: #top_center color: #white font: title;
				draw "Time left" at: {x_origin + (8 * x_interval) #px, y_interval #px} anchor: #top_center color: #white font: title;
				map<rgb, team> temp <- [];
				loop t over: (player_teams.values sort_by each.score) {
					temp[t.color] <- t;
				}

				loop p over: reverse(temp.pairs) {
					draw rectangle((10 * x_interval) #px, box_size #px) at: {x_origin + (4 * x_interval) #px, y} color: (p.key);
					//draw rectangle(x_interval #px, box_size #px) at: {x_interval / 2, y} color: p.key;
					draw string(p.value.score) at: {x_origin + (1 * x_interval) #px, y} anchor: #center color: text_colors[p.key] font: text;
					draw "#" + p.value.generation at: {x_origin + (4 * x_interval) #px, y} anchor: #center color: text_colors[p.key] font: text;
					unity_player last <- last(p.value.players);
					if (last != nil) {
						draw string(last.current_score) at: {x_origin + (6 * x_interval) #px, y} anchor: #center color: text_colors[p.key] font: text;
						draw string(last.remaining_time) + " sec" at: {x_origin + (8 * x_interval) #px, y} anchor: #center color: text_colors[p.key] font: text;
					}

					y <- y + y_interval #px;
				}

			}

			species unity_player;
		}

	}

}
