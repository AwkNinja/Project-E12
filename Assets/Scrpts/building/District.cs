﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class District : Building
{
    public enum Type{
        camp,
        chiefs_hut,
        village_green,
        growth,
        military,
        knowledge,
        Forge,
        market_place,
        shipyard,
        air_shipyard
    }
    //List<Building> buildings;
    public int buildingStartingIndex, numOfBuildings;
    public int buildingProduction=-1;
    public int limitOfBuildings=3;
    public Type type;
    public int level;
    public int daysToBeProduced=2;
    public string source;
    public int daysTillProduced=-1;
    public Vector2 buildingLocation;
    [HideInInspector]
    public bool positionSelectingMode=false;
    public List<Building> buildings;
    //public int maxPopulation;


    public void setCamp(){
        city = GetComponent<City>();
        //setting up yields
        resourcesYield = new Resources();
        resourcesYield.resources= new DictionaryResFloat();
        resourcesYield.resources.Add(Resource.production,2);
        resourcesYield.resources.Add(Resource.food,2);
        resourcesYield.Energies= new DictionaryEnergyFloat();
        resourcesYield.Energies.Add(Energy.Fire,1);
        resourcesYield.OtherResources = new DictionaryOtherResFloat();
        resourcesYield.OtherResources.Add(OtherResource.alchemy,0.5f);

        //maxPopulation = 7;

        //type
        type = Type.camp;
        setYield();
    }
    public void setVillage(){
        removeYield();
        resourcesYield = new Resources();
        //setting up yields
        resourcesYield.resources = new DictionaryResFloat();
        resourcesYield.resources.Add(Resource.production,2);
        resourcesYield.resources.Add(Resource.food,2);
        resourcesYield.Energies= new DictionaryEnergyFloat();
        resourcesYield.Energies.Add(Energy.Fire,1);
        resourcesYield.OtherResources = new DictionaryOtherResFloat();
        resourcesYield.OtherResources.Add(OtherResource.approval,2);
        resourcesYield.OtherResources.Add(OtherResource.alchemy,1);

        //maxPopulation = 10;

        type = Type.chiefs_hut;
        setYield();
    }

    public override void setYield(){
        if(resourcesYield.Energies!=null){
            foreach(KeyValuePair<Energy, float> entry in resourcesYield.Energies){
                city.resourcesYield.Energies[entry.Key]+=resourcesYield.Energies[entry.Key];
            }
        }
        //checking if Resource cost is met
        if(resourcesYield.resources!=null){
            foreach(KeyValuePair<Resource, float> entry in resourcesYield.resources){
                city.resourcesYield.resources[entry.Key]+=resourcesYield.resources[entry.Key];
            }
        }
        //checking if Raw material cost is met
        if(resourcesYield.RawMaterials!=null){
            foreach(KeyValuePair<Raw_Material,float> entry in resourcesYield.RawMaterials){
                city.resourcesYield.RawMaterials[entry.Key]+=resourcesYield.RawMaterials[entry.Key];
            }
        }
        //checking if otherResource cost is met
        if(resourcesYield.OtherResources!=null){
            foreach(KeyValuePair<OtherResource,float> entry in resourcesYield.OtherResources){
                city.resourcesYield.OtherResources[entry.Key]+=resourcesYield.OtherResources[entry.Key];
            }
        }
        //checking if otherResource cost is met
        if(resourcesYield.Crystals!=null){
            foreach(KeyValuePair<crystal,float> entry in resourcesYield.Crystals){
                city.resourcesYield.Crystals[entry.Key]+=resourcesYield.Crystals[entry.Key];
            }
        }
    }
    public override void removeYield(){
        //checking if energy cost is me

        if(resourcesYield.Energies!=null){
            foreach(KeyValuePair<Energy, float> entry in resourcesYield.Energies){
                city.resourcesYield.Energies[entry.Key]-=resourcesYield.Energies[entry.Key];
            }
        }
        //checking if Resource cost is met
        if(resourcesYield.resources!=null){
            foreach(KeyValuePair<Resource, float> entry in resourcesYield.resources){
                city.resourcesYield.resources[entry.Key]-=resourcesYield.resources[entry.Key];
            }
        }
        //checking if Raw material cost is met
        if(resourcesYield.RawMaterials!=null){
            foreach(KeyValuePair<Raw_Material,float> entry in resourcesYield.RawMaterials){
                city.resourcesYield.RawMaterials[entry.Key]-=resourcesYield.RawMaterials[entry.Key];
            }
        }
        //checking if otherResource cost is met
        if(resourcesYield.OtherResources!=null){
            foreach(KeyValuePair<OtherResource,float> entry in resourcesYield.OtherResources){
                city.resourcesYield.OtherResources[entry.Key]-=resourcesYield.OtherResources[entry.Key];
            }
        }
        //checking if otherResource cost is met
        if(resourcesYield.Crystals!=null){
            foreach(KeyValuePair<crystal,float> entry in resourcesYield.Crystals){
                city.resourcesYield.Crystals[entry.Key]-=resourcesYield.Crystals[entry.Key];
            }
        }
    }

    public void ProduceBuilding(int index,int days, Vector2 loc){
        buildingProduction = index;
        buildingLocation = loc;
        daysTillProduced = days;

        //this place is considered occupied even before it is built
        GameState.Instance.occupiedHexes.Add(loc);
        
    }
    public override void StartTurn(){
        if(daysTillProduced!=-1)
            daysTillProduced--;
            
        if(buildingProduction!=-1 && daysTillProduced==0){
            //AddItem(getPlayer().kingdom.itemPrefabs[itemProduction]);
            buildings.Add(getPlayer().kingdom.districts[buildingProduction]);
            GameObject buil = HexOperations.Instance.BuildBuilding(buildingLocation,buildingProduction,city);
            //setting the yield of this building
            buil.GetComponent<Building>().setYield();


            //destroying the info object in the producing slot if this object is selected
            
            buildingProduction=-1;
            daysTillProduced = -1;
            buildingLocation=Vector2.zero;
            if(GameState.Instance.selectedObject==this.gameObject){
                UIController.Instance.openDistrictHub();
            }
        }
    }
    private Player getPlayer(){
        Player tempPlayer = PlayerController.Instance.player;
        if(player!=-1)
            tempPlayer = AIController.Instance.AIPlayers[player];
        
        return tempPlayer;
    }
}
