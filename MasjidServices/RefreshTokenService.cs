using Azure;
using GravyFoodsApi.Data;
using GravyFoodsApi.Models;
using GravyFoodsApi.Models.DTOs;
using Microsoft.EntityFrameworkCore;
using System;

namespace GravyFoodsApi.MasjidServices
{

    public class RefreshTokenService
    {
        private readonly MasjidDBContext _context;

        public RefreshTokenService(MasjidDBContext context)
        {
            _context = context;
        }


        public async Task RefreshTokenAsync(string token)
        {
            return;

        }
    }
}