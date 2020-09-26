

var lodingConfig = { num: 0, handle: null };
(function () {
    new Vue({
        el: '#vm',
        filters: {
            ApiState: function (value) {
                switch (value) {
                    case 1:
                        return "Get";
                    case 2:
                        return "Post";
                    case 3:
                        return "Put";
                    case 4:
                        return "Delete";
                    case 0:
                        return "不限";
                }
                return "未知";
            }
        },
        watch: {
            UserEntity: function (newVal, oldVal) {

            },
        },
        data: {
            UserEntity: null,//用户信息
            DomainList: null,//接口域列表
            ProjectList: null,//项目列表
            IframeProjectList: [],//项目列表
            UserLoginEntity: {
                Account: "xinyanan",
                Pwd: "xin521258",
            },  //登入信息





            View_Variable: {
                DomainId: null,
                TabId:"",
            },

            //-------视图变量-------
            View_LoginWindow: false,//登入窗口
            View_SetDomainWindow: false,//设置窗口
            rules: {
                Account: [
                    { required: true, message: '请输入活动名称', trigger: 'blur' },
                    { min: 3, max: 5, message: '长度在 3 到 5 个字符', trigger: 'blur' }
                ],
                Pwd: [{ required: true, message: '请输入密码', trigger: 'blur' }],
            },
        },
        created: function () {
            //获取用户信息
            this.onPublicCallBack(1);
        },
        methods: {
            //网络请求
            onRequest: function (otion) {
                var _this = this;
                otion = otion || {};
                otion.data = otion.data || {};
                otion.url = otion.url || "";
                otion.type = otion.type || "post";
                otion.async = otion.async || true;
                otion.loading = otion.loading || true;
                otion.loadingmsg = otion.loadingmsg || "加载中......";
                otion.beforeSend = otion.beforeSend || function () { };
                otion.complete = otion.complete || function () { };
                otion.error = otion.error || function (e, a) {
                    console.log(e);
                    switch (e.status) {
                        case 401:
                            _this.$message.error('您好像没有权限先登录吧！');
                            _this.View_LoginWindow = true;
                            break;
                        case 500:
                            _this.$message.error('服务器发生错误！');
                            break;
                        default:
                            _this.$message.error('服务器异常！');
                    }


                };
                otion.success = otion.success || function () { };
                $.ajax({
                    url: otion.url,
                    type: otion.type,
                    data: otion.data,
                    async: otion.async,
                    beforeSend: function () {
                        if (otion.loading) {
                            if (lodingConfig.num == 0) {
                                lodingConfig.handle = _this.$loading({
                                    lock: true,
                                    text: otion.loadingmsg,
                                    spinner: 'el-icon-loading',
                                    background: 'rgba(0, 0, 0, 0.7)'
                                });
                            }
                            lodingConfig.num++;
                        }
                        otion.beforeSend();
                    },
                    complete: function () {
                        if (otion.loading) {
                            lodingConfig.num--;
                            if (lodingConfig.handle != null && lodingConfig.num <= 0) lodingConfig.handle.close();
                        }

                        otion.complete();
                    },
                    error: otion.error,
                    success: otion.success,
                });
            }
            //获取接口域数据
            , onGetDomain: function () {
                var _this = this;
                var param = {
                    url: "/AJAX/GetDomain",
                    loadingmsg: "",
                    success: function (data) {
                        if (data.Code != 1000) {
                            _this.onPublicCallBack(301, data);
                            return;
                        }
                        _this.onPublicCallBack(300, data);
                    }
                };
                this.onRequest(param);
            }
            //退出登入
            , onOutLogin: function () {
                var _this = this;
                var param = {
                    url: "/AJAX/logout",
                    loadingmsg: "",
                    success: function (data) {
                        if (data.Code != 1000) {
                            _this.$message.error(data.Message);
                            return;
                        }
                        _this.onPublicCallBack(102);
                    }
                };
                this.onRequest(param);
            }
            //获取用户信息
            , onGetUserEntity: function () {
                var _this = this;
                var param = {
                    url: "/AJAX/GetUserInfo",
                    loadingmsg: "登入中请稍等........",
                    success: function (data) {
                        if (data.Code == 400) {//未登入
                            _this.onPublicCallBack(400, data);
                            return;
                        }
                        else if (data.Code != 1000) {
                            _this.onPublicCallBack(201, data);
                            return;
                        }
                        _this.onPublicCallBack(200, data);
                    }
                };
                this.onRequest(param);
            }
            //登入
            , onLogin: function (formName) {
                var _this = this;
                var Isvalid = false;
                _this.$refs.UserLoginEntity.validate((valid) => {
                    Isvalid = valid;
                    if (!valid) return false;
                });
                if (!Isvalid) return Isvalid;
                var param = {
                    url: "/AJAX/Login",
                    data: _this.UserLoginEntity,
                    success: function (data) {
                        if (data.Code != 1000) {
                            _this.onPublicCallBack(101, data);
                            return;
                        }
                        _this.onPublicCallBack(100, data);
                    }
                };
                this.onRequest(param);
            }
            //获取接口项目列表
            , onGetProjectList: function (DoId, ProId) {
                var _this = this;
                if (!DoId || DoId <= 0) {
                    _this.$message.error("请选择接口域");
                    return;
                }
                var param = {
                    url: "/AJAX/GetProject",
                    data: { doId: DoId, id: ProId },
                    success: function (data) {
                        if (data.Code != 1000) {
                            _this.onPublicCallBack(501, data);
                            return;
                        }
                        _this.onPublicCallBack(500, data);
                    }
                };
                this.onRequest(param);
            }


            //打开登入窗口
            , onOpenLoginWindow: function () {
                var _this = this;
                _this.View_LoginWindow = true;
            }
            //设置接口域窗口
            , onDomainWindow: function (val) {
                var _this = this;
                _this.View_SetDomainWindow = val;
                //if (_this.DomainList == null || _this.DomainList.length == 0) {
                //    _this.$message.error("没有找到接口域数据");
                //    return;
                //}
                //if (doid > 0) {
                //    E(_this.DomainList).where(p => p.Id == doid).firstOrDefault().Select = true;
                //    _this.onPublicCallBack(302);
                //    if (_this.View_SetDomainWindow == true) _this.View_SetDomainWindow = false;
                //}
                //else {

                //}
            }
            //菜单导航点击
            , onMenuClick: function (ApiEntity) {
                var _this = this;
                var fl = _this.IframeProjectList.find(p => p == ApiEntity);
                if (ApiEntity != fl) {
                    _this.IframeProjectList.push(ApiEntity);
                }
                
            }
            //下拉菜单点击事件
            , onDropdownClick: function (com) {
                var _this = this;
                switch (com) {
                    case 'onDomainWindow':
                        _this.onDomainWindow(true);
                        break;
                    case 'onOutLogin':
                        _this.onOutLogin();
                        break;
                }
            }
            , onPublicCallBack: function (source, data) {
                var _this = this;
                switch (source) {
                    case 1://
                        _this.onGetUserEntity();
                        break;
                    case 100://登入成功
                        _this.$message({
                            message: '登入完成',
                            type: 'success'
                        });
                        _this.UserEntity = data.Data.UserEntityCookie;
                        _this.View_LoginWindow = false;
                        _this.UserLoginEntity.Pwd = "";
                        _this.onGetDomain();//获取接口域数据
                        break;
                    case 101://登入失败
                        _this.$message.error(data.Message);
                        break;
                    case 102://退出登入
                        _this.UserEntity = null;
                        _this.onOpenLoginWindow();
                        break;
                    case 200://获取用户成功
                        _this.UserEntity = data.Data;//设置用户信息
                        _this.onGetDomain();//获取接口域数据
                        break;
                    case 201://获取用户失败
                        _this.$message.error(data.Message);
                        break;
                    case 300://获取接口域数据成功
                        _this.DomainList = data.Data;
                        _this.onDomainWindow(true);
                        break;
                    case 301://获取接口域数据失败
                        _this.$message.error(data.Message);
                        break;
                    case 302://选择接口完成
                        break;
                    case 400://用户未登入
                        _this.onOpenLoginWindow();
                        break;
                    case 500://获取接口项目数据成功
                        _this.ProjectList = data.Data;
                        _this.onDomainWindow(false);
                        break;
                    case 501://获获取接口项目数据失败
                        _this.$message.error(data.Message);
                        break;
                }
            }
        }
    });
})();