using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GroundGenerator {
	[System.NonSerialized]
	public Mesh mesh;

	private MeshFilter meshFilter;
	private MeshRenderer meshRenderer;
	private MeshCollider meshCollider;

	[System.NonSerialized]
	public Vector3[] newVertices;
	[System.NonSerialized]
	public Vector2[] newUV;
	[System.NonSerialized]
	public int[] newTriangles;

	[System.NonSerialized]
	public float blockSize = 1;
	private int topSurfaceQty;

	private List<int[]> groundData;
	private List<Vector3> tempVertices;
	private List<int> tempTriangles;
	private List<Vector2> tempUV;

	private float mapChipSize = 16f / 128f;

	public GroundGenerator(GameObject pGameObject, List<int[]> pData){
		topSurfaceQty = pData.Count * pData[0].Length;

		groundData = pData;

		tempVertices = new List<Vector3>();
		tempTriangles = new List<int>();
		tempUV = new List<Vector2>();

		GenerateSurfaces();

		newVertices = CreateNewVertices();
		newTriangles = CreateNewTriangles();
		newUV = CreateNewUV();

		mesh = new Mesh();
		meshFilter = pGameObject.GetComponent<MeshFilter>();
		meshFilter.mesh = mesh;
		meshRenderer = pGameObject.GetComponent<MeshRenderer>();

		mesh.vertices = newVertices;
		mesh.uv = newUV;
		mesh.triangles = newTriangles;

		mesh.RecalculateNormals();
		mesh.RecalculateBounds();
		meshFilter.sharedMesh = mesh;
		meshFilter.sharedMesh.name = "Ground";

		meshRenderer.material.SetTextureScale("_MainTex", new Vector2(1, 1));
	}
	private void Generate1Surface(Vector3 pPosition, Vector3[] pSquare, int[] pMapChipIndex){
		List<Vector3> tVertices = new List<Vector3>();
		Vector3 tPosition;
		for (int i = 0; i < 4; i++){
			tPosition = new Vector3();
			tPosition = pPosition + pSquare[i];
			tVertices.Add(tPosition);
		}

		GenerateTriangle(tempVertices.Count);

		for (int i = 0; i < tVertices.Count; i++){
			tVertices[i] *= blockSize;
			tempVertices.Add(tVertices[i]);
			GenerateUV(tempVertices.Count -1, pMapChipIndex[0], pMapChipIndex[1]);
		}
	}
	private void GenerateTriangle(int pIndex){
		tempTriangles.Add(pIndex +0);
		tempTriangles.Add(pIndex +1);
		tempTriangles.Add(pIndex +2);
		tempTriangles.Add(pIndex +1);
		tempTriangles.Add(pIndex +3);
		tempTriangles.Add(pIndex +2);
	}
	private void GenerateUV(int pIndex, int pMapChipIndexU, int pMapChipIndexV){
		float tU = 0.0f;
		float tV = 0.0f;
		switch (pIndex %4){
		case 0:
			tU = 0.0f;
			tV = 0.0f;
			break;
		case 1:
			tU = 0.0f;
			tV = 1.0f;
			break;
		case 2:
			tU = 1.0f;
			tV = 0.0f;
			break;
		case 3:
			tU = 1.0f;
			tV = 1.0f;
			break;
		}
		tU += pMapChipIndexU;
		tV += pMapChipIndexV;
		tU *= mapChipSize;
		tV *= mapChipSize;
		tempUV.Add(new Vector2(tU, tV));
	}

	private void GenerateSurfaces(){
		for (int i = 0; i < groundData.Count; i++){
			GenerateSurfaces1Line(i);
		}
	}
	private void GenerateSurfaces1Line(int pIndexZ){
		for (int i = 0; i < groundData[pIndexZ].Length; i++){
			Generate1TopSurface(i, pIndexZ);
			GenerateFrontSurfaceVerticalLine(i, pIndexZ);
			GenerateLeftSurfaceVerticalLine(i, pIndexZ);
			GenerateRightSurfaceVerticalLine(i, pIndexZ);
			GenerateBackSurfaceVerticalLine(i, pIndexZ);
		}
	}
	private void Generate1TopSurface(int pIndexX, int pIndexZ){
		int tIndexY = groundData[pIndexZ][pIndexX];

		//TopSurface
		Vector3[] tSquare = new Vector3[4]{
			new Vector3(0, 0, 0),
			new Vector3(0, 0, 1),
			new Vector3(1, 0, 0),
			new Vector3(1, 0, 1),
		};
		int[] tMapChipIndex;
		if (tIndexY < 0){
			tMapChipIndex = new int[2]{0, 5};
		} else {
			tMapChipIndex = new int[2]{0, 7};
		}
		Generate1Surface(new Vector3(pIndexX, tIndexY, -pIndexZ), tSquare, tMapChipIndex);
	}

	private void GenerateFrontSurfaceVerticalLine(int pIndexX, int pIndexZ){
		int tIndexY = groundData[pIndexZ][pIndexX];
		int tFrontIndexY;
		bool isEdge = false;
		if (pIndexZ == groundData.Count -1){
			isEdge = true;
			tFrontIndexY = 0;
		} else {
			tFrontIndexY = groundData[pIndexZ +1][pIndexX];
		}
		if (tIndexY <= tFrontIndexY){
			if (isEdge == false){
				return;
			}
		}

		//FrontSurface
		Vector3[] tSquare = new Vector3[4]{
			new Vector3(0, -1, 0),
			new Vector3(0,  0, 0),
			new Vector3(1, -1, 0),
			new Vector3(1,  0, 0)
		};
		int[] tMapChipIndex = new int[2]{0, 6};

		int height = tIndexY -tFrontIndexY;
		for (int i = 0; i < height; i++){
			Generate1Surface(new Vector3(pIndexX, height -i +tFrontIndexY, -pIndexZ), tSquare, tMapChipIndex);
		}
		if (isEdge == true){
			Generate1Surface(new Vector3(pIndexX, 0, -pIndexZ), tSquare, tMapChipIndex);
		}
	}

	private void GenerateLeftSurfaceVerticalLine(int pIndexX, int pIndexZ){
		int tIndexY = groundData[pIndexZ][pIndexX];
		int tLeftIndexY;
		bool isEdge = false;
		if (pIndexX == 0){
			isEdge = true;
			tLeftIndexY = 0;
		} else {
			tLeftIndexY = groundData[pIndexZ][pIndexX -1];
		}
		if (tIndexY <= tLeftIndexY){
			if (isEdge == false){
				return;
			}
		}

		//LeftSurface
		Vector3[] tSquare = new Vector3[4]{
			new Vector3(0,  0, 0),
			new Vector3(0, -1, 0),
			new Vector3(0,  0, 1),
			new Vector3(0, -1, 1)
		};
		int[] tMapChipIndex = new int[2]{0, 6};

		int height = tIndexY -tLeftIndexY;
		for (int i = 0; i < height; i++){
			Generate1Surface(new Vector3(pIndexX, height -i +tLeftIndexY, -pIndexZ), tSquare, tMapChipIndex);
		}
		if (isEdge == true){
			Generate1Surface(new Vector3(pIndexX, 0, -pIndexZ), tSquare, tMapChipIndex);
		}
	}

	private void GenerateRightSurfaceVerticalLine(int pIndexX, int pIndexZ){
		int tIndexY = groundData[pIndexZ][pIndexX];
		int tRightIndexY;
		bool isEdge = false;
		if (pIndexX == groundData[pIndexZ].Length -1){
			isEdge = true;
			tRightIndexY = 0;
		} else {
			tRightIndexY = groundData[pIndexZ][pIndexX +1];
		}
		if (tIndexY <= tRightIndexY){
			if (isEdge == false){
				return;
			}
		}

		//RightSurface
		Vector3[] tSquare = new Vector3[4]{
			new Vector3(1, -1, 0),
			new Vector3(1,  0, 0),
			new Vector3(1, -1, 1),
			new Vector3(1,  0, 1)
		};
		int[] tMapChipIndex = new int[2]{0, 6};

		int height = tIndexY -tRightIndexY;
		for (int i = 0; i < height; i++){
			Generate1Surface(new Vector3(pIndexX, height -i +tRightIndexY, -pIndexZ), tSquare, tMapChipIndex);
		}
		if (isEdge == true){
			Generate1Surface(new Vector3(pIndexX, 0, -pIndexZ), tSquare, tMapChipIndex);
		}
	}

	private void GenerateBackSurfaceVerticalLine(int pIndexX, int pIndexZ){
		int tIndexY = groundData[pIndexZ][pIndexX];
		int tBackIndexY;
		bool isEdge = false;
		if (pIndexZ == 0){
			isEdge = true;
			tBackIndexY = 0;
		} else {
			tBackIndexY = groundData[pIndexZ -1][pIndexX];
		}
		if (tIndexY <= tBackIndexY){
			if (isEdge == false){
				return;
			}
		}

		//BackSurface
		Vector3[] tSquare = new Vector3[4]{
			new Vector3(0,  0, 1),
			new Vector3(0, -1, 1),
			new Vector3(1,  0, 1),
			new Vector3(1, -1, 1)
		};
		int[] tMapChipIndex = new int[2]{0, 6};

		int height = tIndexY -tBackIndexY;
		for (int i = 0; i < height; i++){
			Generate1Surface(new Vector3(pIndexX, height -i +tBackIndexY, -pIndexZ), tSquare, tMapChipIndex);
		}
		if (isEdge == true){
			Generate1Surface(new Vector3(pIndexX, 0, -pIndexZ), tSquare, tMapChipIndex);
		}
	}

	private Vector3[] CreateNewVertices(){
		Vector3[] tVertices = new Vector3[tempVertices.Count];
		for (int i = 0; i < tempVertices.Count; i++){
			tVertices[i] = tempVertices[i];
		}
		return tVertices;
	}
	private int[] CreateNewTriangles(){
		int[] tTriangles = new int[tempTriangles.Count];
		for (int i = 0; i < tempTriangles.Count; i++){
			tTriangles[i] = tempTriangles[i];
		}
		return tTriangles;
	}
	private Vector2[] CreateNewUV(){
		Vector2[] tUV = new Vector2[tempUV.Count];
		for (int i = 0; i < tempUV.Count; i++){
			tUV[i] = tempUV[i];
		}
		return tUV;
	}
}