using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class PathMarker
{
    public MapLocation location;
    public float G;
    public float H;
    public float F;
    public GameObject marker;
    public PathMarker parent;

    public PathMarker(MapLocation l, float g, float h, float f, GameObject marker, PathMarker p)
    {
        location = l;
        G = g;
        H = h;
        F = f;
        this.marker = marker;
        parent = p;
    }

    public override bool Equals(object obj)
    {
        if ((obj == null) || !this.GetType().Equals(obj.GetType()))
        {
            return false;
        }
        else
        {
            return location.Equals(((PathMarker)obj).location);
        }
    }

    public override int GetHashCode()
    {
        return 0;
    }
}





public class FindPathAStar : MonoBehaviour
{
    public Maze maze;
    public Material closedMaterial;
    public Material openMaterial;

    List<PathMarker> open = new List<PathMarker>();
    List<PathMarker> closed = new List<PathMarker>();

    public GameObject start;
    public GameObject end;
    public GameObject pathP;

    public GameObject targetGameObject;
    public GameObject StartGameObject;

    PathMarker goalNode;
    PathMarker startNode;

    PathMarker lastPos;
    bool done = false;

    void RemoveAllMarkers()
    {
        GameObject[] markers = GameObject.FindGameObjectsWithTag("Marker");
        foreach (GameObject m in markers)
        {
            Destroy(m);
        }
    }

    void BeginSearch()
    {
        done = false;
        RemoveAllMarkers();

        // Find Random location in view radius to pick as a goal.

        // TODO = Find StartLocation
        //List<MapLocation> locations = new List<MapLocation>();
        //locations.Add(new MapLocation(0, 0));

        Vector3 startLocation = new Vector3(StartGameObject.transform.position.x, StartGameObject.transform.position.y, StartGameObject.transform.position.z);
        startNode = new PathMarker(new MapLocation(StartGameObject.transform.position.x, targetGameObject.transform.position.y, StartGameObject.transform.position.z), 0, 0, 0,
                                                                    Instantiate(start, startLocation, Quaternion.identity), null);
        Debug.Log("BeginSearch");

        // TODO - Find Goal Location
        Vector3 goalLocation = new Vector3(targetGameObject.transform.position.x, targetGameObject.transform.position.y, targetGameObject.transform.position.z);
        goalNode = new PathMarker(new MapLocation(targetGameObject.transform.position.x, targetGameObject.transform.position.y, targetGameObject.transform.position.z), 0, 0, 0,
                                                                    Instantiate(end, goalLocation, Quaternion.identity), null);


        open.Clear();
        closed.Clear();
        open.Add(startNode);
        lastPos = startNode;
    }

    void Search(PathMarker thisNode)
    {
        if (thisNode == null) return;
        if (thisNode.Equals(goalNode)) { done = true; return; }

        foreach(MapLocation dir in maze.directions)
        {
            MapLocation neighbour = dir + thisNode.location;
            if (maze.map[(int)neighbour.x, (int)neighbour.y, (int)neighbour.z] == 1) continue;// problem here somewhere.
            if (neighbour.x < 1 || neighbour.x >= maze.width || neighbour.y < 1 || neighbour.y >= maze.height || neighbour.z < 1 || neighbour.z >= maze.depth) continue;
            if (IsClosed(neighbour)) continue;

            float G = Vector3.Distance(thisNode.location.ToVector(), neighbour.ToVector()) + thisNode.G;
            float H = Vector3.Distance(neighbour.ToVector(), goalNode.location.ToVector());
            float F = G + H;

            GameObject pathBlock = Instantiate(pathP, new Vector3(neighbour.x, neighbour.y, neighbour.z), Quaternion.identity);

            // I need to get the text meshes.
            TextMesh[] values = pathBlock.GetComponentsInChildren<TextMesh>();
            values[0].text = "G" + G.ToString("0.00");
            values[1].text = "H" + G.ToString("0.00");
            values[2].text = "F" + G.ToString("0.00");

            if (!UpdateMarker(neighbour, G, H, F, thisNode))
                open.Add(new PathMarker(neighbour, G, H, F, pathBlock, thisNode));
        }

        open  = open.OrderBy(p => p.F).ThenBy(n => n.H).ToList<PathMarker>();
        PathMarker pm = (PathMarker) open.ElementAt(0);
        closed.Add(pm);

        open.RemoveAt(0);
        pm.marker.GetComponent<Renderer>().material = closedMaterial;

        lastPos = pm;
    }

    bool UpdateMarker(MapLocation pos, float g, float h, float f, PathMarker prt)
    {
        foreach (PathMarker p in open)
        {
            if (p.location.Equals(pos))
            {
                p.G = g;
                p.H = h;
                p.F = f;
                p.parent = prt;
                return true;
            }
        }
        return false;
    }

    bool IsClosed(MapLocation marker)
    {
        foreach(PathMarker p in closed )
        {
            if (p.location.Equals(marker)) return true;
        }
        return false;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P)) BeginSearch();
        if (Input.GetKeyDown(KeyCode.C)) Search(lastPos);
    }
}
