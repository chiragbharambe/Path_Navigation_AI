using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TerrainComposerRuntime : MonoBehaviour 
{
	float frames;	
	public int targetFrames = 90;
	 
	GameObject TerrainComposer;
	object tc_script;
	
	GameObject TerrainComposerClone;
	object tc_script2;
	 
	GUIStyle myStyle;
	bool autoSpeed = true;
	object item;
	float strength;
	
	void Start () 
	{
		TerrainComposer = GameObject.Find("TerrainComposer_Save");
		tc_script = TerrainComposer.GetComponent("terraincomposer_save");
		
		tc_script.GetType().GetField("heightmap_output").SetValue(tc_script, false);
		tc_script.GetType().GetField("splat_output").SetValue(tc_script, false);
		
		tc_script.CallMethod ("disable_outputs");
		myStyle = new GUIStyle();
	}
	
	void Update () 
	{
		frames = 1/Time.deltaTime;
		
		if (Input.GetKeyDown(KeyCode.Escape)){Application.Quit();} 
		
		if (tc_script2 != null)
		{
			if ((bool)tc_script2.GetFieldValue("generate"))
			{
				tc_script2.CallMethod("generate_output", tc_script2.GetFieldValue("prelayers").CallMethod("get_Item", 0));
			}
			else
			{
				// generation is ready so the clone can be destroyed
				terrainsFlush();
			}
		}
	}
	
	void OnGUI()
	{
		if (GUI.Button(new Rect(5,5,150,25),"Generate Heightmap"))
		{
			createClone();
			tc_script2.GetType().GetField("heightmap_output").SetValue(tc_script2, true);
			generateStart();
		}
		if (GUI.Button(new Rect(305,5,150,25),"Reset Heightmap"))
		{
			tc_script.CallMethod("terrain_reset_heightmap", tc_script.GetFieldValue("terrains").CallMethod("get_Item", 0),true);
		}
		if (GUI.Button(new Rect(155,5,150,25),"Randomize Heightmap"))
		{
			Random.seed = (int)Time.time;
			
			item = tc_script.GetFieldValue("prelayers").CallMethod("get_Item",0).GetFieldValue("layer").CallMethod("get_Item",0);
		    item.GetType().GetField("offset").SetValue(tc_script.GetFieldValue("prelayers").CallMethod("get_Item",0).GetFieldValue("layer").CallMethod("get_Item",0),new Vector2(Random.Range(-2000,2000),Random.Range(-2000,2000)));
			
			item = tc_script.GetFieldValue("prelayers").CallMethod("get_Item",0).GetFieldValue("layer").CallMethod("get_Item",1);
		    item.GetType().GetField("offset").SetValue(tc_script.GetFieldValue("prelayers").CallMethod("get_Item",0).GetFieldValue("layer").CallMethod("get_Item",0),new Vector2(Random.Range(-2000,2000),Random.Range(-2000,2000)));
		}
		
		if (GUI.Button(new Rect(5,30,150,25),"Generate Splatmap"))
		{
			createClone();
			tc_script2.GetType().GetField("splat_output").SetValue(tc_script2, true);
			generateStart();
		}
		if (GUI.Button(new Rect(305,30,150,25),"Reset Splatmap"))
		{
			tc_script.CallMethod("terrain_all_reset_splat");
		}
		
		if (GUI.Button(new Rect(5,55,150,25),"Generate Trees"))
		{
			createClone();
			tc_script.GetType().GetField("tree_output").SetValue(tc_script2, true);
			generateStart();
		}
		if (GUI.Button(new Rect(305,55,150,25),"Reset Trees"))
		{
			tc_script.CallMethod("terrain_reset_trees", tc_script.GetFieldValue("terrains").CallMethod("get_Item", 0),true);
		}
		if (GUI.Button(new Rect(155,55,150,25),"Randomize Trees"))
		{
			Random.seed = (int)Time.time;
			
			item = tc_script.GetFieldValue("subfilter").CallMethod("get_Item",3).GetFieldValue("precurve_list").CallMethod("get_Item",0);
		    item.GetType().GetField("offset").SetValue(tc_script.GetFieldValue("subfilter").CallMethod("get_Item",3).GetFieldValue("precurve_list").CallMethod("get_Item",0),new Vector2(Random.Range(-2000,2000),Random.Range(-2000,2000)));
		}
		
		myStyle.fontStyle = FontStyle.Bold;
		myStyle.normal.textColor = Color.red;
		
		GUI.Box(new Rect(5,96,300,29),"");
		GUI.Label(new Rect(10,100,120,25),"Generate Speed",myStyle);
		
		if (autoSpeed){GUI.color = Color.green;}
		if (GUI.Button(new Rect(305,100,150,25),"Auto Speed"))
		{
			autoSpeed = !autoSpeed;
		}
		GUI.color = Color.white;
		
		tc_script.GetType().GetField("generate_speed").SetValue(tc_script,(int)GUI.HorizontalSlider(new Rect(120,105,175,25),(int)tc_script.GetFieldValue("generate_speed"),1,200));
		if (tc_script2 != null){tc_script2.GetType().GetField("generate_speed").SetValue(tc_script2,tc_script.GetFieldValue("generate_speed"));}
		
		GUI.Box(new Rect(5,126,300,54),"");
		GUI.Label(new Rect(10,130,120,25),"Layer0 Strength",myStyle);
		GUI.Label(new Rect(10,155,120,25),"Layer1 Strength",myStyle);
		
		item = tc_script.GetFieldValue("prelayers").CallMethod("get_Item",0).GetFieldValue("layer").CallMethod("get_Item",0);
		strength = (float)item.GetFieldValue("strength");
		item.GetType().GetField ("strength").SetValue(tc_script.GetFieldValue("prelayers").CallMethod("get_Item",0).GetFieldValue("layer").CallMethod("get_Item",0),GUI.HorizontalSlider(new Rect(120,135,175,25),strength,0,1));
		
		item = tc_script.GetFieldValue("prelayers").CallMethod("get_Item",0).GetFieldValue("layer").CallMethod("get_Item",1);
		strength = (float)item.GetFieldValue("strength");
		item.GetType().GetField ("strength").SetValue(tc_script.GetFieldValue("prelayers").CallMethod("get_Item",0).GetFieldValue("layer").CallMethod("get_Item",1),GUI.HorizontalSlider(new Rect(120,155,175,25),strength,0,1));
		
		GUI.Box(new Rect(5,184,300,54),"");
		GUI.Label(new Rect(10,190,200,20),"Frames: "+frames.ToString("F0"),myStyle);
		GUI.HorizontalSlider(new Rect(120,193,175,25),frames,10,60);
		GUI.Label(new Rect(10,210,200,20),"Target: "+targetFrames.ToString(),myStyle);
		targetFrames = (int)GUI.HorizontalSlider(new Rect(120,213,175,25),targetFrames,10,60);
				
		if (tc_script2 != null)
		{
			if ((bool)tc_script2.GetFieldValue("generate"))
			{
				GUI.Label(new Rect(10,245,200,20),"Generating...",myStyle);
				if (GUI.Button(new Rect(120,245,75,20),"Stop")) {
					generateStop();
				}
			}
		}
	}
	 
	void generateStart()
	{
		tc_script2.GetType().GetField("auto_speed").SetValue(tc_script2, autoSpeed);
		tc_script2.GetType().GetField("target_frame").SetValue(tc_script2,targetFrames);
		tc_script2.GetType().GetField("runtime").SetValue(tc_script2, true);
		tc_script2.CallMethod("generate_begin");
		tc_script2.GetType().GetField("generate").SetValue(tc_script2, true);
	}
	
	void generateStop()
	{
		Destroy (TerrainComposerClone);
		tc_script2 = null;
	}
	
	void createClone()
	{
		if (tc_script2 != null){generateStop();}
		
		TerrainComposerClone = Instantiate(TerrainComposer) as GameObject;
		TerrainComposerClone.name = "<Generating>";
		tc_script2 = TerrainComposerClone.GetComponent("terraincomposer_save");
		tc_script2.GetType().GetField("script_base").SetValue(tc_script, tc_script);
	}
	
	void terrainsFlush()
	{
		item = tc_script.GetFieldValue("terrains");
		int length = (int)item.GetProprtyValue("Count");
		
		for (int count_terrain = 0;count_terrain < length;++count_terrain)
		{
			tc_script.GetFieldValue("terrains").CallMethod("get_Item",count_terrain).GetFieldValue("terrain").CallMethod ("Flush");
		}
	}
}


