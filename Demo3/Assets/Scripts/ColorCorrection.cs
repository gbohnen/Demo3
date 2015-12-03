using System; 
using UnityEngine; 
using System.Collections; 

[ExecuteInEditMode] 
public class ColorCorrection :  MonoBehaviour {  
   
	// the shader
	[SerializeField]     
	public Shader shader; 
	// the reference texture to lookup
	[SerializeField]     
	public Texture2D SourceTex = null;   
	// the output 
	private Texture3D My3DTexture = null; 
	// is the texture created?
	private bool Built = false;    
	private Material material;     

	// converts the 2D texture to a 3D texture     
	public void Convert_To_3D_Texture ()      
	{         
		if (SourceTex)          
		{             
			// get the total size of the source texture in height             
			int BitSize = SourceTex.height;             
			// create a new array of colors based on the Source Texture             
			Color[] SourcePixels = SourceTex.GetPixels();             
			// create a new array of colors based on the previous array             
			Color[] NewColor = new Color[SourcePixels.Length];     

			// loop the length to recolor each pixel on the screen	          
			for(int x = 0; x < BitSize; x++)              
			{                  
				for(int y = 0; y < BitSize; y++)                  
				{                    
					for(int z = 0; z < BitSize; z++)                      
					{                         
						// create a new color instance in the array at position based on the position in the source                         
						NewColor[x + (y*BitSize) + (z*BitSize*BitSize)] = SourcePixels[z*BitSize+x+(BitSize-y-1)*BitSize*BitSize];                     
					}                 
				}             
			}

			// create a new instance of the 3D texture             
			My3DTexture = new Texture3D (BitSize, BitSize, BitSize, TextureFormat.ARGB32, false);             
			// set the pixels on the 3D texture to the new color array             
			My3DTexture.SetPixels (NewColor);             
			// apply the 3D texture at the GPU             
			My3DTexture.Apply ();         
		}     
	}  

	void OnDisable ()      
	{         
		// invalidate the built bit         
		Built = false;     
	}     

	void OnRenderImage (RenderTexture source, RenderTexture destination)     
	{         
		// if the 3D texture has not been built         
		if(Built == false)         
		{             
			// set it as built             
			Built = true;             
			// Convert the 3D texture from the 2D source             
			Convert_To_3D_Texture();             
			// create a new instance of the shader             
			material = new Material(shader);             
			// set the wrapping mode to clamp the image             
			My3DTexture.wrapMode = TextureWrapMode.Clamp;             
			// set the scale of property of the shader             
			material.SetFloat("_Scale", (My3DTexture.width - 1) / (1.0f*My3DTexture.width));             
			// set the Lookup Texture property of the shader             
			material.SetTexture("_LookupTex", My3DTexture);        
		}         
		//blit the images         
		Graphics.Blit (source, destination, material);              
	} 

	// code for on-screen buttons. Each of these will jump to a new scene with a different Lookup texture
	public void DefaultPress()
	{
		Application.LoadLevel ("Torch - Default");		
	}

	public void MonotonePress()
	{
		Application.LoadLevel ("Torch - Monotone");	
	}

	public void DarkPress()
	{
		Application.LoadLevel ("Torch - Dark");	
	}

	public void WarmPress()
	{
		Application.LoadLevel ("Torch - Warm");	
	}

	public void CoolPress()
	{
		Application.LoadLevel ("Torch - Cool");	
	}
}