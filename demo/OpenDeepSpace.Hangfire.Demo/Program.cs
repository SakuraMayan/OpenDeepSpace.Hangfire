using Hangfire;
using Hangfire.MySql;
using OpenDeepSpace.Hangfire;
using OpenDeepSpace.Hangfire.Demo;
using OpenDeepSpace.Hangfire.Demo.Jobs;
using OpenDeepSpace.Hangfire.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


//Hangfire��ʹ��
builder.Services.AddHangfire(
		opt =>
		{
			opt.UseStorage(new MySqlStorage("Data Source=192.168.101.128;Initial Catalog=ods;User ID=root;Password=Mysql80:960325;Charset=utf8;Port=3306;Allow User Variables=true;", new MySqlStorageOptions()
			{

				TablesPrefix = "hangfire"
			}));

			//ע��ʹ����RecurringJobAttribute���Ե�������Job
			opt.RegisterRecurringJobs(new List<System.Reflection.Assembly>() { typeof(RecurringJobs).Assembly});

		}
	);

//���HangfireServer
builder.Services.AddHangfireServer(opt =>
{
    //���Hangfire���������

    opt.Queues =new[] { "default", "local", "recurringjobqueue" };//����һ��Ҫָ�� ���Jobָ���˶��� ����û���� Job���޷�ִ�� ��������ָ��ǰ�������м�default local

});

//ע�������Job
builder.Services.RegisterParametricJobs(new List<System.Reflection.Assembly>() { typeof(RecurringJobs).Assembly });
//���JobState״̬��� ���ڳɹ���ʧ��ִ�н���Ĵ���
GlobalJobFilters.Filters.Add(new JobStateFilter(builder.Services.BuildServiceProvider()));
//ע��Job�ɹ�ʧ�ܵĴ���ʵ��
builder.Services.AddTransient<IJobExecuteResultHandler, JobExecuteResultHandler>();
//���óɹ�ִ�е�Job�־û�ʱ��
GlobalStateHandlers.Handlers.Add(new SucceededJobExpiredHandler());

builder.Services.AddTransient<IScopedService, ScopedService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//��ӿ������ѡ�� ����Ȩ�� ��Ҫ��¼���ܷ��ʿ������
DashboardOptions dashboardOptions = new DashboardOptions();
dashboardOptions.Authorization = new[] { new BasicAuthorizationFilter("wy", "wy.023") };
//ʹ�ÿ������
app.UseHangfireDashboard(options:dashboardOptions);

app.UseAuthorization();

app.MapControllers();

app.Run();
