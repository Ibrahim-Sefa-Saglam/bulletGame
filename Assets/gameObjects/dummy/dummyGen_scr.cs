using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyGenerator : MonoBehaviour
{
    // Public fields for customization in the Inspector
    public GameObject dummyPrefab; // The dummy prefab to instantiate
    public GameObject generationPoint; // The central point of the table
    public float rows = 3; // Number of rows
    public float columns = 3; // Number of columns
    public float rowSpacing = 1.5f; // Spacing between rows
    public float columnSpacing = 1.5f; // Spacing between columns
    public float initialParameter = 1f; // The starting parameter value
    public float growthRate = 1.2f; // Growth rate of the parameter per row

    // Start is called before the first frame update
    void Start()
    {
        GenerateTable();
    }

    // Function to generate the dummy table
   void GenerateTable()
    {
        // Get the center position from the generation point
        Vector3 center = generationPoint.transform.position;

        // Starting parameter for dummies
        float parameter = initialParameter;
        
        float maxParameter = Mathf.Pow(growthRate, rows-1) * initialParameter;

        // Calculate the top-left position to align the table center at the generationPoint
        Vector3 rightOfGenerator = generationPoint.transform.right;
        Vector3 forwardOfGenerator = generationPoint.transform.forward;

        Vector3 start = center -( rightOfGenerator*(columnSpacing/2)*columns + forwardOfGenerator*(rowSpacing/2)*rows);

        // Generate rows (vertical) and columns (horizontal)
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                // Calculate position for the current dummy
                Vector3 position = start +  rightOfGenerator * col * columnSpacing + forwardOfGenerator* row * rowSpacing;

                // Instantiate dummy and initialize it
                GameObject dummy = Instantiate(dummyPrefab, position, Quaternion.identity);
                dummy.transform.rotation = transform.rotation;
                dummy.GetComponent<dummy_scr>().Initialize(parameter, maxParameter);
            }
            // Increase the parameter exponentially for the next row
            parameter *= growthRate;
        }
    }
    }
