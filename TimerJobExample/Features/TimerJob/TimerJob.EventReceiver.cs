using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;

namespace TimerJobExample.Features.TimerJob
{
    /// <summary>
    /// 此类用于处理在激活、停用、安装、卸载和升级功能的过程中引发的事件。
    /// </summary>
    /// <remarks>
    /// 附加到此类的 GUID 可能会在打包期间使用，不应进行修改。
    /// </remarks>

    [Guid("639bba7a-4921-48f2-b696-928af8ceb001")]
    public class TimerJobEventReceiver : SPFeatureReceiver
    {
        // 取消对以下方法的注释，以便处理激活某个功能后引发的事件。
        const string JOB_NAME = "TimerJobTest";
        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {
            SPSite site = properties.Feature.Parent as SPSite;

            // make sure the job isn't already registered
            foreach (SPJobDefinition job in site.WebApplication.JobDefinitions)
            {
                if (job.Name == JOB_NAME)
                {
                    job.Delete();
                }
            }
            // install the job
            TimerJobClass Doc = new TimerJobClass(JOB_NAME, site.WebApplication);

            // 传递参数
            Doc.Properties.Add("SiteUrl", site.Url);


            SPMinuteSchedule schedule = new SPMinuteSchedule();
            schedule.BeginSecond = 0;
            schedule.EndSecond = 59;
            schedule.Interval = 1;
            Doc.Schedule = schedule;
            Doc.Update();
        }


        // 取消对以下方法的注释，以便处理在停用某个功能前引发的事件。

        public override void FeatureDeactivating(SPFeatureReceiverProperties properties)
        {
            SPSite site = properties.Feature.Parent as SPSite;
            // delete the job
            foreach (SPJobDefinition job in site.WebApplication.JobDefinitions)
            {

                if (job.Name == JOB_NAME)
                {
                    job.Delete();
                }
            }
        }


        // 取消对以下方法的注释，以便处理在安装某个功能后引发的事件。

        //public override void FeatureInstalled(SPFeatureReceiverProperties properties)
        //{
        //}


        // 取消对以下方法的注释，以便处理在卸载某个功能前引发的事件。

        //public override void FeatureUninstalling(SPFeatureReceiverProperties properties)
        //{
        //}

        // 取消对以下方法的注释，以便处理在升级某个功能时引发的事件。

        //public override void FeatureUpgrading(SPFeatureReceiverProperties properties, string upgradeActionName, System.Collections.Generic.IDictionary<string, string> parameters)
        //{
        //}
    }
}
