﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;


namespace XNAGameEngine
{
    class PhysicsManager
    {
        private Vector2 _pos;
        private Vector2 _vel;
        private float _mass;

        public Vector2 pos { get { return _pos; } set { _pos = value; } }
        public Vector2 vel { get { return _vel; } set { _vel = value; } }
        public float mass { get { return _mass; } set { _mass = value; } }

        public PhysicsManager(Vector2 position) 
        {
            _pos = position;
            _mass = 1;
        }

        public void SetVel(float x, float y)
        {
            _vel = new Vector2(x, y);
        }

        public static PhysicsEvent Collision(GameObject obj, GameObject obj2)
        {

            PhysicsManager o1 = obj.physics;
            PhysicsManager o2 = obj2.physics;

            Vector2 n_c = Vector2.Normalize(o1.pos - o2.pos);
            Vector2 n_tc = new Vector2(n_c.Y * -1, n_c.X);

            float o1_z = Vector2.Dot(n_c, o1.vel);
            float o1_t = Vector2.Dot(n_tc, o1.vel);
            float o2_z = Vector2.Dot(n_c, o2.vel);
            float o2_t = Vector2.Dot(n_tc, o2.vel);

            float o1_c = (o1_z * (o1.mass - o2.mass) + 2 * (o2.mass * o2_z)) / (o1.mass + o2.mass);
            float o2_c = (o2_z * (o2.mass - o1.mass) + 2 * (o1.mass * o1_z)) / (o1.mass + o2.mass);

            Vector2 v1_c = Vector2.Multiply(n_c, o1_c);
            Vector2 v1_t = Vector2.Multiply(n_tc, o1_t);
            Vector2 v2_c = Vector2.Multiply(n_c, o2_c);
            Vector2 v2_t = Vector2.Multiply(n_tc, o2_t);

            o1.vel = v1_c + v1_t;
            o2.vel = v2_c + v2_t;
            return new PhysicsEvent(o1.vel, o2.vel);


            //Vector2 v1 = new Vector2(obj.physics.vel.X * -1, obj.physics.vel.Y * -1);
            //Vector2 v2 = new Vector2(obj2.physics.vel.X * -1, obj2.physics.vel.Y * -1);
            //return new PhysicsEvent(v1, v2);
        }

        public void Update()
        {
            _pos = _pos + _vel;
        }

    }

    class PhysicsEvent
    {
        public Vector2 object1;
        public Vector2 object2;

        public PhysicsEvent(Vector2 obj1, Vector2 obj2)
        {
            object1 = obj1;
            object2 = obj2;
        }
    }
}
