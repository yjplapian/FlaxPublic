using System;
using System.Collections.Generic;
using System.Linq;
using FlaxEngine;

namespace Game
{
    /// <summary>
    /// ScriptExtensiosn Script.
    /// </summary>
    public class ActorExtensions : Script
    {
        [Header("Generic")]
        public Actor rootActor;

        [Header("Children")]
        public List<Actor> recursiveChilds = new();
        public GettersTest getInChild;
        public GettersTest[] getInChildren;

        [Header("Parent")]
        public GettersTest getInParent;
        public GettersTest[] getInParents;

        public override void OnStart()
        {
            rootActor = GetRoot(false);
            recursiveChilds = GetChildrenRecursive();
            getInChild = GetScriptInChild<GettersTest>();
            getInChildren = GetScriptsInChildren<GettersTest>();
            getInParent = GetScriptInParent<GettersTest>();
            getInParents = GetScriptsInParents<GettersTest>();
        }

        /// <summary>
        /// Gets the root (unparented actor) actor of this actor
        /// </summary>
        /// <param name="includeScene"> whetherr or not to include the scene object </param>
        /// <returns> includeScene = true returns the scene the actor is part of 
        /// includeScene = false returns the first unparented actor this actor is part of </returns>
        public Actor GetRoot(bool includeScene = false)
        {
            Actor parent = Actor.Parent;

            List<Actor> parents = new List<Actor> 
            {
                parent 
            };

            while(parent.Parent != null)
            {
                if (parents.Contains(parent.Parent) || !includeScene && parent.Parent.GetType().Equals(typeof(Scene)))
                    break;

                parent = parent.Parent;
                parents.Add(parent);
            }

            return parents.Last();
        }

        /// <summary>
        /// Gets a list of all parent objects
        /// </summary>
        /// <param name="includeScene"> Whether or not to include the scene as a parent as it technically is a parent object</param>
        /// <returns> a list of actors that are considered to be parents of this actor </returns>
        private List<Actor> GetParents(bool includeScene = false)
        {
            Actor parent = Actor.Parent;

            List<Actor> parents = new List<Actor>
            {
                parent
            };

            while (parent.Parent != null)
            {
                if (parents.Contains(parent.Parent) || !includeScene && parent.Parent.GetType().Equals(typeof(Scene)))
                    break;

                parent = parent.Parent;
                parents.Add(parent);
            }

            return parents;
        }

        /// <summary>
        /// Gets the target type in the first child it comes across
        /// </summary>
        /// <typeparam name="T"> The targeted type it needs to look for </typeparam>
        /// <returns> The first type it comes across from all the child actors </returns>
        public T GetScriptInChild<T>() where T : Script
        {
            List<Actor> children = GetChildrenRecursive();
            T target = null; 

            foreach (var child in children)
            {
                target = child.GetScript<T>();

                if (target != null)
                    break;
            }

            return target;
        }

        /// <summary>
        /// Gets the target type in all child actors it comes across
        /// </summary>
        /// <typeparam name="T"> The targeted type it needs to look for </typeparam>
        /// <returns> The array of the type it comes across from all the child actors </returns>
        public T[] GetScriptsInChildren<T>() where T : Script
        {
            List<Actor> children = GetChildrenRecursive();
            List<T> targets = new();

            foreach (var child in children)
            {
                var tmp = child.GetScript<T>();

                if(tmp != null && !targets.Contains(tmp))
                    targets.Add(tmp);
            }

            return targets.ToArray();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetScriptInParent<T>() where T : Script
        {
            var parents = GetParents();
            T target = null;

            foreach(var parent in parents)
            {
                target = parent.GetScript<T>();

                if (target != null)
                    break;
            }
   
            return target;
        }

        public T[] GetScriptsInParents<T>() where T : Script
        {
            var parents = GetParents();
            List<T> targets = new();

            foreach (var parent in parents)
            {
               T target = parent.GetScript<T>();

                if(target != null && !targets.Contains(target))
                    targets.Add(target);
            }

            return targets.ToArray();
        }

        /// <summary>
        /// Gets all children actors and puts them in a list for use (probably needs optimization - It iterates now based on indented layers)
        /// </summary>
        /// <param name="includeThisActor"> whether or not to include the base actor into the list </param>
        /// <returns>A list of all child actors </returns>
        private List<Actor> GetChildrenRecursive(bool includeThisActor = false)
        {
            List<Actor> actors = new();
            var directChildren = Actor.GetChildren<Actor>();

            if (includeThisActor)
                actors.Add(Actor);

            for (int i = 0; i < directChildren.Length; i++)
                actors.Add(directChildren[i]);
            
            for(int j = 0 ; j < actors.Count; j++)
            {
                if (actors[j].HasChildren)
                {
                    var newChildren = actors[j].GetChildren<Actor>();

                    for (int k = 0; k < newChildren.Length; k++)
                        actors.Add(newChildren[k]);
                }
            }

            return actors;
        }

        /// <summary>
        /// Gets all children actors and puts them in a list for use (probably needs optimization - It iterates now based on indented layers)
        /// </summary>
        /// <param name="target"> the external actor object that is used instead of the default one </param>
        /// <param name="includeThisActor"> whether or not to include the base actor into the list </param>
        /// <returns>A list of all child actors </returns>
        public List<Actor> GetChildrenRecursive(Actor target, bool includeThisActor = false)
        {
            List<Actor> actors = new();
            var directChildren = target.GetChildren<Actor>();

            if (includeThisActor)
                actors.Add(Actor);

            for (int i = 0; i < directChildren.Length; i++)
                actors.Add(directChildren[i]);

            for (int j = 0; j < actors.Count; j++)
            {
                if (actors[j].HasChildren)
                {
                    var newChildren = actors[j].GetChildren<Actor>();

                    for (int k = 0; k < newChildren.Length; k++)
                        actors.Add(newChildren[k]);
                }
            }

            return actors;
        }
    }
}