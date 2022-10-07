using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace TunaszUtils
{
    //Made from this tutorial 
    //https://www.youtube.com/watch?v=znZXmmyBF-o
    //[ExecuteInEditMode]
    public class AiSensor : MonoBehaviour
    {
        public float distance=10; 
        [Range(0,180)]
        public float angle=30;
        public float height = 30;
        public Color meshColor = Color.red;
        public int scanFrequency = 30;
        public LayerMask targetlayers;
        public LayerMask occlusionLayers;
        Collider[] colliders = new Collider[50];
        Mesh mesh;
        int count;
        float scanInterval;
        float scanTimer;
        [SerializeField]
        private Transform sensorLocation;
        public List<GameObject> Objects 
        {
            get {
                objects.RemoveAll(obj => !obj);
                return objects;
            }
        }
        public List<GameObject> objects = new List<GameObject>();
        [SerializeField]
        private bool showGizmos=true;
        // Start is called before the first frame update
        void Start()
        {
            scanInterval = 1.0f / scanFrequency;
        }

        // Update is called once per frame
        void Update()
        {
            scanTimer -= Time.deltaTime;
            if (scanTimer < 0) 
            {
                scanTimer += scanInterval;
                Scan();
            }
        }

        private void Scan()
        {

            count = Physics.OverlapSphereNonAlloc(sensorLocation.transform.position,
                distance,
                colliders,
                targetlayers,
                QueryTriggerInteraction.Collide);
            objects.Clear();
            for (int i = 0; i < count; ++i)
            {
                GameObject obj = colliders[i].gameObject;
                if (IsInSight(obj))
                {
                    objects.Add(obj);
                }
            }

        }

        public bool IsInSight(GameObject obj)
        {
            Vector3 origin = sensorLocation.transform.position;
            Vector3 dest = obj.transform.position;
            Vector3 dir = dest - origin;
            if (dir.y < 0 || dir.y > height) return false;
            dir.y = 0;
            float deltaAngle = Vector3.Angle(dir, sensorLocation.transform.forward);
            if (deltaAngle > angle) return false;

            origin.y += height / 2;
            dest.y = origin.y;
            if (showGizmos) Debug.DrawLine(origin, dest, Color.blue, 1);
            
            if (Physics.Linecast(origin, dest, occlusionLayers)) return false;
             
            return true;
        }
        public bool IsInSight(LayerMask layer, out List<GameObject> targets)
        {
            targets = new List<GameObject>();
            foreach (var obj in Objects)
            {
                if (layer.Contains(obj.layer))
                {
                    targets.Add(obj);
                }
            }
            return targets.Count>0;
        }
        public bool IsInSight(LayerMask layer)
        {
            var targets = new List<GameObject>();
            foreach (var obj in Objects)
            {
                if (layer.Contains(obj.layer))
                {
                    targets.Add(obj);
                }
            }
            return targets.Count > 0;
        }
        Mesh CreateWedgeMesh()
        {
            Mesh mesh = new Mesh();

            int segments = 10;



            int numTriagles = (segments* 4)+2+2;
            int numVertices = numTriagles * 3;
            Vector3[] vertices = new Vector3[numVertices];
            int[] triangles= new int[numVertices];
            Vector3 bottomCenter = Vector3.zero;
            Vector3 bottomLeft = Quaternion.Euler(0, angle, 0)* Vector3.forward*distance;
            Vector3 bottomRight = Quaternion.Euler(0, -angle, 0)* Vector3.forward*distance;

            Vector3 topCenter = bottomCenter + Vector3.up * height;
            Vector3 topLeft = bottomLeft + Vector3.up * height;
            Vector3 topRight = bottomRight + Vector3.up * height;

            int vert = 0;
            //left side
            vertices[vert++] = bottomCenter ;
            vertices[vert++] = bottomLeft ;
            vertices[vert++] = topLeft ;

            vertices[vert++] = topLeft;
            vertices[vert++] = topCenter;
            vertices[vert++] = bottomCenter;
            //right side
            vertices[vert++] = bottomCenter;
            vertices[vert++] = topCenter;
            vertices[vert++] = topRight;

            vertices[vert++] = topRight;
            vertices[vert++] = bottomRight;
            vertices[vert++] = bottomCenter;


            float currentAngle = -angle;
            float deltaAngle = (angle * 2) / segments;
            for (int i = 0; i < segments; ++i)
            {

                bottomLeft = Quaternion.Euler(0, currentAngle, 0) * Vector3.forward * distance;
                bottomRight = Quaternion.Euler(0, currentAngle+deltaAngle, 0) * Vector3.forward * distance;

                topLeft = bottomLeft + Vector3.up * height;
                topRight = bottomRight + Vector3.up * height;

                //far side

                vertices[vert++] = bottomLeft;
                vertices[vert++] = bottomRight;
                vertices[vert++] = topRight;

                vertices[vert++] = topRight;
                vertices[vert++] = topLeft;
                vertices[vert++] = bottomLeft;

                //top


                vertices[vert++] = topCenter;
                vertices[vert++] = topLeft;
                vertices[vert++] = topRight;


                //bottom

                vertices[vert++] = bottomCenter;
                vertices[vert++] = bottomRight;
                vertices[vert++] = bottomLeft;
                currentAngle += deltaAngle;

            }


            for (int i = 0; i < numVertices; ++i)
            {
                triangles[i] = i;
            }
            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.RecalculateNormals();
            
            return mesh;

        }
        public void OnValidate()
        {
            mesh = CreateWedgeMesh();
            if (!sensorLocation)
            {
                sensorLocation = transform;
            }
        }

        private void OnDrawGizmos()
        {
            if (!showGizmos) return;
            
            if (mesh && sensorLocation)
            {
                Gizmos.color = meshColor;
                Gizmos.DrawWireMesh(mesh, sensorLocation.transform.position, sensorLocation.transform.rotation);
            }
            //Gizmos.DrawWireSphere(sensorLocation.transform.position, distance); 
            Gizmos.color = Color.green;

            foreach (var obj in Objects)
            {
                Gizmos.DrawSphere(obj.transform.position, .2f);

            }
        }
        public int FilteredSearch(out List<GameObject>buffer,LayerMask layer)
        {
            buffer = new List<GameObject>();
            int count=0;
            foreach (var obj in Objects)
            {
                if (layer.Contains(obj.layer))
                {
                    buffer.Add( obj);
                } 
            }
            return count;
        }
        public int FilteredSearch(LayerMask layer)
        { 
            int count = 0;
            foreach (var obj in Objects)
            {
                if (layer.Contains(obj.layer))
                {
                    count++;
                }
            }
            return count;
        }

    }
}
