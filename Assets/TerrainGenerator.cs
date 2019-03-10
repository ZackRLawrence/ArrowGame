using UnityEngine;

public class TerrainGenerator : MonoBehaviour {
    public int depth = 20;

    [Range((int)1, (int)10)]
    public int octaves = 3;

    [Range(0.0f, 1024.0f)]
    public int width = 1024;
    [Range(0.0f, 1024.0f)]
    public int height = 1024;

    int width2;
    int height2;

    [Range(0.0f, 100.0f)]
    public float scale= 20f;

    public float offsetX = 100f;
    public float offsetY = 100f;

    //[Range(-50.0f, 50.0f)]
    //public float speedX = 5f;

    //[Range(-50.0f, 50.0f)]
    //public float speedY = 0f;

    [Range(0.0f, 2.0f)]
    public float persistence = 0.5f;

    Terrain terrain;
    float[,,] splatmapData;
    private float[,] heightField;

    public Texture2D GuideImage;
    [Range(0.01f, 1.0f)]
    public float mixFraction = 0.7f;

    //trees
    public float treeColorAdjustment = 0.4f;
    public int numTrees = 2000;

    [Range(0.0f, 90f)]
    public float slopeCut = 50f;

    [Range(0f, 1f)]
    public float weight = 0.5f;

    public bool run;

    private void Start()
    {
        if (run == true)
        {
            width2 = width * 2;
            height2 = height * 2;
            //for land
            offsetX = Random.Range(0f, 9999f);
            offsetY = Random.Range(0f, 9999f);
            terrain = GetComponent<Terrain>();

            Terrain.activeTerrain.terrainData.treeInstances = new TreeInstance[0];
            terrain.terrainData = GenerateTerrain(terrain.terrainData, GuideImage, mixFraction);
            //GenerateTerrainDetail(terrain.terrainData);

            //for texture
            heightField = new float[width, height];
            AssignSplatMap2(terrain.terrainData);
        }


        //for trees
        //PlaceTreesAcrossTerrain(numTrees, terrain.terrainData, GuideImage);
    }

    private void Update()
    {
        //terrain.terrainData = GenerateTerrain(terrain.terrainData);
        //AssignSplatMap2(terrain.terrainData);

        //offsetX += Time.deltaTime * speedX;
        //offsetY += Time.deltaTime * speedY;
        //AssignSplatMap2(terrain.terrainData);
    }

    TerrainData GenerateTerrain ( TerrainData terrainData, Texture2D guideTexture, float mixFraction)
    {
        terrainData.heightmapResolution = width2 - 1;
        terrainData.size = new Vector3(width, depth, height);


        terrainData.SetHeights(0, 0, GenerateHeights(guideTexture, mixFraction));
        return terrainData;
    }

    float[,] GenerateHeights (Texture2D guideTexture, float mixFraction)
    { 
        float[,] heights = new float[width2, height2];
        for (int x = 0; x < width2; x++)
        {
            for (int y = 0; y < height2; y++)
            {
                heights[x, y] = CalculateHeight(guideTexture, x, y, mixFraction);

                int tempX = x * 2;
                int tempY = y * 2;
            }
        }
        for (int oof = 0; oof < 2; oof++)
        {
            for (int x = 1; x < width2 - 1; x++)
            {
                for (int y = 1; y < height2 - 1; y++)
                {
                    float total = 0;
                    for (int k = 0; k < 9; k++)
                    {
                        int newX = x - 1 + (k % 3);
                        int newY = y - 1 + (int)k / (int)3;
                        total += heights[newX, newY];
                    }

                    heights[x, y] = total / 9f;
                }
            }
        }
        //Debug.Log("Height: " + height);

        return heights;
    }

    float CalculateHeight (Texture2D guideTex, int x, int y, float mixFraction)
    {
        float noiseVal = 0.0f; float frequency = 1.0f; float amplitude = 1.0f; float maxValue = 0.0f;
        float xfrac, yfrac; float greyScaleVal;
        xfrac = (float)x / (float)width2;
        yfrac = (float)y / (float)width2;

        greyScaleVal = guideTex.GetPixelBilinear(xfrac, yfrac).grayscale;

        for (int i = 0; i < octaves; i++)
        {
            float xCoord = (float)x / width * scale * frequency + offsetX;
            float yCoord = (float)y / height * scale * frequency + offsetY;

            noiseVal += Mathf.PerlinNoise(xCoord, yCoord) * amplitude;

            maxValue += amplitude;
            amplitude *= persistence;
            frequency *= 2;
        }

        noiseVal = noiseVal / maxValue;

        return (greyScaleVal * mixFraction) + noiseVal * (1- mixFraction);
    }

    
    void AssignSplatMap2(TerrainData terrainData)
    {
        int detailWidth = terrainData.detailWidth;
        int detailHeight = terrainData.detailHeight;
        int[,] details0 = new int[detailWidth, detailHeight];
        int[,] details1 = new int[detailWidth, detailHeight];


        splatmapData = new float[terrainData.alphamapWidth, terrainData.alphamapHeight, terrainData.alphamapLayers];


        for(int y = 0; y < terrainData.alphamapHeight; y++)
        {
            for(int x = 0; x < terrainData.alphamapWidth; x++)
            {
                //Get normalized terrain coordinate that corresponds to point
                float normX = x * 1.0f / (terrainData.alphamapWidth - 1);
                float normY = y * 1.0f / (terrainData.alphamapHeight - 1);

                var angle = terrainData.GetSteepness(normY, normX);
                if (angle < slopeCut)
                {
                    var frac = angle / slopeCut;
                    splatmapData[x, y, 0] = (float)(1 - frac);
                    splatmapData[x, y, 1] = (float)frac;
                } else
                {
                    splatmapData[x, y, 0] = 0;
                    splatmapData[x, y, 1] = 1;
                }
                /*
                for (int k = 0; k < 4; k++)
                {
                    float frac = (angle - slopeCut / 2) / (slopeCut / 2);
                    if (Random.Range(0, 1f) < (((1 - frac) * (1 - frac) * (1 - frac)) / 8))
                    {
                        int newX = x * 2 + (k % 2);
                        int newY = y * 2 + (int)k / (int)2;
                        float max = 17 * (1 - frac) * (1 - frac);
                        float temp = Random.Range(0, 1f);
                        if(temp < 0.6)
                            details0[newX, newY] = Random.Range(0, (int)max);
                        else if(temp < 0.8)
                            details1[newX, newY] = Random.Range(0, (int)max);
                    }
                }
                */

                //if(normY < 0.8)
                //{
                //    splatmapData[x, y, 0] = 0;
                //    splatmapData[x, y, 1] = 1;
                //}
                //Debug.Log(normX);
            }
        }

        //Debug.Log("AlphaH: " + terrainData.alphamapHeight);
        terrainData.SetDetailLayer(0, 0, 0, details0);
        terrainData.SetDetailLayer(0, 0, 1, details1);
        terrainData.SetAlphamaps(0, 0, splatmapData);
    }
    
    /*
    void GenerateTerrainDetail(TerrainData terrainData)
    {

        Debug.Log(detailHeight);

        int x, y, strength;

        for (x = 0; x < detailWidth; x++) // divided by 4 just to show a test patch
        {
            for (y = 0; y < detailHeight; y++) // test patch
            {
                strength = (x % 2 == 0 ? (x / 2) % 17 : 0); // just to spread the grass out a bit to see the difference

                if (y % 4 == 0) // set detail layer 0 for every first row in 4
                    details0[y, x] = strength;
                else if (y % 4 == 2) // set detail layer 1 for every third row in 4
                    details1[y, x] = strength;
            }
        }

        terrainData.SetDetailLayer(0, 0, 0, details0);
        terrainData.SetDetailLayer(0, 0, 1, details1);
    }
    */
    /*
void PlaceTreesAcrossTerrain(int numTrees, TerrainData terrainData, Texture2D guideTex)
    {
        float treeX = 0;
        float treeZ = 0;

        for(int i = 0; i < numTrees; i++)
        {
            treeX = Random.Range(0f, 1f);
            treeZ = Random.Range(0f, 1f);
            PlaceTree(treeX, treeZ, terrainData, guideTex);
        }
    }

    void PlaceTree(float treeX, float treeZ, TerrainData terrainData, Texture2D guideTex)
    {
        
        float normX = treeX * terrain.terrainData.alphamapWidth * 1.0f / (terrainData.alphamapWidth - 1);
        float normY = treeZ * terrain.terrainData.alphamapHeight * 1.0f / (terrainData.alphamapHeight - 1);
        var angle = terrainData.GetSteepness(normX, normY);

        float greyScaleVal = guideTex.GetPixelBilinear(treeZ, treeX).grayscale;
        

        //if(test < 5)
        //{
        //     return;
        //}

        //Debug.Log(greyScaleVal);
        if (angle > 45)
        {
            return;
        }
        int selectedPrototype;
        if(greyScaleVal < 0.33)
        {
            return;
        }
        else if (greyScaleVal < 0.65)
        {
            if (Random.Range(0f, 1f) < 0.05)
                selectedPrototype = 4;
            else
                selectedPrototype = Random.Range(0, 4);
        } else
        {

            if (Random.Range(0f, 1f) < 0.5)
                return;
            else if(Random.Range(0f, 1f) < 0.05)
                selectedPrototype = 4;
            else
                selectedPrototype = Random.Range(0, 4);
        }


        TreeInstance myTreeInstance = new TreeInstance();
        Vector3 position = new Vector3(treeX, 0, treeZ);
        int numPrototypes = terrain.terrainData.treePrototypes.Length;
        //int selectedPrototype = Random.Range(0, numPrototypes);

        if (numPrototypes == 0) return; // Terrain editor needs trees

        myTreeInstance.position = position;
        myTreeInstance.color = GetTreeColor();
        myTreeInstance.lightmapColor = Color.white;
        myTreeInstance.prototypeIndex = selectedPrototype;
        myTreeInstance.heightScale = 1.0f;
        myTreeInstance.widthScale = 1.0f;
        myTreeInstance.rotation = Random.Range(0.0f, 6.283185f);
        terrain.AddTreeInstance(myTreeInstance);
    }

    private Color GetTreeColor()
    {
        Color color = Color.white * Random.Range(1f, 1f - treeColorAdjustment);
        color.a = 1f;
        return color;
    }
    */

}
