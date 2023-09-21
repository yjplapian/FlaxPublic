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
        /// <returns> The root (unparented) Actor </returns>
        public static Actor GetRoot(this Actor Main, bool IncludeScene = false)
        {
            Actor Parent = Main.Parent;
            List<Actor> Parents = new();

            if (!IncludeScene && Parent.GetType() == typeof(Scene))
                return null;

            if (IncludeScene && Parent.GetType() != typeof(Scene))
                Parents.Add(Parent);

            else if(!IncludeScene && Parent.GetType() != typeof(Scene))
                Parents.Add(Parent);

            while(Parent.Parent != null)
            {
                if (Parents.Contains(Parent.Parent) || !IncludeScene && Parent.Parent.GetType().Equals(typeof(Scene)))
                    break;

                Parent = Parent.Parent;
                Parents.Add(Parent);
            }

            return Parents[^1];
        }

        /// <summary>
        /// Gets the first parent actor it comes across and casts it as type of T
        /// </summary>
        /// <typeparam name="T"> The type the actor needs to be cast as </typeparam>
        /// <param name="Main"> The target actor it starts searching from </param>
        /// <returns> The first cast actor it finds </returns>
        public static T GetParent<T>(this Actor Main) where T : Actor
        {
            T Tmp = null;
            List<Actor> Parents = Internal_GetParents(Main, false);

            foreach (var Parent in Parents)
            {
                Tmp = (T)Parent;

                if (Tmp != null)
                    break;
            }

            return Tmp;
        }

        /// <summary>
        /// Gets all parent actors it comes across and casts them as type of T
        /// </summary>
        /// <typeparam name="T"> The type the actor needs to be cast as </typeparam>
        /// <param name="Main"> The target actor it starts searching from </param>
        /// <returns> Array of actors cast as T </returns>
        public static T[] GetParents<T>(this Actor Main) where T : Actor
        {
            List<Actor> Parents = Internal_GetParents(Main, false);
            List<T> Targets = new List<T>();

            foreach (var Parent in Parents)
            {
                T Tmp = Parent as T;

                if (Tmp != null && !Targets.Contains(Tmp))
                    Targets.Add(Tmp);            
            }

            return Targets.ToArray();
        }

        #region Get Scripts Functions
        /// <summary>
        /// Gets the target type in the first child it comes across
        /// </summary>
        /// <typeparam name="T"> The targeted type it needs to look for </typeparam>
        /// <param name="Main"> The targeted Actor it needs to check the child actors of </param>
        /// <param name="IncludeInactive"> Whether or not to skip inactive actors </param>
        /// <returns> The first type it comes across from all the child actors </returns>
        public static T GetScriptInChild<T>(this Actor Main, bool IncludeInactive = false) where T : Script
        {
            List<Actor> Children = Internal_GetAllChildren(Main);
            T Target = null;

            foreach (var Child in Children)
            {
                 Target = Child.GetScript<T>();

                if (Target != null)
                {
                    if (!Target.Enabled && !IncludeInactive || !Target.Actor.IsActive && !IncludeInactive)
                        continue;

                    else
                        break;
                }
            }

            return Target;
        }

        /// <summary>
        /// Gets the target type in all childs it comes across
        /// </summary>
        /// <typeparam name="T"> The targeted type it needs to look for </typeparam>
        /// <param name="Main"> The targeted Actor it needs to check the child actors of </param>
        /// <param name="IncludeInactive"> Whether or not to skip inactive actors or inactive types </param>
        /// <returns> The array of the type it comes across from all the child actors </returns>
        public static T[] GetScriptsInChildren<T>(this Actor Main, bool IncludeInactive = false) where T : Script
        {
            List<Actor> Children = Internal_GetAllChildren(Main);
            List<T> Targets = new();

            foreach (var Child in Children)
            {
                T Target = Child.GetScript<T>();

                if (Target != null)
                {
                    if (!Target.Enabled && !IncludeInactive || !Target.Actor.IsActive && !IncludeInactive)
                        continue;
                }

                if (Target != null && !Targets.Contains(Target))
                    Targets.Add(Target);
            }

            return Targets.ToArray();
        }

        /// <summary>
        /// Gets the first instance of the type in any parent it comes across
        /// </summary>
        /// <typeparam name="T"> the target type </typeparam>
        /// <param name="Main"> the actor it starts at </param>
        /// <returns> first instance of the targeted type </returns>
        public static T GetScriptInParent<T>(this Actor Main) where T : Script
        {
            var Parents = Internal_GetParents(Main);
            T Target = null;

            foreach(var Parent in Parents)
            {
                Target = Parent.GetScript<T>();

                if (Target != null)
                {
                        break;
                }
            }
   
            return Target;
        }

        /// <summary>
        /// Gets all instances of the type in any parent it comes across
        /// </summary>
        /// <typeparam name="T"> the target type </typeparam>
        /// <param name="Main"> the actor it starts at </param>
        /// <param name="IncludeInactive"> Whether or not to skip inactive actors or inactive types </param>
        /// <returns> an array of instances of the targeted type </returns>
        public static T[] GetScriptsInParents<T>(this Actor Main, bool IncludeInactive = false) where T : Script
        {
            var Parents = Internal_GetParents(Main);
            List<T> Targets = new();

            foreach (var parent in Parents)
            {
               T Target = parent.GetScript<T>();

                if (Target != null)
                {
                    if (!Target.Enabled && !IncludeInactive)
                        continue;

                    if (!Targets.Contains(Target))
                        Targets.Add(Target);
                }
            }

            return Targets.ToArray();
        }
        #endregion

        #region Internal Functions
        /// <summary>
        /// Gets all children actors and puts them in a list for use (probably needs optimization - It iterates now based on indented layers)
        /// </summary>
        /// <param name="Main"> the default actor it needs to start from </param>
        /// <param name="IncludeThisActor"> whether or not to include the base actor into the list </param>
        /// <returns>A list of all child actors </returns>
        private static List<Actor> Internal_GetAllChildren(Actor Main, bool IncludeThisActor = false)
        {
            List<Actor> Actors = new();
            var DirectChildren = Main.GetChildren<Actor>();

            if (IncludeThisActor)
                Actors.Add(Main);

            for (int i = 0; i < DirectChildren.Length; i++)
            {
                if (!Actors.Contains(DirectChildren[i]))
                    Actors.Add(DirectChildren[i]);
            }
            
            for(int j = 0 ; j < Actors.Count; j++)
            {
                if (Actors[j].HasChildren)
                {
                    var NewChildren = Actors[j].GetChildren<Actor>();

                    for (int k = 0; k < NewChildren.Length; k++)
                    {
                        if (!Actors.Contains(NewChildren[k]))
                            Actors.Add(NewChildren[k]);
                    }
                }
            }

            return Actors;
        }

        /// <summary>
        /// Gets a list of all parent objects (probably needs optimization - It iterates now based on indented layers)
        /// </summary>
        /// <param name="Main"> The base actor it starts at </param>
        /// <param name="IncludeScene"> Whether or not to include the scene as a parent as it technically is a parent object</param>
        /// <returns> a list of actors that are considered to be parents of this actor </returns>
        private static List<Actor> Internal_GetParents(Actor Main, bool IncludeScene = false)
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
        #endregion
    }
}