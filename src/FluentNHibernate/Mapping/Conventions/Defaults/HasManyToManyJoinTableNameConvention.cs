namespace FluentNHibernate.Mapping.Conventions.Defaults
{
    public class HasManyToManyJoinTableNameConvention : IHasManyToManyConvention
    {
        public bool Accept(IManyToManyPart target)
        {
            return string.IsNullOrEmpty(target.TableName);
        }

        public void Apply(IManyToManyPart target, ConventionOverrides overrides)
        {
            if (overrides.GetManyToManyTableName == null)
                target.WithTableName(target.ChildType.Name + "To" + target.ParentType.Name);
            else
                target.WithTableName(overrides.GetManyToManyTableName(target));
        }
    }
}