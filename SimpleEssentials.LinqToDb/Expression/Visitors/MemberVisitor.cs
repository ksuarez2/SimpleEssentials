﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using SimpleEssentials.LinqToDb.Expression.Interpretor;

namespace SimpleEssentials.LinqToDb.Expression.Visitors
{
   public class MemberVisitor : Visitor
    {
        private readonly MemberExpression node;
        public MemberVisitor(MemberExpression node, ref IInterpreter interpreter) : base(node)
        {
            this.node = node;
            this.Interpretor = interpreter;
        }

        public override void Visit(string prefix = "", string postfix = "")
        {
            if (node.Member is PropertyInfo property)
            {
                var propName = property.Name;
                //Console.WriteLine($"{prefix}This expression is a {NodeType} expression type");
                //Console.WriteLine($"{prefix}The name of the property is {propName}");
                //Console.WriteLine($"{prefix}The return type is {node.Type}");

                //Interpretor.WherePart.Sql += $"[{propName}]";
                if(node.Member.DeclaringType != null)
                    this.Interpretor.WherePart.Concat($"[{node.Member.DeclaringType.Name}].[{prefix}{propName}{postfix}]");
                else
                    this.Interpretor.WherePart.Concat($"[{prefix}{propName}{postfix}]");

            }
            if (node.Member is FieldInfo fieldInfo)
            {
                var fieldName = fieldInfo.Name;
                //Console.WriteLine($"{prefix}This expression is a {NodeType} expression type");
                //Console.WriteLine($"{prefix}The name of the FieldInfo is {fieldName}");
                //Console.WriteLine($"{prefix}The return type is {node.Type}");
                //Interpretor.WherePart.Sql += $"[{fieldName}]";
                this.Interpretor.WherePart.Concat($"[{prefix}{fieldName}{postfix}]");

            }
            //throw new Exception($"Expression does not refer to a property or field: {node}");
        }

        public object GetValue(System.Linq.Expressions.Expression member)
        {
            // source: http://stackoverflow.com/a/2616980/291955
            var objectMember = System.Linq.Expressions.Expression.Convert(member, typeof(object));
            var getterLambda = System.Linq.Expressions.Expression.Lambda<Func<object>>(objectMember);
            var getter = getterLambda.Compile();
            return getter();
        }
    }
}