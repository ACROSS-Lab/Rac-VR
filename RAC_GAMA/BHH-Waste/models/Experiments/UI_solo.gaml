/**
* Name: U1
* The model used for the main demonstrations
* Author: A. Drogoul
* 
* This model has been designed using resources (icons) from Flaticon.com
* 
* Tags: 
*/

model UI_solo

 
import "../Global.gaml"
 
global {
	bool dark_theme <- false;
	
	/********************** COLORS ************************************************/
	
	list<rgb> greens <- reverse(palette(rgb(43, 108, 1), rgb(103, 156, 36), rgb(141, 173, 39), rgb(172, 172, 37), rgb(235, 188, 86)));
	list<rgb> blues <- reverse(palette(rgb(239, 243, 255), rgb(189, 215, 231), rgb(107, 174, 214), rgb(49, 130, 189), rgb(8, 81, 156)));
	list<rgb> reds <- palette(rgb(254, 229, 217), rgb(252, 174, 145), rgb(251, 106, 74), rgb(222, 45, 38), rgb(165, 15, 21));
	rgb city_color <- rgb(228, 141, 104);
	rgb selected_color <- rgb(255,255,255);
	rgb unselected_color <-rgb(200,200,200,0.7);
	list<rgb> village_color <- [rgb(183, 73, 77), rgb(255, 217, 67), rgb(65, 149, 205), rgb(80, 174, 76)]; // color for the 4 villages
	map<string, rgb> color_col <- ["Production":: rgb(118, 189, 30), "Total"::rgb(253, 161, 69), "Water"::rgb(120, 172, 217), "Solid"::rgb(137, 100, 73)]; //color used when indicators are not broken down by villages
	map<string, rgb> color_col_background <- ["Production":: rgb(231, 255, 140), "Total"::rgb(255, 225, 177), "Water"::rgb(213, 243, 243), "Solid"::rgb(229, 194, 163)];
	rgb map_background <- rgb(248, 246, 245);
	rgb map_background2 <- rgb(237, 234, 233); //bandeau
	rgb legend_background <- dark_theme ? #black: #white; //rgb(60,60,60);
	rgb timer_main <- rgb(29, 98, 223);
	rgb timer_second <- rgb(205, 226, 242);
	rgb calendar <- rgb(202, 56, 40);
	rgb calendar_second <- rgb(236, 187, 175);
	rgb ecolabel <- rgb(97, 180, 31);
	rgb ecolabel_second <- rgb(174, 224, 128);
	rgb river <- rgb(178, 193, 149);
	rgb point_of_interest_color <- rgb(217, 104, 76);
	rgb player_color <- rgb(223, 204, 76);
	
	/********************** PROPORTION OF THE DISPLAYS ****************************/
	
	int small_vertical_prop <- 1000;
	int large_vertical_prop <- 3500;
	int small_horizontal_prop <- 1500;
	int large_horizontal_prop <- 3000;
	
	/********************** POSITIONS AND SIZES ****************************/
	
	float chart_line_width -> #fullscreen ? 10.0 : 10.0;
	float w_height <- shape.height;
	float w_width <- shape.width;
	matrix position <- matrix([{1400,-800,2}, {1300,500,2}, {-600,1000,2}, {-1000,-1800,2}]);
	matrix position2 <- matrix([[1400,-800,2], [1300,500,2], [-600,1000,2], [-1000,-1800,2]]);
	float y_centerdis <- 1100.0;
	
	/********************** FONTS ************************************************/
	// UNCOMMENT FOR THE LATEST VERSION IN GAMA 
	int text_size -> #hidpi ? (#fullscreen ? 100 : 30) : (#fullscreen ? 50 : 30);
	font ui_font -> font("Jaldi", text_size, #bold);
	
	/******************* GENERAL PARAMETERS *************************************/
	
	bool confirmation_popup <- false;
	bool no_starting_actions <- true;
	bool about_to_pause <- false;
	bool CHOOSING_VILLAGE_FOR_POOL <- false;
	bool PASS_CHOOSING_VILLAGE <- false;
	bool display_water_flow <- true;
	stacked_chart global_chart;
	int chosen_village <- -1;
	int number_of_days_passed <- 0;
	map<village,list<string>> village_actions <- nil;
	
	/****************** DISPLAY OF WATER DYNAMICS *****************************/ 
	
	graph canal_network_<- nil;
	int number_to_add <- 10;
	map<point, list<point>> possible_targets;
	
	/******************* TIMERS *************************************/
	
	bool use_timer_player_turn <- false;	
	bool use_timer_for_discussion <- true;
	bool use_timer_for_exploration <- false;
	bool timer_just_for_warning <- false; //if true, if the timer is finished, just a warning message is displayed; if false, the turn passes to the next player - for the moment, some issue with the automatic change of step
	float initial_time_for_discussion <- 1 #mn const: true; // time before the player turns
	float initial_time_for_exploration <- 5 #mn const: true;
	float initial_time_for_choosing_village <- 20 #s const: true;
	float time_for_choosing_village <- initial_time_for_choosing_village;
	float start_choosing_village_time;
	int remaining_time_for_choosing_village <- 0;
	float pause_started_time <- 0.0;
	
	/********************* MANAGEMENT OF BUTTONS ****************************/
	
	geometry show_waterflow_button;
	geometry show_map_button;
	geometry show_chart_button;
	geometry show_chart_vil_button;
	bool show_map_selected;
	bool show_chart_selected;
	bool show_waterflow_selected;
	bool show_chart_vil_selected;
	bool show_chart_by_vil <- false;
	bool show_chart <- true;
	bool show_player_numbers <- true;
	bool play_pause_selected <- false;
	bool next_selected <- false;
	map<string, point> action_locations <- [];
	map<int,geometry> village_buttons <- [];
	point next_location <- {0,0};
	point pause_location <- {0,0};
	string over_action;
	int village_selected;
	int turn_see_indicators <- 1;
	
	/********************** ICONS *************************************************/
	
	image_file label_icon <- image_file("../../includes/icons/Logo jour de label.png");
	image_file waste_icon <- image_file("../../includes/icons/solidwaste.png");
	image_file tokens_icon <- image_file("../../includes/icons/logo argent.png");
	image_file water_icon <- image_file("../../includes/icons/waterwaste.png");
	image_file plant_icon <- image_file("../../includes/icons/Logo production en bas.png");
	list<image_file> numbers <- [1,2,3,4] collect image_file("../../includes/icons/Logo "+each+".png");
	list<image_file> smileys <- [0,1,2,3,4] collect image_file("../../includes/icons/smiley"+each+".png");
	image_file calendar_icon <- image_file("../../includes/icons/Logo calendrier.png");
	image_file discussion_icon <- image_file("../../includes/icons/Logo gens assis.png");
	image_file sandclock_icon <- image_file("../../includes/icons/sablier.png");
	image_file computer_icon <- image_file("../../includes/icons/ICON_Simulation.png");
	image_file next_icon <- image_file("../../includes/icons/Bouton suivant/suivant.png");
	image_file button_background <- image_file("../../includes/icons/Bouton suivant/fond.png");
	image_file play_icon <- image_file("../../includes/icons/Bouton play_pause/Play.png");
	image_file pause_icon <- image_file("../../includes/icons/Bouton play_pause/Pause.png");
	image_file garbage_icon <- image_file("../../includes/icons/garbage.png");
	image_file city_icon <- image_file("../../includes/icons/logo ville en bas.png");
	image_file score_icon <- image_file("../../includes/icons/trophy.png");
	image_file pollution_icon <- image_file("../../includes/icons/Total_waste.png");
	image_file vr_icon <- image_file("../../includes/icons/ICON_VR.png");
	image_file graph_icon <- image_file("../../includes/icons/graph.png");
	image_file background <- image_file("../../includes/icons/fond.jpeg");
	image_file line_threshold <- image_file("../../includes/icons/Line.png");
	image_file ecolabel_icon <- image_file("../../includes/icons/Ecolabel_On.png");
	image_file no_ecolabel_icon <- image_file("../../includes/icons/Ecolabel_Off.png");
	image_file minimap <- image_file("../../includes/icons/mini_map.png");
	image_file player <- image_file("../../includes/icons/Icone_Player.png");
	image_file interest <- image_file("../../includes/icons/Icone_PointOfInterest.png");

	/********************** VARIOUS FUNCTIONS  ***************************/

	//for display on main map
	int production_class_current(plot p) {
		float w <- p.current_productivity; 
		switch(w) {
			match_between [0, 0.000079] {return 0;}
			match_between [0.00008, 0.000012] {return 1;}
			match_between [0.00013, 0.00019] {return 2;}
			match_between [0.0002, 0.00029] {return 3;}
			default {return 4;}	
		}
	}
	
	//for display on main map
	int water_pollution_class_current(canal p) {
		float w <- p.pollution_density; 
		switch(w) {
			match_between [0, 0.9] {return 0;}
			match_between [1, 9] {return 1;}
			match_between [10, 19] {return 2;}
			match_between [20, 39] {return 3;}
			default {return 4;}
		}
	}

	action choose_village_for_pool {
		if (not CHOOSING_VILLAGE_FOR_POOL) {
			commune_money <- 0;
			ask village {
				commune_money <- commune_money + budget;
				budget <- 0;
			}
			extra_turn <- true;
			CHOOSING_VILLAGE_FOR_POOL <- true;
			start_choosing_village_time <- gama.machine_time;
		}
	}
	
	action action_executed(string action_name) {
		if village_actions = nil or empty(village_actions) {
			loop v over: village {
				village_actions[v]<-[];
			}
		}
		list the_list <- village_actions[villages_order[index_player]];
		if the_list != nil {
				the_list <+ action_name;
		}

	} 
			
	action tell (string msg, bool add_name <- false) {
		 if (confirmation_popup) {
		 	invoke tell(msg, add_name);
		 }
	}
	
	action pause {
		about_to_pause <- true;
		ask experiment {do update_outputs;}
		invoke pause;
	}
	
	action resume {
		about_to_pause <- false;
		invoke resume;
	}

	action update_chart_by_vil(stacked_chart chart) {
		ask chart {
			loop i from: 0 to: 3 {
				do update_all2(village_color[i], ["Total"::(village_water_pollution[i] + village_solid_pollution[i]) / max_pollution_ecolabel, "Water"::village_water_pollution[i] / max_pollution_ecolabel, "Solid"::village_solid_pollution[i] / max_pollution_ecolabel, "Production"::village_production[i] / min_production_ecolabel]);
			}

			float total_value <- (village_water_pollution sum_of(each) + village_solid_pollution sum_of(each)) / max_pollution_ecolabel;
			float production_value <- village_production sum_of(each) / min_production_ecolabel;
			float water_value <- village_water_pollution sum_of(each)/ max_pollution_ecolabel;
			float solid_value <- village_solid_pollution sum_of(each) / max_pollution_ecolabel;
			do update_all(["Total"::total_value, "Water"::water_value, "Solid"::solid_value, "Production"::production_value]);
		}
	}


	/********************** INITIALIZATION & REFLEXES ***************************/
	
	init {
		create stacked_chart {
			show_pol_chart_by_cat_glob <- always_display_sub_charts ;
			desired_value <- 1.0;
			max_value <- ["Production"::2.0, "Total"::2.0, "Water"::0.70, "Solid"::1.78];
			do add_column("Production");
			do add_column("Total");
			do add_column("Water");
			do add_column("Solid");

			icons <- ["Total"::pollution_icon, "Water"::water_icon, "Solid"::waste_icon, "Production"::plant_icon];
		 	inf_or_sup <- ["Total"::true,"Water"::true, "Solid"::true, "Production"::false];
		 	draw_smiley <- ["Total"::true,"Water"::false, "Solid"::false, "Production"::true];
			
			loop i from: 0 to: 3 {
				do add_element(village_color[i]);
			}
		}
		global_chart <- stacked_chart[0];
		if isDemo {
			always_display_sub_charts <- true;
			
			create pointInterestManager;
			create pointInterest {
				location <- {2769.2, 2741.2};
				manager <- first(pointInterestManager);
				do addSelfToManager;
			}
		}
	}

	reflex update_charts when: stage = COMPUTE_INDICATORS{
		village_actions <- nil;
		do update_chart_by_vil(global_chart);
 
		time_for_discussion <- initial_time_for_discussion;
		time_for_exploration <- initial_time_for_exploration;
		pause_started_time <- 0.0;
		number_of_days_passed <- number_of_days_passed + 1;
	}

	
	reflex end_of_exploration_turn when: use_timer_for_exploration and stage = PLAYER_VR_EXPLORATION_TURN {
		if turn = turn_see_indicators +1 and !always_display_chart_by_vil{
			show_chart_by_vil <- false;
		}
		remaining_time <- int(time_for_exploration - machine_time/1000.0 + start_exploration_turn_time/1000.0);
		if remaining_time <= 0 {
			do end_of_exploration_phase;
			if !always_display_sub_charts {
				show_pol_chart_by_cat_glob <- true;
			}
		}
	}
	
	reflex end_of_discussion_turn when: use_timer_for_discussion and stage = PLAYER_DISCUSSION_TURN {
		remaining_time <- int(time_for_discussion - machine_time/1000.0 + start_discussion_turn_time/1000.0); 
		if remaining_time <= 0 {
			do end_of_discussion_phase;		
		}
	}

	reflex add_waste when: display_water_flow and every(25 #cycle){
		if canal_network_ = nil {
			canal_network_ <- directed(as_edge_graph(canal));
			list<point> sources <- canal_network_.vertices where (empty(canal_network_ in_edges_of each));
			list<point> targets <- canal_network_.vertices where (empty(canal_network_ out_edges_of each));
			loop s over: sources {
				list<point> ts <- [];
				loop t over: targets {
					if (canal_network_ path_between(s,t)) != nil {
						ts << t;
					}
				}
				
				possible_targets[s] <- ts;
			}
		}
		create waste_on_canal number: number_to_add {
			location <- one_of(possible_targets.keys);
			target <- one_of (possible_targets[location]);
		}
	}
	
	reflex end_of_choosing_village when: CHOOSING_VILLAGE_FOR_POOL {
		remaining_time_for_choosing_village <- int(time_for_choosing_village - machine_time/1000.0  +start_choosing_village_time/1000.0); 
		if remaining_time_for_choosing_village <= 0 or chosen_village > -1 or PASS_CHOOSING_VILLAGE{
			if (chosen_village > -1){
				villages_order << village[chosen_village];
				ask village[chosen_village] {
					budget <- commune_money;
				}
				//do before_start_turn();
			} else {
				commune_budget_dispatch <- true;
			}
			CHOOSING_VILLAGE_FOR_POOL <- false;
			PASS_CHOOSING_VILLAGE <- false;
			chosen_village  <- -1;
			commune_money <- 0;
			to_refresh <- true;
		}
	}
}

species waste_on_canal skills: [moving]{
	point target;
	point prev_loc <- copy(location);
	
	reflex move {
	 	speed <- ((int(duration) = 0) ? 50 : ( int(duration))) / step;
		prev_loc <- copy(location);
		do goto target: target on:canal_network_ ;
		if location = target or location = prev_loc{
			do die;
		}
	}
}

species stacked_chart {
	point location <- {w_width/2 ,w_height/2};
	map<string, float> data <- [];	//not by village
	map<string, map<rgb,float>> data2 <- []; //by village
	map<string, image_file> icons <- [];
	map<string, bool> inf_or_sup ;
	map<string, bool> draw_smiley;
	float chart_width <- 2 * w_width;
	float chart_height <- 3 * w_height/4;
	map<string,float> max_value;
	float desired_value;
	bool show_pol_chart_by_cat; //to show solid and water pollution indicators
	
	action add_column(string column) {
		if (!(column in data.keys)) {
			data[column] <- [];
			data2[column] <- [];
		}
	} 
	
	action add_element(rgb element) {
		loop c over: data2.keys {
			data2[c][element] <- 0.0;
		}
 	}
	
	//update values by villages
 	action update_all2(rgb element, map<string, float> values) {
 		loop col over: data2.keys {
 			data2[col][element] <- values[col];
 		}
 	}
 	
 	//update values not by villages
 	action update_all(map<string, float> values) {
 		loop col over: data.keys {
 			data[col] <- values[col];
 		}
	}
 	
 	reflex update_show_pol_by_cat when: !always_display_sub_charts {
 		show_pol_chart_by_cat <- show_pol_chart_by_cat_glob;
 	}
 	
 	aspect horizontal {
 		float my_width <- chart_width;
 		float my_height <- chart_height;
 		float original_col_width <- my_width / ((length(data) + 4));
		float col_width <- original_col_width;
		float gap <- 200.0;
 		bool gap_added <- false;
 		map<string,float> max_heights <- ["Production"::my_height, "Total"::my_height, "Water"::my_height/2/2.54, "Solid"::my_height/2];
 		map<string,float> y_rect <- ["Production"::my_height, "Total"::my_height, "Water":: 1.405*my_height, "Solid":: 1.25*my_height];
 		
 		//draw rectangle(2.5*original_col_width, chart_height/2) at: {location.x-original_col_width*1.5, location.y +chart_height/4} border: dark_theme ? #white : #black wireframe: true;
 		//draw rectangle(2.5*original_col_width, chart_height/2) at: {location.x-original_col_width*1.5, location.y-chart_height/4} border: dark_theme ? #white : #black wireframe: true;
 		
 		float current_x <- 0.0;
 		float total_prod ;
 		float total_pol ;
		
		loop col over: show_pol_chart_by_cat_glob ? data.keys : ["Production", "Total"]{
 			float current_y <-chart_height/2;
 			float total <- 0.0;
 			
 			if (!draw_smiley[col] and !gap_added) {
 				gap_added <- true;
 				col_width<-original_col_width/3;
 				current_x <- current_x + col_width;
 			}
 			
 			if show_chart_by_vil {
 				draw rectangle(col_width,max_heights[col]) color: map_background2 at: {current_x, y_rect[col]};
				loop c over: data2[col].keys {
	 				float v <- data2[col][c];
	 				total <- total+v;
	 				float col_height <- (v * max_heights[col])/max_value[col];
	 				draw rectangle(col_width,col_height) color: c at: {current_x, my_height + current_y - col_height/2};
	 				//draw rectangle(col_width,col_height) wireframe: true border: dark_theme ? #black : #black width: 2 at: {current_x,my_height  + current_y -  col_height/2};
	 				current_y <- current_y - col_height;
	 			}
 			} else {
 				float v <- data[col];
				total <- total+v;
				float col_height <- (v * max_heights[col])/max_value[col];
				draw rectangle(col_width,max_heights[col]) color: color_col_background[col] at: {current_x, y_rect[col]};
				draw rectangle(col_width,col_height) color: color_col[col] at: {current_x,my_height  + current_y - col_height/2};
				//draw rectangle(col_width,col_height) wireframe: true border: dark_theme ? #black : #black width: 2 at: {current_x,my_height  + current_y -  col_height/2};
				current_y <- current_y - col_height;
			}
			
			if (col = "Production") 
			{
				total_prod <- total;
			} 
			else if (col = "Total") 
			{
				total_pol <- total;
			}
 			
 			if (icons[col] != nil) {
 				draw icons[col] at: {current_x, my_height+chart_height/1.5 } size: {original_col_width/(gap_added? 4:2), original_col_width/(gap_added? 4:2)};
 				/**
 				if draw_smiley[col] {
	 				if (total <= 1 and inf_or_sup[col] or total > 1 and !inf_or_sup[col]) {
	 					draw smileys[0]  at: {current_x, my_height+original_col_width/2} size: {original_col_width/2, original_col_width/2};
	 				} 
	 				else {
	 					draw smileys[4]  at: {current_x, my_height+original_col_width/2} size: {original_col_width/2, original_col_width/2};
	 				}
 				}
 				*/
 			}
 			current_x <- current_x + col_width + gap;
 		}
 		if (total_pol <= 1 and total_prod > 1) {
			draw ecolabel_icon at: {original_col_width*1.75, my_height-2.25*original_col_width} size: {1300, 1600};
		} 
		else {
			draw no_ecolabel_icon at: {original_col_width*1.75, my_height-2.25*original_col_width} size: {1300, 1600};
		}
 				
 		draw line({-original_col_width/2, 3* chart_height / 2 - max_heights["Production"]/2}, {original_col_width/2, 3* chart_height / 2 - max_heights["Production"]/2}) width:20 color: map_background ;
 		draw line({original_col_width/2 + gap, 3* chart_height / 2 - max_heights["Production"]/2}, {3*original_col_width/2 + gap, 3* chart_height / 2 - max_heights["Production"]/2}) width:20 color: map_background;
 	}
} 

experiment Open {
	
	action affiche_coord {
		write sample(#user_location);
	}
	
	int ambient_intensity <- 100;
	
	action _init_ {
		//Requires latest version of GAMA 1.8.2
		//map<string, unknown> params <- user_input_dialog("Welcome to RÁC",[enter("Dark theme",true), choose("Language", string, "English",["English","Français","Tiếng Việt"])], font("Helvetica",18, #bold), nil, false);
		map<string, unknown> params <- user_input_dialog("Welcome to RÁC",[choose("Language", string, "English",["English","Français","Tiếng Việt"])], ui_font, map_background, false);
		gama.pref_display_slice_number <- 12; /* 128 too slow ! */
		gama.pref_display_show_rotation <- false;
		gama.pref_display_show_errors <- false;
		gama.pref_errors_display <- false;
		gama.pref_errors_stop <- false;
		gama.pref_errors_in_editor <- false;
		gama.pref_display_numkeyscam <- false;
		//create simulation with: [dark_theme::bool(params["Dark theme"]), langage::params["Language"]];
		create simulation with: [langage::params["Language"]];
	}

	output {
		/********************** LAYOUT ***********************************************************/	
		layout vertical([0::82,horizontal([1::650,2::450])::368]) consoles: false tabs:false toolbars:false controls:false editors: false navigator: false tray: false parameters: false background: map_background;

		/********************** CENTER DISPLAY *************************************************/
		display "Controls" type: opengl axes: false background: map_background2  {
			
			camera #default locked: true;				
			light #ambient intensity: ambient_intensity;
			
			
			graphics "Turn#" position: {-w_width, y_centerdis} {
				int value <- number_of_days_passed;
				float total <- 365.0 * end_of_game;
				float radius <- w_width/1.5;
				float start_angle <-  - 180.0;
				float arc_angle <- (value * 180/total);
				draw arc(radius, start_angle + arc_angle/2, arc_angle) color: calendar;
				start_angle <- start_angle + arc_angle;
				arc_angle <- (total - value) * 180/total;
				draw arc(radius, start_angle + arc_angle/2, arc_angle) color: calendar_second;
				draw arc(radius/2, -90, 180) color: #white;
				draw calendar_icon size: w_width / 6;
				draw ""+value + " [" +value div 365 + "]" at: {location.x, location.y- 6*radius/10, 0.01}  color: calendar font: ui_font anchor: #bottom_center;
			}
					
			graphics "Score#" position: {w_width, y_centerdis}{
				int value <- days_with_ecolabel;
				float total <- 365.0 * end_of_game;
				float radius <- w_width/1.5;
				float start_angle <-  - 180.0;
				float arc_angle <- (value * 180/total);
				draw arc(radius, start_angle + arc_angle/2, arc_angle) color: ecolabel;
				start_angle <- start_angle + arc_angle;
				arc_angle <- (total - value) * 180/total;
				draw arc(radius, start_angle + arc_angle/2, arc_angle) color: ecolabel_second;
				draw arc(radius/2, -90, 180) color: #white;
				draw label_icon size: w_width / 5;
				draw ""+value  at: {location.x, location.y- 6*radius/10, 0.01}  color: ecolabel font: ui_font anchor: #bottom_center;
			}
		
			graphics "Timer for the discussion" visible: stage = PLAYER_DISCUSSION_TURN and turn <= end_of_game and use_timer_for_discussion {
				float y <- location.y + w_height/5 + y_centerdis;
				float left <- location.x - w_width/2;
				float right <- location.x + w_width/2;
				draw "" + int(remaining_time) + " s" color: timer_main font: ui_font anchor: #left_center at: {right + 500, y};
				draw line({left, y}, {right, y}) buffer (100, 200) color: timer_second;
				float width <- (initial_time_for_discussion - remaining_time) * (right - left) / (initial_time_for_discussion);
				draw line({left, y}, {left + width, y}) buffer (100, 200) color: timer_main;
				draw sandclock_icon /*rotate: (180 - remaining_time)*3*/ at: {left + width, y} size: w_height / 6;
			}
			
			graphics "Timer for the exploration" visible: stage = PLAYER_VR_EXPLORATION_TURN and turn <= end_of_game and use_timer_for_exploration{
				float y <- location.y + w_height/5 + y_centerdis;
				float left <- location.x - w_width/2;
				float right <- location.x + w_width/2;
				draw "" + int(remaining_time) + " s" color: timer_main font: ui_font anchor: #left_center at: {right + 500, y};
				draw line({left, y}, {right, y}) buffer (100, 200) color: timer_second;
				float width <- (initial_time_for_exploration - remaining_time) * (right - left) / (initial_time_for_exploration);
				draw line({left, y}, {left + width, y}) buffer (100, 200) color: timer_main;
				draw sandclock_icon /*rotate: (180 - remaining_time)*3*/ at: {left + width, y} size: w_height / 6;
			}
			
			graphics "Timer for the village choice" visible: CHOOSING_VILLAGE_FOR_POOL and turn <= end_of_game {
				float y <- location.y + 3*w_height/8 + y_centerdis;
				float left <- location.x - w_width/2;
				float right <- location.x + w_width/2;
				draw "" + int(remaining_time_for_choosing_village) + " s" color: timer_main font: ui_font anchor: #left_center at: {right + 500, y};
				draw line({left, y}, {right, y}) buffer (100, 200) color: timer_second ;
				float width <- (initial_time_for_choosing_village - remaining_time_for_choosing_village) * (right - left) / (initial_time_for_choosing_village);
				draw line({left, y}, {left + width, y}) buffer (100, 200) color: timer_main;
				draw sandclock_icon at: {left + width, y} size: w_height / 6;
			}	
	
			graphics "Choose village" visible: CHOOSING_VILLAGE_FOR_POOL {
				float y <- location.y + w_height/5 + y_centerdis;
				float left <- location.x - w_width/2;
				float right <- location.x + w_width/2;
				float gap <- (right - left) /4;
				float index <- 0.5;
				// Used as a mask for the position of the mouse
				draw rectangle(right-left,w_width / 8) color: legend_background at: {location.x, y};
				loop s over: numbers { 
					int village_index <- int(index - 0.5);
					bool selected <- village_selected = village_index;
					draw s size: w_width / 10 at: {left + gap * index, y};
					village_buttons[village_index] <- circle(w_width / 10) at_location {left + gap * index, y};
					if (selected) {
						draw village_buttons[village_index] wireframe: true width: 2 color: dark_theme ? #white : #black;
					}
	
					index <- index + 1;
				}
	
			}		
			
			graphics "Actions of players" visible: stage = PLAYER_ACTION_TURN and !CHOOSING_VILLAGE_FOR_POOL {
				village v <- villages_order[index_player];
				float y <- location.y + w_height/5 + 500 + y_centerdis;
				float left <- location.x - w_width + w_width / 5 - 2800;
				float right <- location.x + w_width - w_width / 5 + 2800;
				float gap <- (right - left) / length(actions_name_without_end);
				float index <- 0.5;
				// Used as a mask for the position of the mouse
				draw rectangle(right-left,w_width / 8) color: legend_background at: {location.x, y};
				
				loop s over: (sort(actions_name_without_end, each)) {
	
					bool selected <- village_actions[v] != nil and village_actions[v] contains s;
					//write sample(selected) + " " + sample(village_actions[v]) + " " + sample(s) + " " + sample(village_actions) + " " + sample(v);
					draw s color:  s = over_action or selected ? (dark_theme ? #white : #black) : (dark_theme ? rgb(255, 255, 255, 130) : rgb(0, 0, 0, 130)) font: ui_font anchor: #center at: {left + gap * index, y} depth: 1;
					if (selected) {
						draw circle(w_width / 10) wireframe: true width: 2 color: #black at: {left + gap * index, y, 0.1};
					}
					action_locations[s] <- {left + gap * index, y};
					index <- index + 1;
				}
	
			}
	
			graphics "Stage"  {
				image_file icon;
				if (stage = PLAYER_DISCUSSION_TURN) {
					icon <- discussion_icon; 
				} else if (stage = PLAYER_ACTION_TURN) {
					if (CHOOSING_VILLAGE_FOR_POOL) {
						icon <- tokens_icon;
					} else {
						icon <- numbers[int(villages_order[index_player])];
					}
				} else if (stage = PLAYER_VR_EXPLORATION_TURN) {
					icon <- vr_icon;
				} else {
					icon <- computer_icon;
				}
				draw icon size: w_width / 3 at:  {location.x, location.y-w_height/8 + y_centerdis};
			}
			
			graphics "Money" position: {0,0 } visible: CHOOSING_VILLAGE_FOR_POOL {
				float radius <- w_width/2;
				draw ""+commune_money  at: {location.x, location.y- 6*radius/10 + y_centerdis, 0.01}  color: dark_theme ? #gold : rgb (225, 126, 21, 255) font: ui_font anchor: #bottom_center;
			}
	
			graphics "Next" transparency: ((stage = PLAYER_DISCUSSION_TURN or stage = PLAYER_ACTION_TURN or stage = PLAYER_VR_EXPLORATION_TURN) and turn <= end_of_game) ? 0 : 0.6 {
				next_location <- {location.x + w_width / 2.5,  location.y-w_height/8};
				draw button_background at: next_location + {0, y_centerdis} color: (next_selected and ((stage = PLAYER_DISCUSSION_TURN or stage = PLAYER_ACTION_TURN or stage = PLAYER_VR_EXPLORATION_TURN) and turn <= end_of_game)) ? selected_color:unselected_color size: shape.width / 4;
				draw next_icon at: next_location + {100, y_centerdis} size: w_width / 8 color: (next_selected and ((stage = PLAYER_DISCUSSION_TURN or stage = PLAYER_ACTION_TURN or stage = PLAYER_VR_EXPLORATION_TURN) and turn <= end_of_game)) ? selected_color:unselected_color;
			}
	
			graphics "Play Pause" visible: turn <= end_of_game {
				pause_location <- {location.x - w_width / 2.5, location.y- w_height/8};
				draw button_background at: pause_location + {0, y_centerdis} color: play_pause_selected ? selected_color:unselected_color size: shape.width / 4;
				draw simulation.paused or about_to_pause ? play_icon : pause_icon at: simulation.paused or about_to_pause ? pause_location + {100,y_centerdis}: pause_location + {0, y_centerdis} color: play_pause_selected ? selected_color:unselected_color size: shape.width / 8;
			}
			
			graphics "Button chart by village" visible: turn <= turn_see_indicators or always_display_chart_by_vil {
				float x <- 2.0;
				float y <- 0.5;
				show_chart_vil_button <-  circle(w_width/8) at_location {x*w_width, location.y- w_height/8 + y_centerdis};
				draw image_file(show_chart_by_vil ? "../../includes/icons/Visibility_off.png":"../../includes/icons/Visibility_on.png") color: show_chart_vil_selected ? selected_color:unselected_color size: w_width/3.5 at: show_chart_vil_button.location + {400,0};
			}
			
			event #mouse_move {
				using topology(simulation) {
					show_chart_vil_selected <- (show_chart_vil_button covers #user_location) ;
				}
			}
			
			event #mouse_exit {
					show_chart_vil_selected <- false;
			}
			
			event #mouse_down {
				if (show_chart_vil_selected) {
					show_chart_by_vil <- !show_chart_by_vil;
				} 
			}
				
				
			event "1" {
				ask simulation {
					do execute_action(A_1);
				}
			}
			event "2" {
				ask simulation {
					do execute_action(A_2a);
				}
			}
			event "a" {
				ask simulation {
					do execute_action(A_2b);
				}
			}
			event "3" {
				ask simulation {
					do execute_action(A_3);
				}
			}
			event "4" {
				ask simulation {
					do execute_action(A_4);
				}
			}
			event "5" {
				ask simulation {
					do execute_action(A_5a);
				}
			}
			event "r" {
				ask simulation {
					do execute_action(A_5b);
				}
			}
			event "6" {
				ask simulation {
					do execute_action(A_6);
				}
			}
			event "7" {
				ask simulation {
					do execute_action(A_7a);
				}
			}
			event "y" {
				ask simulation {
					do execute_action(A_7b);
				}
			}
			event "8" {
				ask simulation {
					do execute_action(A_8a);
				}
			}
			event "u" {
				ask simulation {
					do execute_action(A_8b);
				}
			}
			event "9" {
				ask simulation {
					do execute_action(A_9);
				}
			}
						
			event #mouse_exit {
				next_selected <- false;
				play_pause_selected <- false;				
			}
	
			event #mouse_move {
				using topology(simulation) {
					if (stage = PLAYER_ACTION_TURN) {
						if (CHOOSING_VILLAGE_FOR_POOL) {
							loop s over: [0, 1, 2, 3] {
								if ((village_buttons[s] covers #user_location)) {
									village_selected <- s;
									return;
								}
							}
						} else {
							loop s over: action_locations.keys {
								if (action_locations[s] distance_to #user_location < w_width / 8) {
									over_action <- s;
									return;
								}
							}
						}
					}
					next_selected <- (next_location distance_to #user_location) < w_width / 5;
					play_pause_selected <- (pause_location distance_to #user_location) < w_width / 5;
					over_action <- nil;
					village_selected <- -1;
				}
			}
	
			event #mouse_down {
				if (stage = PLAYER_ACTION_TURN and over_action != nil) {
					ask simulation {
						do execute_action(over_action);
					}
					over_action <- nil;
					return;
				}
				if (CHOOSING_VILLAGE_FOR_POOL and village_selected > -1) {
					chosen_village <- village_selected;
					village_selected <- -1;
					return;
				}
				using topology(simulation) {
					if (next_location distance_to #user_location) < w_width / 5 {
						if (turn > end_of_game) {
							return;
						}
						if (stage = PLAYER_DISCUSSION_TURN) {
							ask simulation {
								do end_of_discussion_phase;
							}
						} else if (stage = PLAYER_VR_EXPLORATION_TURN) {
							ask simulation {
								do end_of_exploration_phase;
								if !always_display_sub_charts {
									show_pol_chart_by_cat_glob <- true;
								}
							}
						} else if (stage != COMPUTE_INDICATORS) {
							if (CHOOSING_VILLAGE_FOR_POOL) {
								PASS_CHOOSING_VILLAGE <- true;
							} else {
								ask simulation {
									ask villages_order[index_player] {
										do end_of_turn;
									}
								}
							}
						}
	
					} else if (pause_location distance_to #user_location) < w_width / 5 {
						ask simulation {
							if paused or about_to_pause {
								if (pause_started_time > 0) {
									time_for_discussion <- time_for_discussion + int((gama.machine_time - pause_started_time) / 1000);
								}
								pause_started_time <- 0.0;
								do resume;
							} else {
								pause_started_time <- gama.machine_time;
								do pause;
							}
						}
					}
				}
			}
		}
	
	
	
		/********************** MAIN MAP DISPLAY ***************************************************/
		
		display "MAIN MAP" type: 3d background:map_background axes: false {
			
			overlay position: {0.5, 1} size: {0,0} transparency: 0 {
				float x_gap <- 0.1/3;
				float x_init <- 0.35/3;
				float icon_size <-  w_height / 18;
				float y <- 0.2/1.1;
				float x <- x_init;
				
				if (stage = PLAYER_VR_EXPLORATION_TURN) {
					
					//Legend Player position
					draw player at: {x* w_width,y*w_height} size: icon_size*1.5;
					x <- x + 2.3* x_gap;
					draw "Player" at: {x* w_width,y*w_height} color: player_color depth: 0 font: ui_font anchor: #center;
					
					x <- x_init + x_gap * 8.5;
					
					//Legend Point of interest
					draw interest at: {x*w_width,y*w_height} size: icon_size*1.5;
					x <- x + 4*x_gap;
					draw "Point Of Interest" at: {x* w_width,y*w_height} color: point_of_interest_color depth: 0 font: ui_font anchor: #center;
					
				} else {
					
					//Legend Productivity
					draw plant_icon at: {x* w_width,y*w_height} size: icon_size;
					x <- x + 2* x_gap;
					loop c over: reverse(greens) {
						draw square(x_gap*w_width) color: c at: {x*w_width,y*w_height};
						x <- x + x_gap;
					}
					
					x <- x_init + x_gap * 8.5;
					
					//Legend Urban areas
					draw city_icon at: {x*w_width,y*w_height} size:  icon_size;
					x <- x + 2*x_gap;
					draw square(x_gap*w_width) color: city_color at: {x* w_width,y*w_height};
					
				}
				
			}
			
			light #ambient intensity: ambient_intensity;
			camera 'default' distance: 7700 location: #from_above target: {3000,2700,0};
			
			/********************** MAIN MAP DISPLAY ******************************/
			species plot visible: !(stage = PLAYER_VR_EXPLORATION_TURN)  {
				draw shape color: greens[world.production_class_current(self)] border: false;
			}
			species canal visible: !(stage = PLAYER_VR_EXPLORATION_TURN) {
				draw shape buffer (20,10) color: display_water_flow ? river : blues[world.water_pollution_class_current(self)]  ;
			}
			species waste_on_canal visible: !(stage = PLAYER_VR_EXPLORATION_TURN) and display_water_flow  {
					draw sphere(20) color: #lightblue;
			}
			species urban_area visible: !(stage = PLAYER_VR_EXPLORATION_TURN);
			
			species village visible: !(stage = PLAYER_VR_EXPLORATION_TURN){
				float size <- w_width/10;
				draw numbers[int(self)] at: shape.centroid + position[int(self)] size: w_width/10;
				draw shape-(shape-40) color: color;
			}	
			species village position: {0,0,0.01} visible: !(stage = PLAYER_VR_EXPLORATION_TURN) {
				int i <- int(self);
				float size <- w_width/20;
				float spacing <- size * 1;
				float x <- shape.centroid.x + position2[int(self), 0] + 700 - spacing;
				float y <- shape.centroid.y + position2[int(self), 1] + 600 - spacing;
				
				draw tokens_icon at: {x,  y} size: size;
				draw ""+budget at: {x, y + spacing/1.25} color: #black depth: 0 font: ui_font anchor: #center;
			}
			
			/********************** MINI MAP DISPLAY ******************************/
			image minimap size: {0.835,0.99} position:{0.1,0} visible: stage = PLAYER_VR_EXPLORATION_TURN;
			species default_player visible: stage = PLAYER_VR_EXPLORATION_TURN;
			species pointInterest;
			
			
			event #mouse_down action: affiche_coord;
			
		}
	
	
		/********************** CHARTS DISPLAY ***************************************************/
	
		display "Chart 4"  type: 3d axes: false background: map_background refresh: (stage = COMPUTE_INDICATORS and every(data_frequency#cycle)) or (stage = PLAYER_DISCUSSION_TURN and !always_display_sub_charts)  {
			
			light #ambient intensity: ambient_intensity;
			camera #default locked: true;
			
			agents "Global" value: [global_chart] aspect: horizontal visible: show_chart position: {0.25,0} size: {0.7,0.7} transparency:0;
			
		}		
	}
}
