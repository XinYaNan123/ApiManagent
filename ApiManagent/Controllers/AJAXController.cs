using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ApiManagent.Models;
using DataAccess.DBContent;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using System.Threading;

namespace ApiManagent.Controllers
{

    public class AJAXController : BaseController
    {
        private readonly ILogger<HomeController> _logger;

        public AJAXController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        #region 用户信息
        /// <summary>
        /// 登入
        /// </summary>
        /// <param name="uname"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        [HttpPost, AllowAnonymous]
        public async Task<IActionResult> Login(string account, string pwd)
        {
            if (string.IsNullOrWhiteSpace(account) || string.IsNullOrWhiteSpace(pwd)) return Json(new ResultModel() { Message = "账号或者密码错误" });
            using var content = new ApiManagentContext();
            var UserEntity = content.ApiUsers.Where(p => p.Account == account.Trim() && p.Password == pwd.Trim()).FirstOrDefault();
            if (UserEntity == null) return Json(new ResultModel() { Message = "账号或者密码错误" });
            if (UserEntity.State != 1) return Json(new ResultModel() { Message = "账号已被禁用" });

            var identity = new ClaimsIdentity("Account");
            identity.AddClaim(new Claim(ClaimTypes.Sid, UserEntity.Id.ToString()));
            identity.AddClaim(new Claim(ClaimTypes.Name, UserEntity.Account));
            var claimsPrincipal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                claimsPrincipal,
                new AuthenticationProperties()
                {
                    IsPersistent = true
                }
                );
            var UserData = new Dictionary<string, object>();
            //用户信息
            UserData.Add("UserEntityCookie", new
            {
                UserEntity.Id,
                UserEntity.UserName,
                UserEntity.UserType,
                UserEntity.UserFace,
                UserEntity.Account,
            });
            //接口域
            UserData.Add("DomainList", content.ApiDomain.ToList().Select(p => new
            {
                p.Id,
                p.Title,
                p.AddressUrl
            }));
            return Json(new ResultModel()
            {
                Data = UserData,
                Code = 1000
            });
        }
        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> GetUserInfo()
        {
            var Uid = User.FindFirst(ClaimTypes.Sid).Value;
            var Account = User.FindFirst(ClaimTypes.Name).Value;
            using var content = new ApiManagentContext();
            var UserEntity = content.ApiUsers.Where(p => p.Id == int.Parse(Uid) && p.Account == Account).FirstOrDefault();
            if (UserEntity != null && UserEntity.State == 1)//登入成功 返回用户信息
            {
                var UserEntityCookie = new
                {
                    UserEntity.Id,
                    UserEntity.UserName,
                    UserEntity.UserType,
                    UserEntity.UserFace,
                    UserEntity.Account,
                };
                return Json(new ResultModel()
                {
                    Data = UserEntityCookie,
                    Code = 1000
                });
            }
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Response.StatusCode = 401;
            return Json(new ResultModel() { Message = "身份已过期" });
        }
        /// <summary>
        /// 退出登入
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Json(new ResultModel() { Code = 1000 });
        }
        #endregion
        #region 接口操作
        /// <summary>
        /// 获取接口域
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult GetDomain(int id = 0)
        {
            using var content = new ApiManagentContext();
            if (id > 0)
            {
                //接口域
                var DomainEntity = content.ApiDomain.Where(p => p.Id == id).FirstOrDefault();
                return Json(new ResultModel()
                {
                    Data = new
                    {
                        DomainEntity.Id,
                        DomainEntity.Title,
                        DomainEntity.AddressUrl,
                    },
                    Code = 1000
                });
            }
            else
            {
                //接口域
                var DomainList = content.ApiDomain.ToList().Select(p => new
                {
                    p.Id,
                    p.Title,
                    p.AddressUrl
                });
                return Json(new ResultModel()
                {
                    Data = DomainList,
                    Code = 1000
                });
            }
        }
        /// <summary>
        ///  获取接口项目
        /// </summary>
        /// <param name="doId">接口域ID</param>
        /// <param name="id">项目ID</param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult GetProject(int doId, int id = 0)
        {
            using var content = new ApiManagentContext();
            if (id > 0)
            {
                var DomainEntity = content.ApiProject.Where(p => p.IsDel == 0 && p.Id == id && p.ApiDomainId == doId).FirstOrDefault();
                return Json(new ResultModel()
                {
                    Data = DomainEntity,
                    Code = 1000
                });

            }
            else
            {
                var apidetaList = content.ApiApiDetails.Where(p => p.State <= 2).ToList();

                var DomainList = content.ApiProject
                    .Where(p => p.ApiDomainId == doId && p.IsDel == 0)
                    .OrderByDescending(p => p.ShowOrder)
                    .ThenByDescending(p => p.Id).ToList()
                    .Select(p => new
                    {
                        p.Id,
                        p.Title,
                        p.ApiDomainId,
                        ApiDetails = apidetaList.Where(q => q.ApiProjectId == p.Id).Select(q => new
                        {
                            q.ApiProjectId,
                            q.ApiDomainId,
                            q.CreateDate,
                            q.Describe,
                            q.Id,
                            q.Methods,
                            q.NameClass,
                            q.Path,
                            q.ShowOrder,
                            q.State,
                            q.SwitchName,
                            q.Title,
                            q.UpdateDate,
                            q.VersionCode,
                            RequestParam = new string[] { },
                            ReturnParam = new string[] { },
                        })
                    });

                return Json(new ResultModel()
                {
                    Data = DomainList,
                    Code = 1000
                });
            }
        }

        /// <summary>
        /// 通过接口id获取接口参数
        /// </summary>
        /// <param name="apiId"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Get_ApiParam(int apiId)
        {
            if (apiId < 1) return Json(new ResultModel() { Message = "接口id无效！" });
            using var content = new ApiManagentContext();
            var apilist=content.ApiParam.Where(p => p.ApiApiDetailsId == apiId).OrderByDescending(p => p.ShowOrder).ToList();
            if (apilist == null) return Json(new ResultModel() { Message = "接口获取失败！" });
            var RequestParam = apilist.Where(p => p.Ptype == 1).ToList();
            var ReturnParam = apilist.Where(p => p.Ptype == 2).ToList();
            if (RequestParam == null) RequestParam = new List<ApiParam>();
            if (ReturnParam == null) ReturnParam = new List<ApiParam>();
            //获取当前接口的所有版本列表
            //(from R in content.ApiApiDetails
            // join W in content.ApiApiDetails on R.SwitchName equals W.SwitchName
            // where R.Id== apiId && R.State.Contains(0)
            // )
            //var VersionList = bll.Get_ApiVersionList(apiId);
            //return Json(new ResultModel()
            //{
            //    Code = 100,
            //    Data = new
            //    {
            //        RequestParam = RequestParam.Select(p => new
            //        {
            //            p.Id,
            //            p.ShowOrder,
            //            p.Api_ApiDetailsId,
            //            p.Title,
            //            p.Describe,
            //            p.PType,
            //            p.DValueType,
            //            p.DValue,
            //            p.IsMust,
            //            p.IsLower,
            //            _parentId = p.ParentId,
            //        }),
            //        ReturnParam = ReturnParam.Select(p => new
            //        {
            //            p.Id,
            //            p.ShowOrder,
            //            p.Api_ApiDetailsId,
            //            p.Title,
            //            p.Describe,
            //            p.PType,
            //            p.DValueType,
            //            p.DValue,
            //            p.IsMust,
            //            p.IsLower,
            //            _parentId = p.ParentId,
            //        }),
            //        APIVersionList = VersionList.Select(p => new
            //        {
            //            p.Id,
            //            p.VersionCode
            //        })
            //    }
            //});
            return Json(new { });
        }
        #endregion


    }
}
