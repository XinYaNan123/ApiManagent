using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DataAccess.DBContent
{
    public partial class ApiManagentContext : DbContext
    {
        public ApiManagentContext()
        {
        }

        public ApiManagentContext(DbContextOptions<ApiManagentContext> options)
            : base(options)
        {
        }

        public virtual DbSet<ApiApiDetails> ApiApiDetails { get; set; }
        public virtual DbSet<ApiAttribute> ApiAttribute { get; set; }
        public virtual DbSet<ApiDebugUrl> ApiDebugUrl { get; set; }
        public virtual DbSet<ApiDomain> ApiDomain { get; set; }
        public virtual DbSet<ApiOperationLog> ApiOperationLog { get; set; }
        public virtual DbSet<ApiParam> ApiParam { get; set; }
        public virtual DbSet<ApiProject> ApiProject { get; set; }
        public virtual DbSet<ApiUsers> ApiUsers { get; set; }
        public virtual DbSet<ApiVersion> ApiVersion { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Data Source=.;Initial Catalog=ApiManagent;Integrated Security=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ApiApiDetails>(entity =>
            {
                entity.ToTable("Api_ApiDetails");

                entity.Property(e => e.ApiDomainId)
                    .HasColumnName("Api_domainId")
                    .HasComment("隶属接口域id");

                entity.Property(e => e.ApiProjectId)
                    .HasColumnName("Api_ProjectId")
                    .HasComment("隶属项目id");

                entity.Property(e => e.CreateDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())")
                    .HasComment("创建时间");

                entity.Property(e => e.Describe)
                    .HasMaxLength(200)
                    .HasComment("接口描述");

                entity.Property(e => e.Methods)
                    .HasDefaultValueSql("((1))")
                    .HasComment("1全部 2Get 3Post 4Put 5delete ");

                entity.Property(e => e.NameClass)
                    .HasMaxLength(500)
                    .HasComment("自定义html");

                entity.Property(e => e.Path)
                    .HasMaxLength(100)
                    .HasComment("接口具体路径");

                entity.Property(e => e.ShowOrder)
                    .HasDefaultValueSql("((1))")
                    .HasComment("显示顺序越大越靠前");

                entity.Property(e => e.State)
                    .HasDefaultValueSql("((1))")
                    .HasComment("1未上线 2已上线 3已废弃");

                entity.Property(e => e.SwitchName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasComment("接口名称");

                entity.Property(e => e.UpdateDate)
                    .HasColumnType("datetime")
                    .HasComment("更新时间");

                entity.Property(e => e.VersionCode)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasDefaultValueSql("(N'000.000.000')")
                    .HasComment("版本Id");
            });

            modelBuilder.Entity<ApiAttribute>(entity =>
            {
                entity.ToTable("Api_Attribute");

                entity.Property(e => e.ApiDomain)
                    .HasColumnName("Api_Domain")
                    .HasComment("接口域id 0表示所有域公共参数");

                entity.Property(e => e.ApiProjectId)
                    .HasColumnName("Api_ProjectId")
                    .HasComment("项目Id 0表示当前域公共参数");

                entity.Property(e => e.AttributeKey)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasComment("属性名");

                entity.Property(e => e.AttributeVal)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasComment("属性值");
            });

            modelBuilder.Entity<ApiDebugUrl>(entity =>
            {
                entity.ToTable("Api_DebugUrl");

                entity.Property(e => e.DelDate)
                    .HasColumnType("datetime")
                    .HasComment("删除时间");

                entity.Property(e => e.UpdateDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())")
                    .HasComment("调试时间");

                entity.Property(e => e.Url)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasComment("调试的url地址");

                entity.Property(e => e.UserId).HasComment("用户id 如果是0表示是共享的地址");
            });

            modelBuilder.Entity<ApiDomain>(entity =>
            {
                entity.ToTable("Api_Domain");

                entity.Property(e => e.AddressUrl)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasComment("访问url");

                entity.Property(e => e.SecretKey)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasComment("秘钥||签名");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasComment("接口域名称");
            });

            modelBuilder.Entity<ApiOperationLog>(entity =>
            {
                entity.ToTable("Api_OperationLog");

                entity.Property(e => e.ApiApiDetailsId)
                    .HasColumnName("Api_ApiDetailsId")
                    .HasComment("接口id");

                entity.Property(e => e.ApiDomainId)
                    .HasColumnName("Api_DomainId")
                    .HasComment("接口域id");

                entity.Property(e => e.ApiProject)
                    .HasColumnName("Api_Project")
                    .HasComment("接口项目id");

                entity.Property(e => e.CredateDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Otype)
                    .HasColumnName("OType")
                    .HasComment("1接口调试 2接口修改 3接口创建");
            });

            modelBuilder.Entity<ApiParam>(entity =>
            {
                entity.ToTable("Api_Param");

                entity.Property(e => e.ApiApiDetailsId)
                    .HasColumnName("Api_ApiDetailsId")
                    .HasComment("接口id");

                entity.Property(e => e.Describe)
                    .HasMaxLength(200)
                    .HasComment("描述");

                entity.Property(e => e.Dvalue)
                    .HasColumnName("DValue")
                    .HasMaxLength(500)
                    .HasDefaultValueSql("(N'“”')")
                    .HasComment("默认值");

                entity.Property(e => e.DvalueType)
                    .HasColumnName("DValueType")
                    .HasDefaultValueSql("((1))")
                    .HasComment("默认值类型 1int 2string");

                entity.Property(e => e.IsLower).HasComment("是否区分大小写  0否 1是");

                entity.Property(e => e.IsMust)
                    .IsRequired()
                    .HasDefaultValueSql("((1))")
                    .HasComment("是否必须 0否 1是");

                entity.Property(e => e.ParentId).HasComment("父id");

                entity.Property(e => e.Ptype)
                    .HasColumnName("PType")
                    .HasComment("1请求参数  2返回参数");

                entity.Property(e => e.ShowOrder)
                    .HasDefaultValueSql("((1))")
                    .HasComment("显示顺序越大越靠前");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasComment("参数名称");
            });

            modelBuilder.Entity<ApiProject>(entity =>
            {
                entity.ToTable("Api_Project");

                entity.Property(e => e.ApiDomainId)
                    .HasColumnName("Api_domainId")
                    .HasComment("接口域id");

                entity.Property(e => e.IsDel)
                    .HasDefaultValueSql("((0))")
                    .HasComment("0未删除 1已删除");

                entity.Property(e => e.ShowOrder)
                    .HasDefaultValueSql("((1))")
                    .HasComment("显示顺序越大越靠前");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasComment("项目名称");
            });

            modelBuilder.Entity<ApiUsers>(entity =>
            {
                entity.ToTable("Api_Users");

                entity.Property(e => e.Account)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.AddUserId).HasDefaultValueSql("((0))");

                entity.Property(e => e.CreateDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())")
                    .HasComment("添加时间");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(300);

                entity.Property(e => e.State).HasComment("0 禁用  1启用");

                entity.Property(e => e.UserFace).HasMaxLength(300);

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(e => e.UserType).HasComment("1测试人员 2开发人员  3接口开发人员  4总管理员");
            });

            modelBuilder.Entity<ApiVersion>(entity =>
            {
                entity.ToTable("Api_Version");

                entity.Property(e => e.ApiProject)
                    .HasColumnName("Api_Project")
                    .HasComment("项目id");

                entity.Property(e => e.VersionCode)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasComment("版本号");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
