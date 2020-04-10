﻿using Extension.UI;
using System.Linq;
using UnityEngine;

namespace UI
{
    public class Route : MonoBehaviour
    {
        public string path;

        private void Awake()
        {
            var parents = GetComponentsInParent<Route>().ToList();
            parents.Reverse();
            path = string.Join("/", parents.Select(c => c.name));

            Router.Register(this);

            Close();
        }

        private void OnDestroy()
        {
            Router.UnRegister(this);
        }

        public void Close()
        {
            gameObject.Set(false);
        }

        public void Open()
        {
            gameObject.Set(true);
        }
    }
}