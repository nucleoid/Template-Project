
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using FluentNHibernate;
using MbUnit.Framework;
using NHibernate.Mapping;
using SharpArch.Domain.DomainModel;
using TemplateProject.Infrastructure.NHibernateMaps;

namespace TemplateProject.Tests.Infrastructure.NHibernateMaps
{
    [TestFixture]
    public class AutomappingConfigurationTest
    {
        [Test]
        public void ShouldMap_Type_Without_Generic_Type()
        {
            //Arrange
            var type = typeof(List);

            //Act
            var result = new AutomappingConfiguration().ShouldMap(type);

            //Assert
            Assert.IsFalse(result);
        }

        [Test]
        public void ShouldMap_Type_Without_Correct_Type()
        {
            //Arrange
            var type = typeof(List<>);

            //Act
            var result = new AutomappingConfiguration().ShouldMap(type);

            //Assert
            Assert.IsFalse(result);
        }

        [Test]
        public void ShouldMap_Type_With_Correct_Type()
        {
            //Arrange
            var type = typeof (Entity);

            //Act
            var result = new AutomappingConfiguration().ShouldMap(type);

            //Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void ShouldMap_Member_Without_CanWrite()
        {
            //Arrange
            var member = new MethodMember(null);

            //Act
            var result = new AutomappingConfiguration().ShouldMap(member);

            //Assert
            Assert.IsFalse(result);
        }

        [Test]
        public void ShouldMap_Member_CanWrite()
        {
            //Arrange
            var member = new FieldMember(null);

            //Act
            var result = new AutomappingConfiguration().ShouldMap(member);

            //Assert
            Assert.IsTrue(result);
        }

        [Test]
        [Row(typeof(EntityWithTypedId<>))]
        [Row(typeof(Entity))]
        public void AbstractClassIsLayerSupertype_With_Correct_Type(Type type)
        {
            //Act
            var result = new AutomappingConfiguration().AbstractClassIsLayerSupertype(type);

            //Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void AbstractClassIsLayerSupertype_Without_Correct_Type()
        {
            //Arrange
            var type = typeof (String);

            //Act
            var result = new AutomappingConfiguration().AbstractClassIsLayerSupertype(type);

            //Assert
            Assert.IsFalse(result);
        }

        [Test]
        public void IsId_Without_Correct_Name()
        {
            //Arrange
            var member = new MethodMember(null);

            //Act
            var result = new AutomappingConfiguration().IsId(member);

            //Assert
            Assert.IsFalse(result);
        }

        [Test]
        public void IsId_With_Correct_Name()
        {
            //Arrange
            var member = new FieldMember(null);

            //Act
            var result = new AutomappingConfiguration().IsId(member);

            //Assert
            Assert.IsTrue(result);
        }

        private class FieldMember : Member
        {
            private readonly FieldInfo member;

            public override string Name
            {
                get
                {
                    return "Id";
                }
            }

            public override Type PropertyType
            {
                get
                {
                    return this.member.FieldType;
                }
            }

            public override bool CanWrite
            {
                get
                {
                    return true;
                }
            }

            public override MemberInfo MemberInfo
            {
                get
                {
                    return (MemberInfo)this.member;
                }
            }

            public override Type DeclaringType
            {
                get
                {
                    return this.member.DeclaringType;
                }
            }

            public override bool HasIndexParameters
            {
                get
                {
                    return false;
                }
            }

            public override bool IsMethod
            {
                get
                {
                    return false;
                }
            }

            public override bool IsField
            {
                get
                {
                    return true;
                }
            }

            public override bool IsProperty
            {
                get
                {
                    return true;
                }
            }

            public override bool IsAutoProperty
            {
                get
                {
                    return false;
                }
            }

            public override bool IsPrivate
            {
                get
                {
                    return this.member.IsPrivate;
                }
            }

            public override bool IsProtected
            {
                get
                {
                    if (!this.member.IsFamily)
                        return this.member.IsFamilyAndAssembly;
                    else
                        return true;
                }
            }

            public override bool IsPublic
            {
                get
                {
                    return true;
                }
            }

            public override bool IsInternal
            {
                get
                {
                    if (!this.member.IsAssembly)
                        return this.member.IsFamilyAndAssembly;
                    else
                        return true;
                }
            }

            public FieldMember(FieldInfo member)
            {
                this.member = member;
            }

            public override void SetValue(object target, object value)
            {
                this.member.SetValue(target, value);
            }

            public override object GetValue(object target)
            {
                return this.member.GetValue(target);
            }

            public override bool TryGetBackingField(out Member backingField)
            {
                backingField = (Member)null;
                return false;
            }

            public override string ToString()
            {
                return "{Field: " + this.member.Name + "}";
            }
        }

        private class MethodMember : Member
        {
            private readonly MethodInfo member;
            private Member backingField;

            public override string Name
            {
                get
                {
                    return "NotId";
                }
            }

            public override Type PropertyType
            {
                get
                {
                    return this.member.ReturnType;
                }
            }

            public override bool CanWrite
            {
                get
                {
                    return false;
                }
            }

            public override MemberInfo MemberInfo
            {
                get
                {
                    return (MemberInfo)this.member;
                }
            }

            public override Type DeclaringType
            {
                get
                {
                    return this.member.DeclaringType;
                }
            }

            public override bool HasIndexParameters
            {
                get
                {
                    return false;
                }
            }

            public override bool IsMethod
            {
                get
                {
                    return true;
                }
            }

            public override bool IsField
            {
                get
                {
                    return false;
                }
            }

            public override bool IsProperty
            {
                get
                {
                    return false;
                }
            }

            public override bool IsAutoProperty
            {
                get
                {
                    return false;
                }
            }

            public override bool IsPrivate
            {
                get
                {
                    return this.member.IsPrivate;
                }
            }

            public override bool IsProtected
            {
                get
                {
                    if (!this.member.IsFamily)
                        return this.member.IsFamilyAndAssembly;
                    else
                        return true;
                }
            }

            public override bool IsPublic
            {
                get
                {
                    return this.member.IsPublic;
                }
            }

            public override bool IsInternal
            {
                get
                {
                    if (!this.member.IsAssembly)
                        return this.member.IsFamilyAndAssembly;
                    else
                        return true;
                }
            }

            public bool IsCompilerGenerated
            {
                get
                {
                    return Enumerable.Any<object>((IEnumerable<object>)this.member.GetCustomAttributes(typeof(CompilerGeneratedAttribute), true));
                }
            }

            public MethodMember(MethodInfo member)
            {
                this.member = member;
            }

            public override void SetValue(object target, object value)
            {
                throw new NotSupportedException("Cannot set the value of a method Member.");
            }

            public override object GetValue(object target)
            {
                return this.member.Invoke(target, (object[])null);
            }

            public override bool TryGetBackingField(out Member field)
            {
                if (this.backingField != (Member)null)
                {
                    field = this.backingField;
                    return true;
                }
                else
                {
                    string name = this.Name;
                    if (name.StartsWith("Get", StringComparison.InvariantCultureIgnoreCase))
                        name = name.Substring(3);
                    FieldInfo member = (this.DeclaringType.GetField(name, BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic) ?? this.DeclaringType.GetField("_" + name, BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)) ?? this.DeclaringType.GetField("m_" + name, BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                    if (member == null)
                    {
                        field = (Member)null;
                        return false;
                    }
                    else
                    {
                        field = this.backingField = (Member)new FieldMember(member);
                        return true;
                    }
                }
            }

            public override string ToString()
            {
                return "{Method: " + this.member.Name + "}";
            }
        }
    }
}
