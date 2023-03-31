using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using AutoMapper.Execution;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class UserRepository : IUserRepository
    {
        private readonly DataConetxt _conetxt;
        private readonly IMapper _mapper;
        public UserRepository(DataConetxt conetxt, IMapper mapper)
        {
            _mapper = mapper;
            _conetxt = conetxt;
            
        }

        public async Task<MemberDto> GetMemberAsync(string username)
        {
            return await _conetxt.Users
                .Where(x => x.UserName == username)
                .ProjectTo<MemberDto>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync();
        }

        public async Task<IEnumerable<MemberDto>> GetMemberAsync()
        {
            return await _conetxt.Users
            .ProjectTo<MemberDto>(_mapper.ConfigurationProvider) 
            .ToListAsync();
        }

        public async Task<AppUser> GetUSerByIdAsync(int id)
        {
            return await _conetxt.Users.FindAsync(id);
        }

        public async Task<AppUser> GetUserByUsernameAsync(string username)
        {
            return await _conetxt.Users
                .Include(p => p.Photos)
                .SingleOrDefaultAsync(x => x.UserName == username);
        }

        public async Task<IEnumerable<AppUser>> GetUsersAsync()
        {
            return await _conetxt.Users
                .Include(p => p.Photos)
                .ToListAsync();
        }

        public  async Task<bool> SaveAllAsync()
        {
            return await _conetxt.SaveChangesAsync() > 0;
        }

        public void Update(AppUser user)
        {
            _conetxt.Entry(user).State = EntityState.Modified;
        }
    }
}