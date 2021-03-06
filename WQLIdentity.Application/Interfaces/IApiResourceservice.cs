﻿using IdentityServer4.EntityFramework.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WQLIdentity.Application.Dtos.ApiResources;
using WQLIdentityServer.Infra.Dto;

namespace WQLIdentity.Application.Interfaces
{
    public interface IApiResourceService
    {
        Pagelist<ApiResourceListDto> GetApiResources(PageInputDto pageInput);
        Task<ApiResourceDto> GetApiResource(int Id);
        Task<bool> CreateApiResource(CreateApiResouce model);
        Task<bool> Update(UpdateApiResource model);
        Task<bool> Remove(int Id);

        Pagelist<ApiScopeDto> GetScopes(PageInputDto pageInput, int apiresourceId);
        Task<ApiScope> GetScope(int scopeId);
        Task<bool> AddScope(CreateApiScopeDto apiScope);
        Task<bool> RemoveScope(int scopeId);
        Task<bool> UpdateScope(UpdateScopeDto scopeDto);

        Pagelist<ApiSecret> GetSecrets(PageInputDto pageInput, int apiresourceId);
        Task<bool> RemoveSecret(int secretId);
        Task<bool> AddSecret(CreateApiSecretDto apiScope);

        Pagelist<ApiResourceProperty> GetProperties(PageInputDto pageInput, int apiresourceId);
        Task<bool> RemovePropertiest(int secretId);
        Task<bool> AddProperties(CreateApiPropertiesDto apiScope);


    }
}
