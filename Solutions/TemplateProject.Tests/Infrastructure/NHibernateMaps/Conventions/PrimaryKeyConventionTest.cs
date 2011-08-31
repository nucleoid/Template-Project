
using System;
using System.Collections.Generic;
using FluentNHibernate;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Conventions.Instances;
using FluentNHibernate.Mapping;
using FluentNHibernate.MappingModel;
using MbUnit.Framework;
using TemplateProject.Domain;
using TemplateProject.Infrastructure.NHibernateMaps.Conventions;

namespace TemplateProject.Tests.Infrastructure.NHibernateMaps.Conventions
{
    [TestFixture]
    public class PrimaryKeyConventionTest
    {
        [Test]
        public void Apply_Sets_Column_Id_Name()
        {
            //Arrange
            var convention = new PrimaryKeyConvention();
            var tester = new IdentityInstanceTester();

            //Act
            convention.Apply(tester);

            //Assert
            Assert.AreEqual("ProductId", tester.ColumnName);
        }

        private class IdentityInstanceTester : IIdentityInstance
        {
            public string ColumnName { get; private set; }

            public bool IsSet(Member property)
            {
                throw new NotImplementedException();
            }

            public Type EntityType
            {
                get { return typeof(Product); }
            }

            public string StringIdentifierForModel
            {
                get { throw new NotImplementedException(); }
            }

            public Member Property
            {
                get { throw new NotImplementedException(); }
            }

            public void Column(string column)
            {
                ColumnName = column;
            }

            void IIdentityInstance.UnsavedValue(string unsavedValue)
            {
                throw new NotImplementedException();
            }

            void IIdentityInstance.Length(int length)
            {
                throw new NotImplementedException();
            }

            public void CustomType(string type)
            {
                throw new NotImplementedException();
            }

            public void CustomType(Type type)
            {
                throw new NotImplementedException();
            }

            public void CustomType<T>()
            {
                throw new NotImplementedException();
            }

            void IIdentityInstance.Precision(int precision)
            {
                throw new NotImplementedException();
            }

            void IIdentityInstance.Scale(int scale)
            {
                throw new NotImplementedException();
            }

            void IIdentityInstance.Nullable()
            {
                throw new NotImplementedException();
            }

            void IIdentityInstance.Unique()
            {
                throw new NotImplementedException();
            }

            void IIdentityInstance.UniqueKey(string columns)
            {
                throw new NotImplementedException();
            }

            public void CustomSqlType(string sqlType)
            {
                throw new NotImplementedException();
            }

            void IIdentityInstance.Index(string index)
            {
                throw new NotImplementedException();
            }

            void IIdentityInstance.Check(string constraint)
            {
                throw new NotImplementedException();
            }

            void IIdentityInstance.Default(object value)
            {
                throw new NotImplementedException();
            }

            IAccessInstance IIdentityInstance.Access
            {
                get { throw new NotImplementedException(); }
            }

            public IGeneratorInstance GeneratedBy
            {
                get { throw new NotImplementedException(); }
            }

            public IIdentityInstance Not
            {
                get { throw new NotImplementedException(); }
            }

            Access IIdentityInspectorBase.Access
            {
                get { throw new NotImplementedException(); }
            }

            public string UnsavedValue
            {
                get { throw new NotImplementedException(); }
            }

            public string Name
            {
                get { throw new NotImplementedException(); }
            }

            public IEnumerable<IColumnInspector> Columns
            {
                get { throw new NotImplementedException(); }
            }

            public IGeneratorInspector Generator
            {
                get { throw new NotImplementedException(); }
            }

            public TypeReference Type
            {
                get { throw new NotImplementedException(); }
            }

            public int Length
            {
                get { throw new NotImplementedException(); }
            }

            public int Precision
            {
                get { throw new NotImplementedException(); }
            }

            public int Scale
            {
                get { throw new NotImplementedException(); }
            }

            bool IIdentityInspector.Nullable
            {
                get { throw new NotImplementedException(); }
            }

            bool IIdentityInspector.Unique
            {
                get { throw new NotImplementedException(); }
            }

            public string UniqueKey
            {
                get { throw new NotImplementedException(); }
            }

            public string SqlType
            {
                get { throw new NotImplementedException(); }
            }

            public string Index
            {
                get { throw new NotImplementedException(); }
            }

            public string Check
            {
                get { throw new NotImplementedException(); }
            }

            public string Default
            {
                get { throw new NotImplementedException(); }
            }
        }
    }
}
