using System.Collections.Generic;

namespace FlaxEngine
{
    /// <summary>
    /// Static Extensions for the Actor base
    /// </summary>
    public static class ActorExtensions
    {
        /// <summary>
        /// Gets the root (unparented actor) actor of this actor
        /// </summary>
        /// <param name="Main"> the base actor it starts at </param>
        /// <param name="IncludeScene"> whetherr or not to include the scene object </param>
        /// <returns> includeScene = true returns the scene the actor is part of 
        /// includeScene = false returns the first unparented actor this actor is part of </returns>
        public static Actor GetRoot(Actor Main, bool IncludeScene = false)
        {
            Actor Parent = Main.Parent;
            List<Actor> Parents = new List<Actor> 
            {
                Parent 
            };

            while(Parent.Parent != null)
            {
                if (Parents.Contains(Parent.Parent) || !IncludeScene && Parent.Parent.GetType().Equals(typeof(Scene)))
                    break;

                Parent = Parent.Parent;
                Parents.Add(Parent);
            }

            return Parents[Parents.Count -1];
        }

        /// <summary>
        /// Gets a list of all parent objects
        /// </summary>
        /// <param name="Main"> The base actor it starts at </param>
        /// <param name="IncludeScene"> Whether or not to include the scene as a parent as it technically is a parent object</param>
        /// <returns> a list of actors that are considered to be parents of this actor </returns>
        private static List<Actor> GetParents(Actor Main, bool IncludeScene = false)
        {
            Actor Parent = Main.Parent;
            List<Actor> Parents = new List<Actor>
            {
                Parent
            };

            while (Parent.Parent != null)
            {
                if (Parents.Contains(Parent.Parent) || !IncludeScene && Parent.Parent.GetType().Equals(typeof(Scene)))
                    break;

                Parent = Parent.Parent;
                Parents.Add(Parent);
            }

            return Parents;
        }

        /// <summary>
        /// Gets the target type in the first child it comes across
        /// </summary>
        /// <typeparam name="T"> The targeted type it needs to look for </typeparam>
        /// <returns> The first type it comes across from all the child actors </returns>
        public static T GetScriptInChild<T>(Actor Main) where T : Script
        {
            List<Actor> Children = GetChildrenRecursive(Main);
            T Target = null; 

            foreach (var Child in Children)
            {
                Target = Child.GetScript<T>();

                if (Target != null)
                    break;
            }

            return Target;
        }

        /// <summary>
        /// Gets the target type in all child actors it comes across
        /// </summary>
        /// <typeparam name="T"> The targeted type it needs to look for </typeparam>
        /// <returns> The array of the type it comes across from all the child actors </returns>
        public static T[] GetScriptsInChildren<T>(Actor Main) where T : Script
        {
            List<Actor> Children = GetChildrenRecursive(Main);
            List<T> Targets = new();

            foreach (var Child in Children)
            {
                var Tmp = Child.GetScript<T>();

                if(Tmp != null && !Targets.Contains(Tmp))
                    Targets.Add(Tmp);
            }

            return Targets.ToArray();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T GetScriptInParent<T>(Actor Main) where T : Script
        {
            var Parents = GetParents(Main);
            T Target = null;

            foreach(var Parent in Parents)
            {
                Target = Parent.GetScript<T>();

                if (Target != null)
                    break;
            }
   
            return Target;
        }

        /// <summary>
        /// Gets all instances of the type in any parent it comes across
        /// </summary>
        /// <typeparam name="T"> the target type </typeparam>
        /// <param name="Main"> the actor it starts at </param>
        /// <returns> an array of instances of the targeted type </returns>
        public static T[] GetScriptsInParents<T>(Actor Main) where T : Script
        {
            var Parents = GetParents(Main);
            List<T> Targets = new();

            foreach (var parent in Parents)
            {
               T Target = parent.GetScript<T>();

                if(Target != null && !Targets.Contains(Target))
                    Targets.Add(Target);
            }

            return Targets.ToArray();
        }

        /// <summary>
        /// Gets all children actors and puts them in a list for use (probably needs optimization - It iterates now based on indented layers)
        /// </summary>
        /// <param name="Main"> the default actor it needs to start from </param>
        /// <param name="IncludeThisActor"> whether or not to include the base actor into the list </param>
        /// <returns>A list of all child actors </returns>
        private static List<Actor> GetChildrenRecursive(Actor Main, bool IncludeThisActor = false)
        {
            List<Actor> Actors = new();
            var DirectChildren = Main.GetChildren<Actor>();

            if (IncludeThisActor)
                Actors.Add(Main);

            for (int i = 0; i < DirectChildren.Length; i++)
                Actors.Add(DirectChildren[i]);
            
            for(int j = 0 ; j < Actors.Count; j++)
            {
                if (Actors[j].HasChildren)
                {
                    var NewChildren = Actors[j].GetChildren<Actor>();

                    for (int k = 0; k < NewChildren.Length; k++)
                        Actors.Add(NewChildren[k]);
                }
            }

            return Actors;
        }

        /// <summary>
        /// Gets all children actors and puts them in a list for use (probably needs optimization - It iterates now based on indented layers)
        /// </summary>
        /// <param name="Main"> the actor it starts at </param>
        /// <param name="Target"> the external actor object that is used instead of the default one </param>
        /// <param name="IncludeThisActor"> whether or not to include the base actor into the list </param>
        /// <returns>A list of all child actors </returns>
        public static List<Actor> GetChildrenRecursive(Actor Main, Actor Target, bool IncludeThisActor = false)
        {
            List<Actor> Actors = new();
            var DirectChildren = Target.GetChildren<Actor>();

            if (IncludeThisActor)
                Actors.Add(Main);

            for (int i = 0; i < DirectChildren.Length; i++)
                Actors.Add(DirectChildren[i]);

            for (int j = 0; j < Actors.Count; j++)
            {
                if (Actors[j].HasChildren)
                {
                    var NewChildren = Actors[j].GetChildren<Actor>();

                    for (int k = 0; k < NewChildren.Length; k++)
                        Actors.Add(NewChildren[k]);
                }
            }

            return Actors;
        }
    }
}