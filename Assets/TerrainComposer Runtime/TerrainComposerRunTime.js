#pragma strict
private var TerrainComposer: GameObject;
private var tc_script: terraincomposer_save;

private var TerrainComposerClone: GameObject;
private var tc_script2: terraincomposer_save;

private var frames: float;	
var targetFrames: int = 90;

private var myStyle: GUIStyle;

function Start () 
{
	TerrainComposer = GameObject.Find("TerrainComposer_Save");
	tc_script = TerrainComposer.GetComponent(terraincomposer_save);
	
	tc_script.heightmap_output = false;
	tc_script.splat_output = false; 
	
	myStyle = new GUIStyle();
}

function Update () 
{
	frames = 1/Time.deltaTime;
	
	if (tc_script2)
	{
		if (tc_script2.generate)
		{
			tc_script2.generate_output(tc_script2.prelayers[0]);
		}
		else
		{
			// generation is ready so the clone can be destroyed
			terrainsFlush();
			generateStop();
		}
	}
}

function OnGUI()
{
	if (GUI.Button(Rect(5,5,150,25),"Generate Heightmap"))
	{
		createClone();
		tc_script2.heightmap_output = true;
		generateStart();
	}
	if (GUI.Button(Rect(305,5,150,25),"Reset Heightmap"))
	{
		tc_script.terrain_reset_heightmap(tc_script.terrains[0],true);
	}
	if (GUI.Button(Rect(155,5,150,25),"Randomize Heightmap"))
	{
		Random.seed = Time.time;
		
		tc_script.prelayers[0].layer[0].offset = Vector2(Random.Range(-2000,2000),Random.Range(-2000,2000));
		tc_script.prelayers[0].layer[1].offset = Vector2(Random.Range(-2000,2000),Random.Range(-2000,2000));
	}
	
	if (GUI.Button(Rect(5,30,150,25),"Generate Splatmap"))
	{
		createClone();
		tc_script2.splat_output = true;
		generateStart();
	}
	if (GUI.Button(Rect(305,30,150,25),"Reset Splatmap"))
	{
		tc_script.terrain_all_reset_splat();
	}
	
	if (GUI.Button(Rect(5,55,150,25),"Generate Trees"))
	{
		createClone();
		tc_script2.tree_output = true;
		generateStart();
	}
	if (GUI.Button(Rect(305,55,150,25),"Reset Trees"))
	{
		tc_script.terrain_reset_trees(tc_script.terrains[0],true);
	}
	if (GUI.Button(Rect(155,55,150,25),"Randomize Trees"))
	{
		Random.seed = Time.time;
		tc_script.subfilter[6].precurve_list[0].offset = Vector2(Random.Range(-2000,2000),Random.Range(-2000,2000));
	}
	
	myStyle.fontStyle = FontStyle.Bold;
	myStyle.normal.textColor = Color.red;
	
	GUI.Box(Rect(5,96,300,29),String.Empty);
	GUI.Label(Rect(10,100,120,25),"Generate Speed",myStyle);
	tc_script.generate_speed = GUI.HorizontalSlider(Rect(120,105,175,25),tc_script.generate_speed,1,200);
	
	GUI.Box(Rect(5,126,300,54),String.Empty);
	GUI.Label(Rect(10,130,120,25),"Layer0 Strength",myStyle);
	GUI.Label(Rect(10,155,120,25),"Layer1 Strength",myStyle);
	
	tc_script.prelayers[0].layer[0].strength = GUI.HorizontalSlider(Rect(120,135,175,25),tc_script.prelayers[0].layer[0].strength,0,1);
	tc_script.prelayers[0].layer[1].strength = GUI.HorizontalSlider(Rect(120,153,175,25),tc_script.prelayers[0].layer[1].strength,0,1);
	
	GUI.Box(new Rect(5,184,300,54),"");
	GUI.Label(new Rect(10,190,200,20),"Frames: "+frames.ToString("F0"),myStyle);
	GUI.HorizontalSlider(new Rect(120,193,175,25),frames,10,60);
	GUI.Label(new Rect(10,210,200,20),"Target: "+targetFrames.ToString(),myStyle);
	targetFrames = GUI.HorizontalSlider(new Rect(120,213,175,25),targetFrames,10,60);
	
	if (tc_script.auto_speed){GUI.color = Color.green;}
	if (GUI.Button(new Rect(305,100,150,25),"Auto Speed"))
	{
		tc_script.auto_speed = !tc_script.auto_speed;
	}
	GUI.color = Color.white;
	
	if (tc_script2)
	{
		if (tc_script2.generate)
		{
			GUI.color = Color.red;
			GUI.Label(new Rect(10,245,200,20),"Generating...",myStyle);
			GUI.color = Color.white;
			if (GUI.Button(new Rect(120,245,75,20),"Stop")) {
				generateStop();
			}
		}
	}
}

function generateStart()
{
	tc_script2.target_frame = targetFrames;
	tc_script2.runtime = true;
	tc_script2.generate_begin();
	tc_script2.generate = true;
}

function generateStop()
{
	Destroy (TerrainComposerClone);
}

function createClone()
{
	if (tc_script2){generateStop();}
	
	TerrainComposerClone = Instantiate(TerrainComposer);
	TerrainComposerClone.name = "<Generating>";
	tc_script2 = TerrainComposerClone.GetComponent(terraincomposer_save);
	tc_script2.script_base = tc_script;
}

function terrainsFlush()
{
	for (var count_terrain: int = 0;count_terrain < tc_script.terrains.Count;++count_terrain) {
		tc_script.terrains[count_terrain].terrain.Flush();
	}
}
