using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using XZMHui.Core.Jwt.Model;
using XZMHui.Utils;
using XZMHui.Utils.Extensions;

namespace XZMHui.Core.Jwt.Utils
{
    public class JwtHelper
    {
        /// <summary>
        /// 生成JWT
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="userName"></param>
        /// <param name="roleCode"></param>
        /// <returns></returns>
        public static string IssueJwt(string userid, string userName = null, string[] roleCode = null)
        {
            //创建claim
            var authClaims = new List<Claim> {
                new Claim(ClaimTypes.Name,userName??string.Empty),
                new Claim(JwtRegisteredClaimNames.Jti,userid)
            };

            if (roleCode != null)
                authClaims.AddRange(roleCode.Select(x => new Claim(ClaimTypes.Role, x)));

            // 过期时间
            var utcNow = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Unspecified);

            //秘钥16位
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtOptions.IssuerSigningKey));
            var creds = new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                    //notBefore: utcNow,
                    claims: authClaims,
                    //expires: DateTime.Now.AddMinutes(24 * 60),
                    signingCredentials: creds
                    );
            string jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
        }

        /// <summary>
        /// 获取过期时间
        /// </summary>
        /// <param name="jwtStr"></param>
        /// <returns></returns>
        public static DateTime GetExp(string jwtStr)
        {
            var jwtHandler = new JwtSecurityTokenHandler();
            JwtSecurityToken jwtToken = jwtHandler.ReadJwtToken(jwtStr);

            DateTime expDate = DateTimeHelper.GetDateTimeFromTimestamp((jwtToken.Payload[JwtRegisteredClaimNames.Exp] ?? 0).ParseToLong());
            return expDate;
        }

        public static bool IsExp(string jwtStr)
        {
            return GetExp(jwtStr) < DateTime.Now;
        }

        public static string GetUserId(string jwtStr)
        {
            try
            {
                return new JwtSecurityTokenHandler().ReadJwtToken(jwtStr).Id;
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 解析
        /// </summary>
        /// <param name="jwtStr"></param>
        /// <returns></returns>
        public static string GetUserName(string jwtStr)
        {
            try
            {
                var jwtHandler = new JwtSecurityTokenHandler();
                JwtSecurityToken jwtToken = jwtHandler.ReadJwtToken(jwtStr);
                return jwtToken.Payload[ClaimTypes.Name]?.ToString();
            }
            catch { return string.Empty; }
        }

        /// <summary>
        /// 解析
        /// </summary>
        /// <param name="jwtStr"></param>
        /// <returns></returns>
        public static string GetUserRoleCodes(string jwtStr)
        {
            try
            {
                var jwtHandler = new JwtSecurityTokenHandler();
                JwtSecurityToken jwtToken = jwtHandler.ReadJwtToken(jwtStr);
                return jwtToken.Payload[ClaimTypes.Role]?.ToString();
            }
            catch { return string.Empty; }
        }
    }
}