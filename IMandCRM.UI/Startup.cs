using Business.Abstract;
using Business.Concrete;
using DataAccess.Abstract;
using DataAccess.Concrete;
using IMandCRM.UI.EmailServices;
using IMandCRM.UI.Identity;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace IMandCRM.UI
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //identity ayarlarý
            //services.AddDbContext<ApplicationContext>(options => options.UseSqlServer(@"Data Source=89.19.24.243;Initial Catalog=Rubusoft_Ss;persist security info=True;user id=Login_Ss;Password=mexETH0JauSt7W9b;"));
            services.AddDbContext<ApplicationContext>(options => options.UseSqlServer(@"Data Source=DESKTOP-DHUQA7J\SQLEXPRESS;Initial Catalog=SmartScreen;persist security info=True;user id=frk;Password=123456;"));
            //services.AddDbContext<ApplicationContext>(options => options.UseSqlServer(@"Data Source=LAPTOP-Q5KQP3V5\SQLEXPRESS;Initial Catalog=Rubusoft_Ss;persist security info=True;user id=qbot;Password=123;"));

            services.AddIdentity<User, IdentityRole>().AddEntityFrameworkStores<ApplicationContext>().AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(options =>
            {
                //password
                options.Password.RequireDigit = true; //parola içinde sayýsal deðer olmalýdýr
                options.Password.RequireLowercase = true; // parola içinde küçük harf olmak zorunda
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 8; // parola min. kaç karakter olacaðý
                options.Password.RequireNonAlphanumeric = true; // parola içinde bir karakter olmalýdýr

                //Locaout
                options.Lockout.MaxFailedAccessAttempts = 5; //parolanýn max 5 defa yanlýþ girebilir
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.AllowedForNewUsers = true;


                /*options.User.AllowedUserNameCharacters = "";*///user içinde olmasýný istediðiniz karakterler
                options.User.RequireUniqueEmail = true; // ayný mail adresinden iki kullanýcý olamaz
                options.SignIn.RequireConfirmedEmail = true; ; //kullanýcýya onay maili gönderilmesi
                options.SignIn.RequireConfirmedPhoneNumber = false;


            });

            //cookie ayarlarý

            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/account/login";
                options.LogoutPath = "/account/logout";
                options.AccessDeniedPath = "/account/accessdenied";
                options.SlidingExpiration = true; //cookie nin yaþam süresini belirler
                options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
                options.Cookie = new CookieBuilder
                {
                    HttpOnly = true,
                    Name = ".rubusoft.security.cookie",
                    SameSite = SameSiteMode.Strict
                };

            });

            //AutoMapper
            services.AddAutoMapper(typeof(Startup));

            //Dependency Injection

            services.AddScoped<IProductDal, ProductDal>();
            services.AddScoped<IProductService, ProductService>();

            services.AddScoped<IDeviceDal, DeviceDal>();
            services.AddScoped<IDeviceService, DeviceService>();

            services.AddScoped<IFirmDal, FirmDal>();
            services.AddScoped<IFirmService, FirmService>();

            services.AddScoped<IFirmManagerDal, FirmManagerDal>();
            services.AddScoped<IFirmManagerService, FirmManagerService>();

            services.AddScoped<ICountryDal, CountryDal>();
            services.AddScoped<ICountryService, CountryService>();

            services.AddScoped<ICityDal, CityDal>();
            services.AddScoped<ICityService, CityService>();

            services.AddScoped<IDistrictDal, DistrictDal>();
            services.AddScoped<IDistrictService, DistrictService>();

            services.AddScoped<IAddressDal, AddressDal>();
            services.AddScoped<IAddressService, AddressService>();

            services.AddScoped<IBidDal, BidDal>();
            services.AddScoped<IBidService, BidService>();

            services.AddScoped<IBidProductDal, BidProductDal>();
            services.AddScoped<IBidProductService, BidProductService>();

            services.AddScoped<IAppSettingDal, AppSettingDal>();
            services.AddScoped<IAppSettingService, AppSettingService>();

            services.AddScoped<IGeneralRequirementDal, GeneralRequirementDal>();
            services.AddScoped<IGeneralRequirementService, GeneralRequirementService>();

            services.AddScoped<IAspNetUserDal, AspNetUserDal>();
            services.AddScoped<IAspNetUserService, AspNetUserService>();

            services.AddScoped<ICustomerRequestDal, CustomerRequestDal>();
            services.AddScoped<ICustomerRequestService, CustomerRequestService>();

            services.AddScoped<ITechSupportDal, TechSupportDal>();
            services.AddScoped<ITechSupportService, TechSupportService>();

            services.AddScoped<ITechSupportDetailDal, TechSupportDetailDal>();
            services.AddScoped<ITechSupportDetailService, TechSupportDetailService>();

            services.AddScoped<IStockPointDal, StockPointDal>();
            services.AddScoped<IStockPointService, StockPointService>();

            services.AddScoped<IDeviceStockDal, DeviceStockDal>();
            services.AddScoped<IDeviceStockService, DeviceStockService>();

            services.AddScoped<IProductStockDal, ProductStockDal>();
            services.AddScoped<IProductStockService, ProductStockService>();

            //email injection
            services.AddScoped<IEmailSender, EmailSender>(i => new EmailSender(
                Configuration["EmailSender:Host"],
                Configuration.GetValue<int>("EmailSender:Port"),
                Configuration.GetValue<bool>("EmailSender:EnableSSL"),
                Configuration["EmailSender:UserName"],
                Configuration["EmailSender:Password"]
                ));

            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IConfiguration configuration, UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            var supportedCultures = new[]
            {
                new CultureInfo("en-US"),
                new CultureInfo("es"),
            };

            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture("en-US"),
                // Formatting numbers, dates, etc.
                SupportedCultures = supportedCultures,
                // Localized UI strings.
                SupportedUICultures = supportedCultures
            });
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "login",
                    pattern: "/",
                    defaults: new { controller = "Account", action = "Login" });

                endpoints.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");

                endpoints.MapControllerRoute(name: "ProductEdit", pattern: "{controller=Product}/{action=ProductEdit}/{idKod?}");
                endpoints.MapControllerRoute(name: "ProductDetail", pattern: "{controller=Product}/{action=ProductDetail}/{idKod?}");
                endpoints.MapControllerRoute(name: "ProductDelete", pattern: "{controller=Product}/{action=ProductDelete}/{idKod?}");

                endpoints.MapControllerRoute(name: "DeviceEdit", pattern: "{controller=Device}/{action=DeviceEdit}/{idKod?}");
                endpoints.MapControllerRoute(name: "DeviceDetail", pattern: "{controller=Device}/{action=DeviceDetail}/{idKod?}");
                endpoints.MapControllerRoute(name: "DeviceDelete", pattern: "{controller=Device}/{action=DeviceDelete}/{idKod?}");

                endpoints.MapControllerRoute(name: "FirmDetail", pattern: "{controller=Firm}/{action=FirmDetail}/{idKod?}");
                endpoints.MapControllerRoute(name: "FirmAddresses", pattern: "{controller=Firm}/{action=FirmAddresses}/{firmIdKod?}");
                endpoints.MapControllerRoute(name: "FirmManagers", pattern: "{controller=Firm}/{action=FirmManagers}/{firmIdKod?}");
           
                endpoints.MapControllerRoute(name: "GetAddressByIdKod", pattern: "{controller=Address}/{action=GetFirmAddressByIdKod}/{addressIdKod?}");
                endpoints.MapControllerRoute(name: "DeleteAddress", pattern: "{controller=Address}/{action=AddressDelete}/{idKod?}");

                endpoints.MapControllerRoute(name: "GetManagerByIdKod", pattern: "{controller=FirmManager}/{action=GetFirmManagerByIdKod}/{managerIdKod?}");
                endpoints.MapControllerRoute(name: "DeleteFirmManager", pattern: "{controller=FirmManager}/{action=FirmManagerDelete}/{idKod?}");

                endpoints.MapControllerRoute(name: "EditDraftBid", pattern: "{controller=Bid}/{action=DraftBidEdit}/{bidIdKod?}");
                endpoints.MapControllerRoute(name: "EditExpiredBid", pattern: "{controller=Bid}/{action=ExpiredBidEdit}/{bidIdKod?}");
                endpoints.MapControllerRoute(name: "EditPendingInternalApproval", pattern: "{controller=Bid}/{action=PendingInternalApprovalEdit}/{bidIdKod?}");
                endpoints.MapControllerRoute(name: "EditInternallyApproved", pattern: "{controller=Bid}/{action=InternallyApprovedEdit}/{bidIdKod?}");
                endpoints.MapControllerRoute(name: "SentBidDetail", pattern: "{controller=Bid}/{action=SentBidDetail}/{bidIdKod?}");
                endpoints.MapControllerRoute(name: "BidCustomerApproval", pattern: "{controller=Bid}/{action=BidCustomerApproval}/{bidIdKod?}");
                endpoints.MapControllerRoute(name: "BidCustomerApproval", pattern: "{controller=Bid}/{action=BidCustomerApprovaled}/{bidIdKod}/{bidStatus}");
                endpoints.MapControllerRoute(name: "BidHtmlToPdfSingle", pattern: "{controller=Bid}/{action=HtmlToPdfSingle}/{bidIdKod?}");
                endpoints.MapControllerRoute(name: "BidPdf", pattern: "{controller=Bid}/{action=BidPdf}/{bidIdKod?}");
                endpoints.MapControllerRoute(name: "BidEmail", pattern: "{controller=Bid}/{action=SentBidEmail}/{bidIdKod?}");
                endpoints.MapControllerRoute(name: "DeleteBid", pattern: "{controller=Bid}/{action=BidDelete}/{idKod?}");
                endpoints.MapControllerRoute(name: "BidDownloadPdf", pattern: "{controller=Bid}/{action=DownloadPdf}/{bidIdKod?}");

                endpoints.MapControllerRoute(name: "GeneralRequirementEdit", pattern: "{controller=GeneralRequirement}/{action=GeneralRequirementEdit}/{idKod?}");
                endpoints.MapControllerRoute(name: "GeneralRequirementDelete", pattern: "{controller=GeneralRequirement}/{action=GeneralRequirementDelete}/{idKod?}");

                endpoints.MapControllerRoute(name: "GetCustomer", pattern: "{controller=CustomerRequest}/{action=GetCustomer}/{idKod?}");
                endpoints.MapControllerRoute(name: "CreateBid", pattern: "{controller=CustomerRequest}/{action=CreateBid}/{idKod?}");
                endpoints.MapControllerRoute(name: "GetRequestDescription", pattern: "{controller=CustomerRequest}/{action=GetRequestDescription}/{idKod?}");

                endpoints.MapControllerRoute(name: "DeleteUser", pattern: "{controller=Account}/{action=UserDelete}/{id?}");

            });
            SeedIdentity.Seed(userManager, roleManager, configuration).Wait();
        }
    }
}
