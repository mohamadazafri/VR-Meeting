/*
 * This project is completely free to use/edit and distrubute as you please!
 * Created by Craig & his good friend ChatGPT
 * 
 * Wishlist Airport X-Ray Simulator on steam!
 * 
 */
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class Whiteboard : MonoBehaviour
{
    [Header("Initial Setup")]
    [SerializeField] private bool useFirstImage = false; // Do you want a custom image displayed on the board to begin with?
    [SerializeField] private Texture2D firstImage; // Assign your desired first image

    [Header("Textures")]
    [SerializeField] private RenderTexture renderTexture; // Assign the render texture
    [SerializeField] private Texture2D brushTexture; // Assign the brush texture
    [SerializeField] private Texture2D eraserTexture; // Assign the eraser texture

    [Header("Brush Settings")]
    [SerializeField] private Color brushColor = Color.black; // Current brush color - default to black
    [SerializeField] private float brushSize = 15.0f; // Current brush size - default to 15f
    [SerializeField] private float smoothSteps = 800f; // Smoothsteps, these are used to smooth out the lines, increasing this too high will reduce performance with little noticable difference

    [Header("Eraser Settings")]
    [SerializeField] private GameObject eraserMesh; // Assign the eraser mesh

    [Header("Marker Settings")]
    private int selectedMarker = 0; // Current selected marker
    [SerializeField] private List<Color> colors; // Your whiteboard colors. These colors will be applied to the display pens and the pens you draw with
    [SerializeField] private Material baseMarkerMaterial; // Assign base marker material
    [SerializeField] private List<GameObject> displayMarkers; // List of the display markers on the front of the board. If you want to add more, drop them here
    [SerializeField] private GameObject heroMarker; // The gameobject of the mesh that follows the cursor to draw

    [Header("Other Settings")]
    [SerializeField] private float interactDistance = 2f;
    [SerializeField] private GameObject rag;
    [SerializeField] private float hoverOffset = 0.02f; // Offset for hovering marker
    [SerializeField] private float markerSmoothingSpeed = 20f; // Speed for marker smoothing

    private Material drawMaterial; // Local reference to the draw material
    private Vector2? previousUV = null; // Reference to remember last drawn position

    private MeshRenderer heroMarkerRenderer; // Reference to the hero markers renderer
    
    public Transform vrController; // Assign the VR controller's transform in the Inspector
    public InputActionProperty triggerAction; // Assign the "Select" action for the right hand in the Inspector

    private void Start()
    {
        heroMarkerRenderer = heroMarker.GetComponent<MeshRenderer>();
        
        InitializeRenderTexture();
        InitializeDisplayMarkers();
        ChangeMarkers();
    }
    void ChangeMarkers(int index = 0)
    {
        selectedMarker = index;
        for (int i = 0; i < displayMarkers.Count; i++)
        {
            if (i == selectedMarker)
            {
                displayMarkers[i].SetActive(false); // Hide the selected marker
            }
            else
            {
                displayMarkers[i].SetActive(true); // Show the unselected markers
            }
        }

        if (index == 5) // Eraser
        {
            brushColor = Color.white; // Set brush color to white for erasing
            brushSize = 150f; // Increase brush size for erasing
            drawMaterial.SetTexture("_MainTex", eraserTexture); // Set the eraser texture

            // Hide the hero marker and show the eraser
            heroMarker.SetActive(false);
            eraserMesh.SetActive(true);
        }
        else
        {
            brushColor = colors[index]; // Set brush color to the selected marker's color
            brushSize = 15f; // Set brush size for drawing
            drawMaterial.SetTexture("_MainTex", brushTexture); // Set the brush texture

            Material markerMaterialInstance = new Material(baseMarkerMaterial);
            markerMaterialInstance.color = colors[index]; // Set the marker material color
            heroMarkerRenderer.material = markerMaterialInstance; // Apply the marker material to the hero marker

            // Show the hero marker and hide the eraser
            heroMarker.SetActive(true);
            eraserMesh.SetActive(false);
        }
    }
    private void Update()
    {

        // Create a ray from the VR controller's position and forward direction
        Ray ray = new Ray(vrController.position, vrController.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, interactDistance)) // Perform a raycast
        {
            if (hit.collider.gameObject == gameObject)
            {
                //if (Input.GetMouseButton(0)) // Check for mouse input
                if (triggerAction.action.IsPressed())
                {
                    if (selectedMarker == 5)
                    {
                        // Handle eraser behavior
                        eraserMesh.SetActive(true);
                        eraserMesh.transform.position = Vector3.Lerp(eraserMesh.transform.position, hit.point, Time.deltaTime * markerSmoothingSpeed);
                    }
                    else
                    {
                        // Handle marker behavior
                        heroMarker.SetActive(true);
                        heroMarker.transform.position = Vector3.Lerp(heroMarker.transform.position, hit.point, Time.deltaTime * markerSmoothingSpeed);
                    }

                    Vector2 uv;
                    if (TryGetUVCoordinates(hit, out uv))
                    {
                        if (previousUV.HasValue)
                        {
                            DrawBetween(previousUV.Value, uv); // Draw between the previous and current UV coordinates
                        }
                        else
                        {
                            Draw(uv); // Draw at the current UV coordinate
                        }
                        previousUV = uv;
                    }
                }
                else
                {
                    Vector3 markerPosition = hit.point + hit.normal * hoverOffset; // Calculate the marker position with hover offset
                    if (selectedMarker == 5)
                    {
                        eraserMesh.SetActive(true);
                        eraserMesh.transform.position = Vector3.Lerp(eraserMesh.transform.position, markerPosition, Time.deltaTime * markerSmoothingSpeed);
                    }
                    else
                    {
                        heroMarker.SetActive(true);
                        heroMarker.transform.position = Vector3.Lerp(heroMarker.transform.position, markerPosition, Time.deltaTime * markerSmoothingSpeed);
                    }
                    previousUV = null;
                }
            }
            else
            {
                heroMarker.SetActive(false); // Hide the hero marker
                eraserMesh.SetActive(false); // Hide the eraser mesh
            }

            for (int i = 0; i < displayMarkers.Count; i++)
            {
                if (triggerAction.action.IsPressed() && hit.collider.gameObject == displayMarkers[i])
                {
                    ChangeMarkers(i); // Change the selected marker
                    break;
                }
            }

            //if (Input.GetMouseButtonDown(0) && hit.collider.gameObject == rag)
            if (triggerAction.action.IsPressed() && hit.collider.gameObject == rag)
            {
                ClearRenderTexture(); // Clear the render texture
            }
        }
        else
        {
            previousUV = null;
            heroMarker.SetActive(false); // Hide the hero marker
            eraserMesh.SetActive(false); // Hide the eraser mesh
        }
    }
    void InitializeRenderTexture()
    {
        if (renderTexture == null)
        {
            Debug.LogError("RenderTexture was not assigned."); // Log an error if the render texture is not assigned
        }

        RenderTexture.active = renderTexture; // Set the render texture as active

        if (useFirstImage)
        {
            if (firstImage != null)
            {
                Graphics.Blit(firstImage, renderTexture); // Load the first image onto the render texture
                Debug.Log("First run: loaded first image onto render texture.");
            }
            else
            {
                Debug.LogError("First image not assigned."); // Log an error if the first image is not assigned
                GL.Clear(true, true, Color.white); // Clear the render texture to white
            }

            useFirstImage = false; // Ensure this block is only run once (This might have only been needed for my save/load system and could be removed)
        }

        RenderTexture.active = null; // Release the render texture

        Shader drawShader = Shader.Find("Custom/DrawShader"); // Find the custom draw shader
        if (drawShader == null)
        {
            Debug.LogError("Custom DrawShader not found."); // Log an error if the custom draw shader is not found
            return;
        }
        drawMaterial = new Material(drawShader); // Create a new material with the draw shader
        drawMaterial.SetTexture("_MainTex", brushTexture); // Set the brush texture on the draw material
    }
    void InitializeDisplayMarkers()
    {
        for (int i = 0; i < displayMarkers.Count; i++)
        {
            if (i != 5) // Skip initializing the material for the eraser
            {
                MeshRenderer renderer = displayMarkers[i].GetComponent<MeshRenderer>(); // Get the mesh renderer of the display marker
                Material markerMaterialInstance = new Material(baseMarkerMaterial); // Create a new material instance from the base marker material
                markerMaterialInstance.color = colors[i]; // Set the color of the marker material instance
                renderer.material = markerMaterialInstance; // Assign the material to the renderer
            }
        }
    }
    void ClearRenderTexture()
    {
        RenderTexture.active = renderTexture; // Set the render texture as active

        GL.Clear(true, true, Color.white); // Clear the render texture to white

        RenderTexture.active = null; // Release the render texture
    }
    //Being completely open, ChatGPT wrote this method a long with it's comments.
    bool TryGetUVCoordinates(RaycastHit hit, out Vector2 uv)
    {
        MeshCollider meshCollider = hit.collider as MeshCollider; // Get the mesh collider from the hit
        uv = Vector2.zero; // Initialize the UV coordinates

        if (meshCollider == null || meshCollider.sharedMesh == null)
        {
            return false; // Return false if the mesh collider or shared mesh is null
        }

        Mesh mesh = meshCollider.sharedMesh; // Get the mesh from the mesh collider
        int[] triangles = mesh.triangles; // Get the triangles of the mesh
        Vector3[] vertices = mesh.vertices; // Get the vertices of the mesh
        Vector2[] uvs = mesh.uv; // Get the UVs of the mesh

        int triangleIndex = hit.triangleIndex * 3; // Calculate the triangle index
        Vector3 p0 = vertices[triangles[triangleIndex + 0]]; // Get the first vertex of the triangle
        Vector3 p1 = vertices[triangles[triangleIndex + 1]]; // Get the second vertex of the triangle
        Vector3 p2 = vertices[triangles[triangleIndex + 2]]; // Get the third vertex of the triangle

        Vector2 uv0 = uvs[triangles[triangleIndex + 0]]; // Get the first UV coordinate of the triangle
        Vector2 uv1 = uvs[triangles[triangleIndex + 1]]; // Get the second UV coordinate of the triangle
        Vector2 uv2 = uvs[triangles[triangleIndex + 2]]; // Get the third UV coordinate of the triangle

        Vector3 barycentric = hit.barycentricCoordinate; // Get the barycentric coordinates of the hit
        uv = uv0 * barycentric.x + uv1 * barycentric.y + uv2 * barycentric.z; // Calculate the interpolated UV coordinate

        return true; // Return true if the UV coordinates were successfully calculated
    }
    void Draw(Vector2 textureCoord)
    {
        RenderTexture.active = renderTexture; // Set the render texture as active

        GL.PushMatrix(); // Save the current matrix
        GL.LoadPixelMatrix(0, renderTexture.width, renderTexture.height, 0); // Set up the pixel matrix

        Vector2 brushPos = new Vector2(textureCoord.x * renderTexture.width, (1 - textureCoord.y) * renderTexture.height); // Calculate the brush position in pixel coordinates

        // Set the material to use the correct brush texture and color
        if (selectedMarker == 5) // Eraser
        {
            drawMaterial.SetTexture("_MainTex", eraserTexture); // Set the eraser texture
        }
        else
        {
            drawMaterial.SetTexture("_MainTex", brushTexture); // Set the brush texture
        }
        drawMaterial.SetColor("_Color", brushColor); // Ensure color is set here
        drawMaterial.SetPass(0); // Apply the material pass

        // Draw the brush texture directly on the render texture
        Graphics.DrawTexture(new Rect(brushPos.x - brushSize / 2, brushPos.y - brushSize / 2, brushSize, brushSize), drawMaterial.GetTexture("_MainTex"), drawMaterial);

        GL.PopMatrix(); // Restore the previous matrix
        RenderTexture.active = null; // Release the render texture
    }

    void DrawBetween(Vector2 startUV, Vector2 endUV)
    {
        int steps = Mathf.CeilToInt(Vector2.Distance(startUV, endUV) * smoothSteps); // Calculate the number of steps based on the distance and smooth steps
        for (int i = 0; i <= steps; i++)
        {
            Vector2 interpolatedUV = Vector2.Lerp(startUV, endUV, (float)i / steps); // Interpolate the UV coordinates
            Draw(interpolatedUV); // Draw at the interpolated UV coordinate
        }
    }
}