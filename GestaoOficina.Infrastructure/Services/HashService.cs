using GestaoOficina.Domain.Dtos;
using GestaoOficina.Domain.Entities;
using GestaoOficina.Domain.Interfaces;
using GestaoOficina.Domain.Repositories;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestaoOficina.Infrastructure.Services
{
    public class HashService : IHashService
    {
        private readonly IOficinaRepository _oficinaRepository;
        public HashService(IOficinaRepository oficinaRepository)
        {
            _oficinaRepository = oficinaRepository;
        }

        public string GerarHash(Oficina oficina)
        {
            var passwordHasher = new PasswordHasher<Oficina>();

            return passwordHasher.HashPassword(oficina, oficina.Senha);
        } 
        private async Task AtualizarHashUsuario(Oficina oficina, PasswordHasher<Oficina> passwordHasher)
        {
            var newHash = passwordHasher.HashPassword(oficina, oficina.Senha);
            oficina.Senha = newHash;

            await _oficinaRepository.AtualizarSenhaOficina(oficina);
        }

        public async Task<bool> ValidaEAtualizaHashAsync(Oficina oficina, string hash)
        {
            var passwordHasher = new PasswordHasher<Oficina>();
            var status = passwordHasher.VerifyHashedPassword(oficina, hash, oficina.Senha);
            switch (status)
            {
                case PasswordVerificationResult.Failed:
                    return false;

                case PasswordVerificationResult.Success:
                    return true;

                case PasswordVerificationResult.SuccessRehashNeeded:
                    await AtualizarHashUsuario(oficina, passwordHasher);
                    return true;

                default:
                    throw new InvalidOperationException();
            }
        }
    }
}
