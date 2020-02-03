// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


#pragma warning disable 1591

namespace Fate.Infrastructure.Id4.Entities
{
    public abstract class UserClaim: BaseMongo.Model.IMongoEntity
    {
        public int Id { get; set; }
        public string Type { get; set; }
    }
}