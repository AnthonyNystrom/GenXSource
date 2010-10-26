using System;
using System.Collections.Generic;

namespace Genetibase.NuGenRenderCore.Resources
{
    public interface ISharableResource
    {
        string Name { get; }
        IResourceSet Owner { get; }
        uint UsersCount { get; }
        object Value { get; }
    }

    class SharableResource : ISharableResource
    {
        string name;
        IResourceSet owner;
        uint usersCount;
        object value;

        public SharableResource(string name, IResourceSet owner, uint usersCount, object value)
        {
            this.name = name;
            this.owner = owner;
            this.usersCount = usersCount;
            this.value = value;
        }

        #region ISharableResource Members

        public string Name
        {
            get { return name; }
        }

        public IResourceSet Owner
        {
            get { return owner; }
        }

        public uint UsersCount
        {
            get { return usersCount; }
        }

        public object Value
        {
            get { return value; }
        }
        #endregion
    }

    public interface IResourceSet : IDisposable
    {
        object Key { get; }
        ISharableResource Checkout(string name);
        ISharableResource PeekRz(string name);
        void Checkin(object resource, string name, out ISharableResource rz);
        void Checkin(string name);
        void Checkin(ISharableResource rz);
    }

    class ResourceSet : IResourceSet
    {
        object key;
        Dictionary<string, SharableResource> resources;

        public ResourceSet(object key)
        {
            this.key = key;
            resources = new Dictionary<string, SharableResource>();
        }

        class SharableResource : ISharableResource
        {
            string name;
            IResourceSet owner;
            uint usersCount;
            object value;

            public SharableResource(string name, IResourceSet owner, object value)
            {
                this.name = name;
                this.owner = owner;
                this.value = value;
                this.usersCount = 1;
            }

            #region ISharableResource Members
            public string Name
            {
                get { return name; }
            }

            public IResourceSet Owner
            {
                get { return owner; }
            }

            public uint UsersCount
            {
                get { return usersCount; }
            }

            public object Value
            {
                get { return value; }
            }
            #endregion

            public void AddUser()
            {
                lock (this)
                {
                    usersCount++;
                }
            }

            public void RemoveUser()
            {
                lock (this)
                {
                    usersCount--;
                }
            }
        }

        public object Key
        {
            get { return key; }
        }

        public ISharableResource Checkout(string name)
        {
            SharableResource rz;
            if (resources.TryGetValue(name, out rz))
                rz.AddUser();
            return rz;
        }

        public void Checkin(object resource, string name, out ISharableResource rz)
        {
            ResourceSet.SharableResource sRz = new SharableResource(name, this, resource);
            resources.Add(name, sRz);
            rz = sRz;
        }

        public void Checkin(string name)
        {
            SharableResource rz;
            if (resources.TryGetValue(name, out rz))
            {
                rz.RemoveUser();
                lock (rz)
                {
                    if (rz.UsersCount == 0)
                        resources.Remove(name);
                }
            }
        }

        public void Checkin(ISharableResource rz)
        {
            SharableResource srz = (SharableResource)rz;
            srz.RemoveUser();
            lock (srz)
            {
                if (srz.UsersCount == 0)
                    resources.Remove(srz.Name);
            }
        }

        public ISharableResource PeekRz(string name)
        {
            SharableResource rz;
            resources.TryGetValue(name, out rz);
            return rz;
        }

        #region IDisposable Members

        public void Dispose()
        {
            foreach (KeyValuePair<string, SharableResource> rz in resources)
            {
                if (rz.Value is IDisposable)
                {
                    ((IDisposable)rz.Value).Dispose();
                }
            }
            resources.Clear();
        }

        #endregion
    }

    public class ResourceManager : IDisposable
    {
        Dictionary<object, IResourceSet> resourceSets;

        public ResourceManager()
        {
            resourceSets = new Dictionary<object, IResourceSet>();
        }

        public void AddSet(IResourceSet set)
        {
            resourceSets.Add(set.Key, set);
        }

        public IResourceSet GetSet(object key)
        {
            IResourceSet set;
            resourceSets.TryGetValue(key, out set);
            return set;
        }

        #region IDisposable Members

        public void Dispose()
        {
            foreach (KeyValuePair<object, IResourceSet> set in resourceSets)
            {
                set.Value.Dispose();
            }
            resourceSets.Clear();
        }

        #endregion
    }
}