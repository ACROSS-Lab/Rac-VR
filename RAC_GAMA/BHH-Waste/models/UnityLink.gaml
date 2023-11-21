/**
* Name: UnityLink
* Includes actions, attributes and species facilating the link with Unity. To be used with the GAMA-Unity-VR Package for Unity
* Author: Patrick Taillandier
* Tags: Unity, VR
*/

@no_experiment
model UnityLink


/*global skills: [network]{
	 
	//Activate the unity connection; if activated, the model will wait for an connection from Unity to start
	bool connect_to_unity <- false;
	
	// connection port
	int port <- 8000;
	
	string end_message_symbol <- "&&&";
	
	//as all data (location, rotation) are send as int, number of decimal for the data
	int precision <- 10000;
	
	//possibility to add a delay after moving the player (in ms)
	float delay_after_mes <- 0.0;
	
	//allow to reduce the quantity of information sent to Unity - only the agents at a certain distance are sent
	float player_agent_perception_radius <- 0.0;
	
	//allow to not send to Unity agents that are to close (i.e. overlapping) 
	float player_agents_min_dist <- 0.0;
	
	//the list of agents to sent to Unity. It could be updated each simulation step
	list<agent_to_send> agents_to_send;
	
	//the list of static geometries sent to Unity. Only sent once at the initialization of the connection 
	list<geometry> background_geoms;
	
	// for each geometry sent to Unity, the height of this one.
	list<int> background_geoms_heights;
	
	// for each geometry sent to Unity, does this one has a collider (i.e. a physical existence) ? 
	list<bool> background_geoms_colliders;
	
	// for each geometry sent to Unity, its name in unity 
	list<string> background_geoms_names;
	
	// Information to send to Unity
	list<int> solidwasteSoilClass;
	list<int> solidwasteCanalClass;
	list<int> waterwasteCanalClass;
//	list<float> waterwasteVillageValue;
//	list<float> waterwasteCanalValue;
	
	list<int> solidwasteClass;
	list<int> waterwasteClass;
	list<int> productionClass;
//	list<float> waterwasteValue;
	
	// Information received from Unity
	int choice <- -1;
	int nb_waste;
	
	//allows to manage during which phase GAMA sends/receives information
	bool classUpdatedTour <- false; //for sending the indicators information only once after the indicators computation stage
	bool enter_or_exit_VR <- false; //for receiving position only in the VR phase
	bool hasRestarted <- false;
	
	bool do_send_world <- false;
	
	bool connected_to_unity <- false;
	
	string language;
	string mode;
	
	bool send_help <- false;
	
	

	//allow to create a player agent
	bool create_player <- true;

	//let the player moves in GAMA as it moves in Unity
	bool move_player_from_unity <-true;
	
	//does the player should has a physical exitence in Unity (i.e. cannot pass through specific geometries)
	bool use_physics_for_player <- true;
	
	//init location of the player in the environment - this information will be sent to Unity to move the player accordingly
	point location_init <- {5, -8};

	//player size - only used for displaying the player in GAMA
	float player_size_GAMA <- 300.0;
	
	//player rotation - only used for displaying the player in GAMA
	int rotation_player <- 0;
	
	
	
	 
	//message send by Unity to tell GAMA that it is ready
	string READY <- "ready" const: true;

	// should GAMA receive information from Unity 
	bool receive_information <- true;
	
	// which message GAMA should wait before receiving infomation 
	string waiting_message <- nil;
	
	// the unity clienf 
	unknown unity_client;
	
	//does the Unity client has been initialized?
	bool initialized <- false;
	
	// the player agent
	default_player the_player;
	
	//has the player just moved?
	bool move_player_event <- false;
	
	//the last received position of the player ([x,y,rotation])
	list<int> player_position <- [];
	
	//Benchmark	
	float t1;
	float t2;
	float t3;
	
	//transformation of the position send by unity for the minimap on GAMA
	point translate_coord(point p){
		point o <- {3148.3, 3186.2};
		
		float a <- 121.4599;
		float b <- -2.5249;
		float c <- 8.4725;
		float d <- -103.2179;
		
		float x <- a*p.x + b*p.y + o.x;
		float y <- c*p.x + d*p.y + o.y;
		return {x,y} ;
	}
	
	//used to display well the cone of vision
	float transform_rot(float r){
		return r - 90;
	}
	
	//creation of the player agent - can be overrided
	action init_player {
		create default_player  {
			the_player <- self;
			location <- world.translate_coord(location_init);
		}
	}
	
	//add background geometries from a list of geometries, their heights, their collider usage, their outline rendered usage 
	action add_background_data(list<geometry> geoms, float height, bool collider) {
		do add_background_data_with_names(geoms, [],  height,collider) ;
	}
	
	//add background geometries from a list of geometries, their heights, their collider usage, their outline rendered usage 
	action add_background_data_with_names(list<geometry> geoms, list<string> names, float height, bool collider) {
		background_geoms <- background_geoms + geoms;
		loop times: length(geoms) {
			background_geoms_heights << height;
			background_geoms_colliders << collider;
		}	
		background_geoms_names  <- background_geoms_names +  names;
	}
	
	//Wait for the connection of a unity client and send the paramters to the client
	action send_init_data {
		do connect protocol: "tcp_server" port: port raw: true with_name: "server" force_network_use: true;
		write "waiting for the client to send a connection confirmed message";
		loop while: !has_more_message() {
			do fetch_message_from_network;
		}
		loop while: has_more_message() {
			message s <- fetch_message();
			unity_client <- s.sender;
		}
		do send_parameters;
		if (delay_after_mes > 0.0) {
			do wait_for_message(READY);
		}
		loop while: !has_more_message() {
			do fetch_message_from_network;
		}
		loop while: has_more_message() {
			message s <- fetch_message();
		}
		write "connection established";
		connected_to_unity <- true;
		
		if not empty(background_geoms) {
			do send_geometries(background_geoms, background_geoms_heights,  background_geoms_colliders, background_geoms_names, precision);
		}
		if do_send_world {
			do send_world;
		}
		
	}
	
	//send parameters to the unity client
	action send_parameters {
		map to_send;
		to_send <+ "precision"::precision;
		to_send <+ "world"::[world.shape.width * precision, world.shape.height * precision];
		to_send <+ "delay"::delay_after_mes;
		to_send <+ "physics"::use_physics_for_player;
//		to_send <+ "position"::[int(location_init.x*precision), int(location_init.y*precision)];
		to_send <+ "language"::language;
		to_send <+ "mode"::mode;

		if unity_client = nil {
			write "no client to send to";
		} else {
			do send to: unity_client contents: as_json_string(to_send) + end_message_symbol;	
		}
	}
	
	//send indicators' class to the unity client
	action send_indicators {
		map to_send;
		to_send <+ "solidwasteSoilClass"::solidwasteSoilClass;
		to_send <+ "solidwasteCanalClass"::solidwasteCanalClass;
		to_send <+ "waterwasteClass"::waterwasteClass;
		
		to_send <+ "productionClass"::productionClass;
		
		// To use, define categories
		//to_send <+ "waterwasteVillage"::waterwasteVillageValue;
		//to_send <+ "waterwasteCanal"::waterwasteCanalValue;
		//to_send <+ "waterwasteCanalClass"::waterwasteCanalClass;
		
		
		if unity_client = nil {
			write "no client to send to";
		} else {
			do send to: unity_client contents: as_json_string(to_send) + end_message_symbol;	
		}
	}
	
	action after_sending_background ;
	
	//send the background geometries to the Unity client
	action send_geometries(list<geometry> geoms, list<int> heights, list<bool> geometry_colliders, list<string> names, int precision_) {
		map to_send;
		list points <- [];
		
		loop g over: geoms {
			loop pt over:g.points {
				points <+ map("c"::[int(pt.x*precision_), int(pt.y*precision_)]);
			}
			points <+ map("c"::[]);
		}
		
		to_send <+ "points"::points;
		to_send <+ "heights"::heights;
		to_send <+ "hasColliders"::geometry_colliders;
		to_send <+ "names"::names;
		
		if unity_client = nil {
			write "no client to send to";
		} else {
			do send to: unity_client contents: as_json_string(to_send) + end_message_symbol;	
		}
		do after_sending_background;
	}
	
	//send the new position of the player to Unity (used to teleport the player from GAMA) 
	action send_player_position {
		if (!connect_to_unity) {
			return;
		}
		player_position <- [int(the_player.location.x * precision), int(the_player.location.y * precision)];
		if (delay_after_mes > 0.0) {
			do wait_for_message(READY);
		}
	} 
	
	//set the message to wait
	action wait_for_message(string mes) {
		receive_information <- false;
		waiting_message <- mes;
	}
	
	
	//message that will be sent concerning the agents
	list<map> message_agents(container<agent_to_send> ags_input) {
		list<map> ags;
		ask ags_input {
			ags <+ to_array(precision);
		}
		return ags;
	}
	
	//filter the agents to send according to the player_agent_perception_radius - can be overrided 
	list<agent_to_send> filter_distance(list<agent_to_send> ags) {
		ask the_player {
			ags <- ags at_distance player_agent_perception_radius;
		}
		return ags;
		
	}

	//filter the agents to send to avoid agents too close to each other - can be overrided 
	list<agent_to_send> filter_overlapping(list<agent_to_send> ags) {
		list<agent_to_send> to_remove ;
		ask ags {
			if not(self in to_remove) {
				to_remove <- to_remove + ((ags at_distance player_agents_min_dist));
			}  
		}
		return ags - to_remove;	
	}
	
	
	//send the current state of the world to the Unity Client
	action send_world {
		map to_send;
		list message_ags <- [];
		list<agent_to_send> ags <- copy(agents_to_send where not dead(each));
			
		if (the_player != nil) {
			if (player_agent_perception_radius > 0) {
				ags <- filter_distance(ags);
			}
			if (player_agents_min_dist > 0 ){
				ags <- filter_overlapping(ags);
			} 
		}
		message_ags<-message_agents(ags) ;
			
		to_send <+ "date"::"" + current_date;
		to_send <+ "agents"::message_ags;
		to_send <+ "position"::player_position;
		player_position <- [];
		
		if unity_client = nil {
			write "no client to send to";
		} else {
			string mes <- as_json_string(to_send);
			do send to: unity_client contents: (mes + end_message_symbol) ;	
		}
		do after_sending_world;
	}
	
	action after_sending_world ;
	
	point new_player_location(point loc) {
		return loc;
	}
	
	
	//if necessary, move the player to its new location
	reflex move_player when: move_player_event{
		the_player.location <- new_player_location(#user_location);
		do send_player_position;
		move_player_event <- false;
	}

	//send the new world situtation to the Unity client
	reflex send_update_to_unity when: connect_to_unity {
		float t <- machine_time;
		if !initialized {
			if create_player {
				do init_player;
			}
			do send_init_data;
			initialized <- true;
		}
		if do_send_world {
			do send_world;
		}
		if classUpdatedTour {
			do send_indicators;
			classUpdatedTour <- false;
		}
		if enter_or_exit_VR {
			if unity_client = nil {
				write "no client to send to";
			} else {
				do send to: unity_client contents: "Enter_or_exit_VR" + end_message_symbol;	
			}
			enter_or_exit_VR <- false;
		}
//		if hasRestarted {
//			if unity_client = nil {
//				write "no client to send to";
//			} else {
//				do send to: unity_client contents: "Restart" + end_message_symbol;	
//			}
//			hasRestarted <- false;
//		}
		if send_help {
			do send to: unity_client contents: "Help" + end_message_symbol;	
			send_help <- false;
		}
		
		
		t1<- t1 + machine_time - t;
		
	}
	
//	reflex info when: cycle mod 100 =0 {
//		write (sample(t1) + "  " + sample(t2) + "  " + sample(t3));
//	}
	
	action manage_message_from_unity(message s) {
//		write "s: " + s.contents;
		if (waiting_message != nil and string(s.contents) = waiting_message) {
	    	receive_information <- true;
//	    } else if  the_player != nil and move_player_from_unity and receive_information {
//	    	let answer <- map(s.contents);
//			list<int> position <- answer["position"];
//			if position != nil and length(position) = 2  {
//				the_player.rotation <- int(transform_rot(int(answer["rotation"])/precision));
//				the_player.location <- translate_coord({position[0]/precision, position[1]/precision});
//				the_player.to_display <- true;
////				write sample(the_player.rotation);
//			}
		} else if the_player != nil and receive_information {
			let answer <- map(s.contents);
			if move_player_from_unity and answer contains_key "position" {
				list<int> position <- answer["position"];
				if position != nil and length(position) = 2  {
					the_player.rotation <- int(transform_rot(int(answer["rotation"])/precision));
					the_player.location <- translate_coord({position[0]/precision, position[1]/precision});
					the_player.to_display <- true;
//					write sample(the_player.location);
				}
			} else if answer contains_key "choice" {
				write "s: " + s.contents;
				choice <- int(answer["choice"]);
				nb_waste <- int(answer["nb_waste"]);
			} else if answer contains_key "point_of_interest" {
				write "s: " + s.contents;
				ask pointInterestManager {
					do changeState(int(answer["point_of_interest"]));
				}
			} else if answer contains_key "pnj_pos" {
				write "s: " + s.contents;
				list<int> positionPNJ <- answer["pnj_pos"];
				ask pointInterestManager {
					do setLocation(int(answer["pnj_id"]), world.translate_coord({positionPNJ[0]/precision,positionPNJ[1]/precision}));
				}
			}
		}
	}
	//received informtation about the player from Unity
	reflex messages_from_unity  {
		float t <- machine_time;
		if has_more_message(){
			
		loop while: has_more_message() {
			message s <- fetch_message();
			do manage_message_from_unity(s);
		}
		
		}
		t2<- t2 + machine_time - t;
		
	}	
}

species pointInterestManager {
	list<pointInterest> points <- [];
	
	action changeState(int i) {
		points[i].visited <- true;
	}
	
	action setLocation(int i, point p) {
		points[i].location <- p;
	}
}

species pointInterest {
	rgb color <- rgb(217, 104, 76);
	point location;
	float size <- 300.0;
	bool visited <- false;
	pointInterestManager manager;
	
	action addSelfToManager {
		add self to: manager.points;
	}
	
	aspect default {
		if !visited {
			if file_exists("../../includes/icons/Icone_PointOfInterest.png")  {
				draw image("../../includes/icons/Icone_PointOfInterest.png") size: {size, size} at: location + {0, 0, 5};
			} else {
				draw circle(size/2) at: location + {0, 0, 5} color: color;
			}
		}	
	}
}

//Defaut species for the player
species default_player {
	rgb color <- rgb(223, 204, 76);
	int rotation;
	bool to_display <- true;
	float cone_distance <- 2 * player_size_GAMA;
	int cone_amplitude <- 90;
	
	
	aspect default {
		if to_display {
			draw cone(rotation - cone_amplitude/2,rotation + cone_amplitude/2) inter circle(cone_distance)  translated_by {0,0,4.9} color: rgb(223, 204, 76, 0.5);
			if file_exists("../../includes/icons/Icone_Player.png")  {
				draw image("../../includes/icons/Icone_Player.png")  size: {player_size_GAMA, player_size_GAMA} at: location + {0, 0, 5} ;
			} else {
				draw circle(player_size_GAMA/2) at: location + {0, 0, 5} color: color rotate: rotation - 90;
			}
		}			
	}
}

//Default species for the agent to be send to the Unity Client
species agent_to_send skills: [moving]{
	int index_species <- 0;
	
	point loc_to_send {return location;}
	map to_array(int precision_) {
		point loc <- loc_to_send();
		return map("v"::[index_species, int(self), int(loc.x*precision_), int(loc.y*precision_), int(heading*precision_)]);
	}
}

//Default xp with the possibility to move the player
experiment vr_xp virtual: true  {
	action move_player {
		move_player_event <- true;
	}
}*/

