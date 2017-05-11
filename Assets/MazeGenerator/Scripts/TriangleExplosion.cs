using UnityEngine;
using System.Collections;


public class TriangleExplosion : MonoBehaviour{

    public static IEnumerator SplitMesh(bool destroy, GameObject WallToRemove){

        if (WallToRemove.GetComponent<MeshFilter>() == null || WallToRemove.GetComponent<SkinnedMeshRenderer>() == null){
            yield return null;
        }

        if (WallToRemove.GetComponent<MeshCollider>() != null){
            WallToRemove.GetComponent<MeshCollider>().enabled = false;
        }
        if (WallToRemove.GetComponent<Collider>()){
            WallToRemove.GetComponent<Collider>().enabled = false;
        }

        Mesh M = new Mesh();
        if (WallToRemove.GetComponent<MeshFilter>()){
            M = WallToRemove.GetComponent<MeshFilter>().mesh;
        }
        else if (WallToRemove.GetComponent<SkinnedMeshRenderer>()){
            M = WallToRemove.GetComponent<SkinnedMeshRenderer>().sharedMesh;
        }

        Material[] materials = new Material[0];
        if (WallToRemove.GetComponent<MeshRenderer>()){
            materials = WallToRemove.GetComponent<MeshRenderer>().materials;
        }
        else if (WallToRemove.GetComponent<SkinnedMeshRenderer>()){
            materials = WallToRemove.GetComponent<SkinnedMeshRenderer>().materials;
        }

        Vector3[] verts = M.vertices;
        Vector3[] normals = M.normals;
        Vector2[] uvs = M.uv;
        for (int submesh = 0; submesh < M.subMeshCount; submesh++){

            int[] indices = M.GetTriangles(submesh);

            for (int i = 0; i < indices.Length; i += 3){
                Vector3[] newVerts = new Vector3[3];
                Vector3[] newNormals = new Vector3[3];
                Vector2[] newUvs = new Vector2[3];
                for (int n = 0; n < 3; n++){
                    int index = indices[i + n];
                    newVerts[n] = verts[index];
                    newUvs[n] = uvs[index];
                    newNormals[n] = normals[index];
                }

                Mesh mesh = new Mesh();
                mesh.vertices = newVerts;
                mesh.normals = newNormals;
                mesh.uv = newUvs;

                mesh.triangles = new int[] { 0, 1, 2, 2, 1, 0 };

                GameObject GO = new GameObject("Triangle " + (i / 3));
                GO.layer = LayerMask.NameToLayer("Particle");
                GO.transform.position = WallToRemove.transform.position;
                GO.transform.rotation = WallToRemove.transform.rotation;
                GO.AddComponent<MeshRenderer>().material = materials[submesh];
                GO.AddComponent<MeshFilter>().mesh = mesh;
                GO.AddComponent<BoxCollider>();
                Vector3 explosionPos = new Vector3(WallToRemove.transform.position.x + Random.Range(-0.5f, 0.5f), WallToRemove.transform.position.y + Random.Range(0f, 0.5f), WallToRemove.transform.position.z + Random.Range(-0.5f, 0.5f));
                GO.AddComponent<Rigidbody>().AddExplosionForce(Random.Range(300, 500), explosionPos, 5);
                Destroy(GO, 5 + Random.Range(0.0f, 5.0f));
            }
        }

        WallToRemove.GetComponent<Renderer>().enabled = false;
        if (destroy == true){
            Destroy(WallToRemove);
        }
    }
}