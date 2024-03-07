using BusinessObject.Common.PagedList;
using LinqKit;
using Mapster;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;


namespace Repository.Helper
{
    public static class EFRepositoryHelpers
    {
        public static IQueryable<TEntity> WhereWithExist<TEntity>(this IQueryable<TEntity> query,
            Expression<Func<TEntity, bool>>? condition = null)
            where TEntity : class
        {
            ArgumentNullException.ThrowIfNull(query, nameof(query));

            if (condition != null)
            {
                return query.Where(condition.AddExistCondition());
            }

            return query;
        }

        public static IQueryable<TEntity> WhereStringWithExist<TEntity>(this IQueryable<TEntity> query,
          string? condition = null)
           where TEntity : class
        {
            string existCondition = "e => e.IsDeleted = false";

            ArgumentNullException.ThrowIfNull(query, nameof(query));

            if (!string.IsNullOrEmpty(condition))
            {
                return query.Where(existCondition + "&&" + condition);
            }

            return query.Where(existCondition);
        }

        public static IQueryable<TResult> SelectWithField<TEntity, TResult>(this IQueryable<TEntity> query,
            Expression<Func<TEntity, TResult>>? selector = null)
            where TEntity : class
            where TResult : class
        {
            ArgumentNullException.ThrowIfNull(query, nameof(query));

            if (selector == null)
                return query.ProjectToType<TResult>();

            return query.Select(selector);
        }

        public static IQueryable<TEntity> AddOrderByString<TEntity>(this IQueryable<TEntity> query, string? orderByString)
        {
            if (string.IsNullOrEmpty(orderByString))
                return query;

            ArgumentNullException.ThrowIfNull(query, nameof(query));

            if (Regex.Match(orderByString, "^\\s*\\w+\\s*:\\s*(asc|desc)\\s*(?:,\\s*\\w+\\s*:\\s*(asc|desc)\\s*)*$\r\n").Success)
                throw new ArgumentException("orderByString is invalid formmat");

            var orderbyStringFmoratted = orderByString.Replace(":", " ");

            return query.OrderBy(orderbyStringFmoratted);

        }

        public static async Task<IPagedList<TEntity>> ToPagedListAsync<TEntity>(this IQueryable<TEntity> query,
            PagingQuery pagingQuery)
            where TEntity : class
        {
            pagingQuery ??= new PagingQuery();
            var pagedList = new PagedList<TEntity>();
            await pagedList.LoadDataAsync(query, pagingQuery);
            return pagedList;
        }

        public static async Task<IPagedList<TResult>> ToPagedListAsync<TEntity, TResult>(this IQueryable<TEntity> query,
            PagingQuery pagingQuery)
            where TEntity : class
            where TResult : class
        {
            var pagedList = new PagedList<TResult>();
            var resultQuery = query.ProjectToType<TResult>();
            await pagedList.LoadDataAsync(resultQuery, pagingQuery);
            return pagedList;
        }

        public static string GetPrimaryKeyName<TEntity>()
            where TEntity : class
        {
            var entityIdName = typeof(TEntity).Name + "Id";
            var primaryKeyProperty = typeof(TEntity).GetProperty(entityIdName);

            if (primaryKeyProperty == null)
            {
                throw new ArgumentException($"Entity {typeof(TEntity).Name} has not Id property");
            }

            return primaryKeyProperty.Name;
        }

        #region Generate Delegate 
        /// <summary>
        /// entity => entity.PropertyName
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="entityType"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static Func<TEntity, TProperty> GenerateLamdaAccessPropertyDelegate<TEntity, TProperty>(Type entityType,
            string propertyName)
            where TEntity : class
        {
            var propertyInfo = entityType.GetProperty(propertyName);
            if (propertyInfo == null)
                throw new ArgumentException($"Entity {typeof(TEntity).Name} has not property {propertyName}");

            ParameterExpression parameter = Expression.Parameter(entityType, "entity");
            MemberExpression propertyAccess = Expression.Property(parameter, propertyName);
            Expression<Func<TEntity, TProperty>> lambda = Expression.Lambda<Func<TEntity, TProperty>>(propertyAccess, parameter);
            return lambda.Compile();

        }

        public static Expression<Func<TEntity, bool>> AddExistCondition<TEntity>(this Expression<Func<TEntity, bool>>? filter)
            where TEntity : class
        {
            PropertyInfo? isDeletedProperty = typeof(TEntity).GetProperty("IsDeleted");

            if (isDeletedProperty == null)
                throw new ArgumentNullException($"Entity {typeof(TEntity).Name} has not IsDeleted property");

            ParameterExpression parameter = Expression.Parameter(typeof(TEntity), "isDeleted");
            MemberExpression isDeletedPropertyAccess = Expression.Property(parameter, isDeletedProperty);
            ConstantExpression isDeleted_is_false = Expression.Constant(false);
            BinaryExpression equalityExpression = Expression.Equal(isDeletedPropertyAccess, isDeleted_is_false);
            Expression<Func<TEntity, bool>> isNotDeleteCondition = Expression.Lambda<Func<TEntity, bool>>(equalityExpression, parameter);

            return filter == null
                ? isNotDeleteCondition
                : isNotDeleteCondition.And(filter);
        }

        /// <summary>
        /// Use with Select function
        ///     Select(GenerateProjectionExpression<TEnitty, TResult>())
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <returns></returns>
        public static Expression<Func<TEntity, TResult>> GenerateProjectionExpression<TEntity, TResult>()
            where TEntity : class
            where TResult : class
        {
            var properties = typeof(TResult).GetProperties();
            return entity => CreateProjectedInstance<TEntity, TResult>(entity, properties);
        }
        #endregion Generate Delegate 

        #region Private function
        private static TResult CreateProjectedInstance<TSource, TResult>(TSource source, PropertyInfo[]? resultProperties)
        {
            if (resultProperties == null)
            {
                throw new ArgumentNullException(nameof(resultProperties));
            }

            var resultInstance = Activator.CreateInstance<TResult>();

            var sourceProperties = typeof(TSource).GetProperties();

            foreach (var resultProperty in resultProperties)
            {
                if (resultProperty == null)
                {
                    continue;
                }

                var sourceProperty = sourceProperties.FirstOrDefault(p => p.Name == resultProperty.Name);

                if (sourceProperty != null)
                {
                    var sourceValue = sourceProperty.GetValue(source);
                    resultProperty.SetValue(resultInstance, sourceValue);
                }
            }

            return resultInstance;
        }

        private static IQueryable<TEntity> OrderByDynamic<TEntity>(this IQueryable<TEntity> query,
            string propertyName, bool isAscOrder = true, bool isOrderBy = true)
        {
            // Use reflection to get property info by name
            PropertyInfo? propertyInfo = typeof(TEntity).GetProperty(propertyName);

            if (propertyInfo == null)
            {
                throw new ArgumentException($"Property '{propertyName}' not found in type {typeof(TEntity).Name}");
            }

            // Build the sorting expression
            ParameterExpression parameter = Expression.Parameter(typeof(TEntity), "x");
            MemberExpression propertyExpression = Expression.Property(parameter, propertyInfo);
            LambdaExpression orderByExpression = Expression.Lambda(propertyExpression, parameter);

            // Create a generic method for OrderBy or OrderByDescending
            MethodInfo orderByMethod;

            if (isOrderBy)
            {
                orderByMethod = isAscOrder
                    ? typeof(Queryable).GetMethods().First(m => m.Name == "OrderBy" && m.GetParameters().Length == 2)
                    : typeof(Queryable).GetMethods().First(m => m.Name == "OrderByDescending" && m.GetParameters().Length == 2);
            }
            else
            {
                orderByMethod = isAscOrder
                    ? typeof(Queryable).GetMethods().First(m => m.Name == "ThenBy" && m.GetParameters().Length == 2)
                    : typeof(Queryable).GetMethods().First(m => m.Name == "ThenByDescending" && m.GetParameters().Length == 2);
            }


            MethodInfo orderByGeneric = orderByMethod.MakeGenericMethod(typeof(TEntity), propertyInfo.PropertyType);

            // Use the dynamic expression with OrderBy or OrderByDescending
            var orderedQuery = orderByGeneric.Invoke(null, new object[] { query.AsQueryable(), orderByExpression });

            return (IQueryable<TEntity>)(orderedQuery ??= new object());
        }

        private static string GetIdPropertyName<TEntity>()
            where TEntity : class
        {

            var idProperty = typeof(TEntity).GetProperty("Id");

            if (idProperty == null)
            {
                throw new ArgumentException($"Entity {typeof(TEntity).Name} has not Id property");
            }

            return idProperty.Name;
        }
        #endregion Private function 

        #region Legacy function 
        #region     SelectWithField with string[] selectedFields
        //private static IQueryable<T> SelectWithField<T>(this IQueryable<T> query, string[] selectedFields)
        //   where T : class
        //{
        //    if (query == null)
        //        throw new ArgumentNullException(nameof(query));

        //    if (selectedFields != null && selectedFields.Length > 0)
        //    {
        //        query = query.Select(GenerateProjectionExpression<T>(selectedFields));
        //    }

        //    return query;
        //}

        //public static Expression<Func<T, T>> GenerateProjectionExpression<T>(string[] selectedFields)
        //{
        //    return entity => CreateProjectedInstance(entity, selectedFields);
        //}

        //private static T CreateProjectedInstance<T>(T source, string[] selectedFields)
        //{
        //    var projectedInstance = Activator.CreateInstance<T>();

        //    var properties = typeof(T).GetProperties();

        //    foreach (var field in selectedFields)
        //    {
        //        var property = properties.FirstOrDefault(p => p.Name == field);

        //        if (property != null)
        //        {
        //            var value = property.GetValue(source);
        //            property.SetValue(projectedInstance, value);
        //        }
        //    }

        //    return projectedInstance;
        //}
        #endregion  SelectWithField with string[] selectedFields

        #endregion Legacy function
    }
}
