using System;
using FluentNHibernate;
using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Conventions.Instances;

namespace Infra.NHibernate
{
    public class EnumConvention : IUserTypeConvention
    {
        public void Accept(IAcceptanceCriteria<IPropertyInspector> criteria)
        {
            criteria.Expect(x => x.Property.PropertyType.IsEnum);
        }

        public void Apply(IPropertyInstance target)
        {
            target.CustomType(target.Property.PropertyType);
        }
    }

    public class CustomPrimaryKeyConvention : IIdConvention
    {
        public void Apply(IIdentityInstance instance)
        {
            instance.Column("Id");
        }
    }

    public class CustomForeignKeyConvention : ForeignKeyConvention
    {
        protected override string GetKeyName(Member property, Type type)
        {
            if (property == null)
                return type.Name + "Id";

            return property.Name + "Id";
        }
    }

    public class CustomIndexManyToManyConvention : IIndexManyToManyConvention
    {
        public void Apply(IIndexManyToManyInstance instance)
        {
            instance.ForeignKey(string.Format("IdxMany_{0}",
                   instance.EntityType.Name));
        }
    }

    public class CustomReferenceConvention : IReferenceConvention
    {
        public void Apply(IManyToOneInstance instance)
        {
            instance.ForeignKey(string.Format("FK_{0}_{1}",
                   instance.Name, instance.EntityType.Name));


        }
    }

    public class CustomHasManyConvention : IHasManyConvention
    {
        public void Apply(IOneToManyCollectionInstance instance)
        {
            instance.Key.ForeignKey(string.Format("FK_{0}_{1}",
                   instance.Relationship.Class.Name, instance.EntityType.Name));
        }
    }

    public class CustomHasManyToManyConvention : IHasManyToManyConvention
    {
        public void Apply(IManyToManyCollectionInstance instance)
        {

            instance.Key.ForeignKey(string.Format("FK_{0}_{1}",
                   instance.TableName, instance.EntityType.Name));

            instance.Key.Column(instance.TableName + "ID");
        }
    }

    public class CustomHasOneConvention : IHasOneConvention
    {
        public void Apply(IOneToOneInstance instance)
        {
            instance.PropertyRef(string.Format("HasOnePro_{0}_{1}",
                   instance.Name, instance.EntityType.Name));

            instance.ForeignKey(string.Format("HasOne_{0}_{1}",
                   instance.Name, instance.EntityType.Name));

        }
    }







}
