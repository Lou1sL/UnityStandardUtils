using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace UnityStandardUtils
{
    public static class SuicideGO
    {

        public class SuicideObject
        {
            public GameObject gameObject = null;
            public Rigidbody2D rigidbody2D = null;
            public Collider2D collider2D = null;
        }
        public enum ColliderType
        {
            Box2D, Capsule2D
        }
        public static GameObject CreateSuicideObject(GameObject origin, float suicideT = 15f)
        {
            GameObject go = UnityEngine.Object.Instantiate(origin);

            go.transform.eulerAngles = origin.transform.eulerAngles;
            go.transform.position = origin.transform.position;
            go.layer = origin.layer;
            Suicide gos = go.AddComponent<Suicide>();
            gos.LifeTime = suicideT;

            return go;
        }

        public static SuicideObject CreatePhysics2DSuicideObject(GameObject origin, float suicideT = 15f, ColliderType collider = ColliderType.Capsule2D)
        {
            SuicideObject so = new SuicideObject();

            so.gameObject = CreateSuicideObject(origin, suicideT);

            so.rigidbody2D = so.gameObject.AddComponent<Rigidbody2D>();
            switch (collider)
            {
                case ColliderType.Box2D: so.collider2D = so.gameObject.AddComponent<BoxCollider2D>(); break;
                case ColliderType.Capsule2D: so.collider2D = so.gameObject.AddComponent<CapsuleCollider2D>(); break;
            }

            return so;
        }
    }
}
