using _Project.Scripts.Main.Utilities.Scripts;
using UnityEngine;

namespace _Project.Scripts.Main
{
    public class TestScript : MonoBehaviour
    {
        [Header("Arrange Cards")]
        public GameObject cardPrefab;
        private int numberOfObjects = 5;
        private float angleStep = 12f;
        private float yDropStep = 0.15f;
        public float spacing = 1.8f;
    
        [Header("Bezier Curve")]
        public GameObject arrowPrefab;
        public Transform startPoint;
        public Transform endPoint;
        public float curveHeight = 2f;
        public int resolution = 20;

        void Start()
        {
            // ArrangeCards();
            //DrawCurve();
        }

        private void Update()
        {
      
        }

        void ArrangeCards()
        {
            int middleIndex = numberOfObjects / 2;
        
            for (int i = 0; i < numberOfObjects; i++)
            {
                GameObject obj = Instantiate(cardPrefab,transform);
            
                int offsetFromCenter = i - middleIndex;
                float angle = -offsetFromCenter * angleStep; // Negative to match left/right
                float edgeOffset = numberOfObjects >= 3 && (i == 0 || i == numberOfObjects - 1) ? 0.5f : 0;
          
                // float x = obj.transform.position.x;
                float x = transform.position.x;
                x+= (offsetFromCenter * spacing);
                float y = -Mathf.Abs(offsetFromCenter) * yDropStep;
            
                obj.transform.localPosition = new Vector3(x , y - edgeOffset , 0);
                obj.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            
                //store position and rotation to refresh it
            }
        }
    
        void DrawCurve()
        {
            Vector3 p0 = startPoint.position;
            Vector3 p2 = endPoint.position;

            // Compute a midpoint for the curve with added height
            Vector3 controlPoint = (p0 + p2) / 2 + Vector3.up * curveHeight;
        
            float r = Vector3.Distance(p0, p2);
            int length = (int)r;
            GameObject prevGameObject = null;
        
            for (int i = 0; i <= length; i++)
            {
                float t = i / r;
                Vector3 point = MathUtil.CalculateQuadraticBezierPoint(t, p0, controlPoint, p2);
            
                // float t2 = (i + 1) / r;
                // Vector3 point2 = CalculateQuadraticBezierPoint(t2, p0, controlPoint, p2);
                // Vector3 dir = point2 - point;
                // float angle = MathUtil.DirToAngle(dir.x, dir.y) * Mathf.Rad2Deg;
         
                GameObject obj = Instantiate(arrowPrefab, point, Quaternion.identity);
                //Determine look at rotation
                if (prevGameObject != null)
                {
                    float angle = MathUtil.LookAt2D(prevGameObject.transform.position, obj.transform.position);
                    prevGameObject.transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
                    prevGameObject = obj;
                }
                else
                    prevGameObject = obj;
            
                if (i == length)
                {
                    float angle = MathUtil.LookAt2D(obj.transform.position, endPoint.position);
                    obj.transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
                }
     
            
            
            
                //  obj.transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
                //Can add spacing
            }
        }

    }
}

