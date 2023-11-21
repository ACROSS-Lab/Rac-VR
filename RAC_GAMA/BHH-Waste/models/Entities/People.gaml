/**
* Name: People
* Based on the internal empty template. 
* Author: Patrick Taillandier
* Tags: 
*/


model People

import "../Global.gaml"


/* Insert your model definition here */


species farmer parent: inhabitant {
	rgb color <- #orange;
	plot my_plot;
}

species inhabitant { 
	rgb color <- #midnightblue;
	cell my_house;
	canal closest_canal;
	float water_filtering <- water_waste_filtering_inhabitants;
	float solid_waste_day <- solid_waste_year_inhabitants / 365;
	float water_waste_day <- water_waste_year_inhabitants / 365;
	float part_solid_waste_canal <- part_solid_waste_canal_inhabitants;
	float part_water_waste_canal <- part_water_waste_canal_inhabitants;
	list<cell> my_cells;
	village my_village;
	float environmental_sensibility <- 0.0;
	aspect default {
		draw circle(10.0) color: color border:color-25;
	}
	
	float waste_for_a_day {
		return solid_waste_day;
	} 
	
	//Propagate domestic waste in the canal and ground
	action domestic_waste_production (float solid_waste_canal, float solid_waste_ground, float water_waste_canal, float water_waste_ground) {
		
		if solid_waste_canal > 0 {
			if collect_in_canal {
				if nb_waste >= 20 {
					closest_canal.solid_waste_level <- closest_canal.solid_waste_level * (1 - high_collection_canal_solid_waste) ;
				} else if 10 <= nb_waste {
					closest_canal.solid_waste_level <- closest_canal.solid_waste_level * (1 - mid_collection_canal_solid_waste) ;
				} else if 0 < nb_waste {
					closest_canal.solid_waste_level <- closest_canal.solid_waste_level * (1 - low_collection_canal_solid_waste) ;
				}
			}
			closest_canal.solid_waste_level <- closest_canal.solid_waste_level + solid_waste_canal;
		}
		if solid_waste_ground > 0 {
			ask one_of(my_cells) {
				if collect_on_ground {
					if nb_waste >= 20 {
						solid_waste_level <- solid_waste_level * (1 - high_collection_ground_solid_waste) ;
					} else if 10 <= nb_waste {
						solid_waste_level <- solid_waste_level * (1 - mid_collection_ground_solid_waste) ;
					} else if 0 < nb_waste {
						solid_waste_level <- solid_waste_level * (1 - low_collection_ground_solid_waste) ;
					}
				}
				solid_waste_level <- solid_waste_level + solid_waste_ground;
			}
		}
		if water_waste_canal > 0 {
			closest_canal.water_waste_level <- closest_canal.water_waste_level + water_waste_canal;
		}
		if water_waste_ground > 0 {
			ask one_of(my_cells) {
				water_waste_level <- water_waste_level + water_waste_ground ;
			}
		}
	}
	
	//Calculate values of domestic waste by inhabitants
	list<float> typical_values_computation {
		list<float> typical_values;
	
	// SOLID WASTE
		float solid_waste_day_tmp <- waste_for_a_day();
		
		//Sensibilization effects
		if (environmental_sensibility > 0) {
			solid_waste_day_tmp <- solid_waste_day_tmp * ( 1 - world.sensibilisation_function(environmental_sensibility));
		}
			
		if solid_waste_day_tmp > 0 {
			float to_the_canal <- solid_waste_day_tmp * part_solid_waste_canal;
			typical_values<< to_the_canal;
			typical_values<< solid_waste_day_tmp - to_the_canal;
		}
		
	// WATER WASTE
	
		//Facility treatment effects
		float rate_decrease_due_to_treatment <- 0.0;
		if (my_village != nil and  my_village.treatment_facility_is_activated) {
			rate_decrease_due_to_treatment <- treatment_facility_decrease[my_village.treatment_facility_year - 1];
		} 
		
		if water_waste_day > 0 {
			float w <- (1 - water_filtering) * water_waste_day;
			float to_the_canal <- w * part_water_waste_canal ;
			typical_values << to_the_canal * (1 - rate_decrease_due_to_treatment);
			typical_values << w - to_the_canal; 
			
		}
		return typical_values;
			
	}
}
