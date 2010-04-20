using System;
using System.Collections.Generic;
using FluentNHibernate.Infrastructure;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.Utils;

namespace FluentNHibernate.Automapping.Steps
{
    public class VersionStep : IAutomappingStep
    {
        private static readonly IList<string> ValidNames = new List<string> { "version", "timestamp" };
        private static readonly IList<Type> ValidTypes = new List<Type> { typeof(int), typeof(long), typeof(TimeSpan), typeof(byte[]) };

        public bool ShouldMap(AutomappingTarget target, Member member)
        {
            return ValidNames.Contains(member.Name.ToLowerInvariant()) && ValidTypes.Contains(member.PropertyType);
        }

        public IMemberMapping Map(AutomappingTarget target, Member property)
        {
            //if (!(classMap is ClassMapping)) return;

            var version = new VersionMapping
            {
                Name = property.Name,
            };

            version.SetDefaultValue("Type", GetDefaultType(property));
            version.AddDefaultColumn(new ColumnMapping { Name = property.Name });

            if (IsSqlTimestamp(property))
            {
                version.Columns.Each(x =>
                {
                    x.SqlType = "timestamp";
                    x.NotNull = true;
                });
                version.UnsavedValue = null;
            }

            return version;
        }

        private bool IsSqlTimestamp(Member property)
        {
            return property.PropertyType == typeof(byte[]);
        }

        private TypeReference GetDefaultType(Member property)
        {
            if (IsSqlTimestamp(property))
                return new TypeReference("BinaryBlob");

            return new TypeReference(property.PropertyType);
        }
    }
}