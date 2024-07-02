using System.Text;
using UnityEngine;

public class GameController : MonoBehaviour {
    public Terrain terrain;

    public Transform spheresHolder;
    public GameObject spherePrefab;
    public GameObject cubePrefab;
    public Transform mainCamera;
    private bool camera = true;

    private bool canUseBoat;

    
    // Update Function gets called once evey frame
    private void Update() {
        
        // If B key is pressed, to toggle the can use boat predicate
        if (Input.GetKeyDown(KeyCode.B)) canUseBoat = !canUseBoat;

        
        // On space, the pathfinding executes
        if (Input.GetKeyDown(KeyCode.Space)) {
            // Generate and convert 3d map into input for path finding
            var discreteMap = generateDiscreteTerrainMap(terrain.terrainData);
            var heights = generateHeights(terrain.terrainData);
            var adj = ConvertMatrix.convert(discreteMap, heights, canUseBoat);
            var path = PathFinding.path(65, 4031, adj);

            // goes through all points on the path and draws a cube in the position, making a path
            for (var i = path.Count - 1; i > 0; i--) {
                AddCube(path[i] % 64, path[i] / 64, heights[path[i] % 64, path[i] / 64]);
                Debug.Log(path[i]);
            }
        }

        // If M is pressed, changes camera position between two positions
        if (Input.GetKeyDown(KeyCode.M)) {
            if (camera) {
                mainCamera.position = new Vector3(-10, 45, -10);
                mainCamera.rotation = Quaternion.Euler(50, 45, 0);
            }
            else {
                mainCamera.position = new Vector3(70, 45, 70);
                mainCamera.rotation = Quaternion.Euler(50, -135, 0);
            }

            camera = !camera;
        }
    }

    private void AddSphere(int x, int y, float z) {
        var sphere = Instantiate(spherePrefab, new Vector3(x, z * 10 + 0.5f, y), Quaternion.identity);
        sphere.transform.SetParent(spheresHolder);
    }

    // Creates a cube at a specified x, y and z
    private void AddCube(int x, int y, float z) {
        var cube = Instantiate(cubePrefab, new Vector3(x, z * 10 + 0.5f, y), Quaternion.identity);
        cube.transform.SetParent(spheresHolder);
    }


    private void DeleteAllSpheres() {
        for (var i = 0; i < spheresHolder.childCount; i++) Destroy(spheresHolder.GetChild(i).gameObject);
    }

    // Generates a 2d array of terrain types based on what type of land(water, sand, grass...) is on the 3d map
    private int[,] generateDiscreteTerrainMap(TerrainData terrainData) {
        var res = terrainData.alphamapResolution;
        var alphaMap = terrainData.GetAlphamaps(0, 0, res, res);

        var discreteMap = new int[res, res];
        for (var y = 0; y < res; y++)
        for (var x = 0; x < res; x++) {
            var maxValue = alphaMap[y, x, 0];
            var maxIndex = 0;
            for (var i = 1; i < terrainData.alphamapLayers; i++)
                if (alphaMap[y, x, i] > maxValue) {
                    maxValue = alphaMap[y, x, i];
                    maxIndex = i;
                }

            discreteMap[x, y] = maxIndex;
        }

        return discreteMap;
    }

    private string printDiscreteMap(int[,] discreteMap) {
        var sb = new StringBuilder();
        var size = discreteMap.GetLength(0);
        for (var y = size - 1; y >= 0; y--) {
            for (var x = 0; x < size; x++) {
                sb.Append(discreteMap[x, y]);
                sb.Append(' ');
            }

            sb.AppendLine();
        }

        return sb.ToString();
    }

    // Generates a 2d array of heights at all x, y points
    private float[,] generateHeights(TerrainData terrainData) {
        var heights = new float[terrainData.heightmapResolution - 1, terrainData.heightmapResolution - 1];
        var h = terrainData.GetHeights(0, 0, terrainData.heightmapResolution, terrainData.heightmapResolution);

        for (var y = 0; y < 64; y++)
        for (var x = 0; x < 64; x++) {
            var a = h[y, x];
            var b = h[y, x + 1];
            var c = h[y + 1, x];
            var d = h[y + 1, x + 1];

            heights[x, y] = (a + b + c + d) / 4;
            //  Debug.Log(heights[x,y]);
        }

        return heights;
    }

    private string printAlphaMap(TerrainData terrainData) {
        var res = terrainData.alphamapResolution;
        var alphaMap = terrainData.GetAlphamaps(0, 0, res, res);
        var sb = new StringBuilder();
        for (var y = res - 1; y >= 0; y--) {
            for (var x = 0; x < res; x++) {
                sb.Append("(");
                for (var i = 0; i < terrainData.alphamapLayers; i++) {
                    sb.Append(alphaMap[y, x, i]);
                    if (i < terrainData.alphamapLayers - 1) sb.Append(" ");
                }

                sb.Append(") ");
            }

            sb.AppendLine();
        }

        return sb.ToString();
    }

    private void SetTerrainDataAlphaMap(TerrainData terrainData) {
        // terrainData.alphamapResolution = 16;

        var textureWeightsMap = new float[16, 16, terrainData.alphamapLayers];

        for (var x = 0; x < 16; x++)
        for (var y = 0; y < 16; y++) {
            // Set a single weight to 1 according to terrain type map (others are zero)

            var terrainLayerId = 0;
            textureWeightsMap[y, x, terrainLayerId] = 1;
        }

        // textureWeightsMap[0, 0, 1] = 1;
        // textureWeightsMap[0, 1, 1] = 1;
        // textureWeightsMap[1, 0, 1] = 1;
        textureWeightsMap[1, 1, 1] = 1;

        // textureWeightsMap[0, 0, 0] = 0;
        // textureWeightsMap[0, 1, 0] = 0;
        // textureWeightsMap[1, 0, 0] = 0;
        textureWeightsMap[1, 1, 0] = 0;

        terrainData.SetAlphamaps(0, 0, textureWeightsMap);
    }
}