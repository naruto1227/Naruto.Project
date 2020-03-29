
using System;
using System.Reflection;
using Naruto.AspNetdentity.MongoDB;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.MongoDB;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Microsoft.Extensions.DependencyInjection
{

    /// <summary>
    /// aspnetidentityµÄmongodb´æ´¢À©Õ¹
    /// </summary>
    public static class IdentityMongoDBBuilderExtensions
    {
        /// <summary>
        /// ×¢ÈëmongodbµÄ´æ´¢
        /// </summary>
        public static IdentityBuilder AddMongoDBStores(this IdentityBuilder builder, Action<AspNetdentityMongoContext> context)
        {
            //×¢Èë²Ö´¢
            builder.Services.AddMongoServices()
                .AddMongoContext(context);
            AddStores(builder.Services, builder.UserType, builder.RoleType);
            return builder;
        }

        private static void AddStores(IServiceCollection services, Type userType, Type roleType)
        {
            var identityUserType = FindGenericBaseType(userType, typeof(IdentityUser<>));
            if (identityUserType == null)
            {
                throw new InvalidOperationException(nameof(identityUserType));
            }

            if (roleType != null)
            {
                var identityRoleType = FindGenericBaseType(roleType, typeof(IdentityRole<>));
                if (identityRoleType == null)
                {
                    throw new InvalidOperationException(nameof(identityRoleType));
                }

                services.TryAddScoped(typeof(IUserStore<>).MakeGenericType(userType), typeof(UserStore<>));
                services.TryAddScoped(typeof(IRoleStore<>).MakeGenericType(roleType), typeof(RoleStore<>));
            }
            else
            {   // No Roles
                services.TryAddScoped(typeof(IUserStore<>).MakeGenericType(userType), typeof(UserOnlyStore<>));
            }
        }

        private static TypeInfo FindGenericBaseType(Type currentType, Type genericBaseType)
        {
            var type = currentType;
            while (type != null)
            {
                var typeInfo = type.GetTypeInfo();
                var genericType = type.IsGenericType ? type.GetGenericTypeDefinition() : null;
                if (genericType != null && genericType == genericBaseType)
                {
                    return typeInfo;
                }
                type = type.BaseType;
            }
            return null;
        }
    }
}