﻿using Duende.IdentityServer.Extensions;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using IdentityModel;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using VShop.IdentityServer.Data;

namespace VShop.IdentityServer.Services
{
    public class ProfileAppService : IProfileService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserClaimsPrincipalFactory<ApplicationUser> _userClaimsPrincipalFactory;
        private readonly RoleManager<IdentityRole> _roleManager;

        public ProfileAppService(UserManager<ApplicationUser> userManager, 
            IUserClaimsPrincipalFactory<ApplicationUser> userClaimsPrincipalFactory, 
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _userClaimsPrincipalFactory = userClaimsPrincipalFactory;
            _roleManager = roleManager;
        }

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            //id do usuário no IS
            string id = context.Subject.GetSubjectId();
            //localiza o usuário pelo id
            ApplicationUser user = await _userManager.FindByIdAsync(id);

            //cria ClaimsPrincipal para o usuário
            ClaimsPrincipal userClaims = await _userClaimsPrincipalFactory.CreateAsync(user);

            //define uma coleção de claims para o usuário
            //e inclui o sobrenome e o nome do usuário
            List<Claim> claims = userClaims.Claims.ToList();
            claims.Add(new Claim(JwtClaimTypes.FamilyName, user.LastName));
            claims.Add(new Claim(JwtClaimTypes.GivenName, user.FirstName));

            //se o userManager do identity suportar role
            if(_userManager.SupportsUserRole)
            {
                //obtem a lista dos nomes das roles para o usuário
                IList<string> roles = await _userManager.GetRolesAsync(user);
                //percorre a lista
                foreach(string role in roles)
                {
                    //adiciona a role na claim
                    claims.Add(new Claim(JwtClaimTypes.Role, role));

                    //se romeManager suportar claims para roles
                    if(_roleManager.SupportsRoleClaims)
                    {
                        //localiza o perfil
                        IdentityRole identityRole = await _roleManager.FindByIdAsync(role);

                        //inclui o perfil
                        if(identityRole != null)
                        {
                            //inclui as claims associada com a role
                            claims.AddRange(await _roleManager.GetClaimsAsync(identityRole));
                        }
                    }
                }
            }
            //retorna as claims no contexto
            context.IssuedClaims = claims;

        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            // obtem o id do usuário no IS
            string userid = context.Subject.GetSubjectId(); 
            //localiza o usuário
            ApplicationUser user = await _userManager.FindByIdAsync(userid);

            //verifica se está ativo
            context.IsActive = user != null;
        }
    }
}
