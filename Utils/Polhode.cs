using Godot;
using System;
using System.Collections.Generic;
using Sim;

public partial class Polhode : Node3D
{
    // Unique to my Polhode Class (private)
    private SpinBook spinBookSimulator; //Provided Simulation
    private MeshInstance3D polhodeSphere; // Sphere in 3D space
    private MeshInstance3D polhodePathInstance; // Line Path in 3D space (minimal properties assigned in GODOT)
    private ArrayMesh polhodePathMesh; // Mesh Data for the Line Path above
    private List<Vector3> pathPoints; // Where the Line Path points are held here in this "list"
    private double time = 0.0; // Simulation time Tracker

    public override void _Ready() //This is a setup!
    {
        spinBookSimulator = new SpinBook(); // This is used to initialize the spin book simulator w/ ICs

        polhodeSphere = GetNode<MeshInstance3D>("PolhodeSphere"); // Get the polhode sphere node

        polhodePathInstance = GetNode<MeshInstance3D>("PolhodePath"); // Get the polhode path node

        polhodePathMesh = new ArrayMesh(); // Initialize mesh for the polhode path
           //Next it gets assigned to the Path MeshInstance3D below
        polhodePathInstance.Mesh = polhodePathMesh; 

        pathPoints = new List<Vector3>(); // Initialize the list for path points

        polhodeSphere.Scale = new Vector3(1.99f, 1.99f, 1.99f); // Change the scale of the PolhodeSphere
            // Set slightly smaller than 2,2,2 (trial and error)

        var material = new StandardMaterial3D(); // Create a new material
        material.AlbedoColor = new Color(1, 0, 0); // RGB:(1,0,0) = Just a Red Line. Under Properties --> Albedo 

        polhodePathInstance.MaterialOverride = material; // Material now goes to the PolhodePath meshinstance3D
    }

public override void _Process(double delta)
{
    // Step method to increment the simulation
    spinBookSimulator.Step(time, delta);
    time += delta;

    // Angular momentum divided by moments of inertia (W / MOI)
    float l1 = (float)spinBookSimulator.omega1 / (float)spinBookSimulator.IG1;
    float l2 = (float)spinBookSimulator.omega2 / (float)spinBookSimulator.IG2;
    float l3 = (float)spinBookSimulator.omega3 / (float)spinBookSimulator.IG3;
    
        GD.Print("Using omega1 in Polhode: ", l1);
        GD.Print("Using omega2 in Polhode: ", l2);
        GD.Print("Using omega3 in Polhode: ", l3);

    Vector3 currentPoint = new Vector3(l1, l2, l3).Normalized(); // Normalize to keep on the sphere

    pathPoints.Add(currentPoint); // Add the current point to the path points list

    UpdatePolhodePath(); // Update the polhode path
}


    private void UpdatePolhodePath() 
    {
    
    if (pathPoints.Count < 2) return; // Only draw if we have at least two points to draw a line between

    polhodePathMesh.ClearSurfaces(); // Clear the existing line-path mesh

    // Create a Godot.Collections.Array for the vertices
    var arrays = new Godot.Collections.Array();
    var verts = new List<Vector3>();

    foreach (Vector3 point in pathPoints) // List for points from pathPoints
    {
        verts.Add(point);
    }
    
    // List --> Array ; Add to arrays object...
    arrays.Resize((int)Godot.Mesh.ArrayType.Max); 
    arrays[(int)Godot.Mesh.ArrayType.Vertex] = verts.ToArray();

    // Add the vertices to the ArrayMesh
    // Creat new path mesh surface
    polhodePathMesh.AddSurfaceFromArrays(Mesh.PrimitiveType.LineStrip, arrays);
    polhodePathInstance.Mesh = polhodePathMesh; //Assign the pathmehs to PolhodePathInstance
    }
}
