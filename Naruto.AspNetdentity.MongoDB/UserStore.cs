// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Naruto.AspNetdentity.MongoDB;
using Naruto.MongoDB.Interface;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver.Linq;

/// <summary>
/// 张海波
/// 2020-02-10
/// 更改用户存储操作方式为mongodb
/// </summary>
namespace Microsoft.AspNetCore.Identity.MongoDB
{
    /// <summary>
    /// Represents a new instance of a persistence store for users, using the default implementation
    /// of <see cref="IdentityUser{TKey}"/> with a string as a primary key.
    /// </summary>
    public class UserStore : UserStore<IdentityUser<string>>
    {
        /// <summary>
        /// Constructs a new instance of <see cref="UserStore"/>.
        /// </summary>
        /// <param name="context">The <see cref="DbContext"/>.</param>
        /// <param name="describer">The <see cref="IdentityErrorDescriber"/>.</param>
        public UserStore(IMongoRepository<AspNetdentityMongoContext> context, IdentityErrorDescriber describer = null) : base(context, describer) { }
    }

    /// <summary>
    /// Creates a new instance of a persistence store for the specified user type.
    /// </summary>
    /// <typeparam name="TUser">The type representing a user.</typeparam>
    public class UserStore<TUser> : UserStore<TUser, IdentityRole, IMongoRepository<AspNetdentityMongoContext>, string>
        where TUser : IdentityUser<string>, new()
    {
        /// <summary>
        /// Constructs a new instance of <see cref="UserStore{TUser}"/>.
        /// </summary>
        /// <param name="context">The <see cref="DbContext"/>.</param>
        /// <param name="describer">The <see cref="IdentityErrorDescriber"/>.</param>
        public UserStore(IMongoRepository<AspNetdentityMongoContext> context, IdentityErrorDescriber describer = null) : base(context, describer) { }
    }

    /// <summary>
    /// Represents a new instance of a persistence store for the specified user and role types.
    /// </summary>
    /// <typeparam name="TUser">The type representing a user.</typeparam>
    /// <typeparam name="TRole">The type representing a role.</typeparam>
    /// <typeparam name="TContext">The type of the data context class used to access the store.</typeparam>
    public class UserStore<TUser, TRole, TContext> : UserStore<TUser, TRole, TContext, string>
        where TUser : IdentityUser<string>
        where TRole : IdentityRole<string>
        where TContext : IMongoRepository<AspNetdentityMongoContext>
    {
        /// <summary>
        /// Constructs a new instance of <see cref="UserStore{TUser, TRole, TContext}"/>.
        /// </summary>
        /// <param name="context">The <see cref="DbContext"/>.</param>
        /// <param name="describer">The <see cref="IdentityErrorDescriber"/>.</param>
        public UserStore(TContext context, IdentityErrorDescriber describer = null) : base(context, describer) { }
    }

    /// <summary>
    /// Represents a new instance of a persistence store for the specified user and role types.
    /// </summary>
    /// <typeparam name="TUser">The type representing a user.</typeparam>
    /// <typeparam name="TRole">The type representing a role.</typeparam>
    /// <typeparam name="TContext">The type of the data context class used to access the store.</typeparam>
    /// <typeparam name="TKey">The type of the primary key for a role.</typeparam>
    public class UserStore<TUser, TRole, TContext, TKey> : UserStore<TUser, TRole, TContext, TKey, IdentityUserClaim<TKey>, IdentityUserRole<TKey>, IdentityUserLogin<TKey>, IdentityUserToken<TKey>, IdentityRoleClaim<TKey>>
        where TUser : IdentityUser<TKey>
        where TRole : IdentityRole<TKey>
        where TContext : IMongoRepository<AspNetdentityMongoContext>
        where TKey : IEquatable<TKey>
    {
        /// <summary>
        /// Constructs a new instance of <see cref="UserStore{TUser, TRole, TContext, TKey}"/>.
        /// </summary>
        /// <param name="context">The <see cref="DbContext"/>.</param>
        /// <param name="describer">The <see cref="IdentityErrorDescriber"/>.</param>
        public UserStore(TContext context, IdentityErrorDescriber describer = null) : base(context, describer) { }
    }

    /// <summary>
    /// Represents a new instance of a persistence store for the specified user and role types.
    /// </summary>
    /// <typeparam name="TUser">The type representing a user.</typeparam>
    /// <typeparam name="TRole">The type representing a role.</typeparam>
    /// <typeparam name="TContext">The type of the data context class used to access the store.</typeparam>
    /// <typeparam name="TKey">The type of the primary key for a role.</typeparam>
    /// <typeparam name="TUserClaim">The type representing a claim.</typeparam>
    /// <typeparam name="TUserRole">The type representing a user role.</typeparam>
    /// <typeparam name="TUserLogin">The type representing a user external login.</typeparam>
    /// <typeparam name="TUserToken">The type representing a user token.</typeparam>
    /// <typeparam name="TRoleClaim">The type representing a role claim.</typeparam>
    public class UserStore<TUser, TRole, TContext, TKey, TUserClaim, TUserRole, TUserLogin, TUserToken, TRoleClaim> :
        UserStoreBase<TUser, TRole, TKey, TUserClaim, TUserRole, TUserLogin, TUserToken, TRoleClaim>,
        IProtectedUserStore<TUser>
        where TUser : IdentityUser<TKey>
        where TRole : IdentityRole<TKey>
        where TContext : IMongoRepository<AspNetdentityMongoContext>
        where TKey : IEquatable<TKey>
        where TUserClaim : IdentityUserClaim<TKey>, new()
        where TUserRole : IdentityUserRole<TKey>, new()
        where TUserLogin : IdentityUserLogin<TKey>, new()
        where TUserToken : IdentityUserToken<TKey>, new()
        where TRoleClaim : IdentityRoleClaim<TKey>, new()
    {
        /// <summary>
        /// Creates a new instance of the store.
        /// </summary>
        /// <param name="context">The context used to access the store.</param>
        /// <param name="describer">The <see cref="IdentityErrorDescriber"/> used to describe store errors.</param>
        public UserStore(TContext context, IdentityErrorDescriber describer = null) : base(describer ?? new IdentityErrorDescriber())
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }
            Context = context;
        }

        /// <summary>
        /// Gets the database context for this store.
        /// </summary>
        public virtual TContext Context { get; private set; }

        public override IQueryable<TUser> Users => Context.Query<TUser>().AsQueryable();

        /// <summary>
        /// 创建用户
        /// </summary>
        /// <param name="user"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async override Task<IdentityResult> CreateAsync(TUser user, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            await Context.Command<TUser>().AddAsync(user, null, cancellationToken);

            return IdentityResult.Success;
        }

        /// <summary>
        /// 修改用户
        /// </summary>
        /// <param name="user"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async override Task<IdentityResult> UpdateAsync(TUser user, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            user.ConcurrencyStamp = Guid.NewGuid().ToString();
            await Context.Command<TUser>().ReplaceOneAsync(a => a.UserName == user.UserName, user, null, cancellationToken);
            return IdentityResult.Success;
        }

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="user"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async override Task<IdentityResult> DeleteAsync(TUser user, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            await Context.Command<TUser>().DeleteAsync(a => a.UserName == user.UserName);

            return IdentityResult.Success;
        }

        /// <summary>
        /// 根据id查找
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override Task<TUser> FindByIdAsync(string userId, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            var id = ConvertIdFromString(userId);
            return Context.Query<TUser>().FirstOrDefaultAsync(Builders<TUser>.Filter.Eq(a => a.Id, id), null, cancellationToken);
        }


        /// <summary>
        /// 根据名称查询
        /// </summary>
        /// <param name="normalizedUserName"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override Task<TUser> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();

            return Context.Query<TUser>().FirstOrDefaultAsync(u => u.NormalizedUserName == normalizedUserName, null, cancellationToken);
        }


        /// <summary>
        /// 根据角色名称查询角色信息
        /// </summary>
        /// <param name="normalizedRoleName"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        protected override Task<TRole> FindRoleAsync(string normalizedRoleName, CancellationToken cancellationToken)
        {
            return Context.Query<TRole>().FirstOrDefaultAsync(r => r.NormalizedName == normalizedRoleName, null
                , cancellationToken);
        }


        /// <summary>
        /// 查询 用户角色信息
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="roleId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        protected override Task<TUserRole> FindUserRoleAsync(TKey userId, TKey roleId, CancellationToken cancellationToken)
        {
            return Context.Query<TUserRole>().FirstOrDefaultAsync(Builders<TUserRole>.Filter.And(Builders<TUserRole>.Filter.Eq(a => a.UserId, userId), Builders<TUserRole>.Filter.Eq(a => a.RoleId, roleId)), null
                , cancellationToken);
        }


        /// <summary>
        /// 根据用户id查询用户信息
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        protected override Task<TUser> FindUserAsync(TKey userId, CancellationToken cancellationToken)
        {
            return Context.Query<TUser>().FirstOrDefaultAsync(Builders<TUser>.Filter.Eq(a => a.Id, userId), null, cancellationToken);
        }

        /// <summary>
        /// 获取用户登录信息
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="loginProvider"></param>
        /// <param name="providerKey"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        protected override Task<TUserLogin> FindUserLoginAsync(TKey userId, string loginProvider, string providerKey, CancellationToken cancellationToken)
        {
            FilterDefinition<TUserLogin> where = Builders<TUserLogin>.Filter.And(
                Builders<TUserLogin>.Filter.Eq(a => a.UserId, userId),
                Builders<TUserLogin>.Filter.Eq(a => a.LoginProvider, loginProvider),
                Builders<TUserLogin>.Filter.Eq(a => a.ProviderKey, providerKey)
                );

            return Context.Query<TUserLogin>().FirstOrDefaultAsync(where, null
               , cancellationToken);
        }


        /// <summary>
        /// 获取用户登录信息
        /// </summary>
        /// <param name="loginProvider"></param>
        /// <param name="providerKey"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        protected override Task<TUserLogin> FindUserLoginAsync(string loginProvider, string providerKey, CancellationToken cancellationToken)
        {
            FilterDefinition<TUserLogin> where = Builders<TUserLogin>.Filter.And(
                Builders<TUserLogin>.Filter.Eq(a => a.LoginProvider, loginProvider),
                Builders<TUserLogin>.Filter.Eq(a => a.ProviderKey, providerKey)
                );
            return Context.Query<TUserLogin>().FirstOrDefaultAsync(where, null, cancellationToken);
        }


        /// <summary>
        /// 添加用户的角色关联信息
        /// </summary>
        /// <param name="user"></param>
        /// <param name="normalizedRoleName"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async override Task AddToRoleAsync(TUser user, string normalizedRoleName, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            if (string.IsNullOrWhiteSpace(normalizedRoleName))
            {
                throw new ArgumentException(nameof(normalizedRoleName));
            }
            var roleEntity = await FindRoleAsync(normalizedRoleName, cancellationToken);
            if (roleEntity == null)
            {
                throw new InvalidOperationException(nameof(roleEntity));
            }
            await Context.Command<TUserRole>().AddAsync(CreateUserRole(user, roleEntity));
        }


        /// <summary>
        /// 移除用户的角色信息
        /// </summary>
        /// <param name="user"></param>
        /// <param name="normalizedRoleName"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async override Task RemoveFromRoleAsync(TUser user, string normalizedRoleName, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            if (string.IsNullOrWhiteSpace(normalizedRoleName))
            {
                throw new ArgumentException(nameof(normalizedRoleName));
            }
            var roleEntity = await FindRoleAsync(normalizedRoleName, cancellationToken);
            if (roleEntity != null)
            {
                FilterDefinition<TUserRole> filter = Builders<TUserRole>.Filter.And(Builders<TUserRole>.Filter.Eq(a => a.UserId, user.Id),
                    Builders<TUserRole>.Filter.Eq(a => a.RoleId, roleEntity.Id));
                await Context.Command<TUserRole>().DeleteAsync(filter);
            }
        }

        /// <summary>
        /// 获取用户角色名称
        /// </summary>
        /// <param name="user">The user whose roles should be retrieved.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="Task{TResult}"/> that contains the roles the user is a member of.</returns>
        public override async Task<IList<string>> GetRolesAsync(TUser user, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            var userId = user.Id;

            var query = from userRole in Context.Query<TUserRole>().AsQueryable()
                        join role in Context.Query<TRole>().AsQueryable() on userRole.RoleId equals role.Id
                        where userRole.UserId.Equals(userId)
                        select role.Name;
            return await query.ToListAsync(cancellationToken);
        }

        /// <summary>
        /// Returns a flag indicating if the specified user is a member of the give <paramref name="normalizedRoleName"/>.
        /// </summary>
        /// <param name="user">The user whose role membership should be checked.</param>
        /// <param name="normalizedRoleName">The role to check membership of</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="Task{TResult}"/> containing a flag indicating if the specified user is a member of the given group. If the
        /// user is a member of the group the returned value with be true, otherwise it will be false.</returns>
        public override async Task<bool> IsInRoleAsync(TUser user, string normalizedRoleName, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            if (string.IsNullOrWhiteSpace(normalizedRoleName))
            {
                throw new ArgumentException(nameof(normalizedRoleName));
            }
            var role = await FindRoleAsync(normalizedRoleName, cancellationToken);
            if (role != null)
            {
                var userRole = await FindUserRoleAsync(user.Id, role.Id, cancellationToken);
                return userRole != null;
            }
            return false;
        }

        /// <summary>
        /// Get the claims associated with the specified <paramref name="user"/> as an asynchronous operation.
        /// </summary>
        /// <param name="user">The user whose claims should be retrieved.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="Task{TResult}"/> that contains the claims granted to a user.</returns>
        public async override Task<IList<Claim>> GetClaimsAsync(TUser user, CancellationToken cancellationToken = default(CancellationToken))
        {
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            return await Context.Query<TUserClaim>().AsQueryable().Where(uc => uc.UserId.Equals(user.Id)).Select(c => c.ToClaim()).ToListAsync(cancellationToken);
        }

        /// <summary>
        /// 添加用户声明
        /// </summary>
        /// <param name="user">The user to add the claim to.</param>
        /// <param name="claims">The claim to add to the user.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation.</returns>
        public override Task AddClaimsAsync(TUser user, IEnumerable<Claim> claims, CancellationToken cancellationToken = default(CancellationToken))
        {
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            if (claims == null)
            {
                throw new ArgumentNullException(nameof(claims));
            }
            var addList = new List<TUserClaim>();
            foreach (var claim in claims)
            {
                addList.Add(CreateUserClaim(user, claim));
            }
            Context.Command<TUserClaim>().BulkAddAsync(addList);
            return Task.FromResult(false);
        }

        /// <summary>
        ///替换用户的声明信息
        /// </summary>
        /// <param name="user">The user to replace the claim on.</param>
        /// <param name="claim">The claim replace.</param>
        /// <param name="newClaim">The new claim replacing the <paramref name="claim"/>.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation.</returns>
        public async override Task ReplaceClaimAsync(TUser user, Claim claim, Claim newClaim, CancellationToken cancellationToken = default(CancellationToken))
        {
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            if (claim == null)
            {
                throw new ArgumentNullException(nameof(claim));
            }
            if (newClaim == null)
            {
                throw new ArgumentNullException(nameof(newClaim));
            }

            await Context.Command<TUserClaim>().BulkUpdateAsync(uc => uc.UserId.Equals(user.Id) && uc.ClaimValue == claim.Value && uc.ClaimType == claim.Type, new Dictionary<string, object>()
            {
                { "ClaimValue",newClaim.Value},
                { "ClaimType",newClaim.Type}
            }, null, cancellationToken);
        }

        /// <summary>
        /// 移除声明信息
        /// </summary>
        /// <param name="user">The user to remove the claims from.</param>
        /// <param name="claims">The claim to remove.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation.</returns>
        public async override Task RemoveClaimsAsync(TUser user, IEnumerable<Claim> claims, CancellationToken cancellationToken = default(CancellationToken))
        {
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            if (claims == null)
            {
                throw new ArgumentNullException(nameof(claims));
            }

            foreach (var claim in claims)
            {
                await Context.Command<TUserClaim>().BulkDeleteAsync(uc => uc.UserId.Equals(user.Id) && uc.ClaimValue == claim.Value && uc.ClaimType == claim.Type, null, cancellationToken);
            }
        }

        /// <summary>
        ///添加用户登录信息
        /// </summary>
        /// <param name="user">The user to add the login to.</param>
        /// <param name="login">The login to add to the user.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation.</returns>
        public override Task AddLoginAsync(TUser user, UserLoginInfo login,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            if (login == null)
            {
                throw new ArgumentNullException(nameof(login));
            }
            Context.Command<TUserLogin>().AddAsync(CreateUserLogin(user, login));
            return Task.FromResult(false);
        }

        /// <summary>
        /// 移除用户登录记录
        /// </summary>
        /// <param name="user">The user to remove the login from.</param>
        /// <param name="loginProvider">The login to remove from the user.</param>
        /// <param name="providerKey">The key provided by the <paramref name="loginProvider"/> to identify a user.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation.</returns>
        public override async Task RemoveLoginAsync(TUser user, string loginProvider, string providerKey,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            await Context.Command<TUserLogin>().DeleteAsync(a => a.UserId.Equals(user.Id) && a.LoginProvider == loginProvider && a.ProviderKey == providerKey, null, cancellationToken);
        }

        /// <summary>
        /// 获取用户的登录记录
        /// </summary>
        /// <param name="user">The user whose associated logins to retrieve.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>
        /// The <see cref="Task"/> for the asynchronous operation, containing a list of <see cref="UserLoginInfo"/> for the specified <paramref name="user"/>, if any.
        /// </returns>
        public async override Task<IList<UserLoginInfo>> GetLoginsAsync(TUser user, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            var userId = user.Id;
            return await Context.Query<TUserLogin>().AsQueryable().Where(l => l.UserId.Equals(userId))
                .Select(l => new UserLoginInfo(l.LoginProvider, l.ProviderKey, l.ProviderDisplayName)).ToListAsync(cancellationToken);
        }

        /// <summary>
        /// Retrieves the user associated with the specified login provider and login provider key.
        /// </summary>
        /// <param name="loginProvider">The login provider who provided the <paramref name="providerKey"/>.</param>
        /// <param name="providerKey">The key provided by the <paramref name="loginProvider"/> to identify a user.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>
        /// The <see cref="Task"/> for the asynchronous operation, containing the user, if any which matched the specified login provider and key.
        /// </returns>
        public async override Task<TUser> FindByLoginAsync(string loginProvider, string providerKey,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            var userLogin = await FindUserLoginAsync(loginProvider, providerKey, cancellationToken);
            if (userLogin != null)
            {
                return await FindUserAsync(userLogin.UserId, cancellationToken);
            }
            return null;
        }

        /// <summary>
        /// Gets the user, if any, associated with the specified, normalized email address.
        /// </summary>
        /// <param name="normalizedEmail">The normalized email address to return the user for.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>
        /// The task object containing the results of the asynchronous lookup operation, the user if any associated with the specified normalized email address.
        /// </returns>
        public override Task<TUser> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();

            return Context.Query<TUser>().FirstOrDefaultAsync(u => u.NormalizedEmail == normalizedEmail);
        }

        /// <summary>
        /// Retrieves all users with the specified claim.
        /// </summary>
        /// <param name="claim">The claim whose users should be retrieved.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>
        /// The <see cref="Task"/> contains a list of users, if any, that contain the specified claim.
        /// </returns>
        public async override Task<IList<TUser>> GetUsersForClaimAsync(Claim claim, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (claim == null)
            {
                throw new ArgumentNullException(nameof(claim));
            }

            var query = from userclaims in Context.Query<TUserClaim>().AsQueryable()
                        join user in Context.Query<TUser>().AsQueryable() on userclaims.UserId equals user.Id
                        where userclaims.ClaimValue == claim.Value
                        && userclaims.ClaimType == claim.Type
                        select user;

            return await query.ToListAsync(cancellationToken);
        }

        /// <summary>
        /// Retrieves all users in the specified role.
        /// </summary>
        /// <param name="normalizedRoleName">The role whose users should be retrieved.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>
        /// The <see cref="Task"/> contains a list of users, if any, that are in the specified role.
        /// </returns>
        public async override Task<IList<TUser>> GetUsersInRoleAsync(string normalizedRoleName, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (string.IsNullOrEmpty(normalizedRoleName))
            {
                throw new ArgumentNullException(nameof(normalizedRoleName));
            }

            var role = await FindRoleAsync(normalizedRoleName, cancellationToken);

            if (role != null)
            {
                var query = from userrole in Context.Query<TUserRole>().AsQueryable()
                            join user in Context.Query<TUser>().AsQueryable() on userrole.UserId equals user.Id
                            where userrole.RoleId.Equals(role.Id)
                            select user;

                return await query.ToListAsync(cancellationToken);
            }
            return new List<TUser>();
        }

        /// <summary>
        /// Find a user token if it exists.
        /// </summary>
        /// <param name="user">The token owner.</param>
        /// <param name="loginProvider">The login provider for the token.</param>
        /// <param name="name">The name of the token.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>The user token if it exists.</returns>
        protected override Task<TUserToken> FindTokenAsync(TUser user, string loginProvider, string name, CancellationToken cancellationToken)
            => Context.Query<TUserToken>().FirstOrDefaultAsync(Builders<TUserToken>.Filter.And(Builders<TUserToken>.Filter.Eq(a => a.UserId, user.Id), Builders<TUserToken>.Filter.Eq(a => a.LoginProvider, loginProvider), Builders<TUserToken>.Filter.Eq(a => a.Name, name)), null, cancellationToken);

        /// <summary>
        /// Add a new user token.
        /// </summary>
        /// <param name="token">The token to be added.</param>
        /// <returns></returns>
        protected override Task AddUserTokenAsync(TUserToken token)
        {
            Context.Command<TUserToken>().AddAsync(token);
            return Task.CompletedTask;
        }


        /// <summary>
        /// Remove a new user token.
        /// </summary>
        /// <param name="token">The token to be removed.</param>
        /// <returns></returns>
        protected override Task RemoveUserTokenAsync(TUserToken token)
        {
            Context.Command<TUserToken>().DeleteAsync(Builders<TUserToken>.Filter.And(Builders<TUserToken>.Filter.Eq(a => a.UserId, token.UserId), Builders<TUserToken>.Filter.Eq(a => a.LoginProvider, token.LoginProvider), Builders<TUserToken>.Filter.Eq(a => a.Name, token.Name)));
            return Task.CompletedTask;
        }
    }
}
