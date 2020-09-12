using System;

namespace LukeApps.Authorization.Attributes
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public class RoleObsoleteAttribute : Attribute
    {
    }
}